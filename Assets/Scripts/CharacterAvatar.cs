using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterAvatar : MonoBehaviour
{
    private readonly Dictionary<HumanBodyBones, BoneData> _bones = new Dictionary<HumanBodyBones, BoneData>(Constants.JointToBone.Count);

    private JointType _highestHierarchyBone;

    private bool _isInitialized;

    private Vector3 _originalPosition = Vector3.zero;

    [SerializeField]
    private GameObject _model;

    [SerializeField]
    private bool _updatePose = true;

    [SerializeField]
    private bool _matchScale;

    [SerializeField]
    private bool _useWorldPosition;

    [SerializeField]
    private bool _rotate = true;

    [Range(0f, 1f)]
    [SerializeField]
    private float _smoothing = 0.5f;

    [SerializeField]
    private JointUpdates _updates = new();

    public GameObject Model
    {
        get
        {
            return _model;
        }
        set
        {
            _model = value;
        }
    }

    public bool UpdatePose
    {
        get
        {
            return _updatePose;
        }
        set
        {
            _updatePose = value;
        }
    }

    public bool MatchScale
    {
        get
        {
            return _matchScale;
        }
        set
        {
            _matchScale = value;
        }
    }

    public bool UseWorldPosition
    {
        get
        {
            return _useWorldPosition;
        }
        set
        {
            _useWorldPosition = value;
        }
    }

    public bool Rotate
    {
        get
        {
            return _rotate;
        }
        set
        {
            _rotate = value;
        }
    }

    public float Smoothing
    {
        get
        {
            return _smoothing;
        }
        set
        {
            _smoothing = Mathf.Clamp(value, 0f, 1f);
        }
    }

    public JointUpdates JointUpdates
    {
        get
        {
            return _updates;
        }
        set
        {
            _updates = value;
        }
    }

    private void OnValidate()
    {
        _updates.Validate();
    }

    public void Initialize(GameObject newAvatarRoot)
    {
        _model = newAvatarRoot;
        Initialize();
    }

    public void Initialize()
    {
        _isInitialized = false;
        if (!_model.TryGetComponent(out Animator component))
        {
            Debug.LogError("The avatar does not have an Animator component.");
            return;
        }
        if (!component.isHuman)
        {
            Debug.LogError("The avatar Animator is not humanoid.");
            return;
        }
        _bones.Clear();
        foreach (KeyValuePair<JointType, HumanBodyBones> item in Constants.JointToBone)
        {
            Transform boneTransform = component.GetBoneTransform(item.Value);
            if (boneTransform != null)
            {
                _bones.Add(item.Value, new(boneTransform));
            }
        }
        _highestHierarchyBone = FindHighestHieararchyBone();
        Quaternion rotation = _model.transform.rotation;
        _model.transform.rotation = Quaternion.identity;
        _originalPosition = _model.transform.position;
        TPose.DoTPose(_bones);
        foreach (KeyValuePair<HumanBodyBones, BoneData> bone in _bones)
        {
            bone.Value.CalibrateOriginalRotation();
        }
        _isInitialized = true;
        _model.transform.rotation = rotation;
    }

    public void DoTPose()
    {
        if (!_isInitialized)
        {
            Initialize();
        }
        TPose.DoTPose(_bones);
    }

    internal Vector3 GetDirection(Vector3 to, Vector3 from)
    {
        return (to - from).normalized;
    }

    internal Vector3 GetNormal(Vector3 j1, Vector3 j2, Vector3 j3)
    {
        return Normal(j1, j2, j3).normalized;
    }

    protected void CalculateOrientations(BodyFrame body)
    {
        Joint topSkullJoint = body.Joints[(int)JointType.TopSkull];
        Joint headJoint = body.Joints[(int)JointType.Head];
        Joint noseJoint = body.Joints[(int)JointType.Nose];
        Joint neckJoint = body.Joints[(int)JointType.Neck];
        Joint chestJoint = body.Joints[(int)JointType.Chest];
        Joint waitJoint = body.Joints[(int)JointType.Waist];
        Joint pelvisJoint = body.Joints[(int)JointType.Pelvis];
        Joint shoulderLeftJoint = body.Joints[(int)JointType.ShoulderLeft];
        Joint shoulderRightJoint = body.Joints[(int)JointType.ShoulderRight];
        Joint elbowLeftJoint = body.Joints[(int)JointType.ElbowLeft];
        Joint elbowRightJoint = body.Joints[(int)JointType.ElbowRight];
        Joint wristLeftJoint = body.Joints[(int)JointType.WristLeft];
        Joint wristRightJoint = body.Joints[(int)JointType.WristRight];
        Joint hipLeftJoint = body.Joints[(int)JointType.HipLeft];
        Joint hipRightJoint = body.Joints[(int)JointType.HipRight];
        Joint kneeLeftJoint = body.Joints[(int)JointType.KneeLeft];
        Joint kneeRightJoint = body.Joints[(int)JointType.KneeRight];
        Joint ankleLeftJoint = body.Joints[(int)JointType.AnkleLeft];
        Joint ankleRightJoint = body.Joints[(int)JointType.AnkleRight];
        Joint bigToeLeftJoint = body.Joints[(int)JointType.BigToeLeft];
        Joint bigToeRightJoint = body.Joints[(int)JointType.BigToeRight];


        //Waist
        //Vector3 vector3D = Vector3.Lerp(shoulderLeftJoint.GetPosition3D(),
        //                                shoulderRightJoint.GetPosition3D(),
        //                                0.5f);
        //Vector3 normalized = Normal(pelvisJoint.GetPosition3D(),
        //                            vector3D,
        //                            shoulderRightJoint.GetPosition3D()).normalized;
        //Vector3 normalized2 = (vector3D - waitJoint.GetPosition3D()).normalized;
        //UpdateJointRotation(JointType.Waist,
        //                    Quaternion.LookRotation(normalized2, normalized)
        //                    * Quaternion.Euler(90f, 0f, 180f));

        //Chest
        //normalized2 = GetDirection(shoulderRightJoint.GetPosition3D(),
        //                           shoulderLeftJoint.GetPosition3D());
        //UpdateJointRotation(JointType.Chest,
        //                    Quaternion.LookRotation(normalized2, normalized)
        //                    * Quaternion.Euler(90f, 0f, -90f));

        //Neck
        //normalized2 = GetDirection(headJoint.GetPosition3D(),
        //                           neckJoint.GetPosition3D());
        //UpdateJointRotation(JointType.Neck,
        //                    Quaternion.LookRotation(normalized2, normalized)
        //                    * Quaternion.Euler(90f, 0f, 180f));

        //Head
        //normalized = GetNormal(neckJoint.GetPosition3D(),
        //                       headJoint.GetPosition3D(),
        //                       noseJoint.GetPosition3D());
        //normalized2 = GetDirection(noseJoint.GetPosition3D(),
        //                           headJoint.GetPosition3D());
        //float num = (topSkullJoint.Value.Confidence >= 0f) ? 136f : 150f;
        //UpdateJointRotation(JointType.Head,
        //                    Quaternion.LookRotation(normalized2, normalized)
        //                    * Quaternion.Euler(0f, num, 90f));

        //ShoulderRight
        //normalized = GetNormal(shoulderRightJoint.GetPosition3D(),
        //                       elbowRightJoint.GetPosition3D(),
        //                       wristRightJoint.GetPosition3D());
        //normalized2 = GetDirection(elbowRightJoint.GetPosition3D(),
        //                           shoulderRightJoint.GetPosition3D());
        //UpdateJointRotation(JointType.ShoulderRight,
        //                    Quaternion.LookRotation(normalized2, normalized)
        //                    * Quaternion.Euler(180f, 90f, 0f));

        //ElbowRight
        //normalized2 = GetDirection(wristRightJoint.GetPosition3D(),
        //                           elbowRightJoint.GetPosition3D());
        //UpdateJointRotation(JointType.ElbowRight,
        //                    Quaternion.LookRotation(normalized2, normalized)
        //                    * Quaternion.Euler(180f, 90f, 0f));

        //ShoulderLeft
        //normalized = GetNormal(shoulderLeftJoint.GetPosition3D(),
        //                       elbowLeftJoint.GetPosition3D(),
        //                       wristLeftJoint.GetPosition3D());
        //normalized2 = GetDirection(elbowLeftJoint.GetPosition3D(),
        //                           shoulderLeftJoint.GetPosition3D());
        //UpdateJointRotation(JointType.ShoulderLeft,
        //                    Quaternion.LookRotation(normalized2, normalized)
        //                    * Quaternion.Euler(0f, -90f, 0f));

        //ElbowLeft
        //normalized2 = GetDirection(wristLeftJoint.GetPosition3D(),
        //                           elbowLeftJoint.GetPosition3D());
        //UpdateJointRotation(JointType.ElbowLeft,
        //                    Quaternion.LookRotation(normalized2, normalized)
        //                    * Quaternion.Euler(0f, -90f, 0f));

        //Pelvis
        Vector3 normalized = GetNormal(waitJoint.GetPosition3D(),
                               pelvisJoint.GetPosition3D(),
                               hipRightJoint.GetPosition3D());
        Vector3 normalized2 = GetDirection(waitJoint.GetPosition3D(),
                                   pelvisJoint.GetPosition3D());
        UpdateJointRotation(JointType.Pelvis,
                            Quaternion.LookRotation(normalized2, normalized)
                            * Quaternion.Euler(-90f, 0f, 0f));

        //HipRight
        //normalized = ((bigToeRightJoint.GetPosition3D() - ankleRightJoint.GetPosition3D()).Length()
        //    < (ankleRightJoint.GetPosition3D() - kneeRightJoint.GetPosition3D()).Length()) ?
        //    GetNormal(bigToeRightJoint.GetPosition3D(),
        //              ankleRightJoint.GetPosition3D(),
        //              kneeRightJoint.GetPosition3D())
        //    : GetNormal(hipRightJoint.GetPosition3D(),
        //                kneeRightJoint.GetPosition3D(),
        //                ankleRightJoint.GetPosition3D());
        //normalized2 = GetDirection(kneeRightJoint.GetPosition3D(),
        //                           hipRightJoint.GetPosition3D());
        //UpdateJointRotation(JointType.HipRight,
        //                    Quaternion.LookRotation(normalized2, normalized)
        //                    * Quaternion.Euler(0f, 90f, 90f));

        //KneeRight
        //normalized2 = GetDirection(ankleRightJoint.GetPosition3D(),
        //                           kneeRightJoint.GetPosition3D());
        //UpdateJointRotation(JointType.KneeRight,
        //                    Quaternion.LookRotation(normalized2, normalized)
        //                    * Quaternion.Euler(0f, 90f, 90f));

        //AnkleRight
        //normalized2 = GetDirection(bigToeRightJoint.GetPosition3D(),
        //                           ankleRightJoint.GetPosition3D());
        //UpdateJointRotation(JointType.AnkleRight,
        //                    Quaternion.LookRotation(normalized2, normalized)
        //                    * Quaternion.Euler(0f, 150f, 90f));

        //HipLeft
        //normalized = ((bigToeLeftJoint.GetPosition3D() - ankleLeftJoint.GetPosition3D()).Length()
        //    < (ankleLeftJoint.GetPosition3D() - kneeLeftJoint.GetPosition3D()).Length())
        //    ? GetNormal(bigToeLeftJoint.GetPosition3D(),
        //              ankleLeftJoint.GetPosition3D(),
        //              kneeLeftJoint.GetPosition3D())
        //    : GetNormal(hipLeftJoint.GetPosition3D(),
        //                kneeLeftJoint.GetPosition3D(),
        //                ankleLeftJoint.GetPosition3D());
        //normalized2 = GetDirection(kneeLeftJoint.GetPosition3D(),
        //                           hipLeftJoint.GetPosition3D());
        //UpdateJointRotation(JointType.HipLeft,
        //                    Quaternion.LookRotation(normalized2, normalized)
        //                    * Quaternion.Euler(0f, 90f, 90f));

        //KneeLeft
        //normalized2 = GetDirection(ankleLeftJoint.GetPosition3D(),
        //                           kneeLeftJoint.GetPosition3D());
        //UpdateJointRotation(JointType.KneeLeft,
        //                    Quaternion.LookRotation(normalized2, normalized)
        //                    * Quaternion.Euler(0f, 90f, 90f));

        //AnkleLeft
        //normalized2 = GetDirection(bigToeLeftJoint.GetPosition3D(),
        //                           ankleLeftJoint.GetPosition3D());
        //UpdateJointRotation(JointType.AnkleLeft,
        //                    Quaternion.LookRotation(normalized2, normalized)
        //                    * Quaternion.Euler(0f, 150f, 90f));
    }

    public Vector3 Normal(Vector3 vector1, Vector3 vector2, Vector3 vector3)
    {
        return Cross(vector2 - vector1, vector3 - vector1);
    }

    public Vector3 Cross(Vector3 vector1, Vector3 vector2)
    {
        return new Vector3(vector1.y * vector2.z - vector1.z * vector2.y, vector1.z * vector2.x - vector1.x * vector2.z, vector1.x * vector2.y - vector1.y * vector2.x);
    }

    private void UpdateJointRotation(JointType jointType, Quaternion newRotation)
    {
        if (!_updates[jointType] || !Constants.JointToBone.ContainsKey(jointType))
        {
            return;
        }

        HumanBodyBones key = Constants.JointToBone[jointType];
        if (_bones.ContainsKey(key))
        {
            Quaternion val = newRotation;
            if (_rotate)
            {
                Vector3 eulerAngles = newRotation.eulerAngles;
                eulerAngles += transform.eulerAngles;
                val = Quaternion.Euler(eulerAngles);
            }
            _bones[key].UpdateRotation(val, _smoothing);
        }
    }

    protected void CalculateScale(BodyFrame body)
    {
        if (_matchScale)
        {
            Joint joint = body.Joints[(int)JointType.ElbowRight];
            Joint joint2 = body.Joints[(int)JointType.ShoulderRight];
            Vector3 val = joint.GetPosition3D() - joint2.GetPosition3D();
            BoneData bone = GetBone((HumanBodyBones)16);
            BoneData bone2 = GetBone((HumanBodyBones)14);
            Vector3 val2 = bone.OriginalPosition - bone2.OriginalPosition;
            float newScale = val.magnitude / val2.magnitude;
            ApplyScaleAtBody(newScale);
        }
    }

    public float GetLength(Vector3 pos)
    {
        return (float)Math.Sqrt(pos.x * pos.x + pos.y * pos.y + pos.z * pos.z);
    }

    public BoneData GetBone(HumanBodyBones humanBodyBone)
    {
        if (_bones != null && _bones.ContainsKey(humanBodyBone))
        {
            return _bones[humanBodyBone];
        }
        return null;
    }

    public void Load(BodyFrame body)
    {
        if (body != null && _updatePose)
        {
            if (!_isInitialized)
            {
                Initialize();
            }
            CalculateOrientations(body);
            CalculateScale(body);
            UpdateRootPosition(body);
        }
    }

    public void Reset()
    {
        foreach (KeyValuePair<HumanBodyBones, BoneData> bone in _bones)
        {
            bone.Value.Reset();
        }
    }

    private bool IsVisible(BodyFrame body)
    {
        if (body == null)
        {
            return false;
        }
        Joint joint = body.Joints[(int)JointType.Neck];
        Joint joint2 = body.Joints[(int)JointType.Pelvis];
        Joint joint3 = body.Joints[(int)JointType.EarLeft];
        Joint joint4 = body.Joints[(int)JointType.EarRight];
        Joint joint5 = body.Joints[(int)JointType.KneeLeft];
        Joint joint6 = body.Joints[(int)JointType.KneeRight];
        if (joint.Value.Confidence > 0f && joint2.Value.Confidence > 0f && (joint3.Value.Confidence > 0f || joint4.Value.Confidence > 0f))
        {
            if (!(joint5.Value.Confidence > 0f))
            {
                return joint6.Value.Confidence > 0f;
            }
            return true;
        }
        return false;
    }

    private void UpdateRootPosition(BodyFrame body)
    {
        if (body != null)
        {
            Vector3 position3D = body.Joints[(int)JointType.Pelvis].GetPosition3D();
            if (_useWorldPosition)
            {
                PositionBonesAtPoint(new Vector3(position3D.x, 0f - position3D.y, position3D.z));
            }
            else
            {
                _model.transform.position = new Vector3(_originalPosition.x, _originalPosition.y - position3D.y, _originalPosition.z);
            }
        }
    }

    public bool PositionBonesAtPoint(Vector3 point)
    {
        //IL_001b: Unknown result type (might be due to invalid IL or missing references)
        //IL_002a: Unknown result type (might be due to invalid IL or missing references)
        if (_bones == null)
        {
            return false;
        }
        Transform transform = _bones[Constants.JointToBone[_highestHierarchyBone]].Transform;
        Vector3 localPos = transform.InverseTransformPoint(point);
        transform.localPosition = localPos;
        //_bones[Constants.JointToBone[_highestHierarchyBone]].Transform.position = point;
        return true;
    }

    public void PositionAt(Vector3 point)
    {
        _model.transform.position = (point);
    }

    public void ApplyScaleAtBody(float newScale)
    {
        _model.transform.localScale = (new Vector3(newScale, newScale, newScale));
    }

    public void ApplyScaleAtBones(float newScale)
    {
        if (_bones != null)
        {
            _bones[Constants.JointToBone[_highestHierarchyBone]].Transform.localScale = (new Vector3(newScale, newScale, newScale));
        }
    }

    protected JointType FindHighestHieararchyBone()
    {
        HumanBodyBones val = 0;
        int num = int.MaxValue;
        foreach (KeyValuePair<HumanBodyBones, BoneData> bone in _bones)
        {
            int num2 = CountParents(bone.Value.Transform);
            if (num2 < num)
            {
                val = bone.Key;
                num = num2;
            }
        }
        JointType result = JointType.Pelvis;
        foreach (KeyValuePair<JointType, HumanBodyBones> item in Constants.JointToBone)
        {
            if (item.Value == val)
            {
                return item.Key;
            }
        }
        return result;
    }

    private int CountParents(Transform transform)
    {
        int num = 0;
        Transform val = transform;
        while (val.parent != null)
        {
            val = val.parent;
            num++;
        }
        return num;
    }
}






using System.Collections.Generic;
using UnityEngine;

public static class TPose
{
    public static void DoTPose(Dictionary<HumanBodyBones, BoneData> bones)
    {
        SetHorizontalJointTPose(bones[Constants.JointToBone[JointType.ClavicleLeft]].Transform, bones[Constants.JointToBone[JointType.ShoulderLeft]].Transform.position, left: true);
        SetHorizontalJointTPose(bones[Constants.JointToBone[JointType.ShoulderLeft]].Transform, bones[Constants.JointToBone[JointType.ElbowLeft]].Transform.position, left: true);
        SetHorizontalJointTPose(bones[Constants.JointToBone[JointType.ElbowLeft]].Transform, bones[Constants.JointToBone[JointType.WristLeft]].Transform.position, left: true);
        SetHorizontalJointTPose(bones[Constants.JointToBone[JointType.WristLeft]].Transform, bones[Constants.JointToBone[JointType.ElbowLeft]].Transform, left: true);
        SetVerticalJointTPose(bones[Constants.JointToBone[JointType.HipLeft]].Transform, bones[Constants.JointToBone[JointType.KneeLeft]].Transform.position);
        SetVerticalJointTPose(bones[Constants.JointToBone[JointType.KneeLeft]].Transform, bones[Constants.JointToBone[JointType.AnkleLeft]].Transform.position);
        SetHorizontalJointTPose(bones[Constants.JointToBone[JointType.ClavicleRight]].Transform, bones[Constants.JointToBone[JointType.ShoulderRight]].Transform.position, left: false);
        SetHorizontalJointTPose(bones[Constants.JointToBone[JointType.ShoulderRight]].Transform, bones[Constants.JointToBone[JointType.ElbowRight]].Transform.position, left: false);
        SetHorizontalJointTPose(bones[Constants.JointToBone[JointType.ElbowRight]].Transform, bones[Constants.JointToBone[JointType.WristRight]].Transform.position, left: false);
        SetHorizontalJointTPose(bones[Constants.JointToBone[JointType.WristRight]].Transform, bones[Constants.JointToBone[JointType.ElbowRight]].Transform, left: false);
        SetVerticalJointTPose(bones[Constants.JointToBone[JointType.HipRight]].Transform, bones[Constants.JointToBone[JointType.KneeRight]].Transform.position);
        SetVerticalJointTPose(bones[Constants.JointToBone[JointType.KneeRight]].Transform, bones[Constants.JointToBone[JointType.AnkleRight]].Transform.position);
    }

    public static void SetHorizontalJointTPose(Transform joint1, Vector3 joint2Position, bool left)
    {
        ForceHorizontalOrientation(joint1, joint2Position, left);
        ForceDepthOrientation(joint1, joint2Position, left);
    }

    public static void SetHorizontalJointTPose(Transform joint1, Transform inverseExtensionJoint, bool left)
    {
        Vector3 position = joint1.position;
        Vector3 val = joint1.position - inverseExtensionJoint.position;
        SetHorizontalJointTPose(joint1, position + val.normalized, left);
    }

    private static void ForceHorizontalOrientation(Transform joint1, Vector3 joint2Position, bool left)
    {
        Vector3 right = joint1.right;
        Vector3 up = joint1.up;
        Vector3 forward = joint1.forward;
        Vector3 val = joint2Position - joint1.position;
        Vector3 normalized = val.normalized;
        Vector3 bestAxis = GetBestAxis(right, up, forward, left ? Vector3.back : Vector3.forward);
        float num = Vector3.Angle(new Vector3(normalized.x, normalized.y, 0f), left ? Vector3.left : Vector3.right);
        if (Vector3.Dot(new Vector3(normalized.x, normalized.y, 0f), Vector3.up) > 0f)
        {
            num = 0f - num;
        }
        Quaternion rotation = joint1.rotation;
        Vector3 eulerAngles = rotation.eulerAngles;
        joint1.localRotation *= Quaternion.Euler(bestAxis.x * num, bestAxis.y * num, bestAxis.z * num);
        rotation = joint1.rotation;
        Vector3 eulerAngles2 = rotation.eulerAngles;
        Vector3 val2 = new(GetAngleOffset(eulerAngles.x, eulerAngles2.x), GetAngleOffset(eulerAngles.y, eulerAngles2.y), GetAngleOffset(eulerAngles.z, eulerAngles2.z));
        if (val2.x > 90f)
        {
            eulerAngles2.x = eulerAngles.x;
        }
        if (val2.y > 90f)
        {
            eulerAngles2.y = eulerAngles.y;
        }
        if (val2.z > 90f)
        {
            eulerAngles2.z = eulerAngles.z;
        }
        joint1.eulerAngles = eulerAngles2;
    }

    private static void ForceDepthOrientation(Transform joint1, Vector3 joint2Position, bool left)
    {
        Vector3 right = joint1.right;
        Vector3 up = joint1.up;
        Vector3 forward = joint1.forward;
        Vector3 val = joint2Position - joint1.position;
        Vector3 normalized = val.normalized;
        Vector3 bestAxis = GetBestAxis(right, up, forward, normalized);
        Vector3 bestAxis2 = GetBestAxis(right, up, forward, left ? Vector3.back : Vector3.forward);
        Vector3 val2 = Vector3.Cross(bestAxis, bestAxis2);
        float num = Vector3.Angle(new Vector3(normalized.x, 0f, normalized.z), left ? Vector3.left : Vector3.right);
        if (Vector3.Dot(new Vector3(normalized.x, 0f, normalized.z), Vector3.forward) > 0f)
        {
            num = 0f - num;
        }
        if (left)
        {
            num = 0f - num;
        }
        Quaternion rotation = joint1.rotation;
        Vector3 eulerAngles = rotation.eulerAngles;
        joint1.localRotation *= Quaternion.Euler(val2.x * num, val2.y * num, val2.z * num);
        Vector3 eulerAngles2 = joint1.eulerAngles;
        Vector3 val3 = new(GetAngleOffset(eulerAngles.x, eulerAngles2.x), GetAngleOffset(eulerAngles.y, eulerAngles2.y), GetAngleOffset(eulerAngles.z, eulerAngles2.z));
        if (val3.x > 180f)
        {
            eulerAngles2.x = eulerAngles.x;
        }
        if (val3.y > 180f)
        {
            eulerAngles2.y = eulerAngles.y;
        }
        if (val3.z > 180f)
        {
            eulerAngles2.z = eulerAngles.z;
        }
        joint1.eulerAngles = eulerAngles2;
    }

    public static void SetVerticalJointTPose(Transform joint1, Vector3 joint2Position)
    {
        Vector3 right = joint1.right;
        Vector3 up = joint1.up;
        Vector3 forward = joint1.forward;
        Vector3 val = joint2Position - joint1.position;
        Vector3 normalized = val.normalized;
        Vector3 bestAxis = GetBestAxis(right, up, forward, Vector3.forward);
        float num = Vector3.Angle(new Vector3(normalized.x, normalized.y, 0f), Vector3.down);
        if (Vector3.Dot(new Vector3(normalized.x, normalized.y, 0f), Vector3.right) > 0f)
        {
            num = 0f - num;
        }
        joint1.localRotation *= Quaternion.Euler(bestAxis.x * num, bestAxis.y * num, bestAxis.z * num);
    }

    public static Vector3 GetBestAxis(Vector3 vX, Vector3 vY, Vector3 vZ, Vector3 value)
    {
        Vector3 val = new(Vector3.Dot(vX, value), Vector3.Dot(vY, value), Vector3.Dot(vZ, value));
        if (Mathf.Abs(val.x) < Mathf.Abs(val.y))
        {
            val.x = 0f;
        }
        else
        {
            val.y = 0f;
        }
        if (Mathf.Abs(val.x) < Mathf.Abs(val.z))
        {
            val.x = 0f;
        }
        else
        {
            val.z = 0f;
        }
        if (Mathf.Abs(val.y) < Mathf.Abs(val.z))
        {
            val.y = 0f;
        }
        else
        {
            val.z = 0f;
        }
        if (Mathf.Abs(val.x) > 0f)
        {
            val.x = (val.x > 0f) ? 1 : (-1);
        }
        if (Mathf.Abs(val.y) > 0f)
        {
            val.y = (val.y > 0f) ? 1 : (-1);
        }
        if (Mathf.Abs(val.z) > 0f)
        {
            val.z = (val.z > 0f) ? 1 : (-1);
        }
        return val;
    }

    private static Vector3 RoundEuler(Vector3 euler)
    {
        euler = new Vector3(FilterAngle(euler.x), FilterAngle(euler.y), FilterAngle(euler.z));
        return euler;
    }

    private static float FilterAngle(float angle)
    {
        return (!(angle < 45f)) ? ((angle < 135f) ? 90 : ((angle < 225f) ? 180 : ((angle < 315f) ? 270 : 0))) : 0;
    }

    private static float GetAngleOffset(float angle1, float angle2)
    {
        if (Mathf.Abs(angle1 - angle2) > 180f)
        {
            if (angle1 > 180f)
            {
                if (angle2 > 180f)
                {
                    return Mathf.Abs(360f - angle1 - (360f - angle2));
                }
                return Mathf.Abs(360f - angle1 - angle2);
            }
            if (angle2 > 180f)
            {
                return Mathf.Abs(angle1 - (360f - angle2));
            }
            return Mathf.Abs(angle1 - angle2);
        }
        return Mathf.Abs(angle1 - angle2);
    }
}

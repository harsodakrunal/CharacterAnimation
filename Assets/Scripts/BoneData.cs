using UnityEngine;

public class BoneData
{
    private Quaternion _previous;

    public Transform Transform { get; }

    public Quaternion OriginalRotation { get; private set; }

    public Vector3 OriginalPosition { get; private set; }

    public BoneData(Transform transform)
    {
        Transform = transform;
        CalibrateOriginalRotation();
    }

    public void CalibrateOriginalRotation()
    {
        OriginalRotation = Transform.rotation;
        OriginalPosition = Transform.position;
        _previous = Transform.rotation;
    }

    public void UpdateRotation(Quaternion newRotation, float smoothDelta)
    {
        smoothDelta = Mathf.Clamp01(smoothDelta);
        Quaternion quaternion = newRotation * OriginalRotation;
        Quaternion quaternion2 = _previous = Quaternion.Lerp(_previous, quaternion, smoothDelta);
        //Quaternion localRotation = Quaternion.Inverse(Transform.parent.rotation) * quaternion2; // Convert to local rotation
        //Transform.localRotation = localRotation;
        Transform.rotation = quaternion2;
        //Debug.Log(Transform.name + " - " + Transform.localRotation);
        //smoothDelta = Mathf.Clamp01(smoothDelta);

        //Debug.Log("LOCAL ROTATION :- " + localRotation);
        //Quaternion smoothedRotation = Quaternion.Lerp(Transform.localRotation, localRotation, smoothDelta);
        //Transform.localRotation = smoothedRotation;
    }

    public void UpdatePosition(Vector3 newPosition)
    {
        //IL_0006: Unknown result type (might be due to invalid IL or missing references)
        Transform.position = (newPosition);
    }

    public void Reset()
    {
        Transform.rotation = OriginalRotation;
    }
}



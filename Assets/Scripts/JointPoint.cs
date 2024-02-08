using UnityEngine;
using System;

public partial class CharacterAnimation
{
    [Serializable]
    public class JointPoint
    {
        public Vector2 Pos2D = new();
        public float score2D;

        public Vector3 Pos3D = new();
        public Vector3 Now3D = new();
        public Vector3 PrevPos3D = new();
        public float score3D;

        // Bones
        public Transform Transform = null;
        public Quaternion InitRotation;
        public Quaternion Inverse;

        public JointPoint Child = null;
    }
}

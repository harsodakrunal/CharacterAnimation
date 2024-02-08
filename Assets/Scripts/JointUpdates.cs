using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class JointUpdates
{
    [SerializeField]
    private bool _pelvis = true;

    [SerializeField]
    private bool _waist = true;

    [SerializeField]
    private bool _chest = true;

    [SerializeField]
    private bool _neck = true;

    [SerializeField]
    private bool _head = true;

    [SerializeField]
    private bool _shoulderRight = true;

    [SerializeField]
    private bool _elbowRight = true;

    [SerializeField]
    private bool _shoulderLeft = true;

    [SerializeField]
    private bool _elbowLeft = true;

    [SerializeField]
    private bool _hipRight = true;

    [SerializeField]
    private bool _kneeRight = true;

    [SerializeField]
    private bool _ankleRight = true;

    [SerializeField]
    private bool _hipLeft = true;

    [SerializeField]
    private bool _kneeLeft = true;

    [SerializeField]
    private bool _ankleLeft = true;

    private readonly Dictionary<JointType, bool> _updates = new Dictionary<JointType, bool>();

    /// <summary>
    /// Returns whether the specified joint type should update its rotation.
    /// </summary>
    public bool this[JointType type]
    {
        get
        {
            if (_updates.ContainsKey(type))
            {
                return _updates[type];
            }
            return false;
        }
        set
        {
            if (_updates.ContainsKey(type))
            {
                _updates[type] = value;
            }
        }
    }

    /// <summary>
    /// Initializes the joint dictionary.
    /// </summary>
    public JointUpdates()
    {
        _updates.Add(JointType.Pelvis, _pelvis);
        _updates.Add(JointType.Waist, _waist);
        _updates.Add(JointType.Chest, _chest);
        _updates.Add(JointType.Neck, _neck);
        _updates.Add(JointType.Head, _head);
        _updates.Add(JointType.ShoulderRight, _shoulderRight);
        _updates.Add(JointType.ElbowRight, _elbowRight);
        _updates.Add(JointType.ShoulderLeft, _shoulderLeft);
        _updates.Add(JointType.ElbowLeft, _elbowLeft);
        _updates.Add(JointType.HipRight, _hipRight);
        _updates.Add(JointType.KneeRight, _kneeRight);
        _updates.Add(JointType.AnkleRight, _ankleRight);
        _updates.Add(JointType.HipLeft, _hipLeft);
        _updates.Add(JointType.KneeLeft, _kneeLeft);
        _updates.Add(JointType.AnkleLeft, _ankleLeft);
    }

    /// <summary>
    /// Applies avatar updates.
    /// </summary>
    public void Validate()
    {
        _updates[JointType.Pelvis] = _pelvis;
        _updates[JointType.Waist] = _waist;
        _updates[JointType.Chest] = _chest;
        _updates[JointType.Neck] = _neck;
        _updates[JointType.Head] = _head;
        _updates[JointType.ShoulderRight] = _shoulderRight;
        _updates[JointType.ElbowRight] = _elbowRight;
        _updates[JointType.ShoulderLeft] = _shoulderLeft;
        _updates[JointType.ElbowLeft] = _elbowLeft;
        _updates[JointType.HipRight] = _hipRight;
        _updates[JointType.KneeRight] = _kneeRight;
        _updates[JointType.AnkleRight] = _ankleRight;
        _updates[JointType.HipLeft] = _hipLeft;
    }
}

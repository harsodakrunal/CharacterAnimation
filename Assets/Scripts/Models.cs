// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
using System.Collections.Generic;
using UnityEngine;

public class BodyFrame
{
    public double Confidence;
    public object HandLeft;
    public object HandRight;
    public int ID;
    public List<Joint> Joints;
    //public Dictionary<JointType, Joint> Joints { get; internal set; }
}

public class Joint
{
    public int Key;
    public Value Value;

    public Vector2 GetPosition2D()
    {
        return Value.GetPosition2D();
    }

    public Vector3 GetPosition3D()
    {
        return Value.GetPosition3D();
    }
}

public class Orientation
{
    public double W;
    public double X;
    public double Y;
    public double Z;
}

public class Position2D
{
    public double X;
    public double Y;
}

public class Position3D
{
    public double X;
    public double Y;
    public double Z;
}

public class AnimationData
{
    public List<BodyFrame> data;
}

public class Value
{
    public double Confidence;
    public Orientation Orientation;
    public Position2D Position2D;
    public Position3D Position3D;
    public int Type;

    public Vector2 GetPosition2D()
    {
        return new Vector2((float)Position2D.X, (float)Position2D.Y);
    }

    public Vector3 GetPosition3D()
    {
        return new Vector3((float)Position3D.X, (float)Position3D.Y, (float)Position3D.Z);
    }
}
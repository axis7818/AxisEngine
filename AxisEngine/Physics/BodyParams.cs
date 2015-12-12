namespace AxisEngine.Physics
{

    /* OLD METHOD - TRYING TO IMPLEMENT A SIMPLER PHYSICS SYSTEM WITH LESS PARAMETERS */
    /* BodyParameters MAY BE REMOVED ALL TOGETHER */
    //public struct BodyParams
    //{
    //    public float MaxInternalForce;
    //    public float FrictionCoefficient;
    //    public bool Static;

    //    private float _invMass;
    //    private float _mass;
        
    //    public BodyParams(float mass, bool isStatic, float maxInternalForce, float frictionCoefficient)
    //    {
    //        _mass = mass;
    //        _invMass = 1 / mass;
    //        Static = isStatic;
    //        MaxInternalForce = maxInternalForce;
    //        FrictionCoefficient = frictionCoefficient;
    //    }

    //    public float InvMass
    //    {
    //        get { return _invMass; }
    //    }

    //    public float Mass
    //    {
    //        get { return _mass; }
    //        set
    //        {
    //            _mass = value;
    //            _invMass = 1 / _mass;
    //        }
    //    }
    //}
}
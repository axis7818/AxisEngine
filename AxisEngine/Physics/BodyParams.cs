namespace AxisEngine.Physics
{
    public struct BodyParams
    {
        public float FrictionCoefficient;

        private float _invMass;

        private float _mass;

        public float InvMass
        {
            get
            {
                return _invMass;
            }
        }

        public float Mass
        {
            get
            {
                return _mass;
            }
            set
            {
                _mass = value;
                _invMass = 1 / _mass;
            }
        }

        public float MaxInternalForce;

        public bool Static;

        public BodyParams(float mass, bool isStatic, float maxInternalForce, float frictionCoefficient)
        {
            _mass = mass;
            _invMass = 1 / mass;
            Static = isStatic;
            MaxInternalForce = maxInternalForce;
            FrictionCoefficient = frictionCoefficient;
        }
    }
}
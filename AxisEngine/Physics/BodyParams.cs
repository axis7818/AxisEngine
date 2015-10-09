namespace AxisEngine.Physics
{
    /// <summary>
    /// A set of parameters that defines how an object interacts with the physical environment
    /// </summary>
    public struct BodyParams
    {
        /// <summary>
        /// The coefficient of friction for the object
        /// </summary>
        public float FrictionCoefficient;

        /// <summary>
        /// 1 / Mass
        /// </summary>
        private float _invMass;

        /// <summary>
        /// the mass of the body
        /// </summary>
        private float _mass;

        /// <summary>
        /// 1 / Mass
        /// </summary>
        public float InvMass
        {
            get
            {
                return _invMass;
            }
        }

        /// <summary>
        /// the mass of the body
        /// </summary>
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

        /// <summary>
        /// The maximum force that can be applied from within the object
        /// </summary>
        public float MaxInternalForce;

        /// <summary>
        /// whether or not the body should be static and not move
        /// </summary>
        public bool Static;

        /// <summary>
        /// Instantiates and initializes a BodyParams
        /// </summary>
        /// <param name="mass">the mass of the body</param>
        /// <param name="isStatic">whether or not the body should be static and not move</param>
        /// <param name="maxInternalForce">The maximum force that can be applied from within the object</param>
        /// <param name="frictionCoefficient">The coefficient of friction for the object</param>
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
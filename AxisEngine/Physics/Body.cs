using System;
using Microsoft.Xna.Framework;
using AxisEngine;

namespace AxisEngine.Physics
{
    public class Body : WorldObject
    {
        public bool Static = false;

        private WorldObject _baseObject;
        private Vector2 _velocity = Vector2.Zero;
        private Vector2 _targetVelocity = Vector2.Zero;
        private float _resistance = 0;
        
        public Body()
        {
            _baseObject = RootObject;
        }

        public float Resistance
        {
            get { return _resistance; }
            set
            {
                _resistance = AxisMath.Clamp(value, 0.0f, 1.0f);
            }
        }

        public Vector2 Velocity
        {
            get { return _velocity; }
        }

        public void Move(Vector2 velocity)
        {
            _targetVelocity = velocity;
        }

        protected override void UpdateThis(GameTime t)
        {
            if (!Static)
            {
                _velocity += (_targetVelocity - _velocity) * (1 - _resistance);
                _baseObject.Position += _velocity * (Layer != null ? Layer.TimeManager.TimeMultiplier : 1);
                _targetVelocity = Vector2.Zero;
            }
        }

        protected override void OnOwnerChanged(WorldObject owner)
        {
            base.OnOwnerChanged(owner);
            _baseObject = RootObject;
        }
    }

    /* OLD METHOD - WAS TOO COMPLEX/REALISTIC, TRYING TO IMPLEMENT SOMETHING SIMPLER AND ARCADEY */
    //public class Body : WorldObject
    //{
    //    public BodyParams Parameters;
    //    public Vector2 Velocity;

    //    private Vector2 ExternalForce;
    //    private Vector2 InternalForce;
    //    private WorldObject BaseObject;

    //    public Body(BodyParams parameters)
    //    {
    //        // set Properties
    //        Parameters = parameters;
    //        InternalForce = ExternalForce = Velocity = Vector2.Zero;

    //        OwnerChanged += OnOwnerChanged;
    //    }

    //    private float TimeMultiplier
    //    {
    //        get { return Layer != null ? Layer.TimeManager.TimeMultiplier : 1; }
    //    }

    //    public void AddExternalForce(Vector2 force)
    //    {
    //        ExternalForce += force;
    //    }

    //    public void AddInternalForce(Vector2 force)
    //    {
    //        InternalForce += force;

    //        // restrict the length of the vector
    //        if (InternalForce.Length() > Parameters.MaxInternalForce) 
    //            InternalForce = Vector2.Normalize(InternalForce) * Parameters.MaxInternalForce; 
    //    }

    //    protected override void UpdateThis(GameTime t)
    //    {
    //        if (!Parameters.Static)
    //        {
    //            // get the new velocity
    //            float time = (float)t.ElapsedGameTime.TotalSeconds;
    //            Vector2 netForce = InternalForce + ExternalForce;
    //            Velocity += (time * Parameters.InvMass) * netForce;

    //            // apply friction
    //            if (Velocity.LengthSquared() > 0)
    //            {
    //                Vector2 friction = -Vector2.Normalize(Velocity) * Parameters.Mass * 9.81f * Parameters.FrictionCoefficient;
    //                Vector2 frictionDeltaV = friction * (time * Parameters.InvMass);
    //                if (frictionDeltaV.LengthSquared() <= Velocity.LengthSquared())
    //                {
    //                    Velocity += frictionDeltaV;
    //                }
    //                else
    //                {
    //                    // stop the body if the change in velocity is greater than the velocity itself
    //                    Velocity = Vector2.Zero;
    //                }
    //            }

    //            // clear the forces
    //            InternalForce = ExternalForce = Vector2.Zero;

    //            // apply the velocity
    //            BaseObject.Position += Velocity * TimeMultiplier;
    //        }
    //    }

    //    private void OnOwnerChanged(object sender, WorldObjectEventArgs args)
    //    {
    //        BaseObject = RootObject;
    //    }
    //}
}
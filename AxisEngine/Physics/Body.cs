using Microsoft.Xna.Framework;

namespace AxisEngine.Physics
{
    public class Body : WorldObject
    {
        public BodyParams Parameters;

        public Vector2 Velocity;

        private Vector2 ExternalForce;

        private Vector2 InternalForce;

        private WorldObject BaseObject;

        private float TimeMultiplier
        {
            get
            {
                return Layer != null ? Layer.TimeManager.TimeMultiplier : 1;
            }
        }

        public Body(BodyParams parameters)
        {
            // set Properties
            Parameters = parameters;
            InternalForce = ExternalForce = Velocity = Vector2.Zero;

            base.OwnerChanged += OnOwnerChanged;
        }

        public void AddExternalForce(Vector2 force)
        {
            ExternalForce += force;
        }

        public void AddInternalForce(Vector2 force)
        {
            InternalForce += force;

            // restrict the length of the vector
            if (InternalForce.Length() > Parameters.MaxInternalForce)
            {
                InternalForce = Vector2.Normalize(InternalForce) * Parameters.MaxInternalForce;
            }
        }

        public override void UpdateThis(GameTime t)
        {
            if (!Parameters.Static)
            {
                // get the new velocity
                float time = (float)t.ElapsedGameTime.TotalSeconds;
                Vector2 netForce = InternalForce + ExternalForce;
                Velocity += (time * Parameters.InvMass) * netForce;

                // apply friction
                if (Velocity.LengthSquared() > 0)
                {
                    Vector2 friction = -Vector2.Normalize(Velocity) * Parameters.Mass * 9.81f * Parameters.FrictionCoefficient;
                    Vector2 frictionDeltaV = friction * (time * Parameters.InvMass);
                    if (frictionDeltaV.LengthSquared() <= Velocity.LengthSquared())
                    {
                        Velocity += frictionDeltaV;
                    }
                    else
                    {
                        // stop the body if the change in velocity is greater than the velocity itself
                        Velocity = Vector2.Zero;
                    }
                }

                // clear the forces
                InternalForce = ExternalForce = Vector2.Zero;

                // apply the velocity
                BaseObject.Position += Velocity * TimeMultiplier;
            }

            base.UpdateThis(t);
        }

        private void OnOwnerChanged(object sender, WorldObjectEventArgs args)
        {
            BaseObject = RootObject;
        }
    }
}
using Microsoft.Xna.Framework;
using System;

namespace AxisEngine.Physics
{
    /// <summary>
    /// Provides a method of interaction with the physical world
    /// </summary>
    public class Body : WorldObject
    {
        /// <summary>
        /// A set of physical parameters that determine the behavior of the Body
        /// </summary>
        public BodyParams Parameters;

        /// <summary>
        /// The velocity of the object
        /// </summary>
        public Vector2 Velocity;

        /// <summary>
        /// A force that is applied from an external source
        /// </summary>
        private Vector2 ExternalForce;

        /// <summary>
        /// A force that is applied from within the object
        /// </summary>
        private Vector2 InternalForce;

        /// <summary>
        /// A reference to the root WorldObject
        /// </summary>
        private WorldObject BaseObject;

        /// <summary>
        /// The time scale
        /// </summary>
        private float TimeMultiplier
        {
            get
            {
                return Layer != null ? Layer.TimeManager.TimeMultiplier : 1;
            }
        }

        /// <summary>
        /// initializes a new Body
        /// </summary>
        /// <param name="parameters">A set of physical properties that determine the body's behavior</param>
        public Body(BodyParams parameters)
        {
            // set Properties
            Parameters = parameters;
            InternalForce = ExternalForce = Velocity = Vector2.Zero;
            
            base.OwnerChanged += OnOwnerChanged;
        }

        /// <summary>
        /// Adds a force to the object. This force comes from outside the object and doesn't have a limit
        /// </summary>
        /// <param name="force">the force to add</param>
        public void AddExternalForce(Vector2 force)
        {
            ExternalForce += force;
        }

        /// <summary>
        /// Adds a force to the object. This force has a maximum limit (set in the Parameters)
        /// </summary>
        /// <remarks>IF THIS ISN'T WORKING, CHECK TO MAKE SURE THAT THE FORCE IS STRONG ENOUGH TO OVERCOME FRICTION</remarks>
        /// <param name="force">The force to add</param>
        public void AddInternalForce(Vector2 force)
        {
            InternalForce += force;

            // restrict the length of the vector
            if (InternalForce.Length() > Parameters.MaxInternalForce)
            {
                InternalForce = Vector2.Normalize(InternalForce) * Parameters.MaxInternalForce;
            }
        }

        /// <summary>
        /// Updates the body
        /// </summary>
        /// <param name="t">the amount of time that has passed since the last update</param>
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

        /// <summary>
        /// Handles functionality when the owner of the Body has changed
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="args">Arguments associated with the event</param>
        private void OnOwnerChanged(object sender, WorldObjectEventArgs args)
        {
            BaseObject = RootObject;
        }
    }
}
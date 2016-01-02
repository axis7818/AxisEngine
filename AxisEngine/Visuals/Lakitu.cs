using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxisEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AxisEngine.Visuals
{
    /// <summary>
    /// A WorldObject that holds a Camera. This gives a Camera all of the functionality of a WorldObject and adds additional functionality.
    /// </summary>
    public class Lakitu : WorldObject
    {
        private Camera camera;
        
        private bool _busy = false;
        
        // panning
        private bool _panning = false;
        private float _panTimer;
        private Vector2 panStart;
        private Vector2 panEnd;
        private float panTime;

        // zooming
        private bool _zooming = false;
        private float _zoomTimer;
        private float zoomStart;
        private float zoomEnd;
        private float zoomTime;

        public Lakitu(Camera camera)
        {
            this.camera = camera;
        }

        public event EventHandler<EventArgs> OnBusyStart;
        public event EventHandler<EventArgs> OnBusyEnd;

        public bool Busy
        {
            get { return _busy; }
            private set
            {
                if (value && !_busy && OnBusyStart != null)
                    OnBusyStart(this, EventArgs.Empty);
                if (!value && _busy && OnBusyEnd != null)
                    OnBusyEnd(this, EventArgs.Empty);
                _busy = value;
            }
        }

        public bool Panning
        {
            get { return _panning; }
            private set
            {
                _panning = value;
                if (_panning)
                    Busy = true;
                else if(!Zooming)
                    Busy = false;
            }
        }

        public bool Zooming
        {
            get { return _zooming; }
            private set
            {
                _zooming = value;
                if (_zooming)
                    Busy = true;
                else if(!Panning)
                    Busy = false;
            }
        }

        public Camera Camera
        {
            get { return camera; }
        }

        protected override void UpdateThis(GameTime t)
        {
            if (_panning)
            {
                // increment the camera position
                _panTimer += t.ElapsedGameTime.Milliseconds;
                float progress = AxisMath.Clamp(_panTimer / panTime, 0, 1);
                Vector2 delta = (panEnd - panStart) * progress;
                Camera.Position = panStart + delta;

                // check for panning complete
                if (progress >= 1)
                    Panning = false;
            }

            if (_zooming)
            {
                // increment zoom
                _zoomTimer += t.ElapsedGameTime.Milliseconds;
                float progress = AxisMath.Clamp(_zoomTimer / zoomTime, 0, 1);
                float delta = (zoomEnd - zoomStart) * progress;
                Camera.Zoom = zoomStart + delta;

                // check for zooming complete
                if (progress >= 1)
                    Zooming = false;
            }
        }
        
        public void PanCamera(Vector2 start, Vector2 end, float time)
        {
            if (time <= 0)
                throw new Exception("the time for panning must be positive.");

            Panning = true;
            _panTimer = 0;
            panStart = start;
            panEnd = end;
            panTime = time;
        }

        public void ZoomCamera(float start, float end, float time)
        {
            if (time <= 0)
                throw new Exception("the time for zooming must be positive");

            Zooming = true;
            _zoomTimer = 0;
            zoomStart = start;
            zoomEnd = end;
            zoomTime = time;
        }

        public void FadeIn(float time)
        {
            // TODO: implement
        }

        public void FadeOut(float time)
        {
            // TODO: implement
        }
    }
}

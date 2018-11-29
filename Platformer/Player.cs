using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.DebugView;
using System.Collections.Generic;
using System;
using XELibrary;
using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics.Joints;

namespace Platformer
{
    class Player
    {
        private DrawablePhysicsObject torso;
        private DrawablePhysicsObject wheel;
        private RevoluteJoint axis;
        float speed = 3.0f;
       static public DrawablePhysicsObject _torso;
       static public DrawablePhysicsObject _wheel;

        public Player(World world, Texture2D torsoTexture, Texture2D wheelTexture, Vector2 size, float mass, Vector2 startPosition)
        {

            Vector2 torsoSize = new Vector2(size.X, size.Y - size.X / 2.0f);
            float wheelSize = size.X;

            // Create the torso
          //  torso = new DrawablePhysicsObject(world, torsoTexture, torsoSize, mass / 2.0f);
            torso.Position =  startPosition;
            _torso = torso;


            // Create the feet of the body
           // wheel = new DrawablePhysicsObject(world, wheelTexture, wheelSize, mass / 2.0f);
            wheel.Position = torso.Position + new Vector2(0, torsoSize.Y / 2.0f);
            wheel.body.Friction = 0.8f;
            _wheel = wheel;

            // Create a joint to keep the torso upright
            Body word_axle = BodyFactory.CreateCircle(world, 0.1f, 1f);
            //JointFactory.CreateFixedAngleJoint(world, torso.body);
            JointFactory.CreateAngleJoint(world, torso.body, word_axle);

            // Connect the feet to the torso
            axis = JointFactory.CreateRevoluteJoint(world, torso.body, wheel.body, Vector2.Zero);
            axis.CollideConnected = false;

            axis.MotorEnabled = true;
            axis.MotorSpeed = 0;
            axis.MotorImpulse = 3;
            axis.MaxMotorTorque = 10;
        }

        public enum Movement
        {
            Left,
            Right,
            Stop
        }

        public void Move(Movement movement)
        {
            switch (movement)
            {
                case Movement.Left:
                    axis.MotorSpeed = -MathHelper.TwoPi * speed;
                    break;

                case Movement.Right:
                    axis.MotorSpeed = MathHelper.TwoPi * speed;
                    break;

                case Movement.Stop:
                    axis.MotorSpeed = 0;
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            torso.Draw(spriteBatch);
            wheel.Draw(spriteBatch);
        }
    }
}

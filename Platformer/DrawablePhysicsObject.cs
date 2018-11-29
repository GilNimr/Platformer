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
    public class DrawablePhysicsObject
    {
        

        public Body body;
        public Vector2 Position
        {
            get { return ConvertUnits.ToDisplayUnits(body.Position); }
            set { body.Position = ConvertUnits.ToSimUnits(value); }
        }

        public Texture2D texture;

        private Vector2 size;
        public Vector2 Size
        {
            get { return ConvertUnits.ToDisplayUnits( size) ; }
            set { size = ConvertUnits.ToSimUnits( value) ; }
        }

        public DrawablePhysicsObject(Body _body, Texture2D texture, Vector2 size)
        {
            body = _body;
          
            this.Size = size;
            this.texture = texture;
        }

       

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destination = new Rectangle
            (
                (int)Camera2D.ConvertWorldToScreen(body.Position).X,
                (int)Camera2D.ConvertWorldToScreen(body.Position).Y,
                (int)Size.X,
                (int)Size.Y
            );
            
            spriteBatch.Draw(texture, destination, null, Color.White, body.Rotation, new Vector2(texture.Width / 2.0f, texture.Height / 2.0f), SpriteEffects.None, 0);
        }
    }
}


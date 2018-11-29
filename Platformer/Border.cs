using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace Platformer
{
    class Border
    {
        private World world;
        private Body anchor;

        public Border(World world, float width, float height, float borderWidth)
        {
            this.world = world;

            CreateBorder(width, height, borderWidth);
        }

        private void CreateBorder(float width, float height, float borderWidth)
        {
            width = Math.Abs(width);
            height = Math.Abs(height);

            anchor = new Body(world);
            List<Vertices> borders = new List<Vertices>(4);

            // Bottom
            borders.Add(PolygonTools.CreateRectangle(width , borderWidth, new Vector2(0.0f, height), 0));
            
            // Top
            borders.Add(PolygonTools.CreateRectangle(width, borderWidth, new Vector2(0.0f, -height), 0));
            
            // Right
            borders.Add(PolygonTools.CreateRectangle(borderWidth, height, new Vector2(width, 0.0f), 0));
            
            // Left
            borders.Add(PolygonTools.CreateRectangle(borderWidth, height, new Vector2(-width, 0.0f), 0));

            List<Fixture> fixtures = FixtureFactory.AttachCompoundPolygon(borders, 1.0f, anchor);

            foreach (Fixture fixture in fixtures)
            {
                fixture.CollidesWith = Category.All;
                fixture.CollisionCategories = Category.All;
            }

        }
    }
}

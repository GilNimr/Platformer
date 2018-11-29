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
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Physics
        private World world;
        private float dt = 0.001f;
        private const float G = 9.81f;
        private Vector2 gravity = new Vector2(0.0f, -G);


        Fixture player_fixture;
        DrawablePhysicsObject player;

        //platforms
        DrawablePhysicsObject Platform2;


        private bool paused = false;

        private List<Fixture> circles = new List<Fixture>();
        private List<Fixture> platforms = new List<Fixture>();

        
        

       

        // Camera
        private Camera2D camera;
        private Texture2D texture_player;
        private Texture2D paddle_texture;

        // Input
        private InputHandler input;

        // Debug view
        DebugViewXNA debugView;
        private double jumptime;
        private DrawablePhysicsObject Platform6;
        private Body _ground;
     

        private RevoluteJoint axis;
        private Vector2 gameWorld;
        private DrawablePhysicsObject Platform3;
        private DrawablePhysicsObject Platform4;
        private DrawablePhysicsObject Platform5;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.SynchronizeWithVerticalRetrace = false;
            graphics.PreferMultiSampling = true;
            IsFixedTimeStep = true;

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;

            Content.RootDirectory = "Content";

            input = new InputHandler(this);
            Components.Add(input);

            world = new World(gravity);
            debugView = new DebugViewXNA(world);

            

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
        
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

           camera = new Camera2D(GraphicsDevice);
            camera.MaxZoom = 100;
            camera.MinZoom = -100;
            camera.ZoomOut(0.2f);
            
            texture_player = Content.Load<Texture2D>("crate");
           paddle_texture = Content.Load<Texture2D>("paddle");
#if true


            player_fixture = FixtureFactory.AttachRectangle(4.0f , 4.0f, 10.0f, new Vector2(), new Body(world));
            player_fixture.Body.BodyType = BodyType.Dynamic;
            player_fixture.CollisionCategories = Category.Cat2;
            player_fixture.Body.Position = new Vector2(0f, 3.0f);

            player = new DrawablePhysicsObject(player_fixture.Body, texture_player, new Vector2(40.0f, 40.0f));
            
            //platforms.Add(player);
            
            /*Fixture c1;
            
            c1 = FixtureFactory.AttachCircle(2.0f, 10.0f, new Body(world, new Vector2(-8.0f, 3.0f)));
            c1.Body.BodyType = BodyType.Dynamic;
            c1.CollisionCategories = Category.Cat2;
            c1.Restitution = 0.4f;
            circles.Add(c1);*/
           

           
            Fixture p2;
            
                p2 = FixtureFactory.AttachRectangle(24.0f, 2.0f, 20.0f, new Vector2(), new Body(world));
                p2.Body.Position = new Vector2(30.0f, 7.0f);
                p2.Body.BodyType = BodyType.Dynamic;
                p2.CollisionCategories = Category.Cat3;
                p2.CollidesWith = Category.All & ~Category.Cat3;
                p2.Body.Friction = 0.8f;
                Platform2 = new DrawablePhysicsObject(p2.Body, paddle_texture, new Vector2(240.0f, 20.0f));

            var joint=  JointFactory.CreateRevoluteJoint(world, p2.Body, new Body(world), new Vector2(30.0f -12, 7.0f), new Vector2(30.0f, 7.0f),true);

                // rotate 1/4 of a circle per second
                joint.MotorSpeed = MathHelper.PiOver2;
                // have little torque (power) so it can push away a few blocks

                joint.MotorEnabled = true;
                joint.MaxMotorTorque = 100000;

                platforms.Add(p2);
                
            

            Fixture p3;

            p3 = FixtureFactory.AttachRectangle(24.0f, 2.0f, 5.0f, new Vector2(), new Body(world));
            p3.Body.Position = new Vector2(143.0f, 7.0f);
            p3.Body.BodyType = BodyType.Kinematic;
            p3.CollisionCategories = Category.Cat3;
            p3.CollidesWith = Category.All & ~Category.Cat3;

            Platform3 = new DrawablePhysicsObject(p3.Body, paddle_texture, new Vector2(240.0f, 20.0f));

            platforms.Add(p3);

            Fixture p4;

            p4 = FixtureFactory.AttachRectangle(24.0f, 2.0f, 5.0f, new Vector2(), new Body(world));
            p4.Body.Position = new Vector2(177.0f, 17.0f);
            p4.Body.BodyType = BodyType.Kinematic;
            p4.CollisionCategories = Category.Cat3;
            p4.CollidesWith = Category.All & ~Category.Cat3;

            Platform4 = new DrawablePhysicsObject(p4.Body, paddle_texture, new Vector2(240.0f, 20.0f));

            Fixture p5;

            p5 = FixtureFactory.AttachRectangle(24.0f, 2.0f, 5.0f, new Vector2(), new Body(world));
            p5.Body.Position = new Vector2(143.0f, 27.0f);
            p5.Body.BodyType = BodyType.Kinematic;
            p5.CollisionCategories = Category.Cat3;
            p5.CollidesWith = Category.All & ~Category.Cat3;

            Platform5 = new DrawablePhysicsObject(p5.Body, paddle_texture, new Vector2(240.0f, 20.0f));



            Fixture p6;

            p6 = FixtureFactory.AttachRectangle(10.0f, 2.0f, 5.0f, new Vector2(), new Body(world));
            p6.Body.Position = new Vector2(-22.0f, 45.0f);
            p6.Body.BodyType = BodyType.Kinematic;
            p6.CollisionCategories = Category.Cat3;
            p6.CollidesWith = Category.All & ~Category.Cat3;

            Platform6 = new DrawablePhysicsObject(p6.Body, paddle_texture, new Vector2(100.0f, 20.0f));



            // terrain
            _ground = BodyFactory.CreateBody(world);
            {
                Vertices terrain = new Vertices();

                terrain.Add(new Vector2(150f, 35f));
                terrain.Add(new Vector2(-20f, 35f));
                terrain.Add(new Vector2(-20f, 0f));
                terrain.Add(new Vector2(125f, 0f));

                for (int i = 0; i < terrain.Count - 1; ++i)
                {
                    FixtureFactory.AttachEdge(terrain[i], terrain[i + 1], _ground);
                }

                _ground.Friction = 1.2f;
                _ground.CollisionCategories = Category.Cat3;

                // terrain 2
               
                    Vertices terrain2 = new Vertices();
                terrain2.Add(new Vector2(-20f, 0f));
                terrain2.Add(new Vector2(-35f, 0f));
                terrain2.Add(new Vector2(-35f, 45f));
                    terrain2.Add(new Vector2(-100f, 35f));
               
                    for (int i = 0; i < terrain2.Count - 1; ++i)
                    {
                        FixtureFactory.AttachEdge(terrain2[i], terrain2[i + 1], _ground);
                    }

                Vertices ceiling = new Vertices();
                ceiling.Add(new Vector2(-1000f, 50f));
                ceiling.Add(new Vector2(75f, 50f));
                ceiling.Add(new Vector2(95f, 50f));
                ceiling.Add(new Vector2(1000f, 50f));
                FixtureFactory.AttachEdge(ceiling[0], ceiling[1], _ground);
                FixtureFactory.AttachEdge(ceiling[2], ceiling[3], _ground);
            }

            Fixture block;

            block = FixtureFactory.AttachRectangle(8.0f, 8.0f, 4.0f, new Vector2(70.0f, 40.0f), new Body(world));
            block.Body.BodyType = BodyType.Dynamic;
            block.Body.Restitution = 0.4f;
            block.CollisionCategories = Category.Cat2;

            block = FixtureFactory.AttachRectangle(8.0f, 8.0f, 4.0f, new Vector2(70.0f, 55.0f), new Body(world));
            block.Body.BodyType = BodyType.Dynamic;
            block.Body.Restitution = 0.4f;
            block.CollisionCategories = Category.Cat2;

            block = FixtureFactory.AttachRectangle(8.0f, 8.0f, 4.0f, new Vector2(65.0f, 55.0f), new Body(world));
            block.Body.BodyType = BodyType.Dynamic;
            block.Body.Restitution = 0.4f;
            block.CollisionCategories = Category.Cat2;

            block = FixtureFactory.AttachRectangle(8.0f, 8.0f, 4.0f, new Vector2(100.0f, 55.0f), new Body(world));
            block.Body.BodyType = BodyType.Dynamic;
            block.Body.Restitution = 0.4f;
            block.CollisionCategories = Category.Cat2;

            block = FixtureFactory.AttachRectangle(8.0f, 8.0f, 4.0f, new Vector2(105.0f, 55.0f), new Body(world));
            block.Body.BodyType = BodyType.Dynamic;
            block.Body.Restitution = 0.4f;
            block.CollisionCategories = Category.Cat2;

#endif

            gameWorld =
                Camera2D.ConvertScreenToWorld(new Vector2(camera.ScreenWidth,
                                                        camera.ScreenHeight));
            camera.TrackingBody = player_fixture.Body;
           // camera.Position = ConvertUnits.ToDisplayUnits( camera.Position) ;
          

            debugView.LoadContent(GraphicsDevice, Content);
            debugView.DefaultShapeColor = Color.White;
            debugView.SleepingShapeColor = Color.White;

            debugView.RemoveFlags(DebugViewFlags.Joint);
        }
        
    

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            HandleKeyboard(gameTime);
            camera.Update();
            

            if (!paused)
                world.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalMilliseconds * dt, (1f / 30f)));

            base.Update(gameTime);
        }
        private void HandleKeyboard(GameTime gameTime)
        {
            jumptime += gameTime.ElapsedGameTime.TotalSeconds;

            if (input.KeyboardHandler.IsKeyDown(Keys.Left))
            {
                player_fixture.Body.ApplyLinearImpulse(new Vector2(-100, 0));
               // circles[0].Body.ApplyLinearImpulse(new Vector2(-200, 0));
               // axis.MotorSpeed = -MathHelper.TwoPi * 300;
            }
            else if (input.KeyboardHandler.IsKeyDown(Keys.Right))
            {
                player_fixture.Body.ApplyLinearImpulse(new Vector2(100, 0));
               // circles[0].Body.ApplyLinearImpulse(new Vector2(200, 0));
               // axis.MotorSpeed = -MathHelper.TwoPi * 3;

            }
            else
               // axis.MotorSpeed = -MathHelper.TwoPi * 0;
            if (input.KeyboardHandler.WasKeyPressed(Keys.LeftControl)&&(player_fixture.Body.LinearVelocity.Y<=0.1f&& player_fixture.Body.LinearVelocity.Y >= -0.1f))
            {
                if (jumptime >= 0.1f)
                {
                    player_fixture.Body.ApplyLinearImpulse(new Vector2(0, 2500f));
                    // circles[0].Body.ApplyLinearImpulse(new Vector2(0,2000f));
                    jumptime = 0;
                }
            }
            if (!paused && input.KeyboardHandler.WasKeyPressed(Keys.Space))
            {
                Vector2 impulse = new Vector2(0, -100f);
                circles[0].Body.ApplyLinearImpulse(ref impulse);
            }

            if (paused && input.KeyboardHandler.IsHoldingKey(Keys.Left))
                world.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalMilliseconds * dt, (1f / 30f)));

            if (input.KeyboardHandler.WasKeyPressed(Keys.F1))
                ToggleFlag(DebugViewFlags.Shape);
            if (input.KeyboardHandler.WasKeyPressed(Keys.F2))
                ToggleFlag(DebugViewFlags.DebugPanel);
            if (input.KeyboardHandler.WasKeyPressed(Keys.F3))
                ToggleFlag(DebugViewFlags.PerformanceGraph);
            if (input.KeyboardHandler.WasKeyPressed(Keys.F4))
                ToggleFlag(DebugViewFlags.AABB);
            if (input.KeyboardHandler.WasKeyPressed(Keys.F5))
                ToggleFlag(DebugViewFlags.CenterOfMass);
            if (input.KeyboardHandler.WasKeyPressed(Keys.F6))
                ToggleFlag(DebugViewFlags.Joint);
            if (input.KeyboardHandler.WasKeyPressed(Keys.F7))
            {
                ToggleFlag(DebugViewFlags.ContactPoints);
                ToggleFlag(DebugViewFlags.ContactNormals);
            }
            if (input.KeyboardHandler.WasKeyPressed(Keys.F8))
                ToggleFlag(DebugViewFlags.PolygonPoints);
            if (input.KeyboardHandler.WasKeyPressed(Keys.P))
                paused = !paused;
            if (input.KeyboardHandler.WasKeyPressed(Keys.G))
            {
                if (world.Gravity.Equals(Vector2.Zero))
                    world.Gravity = new Vector2(0.0f, -G);
                else
                    world.Gravity = Vector2.Zero;
            }
        }

        private void ToggleFlag(DebugViewFlags flag)
        {
            if ((debugView.Flags & flag) == flag)
                debugView.RemoveFlags(flag);
            else
                debugView.AppendFlags(flag);
        }

        protected override void Draw(GameTime gameTime)
        {
            
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Camera2D.View.M12 *= -1;
            Camera2D.View.M22 *= -1;
            Camera2D.View.M32 *= -1;
            Camera2D.View.M42 *= -1;

            spriteBatch.Begin();
            
            player.Draw(spriteBatch);
            Platform2.Draw(spriteBatch);
            Platform3.Draw(spriteBatch);
            Platform4.Draw(spriteBatch);
            Platform5.Draw(spriteBatch);
            Platform6.Draw(spriteBatch);


            spriteBatch.End();

          


            debugView.RenderDebugData(ref Camera2D.Projection, ref Camera2D.View);
            //debugView.RenderDebugData(Matrix.Identity, Matrix.Identity);


            base.Draw(gameTime);
        }
    }
}

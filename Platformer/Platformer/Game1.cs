using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.DebugView;
using System;
using XELibrary;
using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics.Contacts;

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

        SpriteFont font;

        //the main player.
        Fixture player_fixture;
        DrawablePhysicsObject player;

        //finish button fixture for end of level.
        Fixture finish_button;

        //for drawing platforms
        private DrawablePhysicsObject Platform2;
        private DrawablePhysicsObject Platform3;
        private DrawablePhysicsObject Platform4;
        private DrawablePhysicsObject Platform5;
        private DrawablePhysicsObject Platform6;
        

        //for drawing blocks
        private DrawablePhysicsObject Draw_block1;
        private DrawablePhysicsObject Draw_block5;
        private DrawablePhysicsObject Draw_block2;
        private DrawablePhysicsObject Draw_block3;
        private DrawablePhysicsObject Draw_block4;

        private DrawablePhysicsObject Draw_finish_button;

        private Body _ground;

        private bool paused = false;

        // Camera
        private Camera2D camera;

        //textures
        private Texture2D texture_player;
        private Texture2D paddle_texture;
        private Texture2D bg_texture;

        // Input
        private InputHandler input;

        // Debug view
        DebugViewXNA debugView;

        //helps with charcters jump.
        private double jumptime;
        private bool level_is_finished;

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

            //adjusting the camera to match the drawing.
            camera.MaxZoom = 100;
            camera.MinZoom = -100;
            camera.ZoomOut(0.2f);

            //loading textures.
           texture_player = Content.Load<Texture2D>("crate");
           paddle_texture = Content.Load<Texture2D>("paddle");
           bg_texture = Content.Load<Texture2D>("video-game-background-blue_451fb1bpx__F0000");

            //loading fonts
            font = Content.Load<SpriteFont>("Font");
#if true

            //creating the player
            player_fixture = FixtureFactory.AttachRectangle(4.0f , 4.0f, 10.0f, new Vector2(), new Body(world));
            player_fixture.Body.BodyType = BodyType.Dynamic;
            player_fixture.CollisionCategories = Category.Cat2;
            player_fixture.Body.Position = new Vector2(-50f, 50.0f);
            player_fixture.CollidesWith = Category.All;
            player_fixture.OnCollision = collision_with_finish_point;
            
            player = new DrawablePhysicsObject(player_fixture.Body, texture_player, new Vector2(40.0f, 40.0f));
            
            //creating platform 2
            Fixture p2;
            
             p2 = FixtureFactory.AttachRectangle(24.0f, 2.0f, 20.0f, new Vector2(), new Body(world));
             p2.Body.Position = new Vector2(30.0f, 7.0f);
             p2.Body.BodyType = BodyType.Dynamic;
             p2.CollisionCategories = Category.Cat3;
             p2.CollidesWith = Category.All & ~Category.Cat3; //meaning this platform can collide with anything
                                                                //ecxept other category 3 fixtures.
             p2.Body.Friction = 0.8f;
             Platform2 = new DrawablePhysicsObject(p2.Body, paddle_texture, new Vector2(240.0f, 20.0f));

            //this is the joint that connects the platform to the world.
            var joint=  JointFactory.CreateRevoluteJoint(world, p2.Body, new Body(world), new Vector2(30.0f -12, 7.0f), new Vector2(30.0f, 7.0f),true);

            // rotate 1/4 of a circle per second
            joint.MotorSpeed = MathHelper.PiOver2;
            joint.MotorEnabled = true;
            joint.MaxMotorTorque = 100000; //motor speed.




            //creating platform 3
            Fixture p3;

            p3 = FixtureFactory.AttachRectangle(24.0f, 2.0f, 5.0f, new Vector2(), new Body(world));
            p3.Body.Position = new Vector2(143.0f, 7.0f);
            p3.Body.BodyType = BodyType.Kinematic;
            p3.CollisionCategories = Category.Cat3;
            p3.CollidesWith = Category.All & ~Category.Cat3;

            Platform3 = new DrawablePhysicsObject(p3.Body, paddle_texture, new Vector2(240.0f, 20.0f));


            //creating platform 4
            Fixture p4;

            p4 = FixtureFactory.AttachRectangle(24.0f, 2.0f, 5.0f, new Vector2(), new Body(world));
            p4.Body.Position = new Vector2(177.0f, 17.0f);
            p4.Body.BodyType = BodyType.Kinematic;
            p4.CollisionCategories = Category.Cat3;
            p4.CollidesWith = Category.All & ~Category.Cat3;

            Platform4 = new DrawablePhysicsObject(p4.Body, paddle_texture, new Vector2(240.0f, 20.0f));


            //creating platform 5
            Fixture p5;

            p5 = FixtureFactory.AttachRectangle(24.0f, 2.0f, 5.0f, new Vector2(), new Body(world));
            p5.Body.Position = new Vector2(143.0f, 27.0f);
            p5.Body.BodyType = BodyType.Kinematic;
            p5.CollisionCategories = Category.Cat3;
            p5.CollidesWith = Category.All & ~Category.Cat3;

            Platform5 = new DrawablePhysicsObject(p5.Body, paddle_texture, new Vector2(240.0f, 20.0f));


            //creating platform 6
            Fixture p6;

            p6 = FixtureFactory.AttachRectangle(10.0f, 2.0f, 5.0f, new Vector2(), new Body(world));
            p6.Body.Position = new Vector2(-22.0f, 45.0f);
            p6.Body.BodyType = BodyType.Kinematic;
            p6.CollisionCategories = Category.Cat3;
            p6.CollidesWith = Category.All & ~Category.Cat3;

            Platform6 = new DrawablePhysicsObject(p6.Body, paddle_texture, new Vector2(100.0f, 20.0f));



            // creating the ground, can only bein seen in debugview
            _ground = BodyFactory.CreateBody(world);
            {
                //terrain
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
                terrain2.Add(new Vector2(-150f, 35f));

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
            //creating block 1
            Fixture block1;

            block1 = FixtureFactory.AttachRectangle(8.0f, 8.0f, 4.0f, new Vector2(), new Body(world));
            block1.Body.Position = new Vector2(70.0f, 40.0f);
            block1.Body.BodyType = BodyType.Dynamic;
            block1.Body.Restitution = 0.4f;
            block1.CollisionCategories = Category.Cat2;
            Draw_block1 = new DrawablePhysicsObject(block1.Body, texture_player, new Vector2(80, 80));

            //creating block 2
            Fixture block2;

            block2 = FixtureFactory.AttachRectangle(8.0f, 8.0f, 4.0f, new Vector2(), new Body(world));
            block2.Body.Position = new Vector2(70.0f, 55.0f);
            block2.Body.BodyType = BodyType.Dynamic;
            block2.Body.Restitution = 0.4f;
            block2.CollisionCategories = Category.Cat2;
            Draw_block2 = new DrawablePhysicsObject(block2.Body, texture_player, new Vector2(80, 80));

            //creating block 3
            Fixture block3;

            block3 = FixtureFactory.AttachRectangle(8.0f, 8.0f, 4.0f, new Vector2(), new Body(world));
            block3.Body.Position = new Vector2(65.0f, 55.0f);
            block3.Body.BodyType = BodyType.Dynamic;
            block3.Body.Restitution = 0.4f;
            block3.CollisionCategories = Category.Cat2;
            Draw_block3 = new DrawablePhysicsObject(block3.Body, texture_player, new Vector2(80, 80));

            //creating block 4
            Fixture block4;

            block4 = FixtureFactory.AttachRectangle(8.0f, 8.0f, 4.0f, new Vector2(), new Body(world));
            block4.Body.Position = new Vector2(100.0f, 55.0f);
            block4.Body.BodyType = BodyType.Dynamic;
            block4.Body.Restitution = 0.4f;
            block4.CollisionCategories = Category.Cat2;
            Draw_block4 = new DrawablePhysicsObject(block4.Body, texture_player, new Vector2(80, 80));

            //creating block 5
            Fixture block5;

            block5 = FixtureFactory.AttachRectangle(8.0f, 8.0f, 4.0f, new Vector2(), new Body(world));
            block5.Body.Position = new Vector2(105.0f, 55.0f);
            block5.Body.BodyType = BodyType.Dynamic;
            block5.Body.Restitution = 0.4f;
            block5.CollisionCategories = Category.Cat2;
            Draw_block5 = new DrawablePhysicsObject(block5.Body, texture_player, new Vector2(80, 80));

            //creating block 5
            

            finish_button = FixtureFactory.AttachRectangle(0.2f, 0.2f, 4.0f, new Vector2(), new Body(world));
            finish_button.Body.Position = new Vector2(-115.0f, 36.0f);
            finish_button.Body.BodyType = BodyType.Static;
            finish_button.Body.Restitution = 0.4f;
            finish_button.CollisionCategories = Category.Cat2;
            Draw_finish_button = new DrawablePhysicsObject(block5.Body, texture_player, new Vector2(2, 2));
#endif

            //make the camera move after the charcter
            camera.TrackingBody = player_fixture.Body;
            
            debugView.LoadContent(GraphicsDevice, Content);
            debugView.DefaultShapeColor = Color.White;
            debugView.SleepingShapeColor = Color.White;

            debugView.RemoveFlags(DebugViewFlags.Joint);
        }

        private bool collision_with_finish_point(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB == finish_button)
            {
                level_is_finished = true;
                
            }
            return true;
        }



        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            
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

            HandleKeyboard(gameTime);
            camera.Update();

           

            if (!paused)
                world.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalMilliseconds * dt, (1f / 30f)));

            base.Update(gameTime);
        }

        private bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            throw new NotImplementedException();
        }

        private void HandleKeyboard(GameTime gameTime)
        {
            jumptime += gameTime.ElapsedGameTime.TotalSeconds;

            if (input.KeyboardHandler.IsKeyDown(Keys.Left))
            {
                player_fixture.Body.ApplyLinearImpulse(new Vector2(-100, 0));
               
            }
            else if (input.KeyboardHandler.IsKeyDown(Keys.Right))
            {
                player_fixture.Body.ApplyLinearImpulse(new Vector2(100, 0));
               
            }
            
            //charcter jump.
            if (input.KeyboardHandler.WasKeyPressed(Keys.LeftControl)&&(player_fixture.Body.LinearVelocity.Y<=0.1f&& player_fixture.Body.LinearVelocity.Y >= -0.1f))
            {
                if (jumptime >= 0.1f)
                {
                    player_fixture.Body.ApplyLinearImpulse(new Vector2(0, 2500f));
                    jumptime = 0;
                }
            }
           
            //debugView flag handeling
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

            spriteBatch.Begin();

            //drawing background
            spriteBatch.Draw(bg_texture, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);

            if (level_is_finished)
                spriteBatch.DrawString(font, "Finished level!", new Vector2(graphics.PreferredBackBufferWidth / 2f, graphics.PreferredBackBufferHeight / 2f), Color.White);
            
            //drawing all DrawablePhysicsObject
            player.Draw(spriteBatch);
            Platform2.Draw(spriteBatch);
            Platform3.Draw(spriteBatch);
            Platform4.Draw(spriteBatch);
            Platform5.Draw(spriteBatch);
            Platform6.Draw(spriteBatch);
            Draw_block1.Draw(spriteBatch);
            Draw_block2.Draw(spriteBatch);
            Draw_block3.Draw(spriteBatch);
            Draw_block4.Draw(spriteBatch);
            Draw_block5.Draw(spriteBatch);
            spriteBatch.End();

            //rendering all world data.
            debugView.RenderDebugData(ref Camera2D.Projection, ref Camera2D.View);
 
            base.Draw(gameTime);
        }
    }
}

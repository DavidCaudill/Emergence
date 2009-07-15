using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using TomShane.Neoforce.Controls;

namespace Emergence
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        PrimitiveBatch primitiveBatch;
        EmergenceGui Gui;
        Manager manager;

        DNA dna1;
        Symet symet1;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            // Basic setup of the game window.
            IsMouseVisible = true;
            IsFixedTimeStep = false;
            graphics.SynchronizeWithVerticalRetrace = false;
            manager = new Manager(this, graphics, "Default", true);
            manager.SkinDirectory = @"Content\Skins\";
            Gui = new EmergenceGui();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            dna1 = new DNA(Shape.Triangle, 50.0f);
            List<VectorP> instructions1 = new List<VectorP>();
            List<VectorP> instructions2 = new List<VectorP>();

            instructions1.Add(new VectorP(.9, 50));
            instructions1.Add(new VectorP(.7, 60));
            instructions1.Add(new VectorP(.5, 70));
            instructions1.Add(new VectorP(.3, 80));

            instructions2.Add(new VectorP(.7, 50));
            instructions2.Add(new VectorP(.5, 155));
            instructions2.Add(new VectorP(.4, 44));
            instructions2.Add(new VectorP(.1, 22));

            dna1.CreateChromosome(instructions1, 0, 1, SegmentType.Photo);
            dna1.CreateChromosome(instructions1, 1, 1, SegmentType.Defend);
            dna1.CreateChromosome(instructions1, 1, 3, SegmentType.Defend);
            dna1.CreateChromosome(instructions2, 2, 2, SegmentType.Attack);
            dna1.CreateChromosome(instructions2, 3, 2, SegmentType.Attack);

            symet1 = dna1.BuildDNA();
            symet1.Position = new Vector2(300);
            Gui.GuiInitialize(manager, graphics);
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
            primitiveBatch = new PrimitiveBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // Test code to move and spin the symet
            Vector2 movement = new Vector2();
            movement.X = Mouse.GetState().X;
            movement.Y = Mouse.GetState().Y;

            float rotation = Mouse.GetState().ScrollWheelValue;
            symet1.Position = movement;
            symet1.Rotation = rotation * .05f;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            symet1.Draw(primitiveBatch);
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}

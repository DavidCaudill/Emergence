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

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        PrimitiveBatch primitiveBatch;
        EmergenceGui Gui;
        Manager manager;
        DNA dna1;
        Symet symet1;
        List<Symet> symets;

        static Random random;
        static Settings globals;

        public static Random GetRandom()
        {
            return random;
        }
        public static Settings GetGlobals()
        {
            return globals;
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Gui = new EmergenceGui();
            globals = new Settings();
            manager = new Manager(this, graphics, "Default", true);
            manager.SkinDirectory = @"..\..\..\Content\Skins\";

            graphics.PreferredBackBufferWidth = 1027;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";

            IsMouseVisible = true;
            IsFixedTimeStep = true;
            
            graphics.SynchronizeWithVerticalRetrace = false;

            random = new Random();
        }

        protected override void Initialize()
        {
            // Test symet creation code
            dna1 = new DNA(Shape.Triangle, 5.0f, SegmentType.Photo, .1f);
            List<VectorP> instructions1 = new List<VectorP>();
            List<VectorP> instructions2 = new List<VectorP>();
            List<VectorP> instructions3 = new List<VectorP>();
            symets = new List<Symet>();

            instructions1.Add(new VectorP(1.2, 5));
            instructions1.Add(new VectorP(.7, 6));
            instructions1.Add(new VectorP(.3, 8));

            instructions2.Add(new VectorP(.9, 5));
            instructions2.Add(new VectorP(.5, 15));

            instructions3.Add(new VectorP(1.1, 10));
            instructions3.Add(new VectorP(1.7, 10));
            instructions3.Add(new VectorP(1.1, 14));
            instructions3.Add(new VectorP(.8, 14));
            instructions3.Add(new VectorP(1.0, 10));

            dna1.CreateChromosome(instructions1, 0, 1, SegmentType.Photo, new Vector2());
            dna1.CreateChromosome(instructions1, 1, 1, SegmentType.Movement, new Vector2(5,0));
            dna1.CreateChromosome(instructions1, 1, 3, SegmentType.Photo, new Vector2(-2,10));
            dna1.CreateChromosome(instructions2, 2, 2, SegmentType.Attack, new Vector2());
            dna1.CreateChromosome(instructions2, 3, 2, SegmentType.Attack, new Vector2());
            dna1.CreateChromosome(instructions3, 1, 2, SegmentType.Defend, new Vector2());
            
            for (int i = 0; i < 100; i++)
            {
                symet1 = dna1.BuildDNA();
                symet1.Position = new Vector2(300);
                symets.Add(symet1);
            }

            base.Initialize();

            Gui.GuiInitialize(manager, graphics);
        }

        protected override void LoadContent()
        {
            Game1.GetGlobals().Load();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            primitiveBatch = new PrimitiveBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
           
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            // Get mouse position
            Vector2 mousePosition = new Vector2();
            mousePosition.X = Mouse.GetState().X;
            mousePosition.Y = Mouse.GetState().Y;

            for (int i = 0; i < symets.Count; i++)
            {
                // Group all the symets at the mouse position
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    symets[i].Position = mousePosition;

                // Scale all the symets
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                    symets[i].Scale *= 1.01f;
                if (Keyboard.GetState().IsKeyDown(Keys.Z))
                    symets[i].Scale /= 1.01f;

                // Update all the symets
                symets[i].Update(gameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            primitiveBatch.Begin(PrimitiveType.LineList);
            for (int i = 0; i < symets.Count; i++)
            {
                symets[i].Draw(primitiveBatch);
            }
            primitiveBatch.End();
  
            base.Draw(gameTime);
        }
    }
}

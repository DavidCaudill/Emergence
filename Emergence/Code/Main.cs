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

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Gui = new EmergenceGui();

            manager = new Manager(this, graphics, "Default", true);
            manager.SkinDirectory = @"..\..\..\Content\Skins\";

            graphics.PreferredBackBufferWidth = 1027;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";

            IsMouseVisible = true;
            IsFixedTimeStep = true;
            
            graphics.SynchronizeWithVerticalRetrace = false;
        }

        protected override void Initialize()
        {
            // Test symet creation code
            dna1 = new DNA(Shape.Triangle, 50.0f, SegmentType.Defend);
            List<VectorP> instructions1 = new List<VectorP>();
            List<VectorP> instructions2 = new List<VectorP>();
            List<VectorP> instructions3 = new List<VectorP>();
            symets = new List<Symet>();

            instructions1.Add(new VectorP(1.2, 50));
            instructions1.Add(new VectorP(.7, 60));
            instructions1.Add(new VectorP(.3, 80));

            instructions2.Add(new VectorP(.9, 50));
            instructions2.Add(new VectorP(.5, 155));

            instructions3.Add(new VectorP(1.1, 100));
            instructions3.Add(new VectorP(1.7, 100));
            instructions3.Add(new VectorP(1.1, 140));
            instructions3.Add(new VectorP(.8, 140));
            instructions3.Add(new VectorP(1.0, 100));

            dna1.CreateChromosome(instructions1, 0, 1, SegmentType.Photo);
            dna1.CreateChromosome(instructions1, 1, 1, SegmentType.Defend);
            dna1.CreateChromosome(instructions1, 1, 3, SegmentType.Defend);
            dna1.CreateChromosome(instructions2, 2, 2, SegmentType.Attack);
            dna1.CreateChromosome(instructions2, 3, 2, SegmentType.Attack);
            dna1.CreateChromosome(instructions3, 1, 2, SegmentType.Defend);
            
            for (int i = 0; i < 1; i++)
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
            spriteBatch = new SpriteBatch(GraphicsDevice);
            primitiveBatch = new PrimitiveBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
           
        }

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

            bool up = false;
            bool down = false;

            if (Keyboard.GetState().IsKeyDown(Keys.A))
                up = true;
            if (Keyboard.GetState().IsKeyDown(Keys.Z))
                down = true;

            for (int i = 0; i < symets.Count; i++)
            {
                symets[i].Position = movement;
                symets[i].Rotation = rotation * .05f;

                if (up)
                    symets[i].Scale += .01f;
                if (down)
                    symets[i].Scale -= .01f;
                symets[i].Update(gameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            for (int i = 0; i < symets.Count; i++)
            {
                symets[i].Draw(primitiveBatch);
            }
  
            base.Draw(gameTime);
        }
    }
}

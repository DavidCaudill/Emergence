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

        World world;

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
            TargetElapsedTime = new TimeSpan(0, 0, 0, 0, 1000 / 20);
            
            graphics.SynchronizeWithVerticalRetrace = false;

            random = new Random();

            world = new World();
        }

        protected override void Initialize()
        {
            world.TestInitialize();

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

            world.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            world.Draw(gameTime, primitiveBatch);
  
            base.Draw(gameTime);
        }
    }
}

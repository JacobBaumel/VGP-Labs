using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ScribblePlatformer {
    public class Game1 : Microsoft.Xna.Framework.Game {
        private const int TargetFrameRate = 60;
        private const int backBufferWidth = 1280;
        private const int backBufferHeight = 720;
        
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Level level;
        

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = backBufferWidth;
            graphics.PreferredBackBufferHeight = backBufferHeight;
            Content.RootDirectory = "Content";

            TargetElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / TargetFrameRate);
        }

        protected override void Initialize() {

            base.Initialize();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            LoadLevel();

        }
        protected override void UnloadContent() {
        }

        protected override void Update(GameTime gameTime) {
            if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            level.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void LoadLevel() {
            level = new Level(Services, @"Content/Levels/Level01.txt");
        }
    }
}

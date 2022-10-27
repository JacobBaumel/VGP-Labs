
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

namespace RockAndHardPlace {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D rock;
        Texture2D hardPlace;

        Rectangle rockRect;
        Rectangle hardplaceRect;
        Color rockColor;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {

            rockRect = new Rectangle(0, 0, 100, 100);
            hardplaceRect = new Rectangle(275, 115, 250, 250);
            rockColor = Color.White;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            rock = Content.Load<Texture2D>("Rock");
            hardPlace = Content.Load<Texture2D>("Hard Place");

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            // Allows the game to exit
            if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            KeyboardState k = Keyboard.GetState();

            if(k.IsKeyDown(Keys.Up))
                rockRect.Y -= 10;
            if(k.IsKeyDown(Keys.Down))
                rockRect.Y += 10;
            if(k.IsKeyDown(Keys.Left))
                rockRect.X -= 10;
            if(k.IsKeyDown(Keys.Right))
                rockRect.X += 10;

            if(isIntersecting(rockRect, hardplaceRect))
                rockColor = Color.Red;
            else
                rockColor = Color.White;

            rockRect.X = (int) MathHelper.Clamp(rockRect.X, 0, 700);
            rockRect.Y = (int) MathHelper.Clamp(rockRect.Y, 0, 380);
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        bool isIntersecting(Rectangle r1, Rectangle r2) {
            return !(r1.X + r1.Width < r2.X || r1.X > r2.X + r2.Width || r1.Y > r2.Y + r2.Height || r1.Y + r1.Height < r2.Y);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(hardPlace, hardplaceRect, Color.White);
            spriteBatch.Draw(rock, rockRect, rockColor);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}

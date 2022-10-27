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

namespace MiniMap {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D galaxy;
        Texture2D baseRect;
        Rectangle world;
        Rectangle minimap;
        Rectangle locator;
        Vector2 pos;
        Color locatorColor;

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
            // TODO: Add your initialization logic here

            base.Initialize();
            world = new Rectangle(0, 0, 2400, 1440);
            minimap = new Rectangle(0, 0, 240, 135);
            pos = new Vector2(0, 0);
            locator = new Rectangle(0, 0, 80, 45);
            locatorColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            galaxy = Content.Load<Texture2D>("galaxy");
            baseRect = new Texture2D(GraphicsDevice, 1, 1);
            baseRect.SetData(new Color[] { Color.White });

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

            // TODO: Add your update logic here

            Vector2 sticks = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left;

            pos.X = MathHelper.Clamp(pos.X + (sticks.X * 1.5f), 0, 160);
            pos.Y = MathHelper.Clamp(pos.Y - (sticks.Y * 1.5f), 0, 90);

            world.X = -(int) (2400 * (pos.X / 240));
            world.Y = -(int) (1440 * (pos.Y / 135));

            locator.X = (int) pos.X;
            locator.Y = (int) pos.Y;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(galaxy, world, Color.White);
            spriteBatch.Draw(galaxy, minimap, Color.White);
            spriteBatch.Draw(baseRect, locator, locatorColor);

            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}

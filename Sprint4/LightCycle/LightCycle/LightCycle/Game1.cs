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

namespace LightCycle {

    

    public class Game1 : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Pixels pixels;
        Cycle c1;
        int time;
        public static State state = State.START;
        public enum State {
            START,
            PLAY,
            P1,
            P2
        }

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
            graphics.PreferredBackBufferWidth = 752;
            graphics.PreferredBackBufferHeight = 752;
            Window.AllowUserResizing = true;
            graphics.ApplyChanges();
            time = 0;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Texture2D baseRect = new Texture2D(GraphicsDevice, 1, 1);
            baseRect.SetData(new Color[] { Color.White });
            pixels = new Pixels(Content.Load<Texture2D>("blue"), Content.Load<Texture2D>("orange"), baseRect);

            // Sets wall pixels
            for(int i = 0; i < pixels.pixels.GetLength(0); i++) {
                pixels.pixels[0, i].set(Pixels.PixelState.LINE_SIDE);
                pixels.pixels[pixels.pixels.GetLength(1) - 1, i].set(Pixels.PixelState.LINE_SIDE);
            }
            for(int i = 0; i < pixels.pixels.GetLength(1); i++) {
                pixels.pixels[i, 0].set(Pixels.PixelState.LINE_SIDE);
                pixels.pixels[i, pixels.pixels.GetLength(1) - 1].set(Pixels.PixelState.LINE_SIDE);
            }

            c1 = new Cycle(pixels, 5, 5, false);
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
            


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            pixels.draw(spriteBatch);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}

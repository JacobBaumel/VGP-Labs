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

namespace BreadCrumbs {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D ball;
        Rectangle[] rects;
        Vector2 center;
        Color fade;
        int xvel;
        int yvel;
        int count;

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
            rects = new Rectangle[11];
            fade = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            center = new Vector2(250, 250);
            xvel = 15;
            yvel = 15;
            count = 0;
            int t = 0;
            for(int i = 0; i < rects.Length; i++) {
                rects[i] = new Rectangle(350, 190, 100 - t, 100 - t);
                t += 10;
            }

            rects[0].X += xvel;
            rects[0].Y += yvel;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ball = Content.Load<Texture2D>("ball");
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

            for(int i = Math.Min(count, rects.Length - 1); i >= 1; i--) {
                rects[i].X = rects[i - 1].X;
                rects[i].Y = rects[i - 1].Y;
            }

            if(rects[0].X - 50 < 0 || rects[0].X + rects[0].Width - 50 > 800)
                xvel *= -1;

            if(rects[0].Y - 50 < 0 || rects[0].Y + rects[0].Height - 50 > 480)
                yvel *= -1;

            rects[0].X += xvel;
            rects[0].Y += yvel;
            // TODO: Add your update logic here
            count++;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            // TODO: Add your drawing code here
            fade.A = 255;
            fade.R = 255;
            fade.G = 255;
            fade.B = 255;

            spriteBatch.Begin();
            for(int i = 0; i < rects.Length; i++) {
                spriteBatch.Draw(ball, rects[i], null, fade, 0, center, SpriteEffects.None, 0);
                fade.A -= 20;
                fade.R -= 20;
                fade.G -= 20;
                fade.B -= 20;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

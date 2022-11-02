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

namespace SpaceInvaders {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D invader;
        Rectangle rect;
        Rectangle drawRect;
        Rectangle[] sourceRects;
        int invaderStyle;
        int counter;
        int vel;

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
            counter = 0;
            invaderStyle = 0;
            sourceRects = new Rectangle[2];
            vel = 5;
            sourceRects[0] = new Rectangle(6, 4, 39, 29);
            sourceRects[1] = new Rectangle(57, 4, 39, 29);
            rect = new Rectangle(50, 50, 39, 29);
            drawRect = new Rectangle(50, 50, 39, 29);
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 1000;
            graphics.ApplyChanges();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            invader = Content.Load<Texture2D>("spaceInvader");
            spriteBatch = new SpriteBatch(GraphicsDevice);

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

            if(drawRect.X >= 975|| rect.X < 25) {
                vel *= -1;
                rect.Y = (int) MathHelper.Clamp(rect.Y + 50, 0, 950);
            }
            rect.X += vel;
            drawRect.X = rect.X;
            drawRect.Y = rect.Y;
            counter++;

            if(counter % 15 == 0) {
                invaderStyle++;
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);


            spriteBatch.Begin();
            
            for(int i = 0; i < 10; i++) {
                spriteBatch.Draw(invader, drawRect, sourceRects[invaderStyle % 2], Color.White);
                drawRect.X += drawRect.Width + 7;
            }
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}

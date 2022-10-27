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

namespace RocketMan {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D ufo;
        Rectangle ufoRect;
        SpriteFont font;
        Vector2 headingText;

        float speed;
        int heading;

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
            ufoRect = new Rectangle(0, 0, 152, 125);
            heading = 0;
            speed = 0;
            headingText = new Vector2(0, 25);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ufo = Content.Load<Texture2D>("ufo");
            font = Content.Load<SpriteFont>("SpriteFont1");

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
            GamePadState g = GamePad.GetState(PlayerIndex.One);
            // Allows the game to exit
            if(g.Buttons.Back == ButtonState.Pressed)
                this.Exit();
            
            float gx = g.ThumbSticks.Right.X;
            float gy = g.ThumbSticks.Right.Y;

            if (gx == 0 && gy == 0) heading = 0;
            else heading = (int)MathHelper.ToDegrees((float) Math.Atan(gx / gy));
            if (gx > 0 && gy < 0) heading += 180;
            else if (gx < 0 && gy < 0) heading += 180;
            else if (gx < 0 && gy > 0) heading += 360;

            if (heading == -90) heading = 270;
            if (heading == 0 && gy < 0) heading = 180;

            speed = MathHelper.Clamp(speed + g.ThumbSticks.Left.Y, 0, 50);
            float x = (((gx * speed) + ufoRect.X) % 800);
            float y = ((ufoRect.Y - (gy * speed)) % 480);

            if (x < 0) x += 800;
            if (y < 0) y += 480;
            ufoRect.X = (int) x;
            ufoRect.Y = (int) y;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(ufo, ufoRect, Color.White);
            spriteBatch.DrawString(font, "Speed: " + ((int) speed), Vector2.Zero, Color.White);
            spriteBatch.DrawString(font, "Heading: " + heading, headingText, Color.White);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}

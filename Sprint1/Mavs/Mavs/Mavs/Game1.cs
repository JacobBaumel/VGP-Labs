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

namespace Mavs
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D basketball;
        Rectangle basketballRect;
        Texture2D court;
        Rectangle courtRect;
        Texture2D hoop;
        Rectangle hoopRect;
        SpriteFont font;

        // Parabolic Flight constants
        const double V = 100;
        const double angle = 20;
        const double initialHeight = 50;
        double Vx;
        double Vy;
        double physicsTime;
        double time;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            basketballRect = new Rectangle(0, 0, 75, 75);
            courtRect = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            hoopRect = new Rectangle(600, 50, 188, 265);
            Vx = V * Math.Cos(angle);
            Vy = V * Math.Sin(angle);
            physicsTime = 0;
            time = 0;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            basketball = Content.Load<Texture2D>("basketball");
            court = Content.Load<Texture2D>("court");
            hoop = Content.Load<Texture2D>("hoop");
            font = Content.Load<SpriteFont>("font");

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            time++;
            if (time / 60 > 6) physicsTime += (double) 1 / 15;
            basketballRect.X = (int)(Vx * physicsTime);
            basketballRect.Y = (int)(GraphicsDevice.Viewport.Height - (initialHeight + (Vy * physicsTime) - (0.5 * 9.8 * Math.Pow(physicsTime, 2))));

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(court, courtRect, Color.White);
            spriteBatch.Draw(hoop, hoopRect, Color.White);
            spriteBatch.Draw(basketball, basketballRect, Color.White);
            if (time / 60 < 6) spriteBatch.DrawString(font, "" + (5 - ((int)time / 60)), new Vector2(50, 50), Color.White);
            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}

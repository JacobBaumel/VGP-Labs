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

namespace Sidekick
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D stationary;
        Rectangle stationaryRect;

        Texture2D big;
        Rectangle bigRect;

        Texture2D small;
        Rectangle smallRect;

        Color bigRectColor;
        Color smallRectColor;

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
            bigRectColor = Color.White;
            smallRectColor = Color.White;
            stationaryRect = new Rectangle(350, 190, 100, 100);
            bigRect = new Rectangle(150, 100, 150, 100);
            smallRect = new Rectangle(0, 215, 50, 50);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            stationary = Content.Load<Texture2D>("StationaryObj");
            big = Content.Load<Texture2D>("BigMovingObj");
            small = Content.Load<Texture2D>("SmallMovingObj");
            spriteBatch = new SpriteBatch(GraphicsDevice);

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

            smallRect.X++;
            bigRect.X++;

            if (stationaryRect.Intersects(bigRect)) bigRectColor = Color.Red;
            else bigRectColor = Color.White;

            if(stationaryRect.Intersects(smallRect)) smallRectColor = Color.Red;
            else smallRectColor = Color.White;

            // TODO: Add your update logic here

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
            spriteBatch.Draw(stationary, stationaryRect, Color.White);
            spriteBatch.Draw(big, bigRect, bigRectColor);
            spriteBatch.Draw(small, smallRect, smallRectColor);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}

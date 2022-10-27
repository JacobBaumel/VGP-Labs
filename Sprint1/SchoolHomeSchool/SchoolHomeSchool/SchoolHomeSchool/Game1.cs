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

namespace SchoolHomeSchool
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteFont font;
        Texture2D classroom;
        Texture2D school;
        Texture2D house;

        Rectangle pos1Rect;
        Rectangle pos2Rect;
        Rectangle pos3Rect;
        Texture2D rect;
        Color[] colors;
        Rectangle[] rects;
        int colorMode;
        int rectMode;
        int textColorMode;
        int time;

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

            pos3Rect = new Rectangle(145, 70, 160, 110);
            pos2Rect = new Rectangle(470, 70, 160, 110);
            pos1Rect = new Rectangle(307, 245, 160, 110);
            
            colors = new Color[] {Color.Blue, Color.Yellow, Color.Red};
            colorMode = 0;
            textColorMode = 0;

            rects = new Rectangle[] { new Rectangle(312, 250, 150, 100), new Rectangle(475, 75, 150, 100), new Rectangle(150, 75, 150, 100) };
            rectMode = 0;

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
            classroom = Content.Load<Texture2D>("Classroom");
            house = Content.Load<Texture2D>("house2");
            school = Content.Load<Texture2D>("School");
            rect = new Texture2D(GraphicsDevice, 1, 1);
            rect.SetData(new Color[] { Color.White });
            font = Content.Load<SpriteFont>("SpriteFont1");
            

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

            if (((double)time / 60) % 4 == 0) {
                rectMode++;
                textColorMode++;
            }

            if (((double)time / 60) % 7 == 0) {
                colorMode++;
                textColorMode++;
            }

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
            spriteBatch.Draw(rect, pos1Rect, colors[colorMode % 3]);
            spriteBatch.Draw(rect, pos2Rect, colors[(colorMode + 1) % 3]);
            spriteBatch.Draw(rect, pos3Rect, colors[(colorMode + 2) % 3]);

            spriteBatch.Draw(classroom, rects[rectMode % 3], Color.White);
            spriteBatch.Draw(house, rects[(rectMode + 1) % 3], Color.White);
            spriteBatch.Draw(school, rects[(rectMode + 2) % 3], Color.White);

            Rectangle curRect = rects[rectMode % 3];
            spriteBatch.DrawString(font, "Classroom Slide", new Vector2(curRect.Location.X, curRect.Location.Y + 110), colors[textColorMode % 3]);

            curRect = rects[(rectMode + 1) % 3];
            spriteBatch.DrawString(font, "House Slide", new Vector2(curRect.Location.X, curRect.Location.Y + 110), colors[(textColorMode + 1) % 3]);

            curRect = rects[(rectMode + 2) % 3];
            spriteBatch.DrawString(font, "School Slide", new Vector2(curRect.Location.X, curRect.Location.Y + 110), colors[(textColorMode + 2) % 3]);

            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}

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

namespace RomanPig
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D pig;
        Rectangle pig1Rect;
        Rectangle pig2Rect;
        SpriteFont font;

        string phrase1;
        string phrase2;
        string phrase3;
        int timer;

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
            pig1Rect = new Rectangle(0, 100, 225, 224);
            pig2Rect = new Rectangle(GraphicsDevice.Viewport.Width - 225, 100, 225, 224);
            phrase1 = "";
            phrase2 = "";
            phrase3 = "";
            timer = 0;

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            pig = Content.Load<Texture2D>("pig");
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

            timer++;

            if (timer / 60 > 6) phrase3 = "I speak pig latin";
            else if (timer / 60 > 4) phrase2 = "How are you today";
            else if (timer / 60 > 2) phrase1 = "Hello there";



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
            spriteBatch.Draw(pig, pig1Rect, null, Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
            spriteBatch.Draw(pig, pig2Rect, Color.White);

            spriteBatch.DrawString(font, phrase1, new Vector2(225, 50), Color.White);
            spriteBatch.DrawString(font, phrase2, new Vector2(225, 150), Color.White);
            spriteBatch.DrawString(font, phrase3, new Vector2(225, 250), Color.White);

            spriteBatch.DrawString(font, translate(phrase1), new Vector2(350, 100), Color.White);
            spriteBatch.DrawString(font, translate(phrase2), new Vector2(275, 200), Color.White);
            spriteBatch.DrawString(font, translate(phrase3), new Vector2(275, 300), Color.White);
            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        public string translate(string s) {
            if (s.Length == 0) return s;
            string[] split = s.Split(' ');
            List<char> vowels = new List<char> { 'a', 'e', 'i', 'o', 'u' };
            
            for(int i = 0; i < split.Length; i++) {
                string temp = "";
                if (split[i].Length < 2) temp = split[i] + "way";
                else if (!vowels.Contains(split[i][0])) {
                    if (vowels.Contains(split[i][1])) temp = split[i].Substring(1) + split[i][0] + "ay";
                    else temp = split[1].Substring(2) + split[i].Substring(0, 2) + "ay";
                }

                else temp = split[i] + "way";

                split[i] = temp;

            }

            string final = "";
            foreach (string st in split) final += st + " ";

            return final;
        }
    }
}

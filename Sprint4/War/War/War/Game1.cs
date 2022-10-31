using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace War {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Rectangle c1;
        Rectangle c2;
        int c1Index;
        int c2Index;
        Card[] cards;
        SpriteFont font;
        bool pushed;
        Random r;

        Vector2 c1Text;
        Vector2 c2Text;

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
            r = new Random((int) DateTime.Now.Ticks);
            c1 = new Rectangle(100, 100, 142, 212);
            c2 = new Rectangle(500, 100, 142, 212);
            c1Index = r.Next(0, 51);
            pushed = false;
            c1Text = new Vector2(c1.X, c1.Y + c1.Height);
            c2Text = new Vector2(c2.X, c2.Y + c2.Height);

            do {
                c2Index = r.Next(0, 51);
            } while(c2Index == c1Index);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("SpriteFont1");
            string[] files = Directory.GetFiles(@"Content\");
            cards = new Card[files.Length];
            for(int i = 0; i < files.Length; i++) {
                string cname = files[i].Substring(0, files[i].LastIndexOf(".")).Substring(files[i].LastIndexOf(@"\") + 1);
                if(cname.Length != 3)
                    continue;
                cards[i] = new Card(Content.Load<Texture2D>(cname), Card.cardFileNameToValue(cname), Card.getSuitFromLetter(cname), 0, 0);
            }
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

            if(Keyboard.GetState().IsKeyDown(Keys.S)) {
                if(!pushed) {
                    pushed = true;
                    shuffle(cards);
                    c1Index = r.Next(0, cards.Length - 1);

                    do {
                        c2Index = r.Next(0, cards.Length - 1);
                    } while(c2Index == c1Index);
                }
            }

            else
                pushed = false;
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Push S to shuffle", Vector2.Zero, Color.White);
            cards[c1Index].draw(spriteBatch, c1);
            cards[c2Index].draw(spriteBatch, c2);
            spriteBatch.DrawString(font, "Suit: " + cards[c1Index].suit + "\nValue: " + cards[c1Index].value, c1Text, Color.White);
            spriteBatch.DrawString(font, "Suit: " + cards[c2Index].suit + "\nValue: " + cards[c2Index].value, c2Text, Color.White);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }


        public T[] shuffle<T>(T[] list) {
            T[] shuffled = new T[list.Length];

            foreach(T t in list) {
                while(true) {
                    int pos = r.Next(0, list.Length);
                    if(shuffled[pos] != null)
                        continue;
                    shuffled[pos] = t;
                    break;
                }
            }

            return shuffled;
        }
        
    }
}

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

namespace FileReaderDemo {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteFont font;
        List<string> lines;
        int xin;
        int yin;

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
            lines = new List<string>();
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
            ReadFile(@"Content/test.txt");
            ReadIntFile(@"Content/nums.txt");
            // TODO: use this.Content to load your game content here
        }

        void ReadFile(string path) {
            try {
                using(StreamReader r = new StreamReader(path)) {
                    while(!r.EndOfStream)
                        lines.Add(r.ReadLine());
                }
            } catch(Exception e) {
                Console.WriteLine("Error reading file");
                Console.WriteLine(e.Message);
            }
        }

        void ReadIntFile(string path) {
            try {
                using(StreamReader r = new StreamReader(path)) {
                    while(!r.EndOfStream) {
                        string[] parts = r.ReadLine().Split(' ');
                        Console.WriteLine(parts[0]);
                        Console.WriteLine(parts[1]);
                        xin = Convert.ToInt32(parts[0]);
                        yin = Convert.ToInt32(parts[1]);
                    }
                }
            } catch(Exception e) {
                Console.WriteLine("Error reading file");
                Console.WriteLine(e.Message);
            }
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

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            Vector2 pos = new Vector2(50, 10);
            foreach(string s in lines) {
                spriteBatch.DrawString(font, s, pos, Color.White);
                pos.Y += 30;
            }

            pos.X = 50;
            pos.Y = 100;
            spriteBatch.DrawString(font, "I read " + xin + " and " + yin + ". The sum is " + (xin + yin), pos, Color.White);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}

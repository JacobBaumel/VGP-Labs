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

namespace FileWriter {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        int numToWrite;
        int num1;
        int num2;
        Keys[] watchkeys;
        KeyboardState old;

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
            old = Keyboard.GetState();
            numToWrite = 0;
            num1 = 0;
            num2 = 0;
            watchkeys = new Keys[] { Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9 };
            Console.WriteLine(Directory.GetCurrentDirectory());
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
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
            KeyboardState k = Keyboard.GetState();
            if(k.IsKeyDown(Keys.Escape))
                this.Exit();

            switch(numToWrite) {
                case 0:
                    if(k.IsKeyDown(Keys.Enter) && old.IsKeyUp(Keys.Enter)) {
                        numToWrite++;
                        break;
                    }

                    for(int i = 0; i < watchkeys.Length; i++) {
                        if(k.IsKeyDown(watchkeys[i]) && old.IsKeyUp(watchkeys[i])) {
                            num1 *= 10;
                            num1 += i;
                        }
                    }
                    break;

                case 2:

                    break;
            }
            
            old = k;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            switch(numToWrite) {
                case 0:
                    spriteBatch.DrawString(font, "Enter Number 1 (enter to continue): ", Vector2.Zero, Color.White);
                    break;

                case 1:
                    spriteBatch.DrawString(font, "Is this correct [y/n]: " + num1, Vector2.Zero, Color.White);
                    break;

                case 2:
                    spriteBatch.DrawString(font, "Enter Number 2 (enter to continue): ", Vector2.Zero, Color.White);
                    break;

                case 3:
                    spriteBatch.DrawString(font, "Is this correct [y/n]: " + num2, Vector2.Zero, Color.White);
                    break;

                default:
                    spriteBatch.DrawString(font, @"File ..\..\..\..\..\..\Test.Dat has been written to with numbers " + num1 + " and " + num2 + ", push ESC to exit", Vector2.Zero, Color.White);
                    break;
            }
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}

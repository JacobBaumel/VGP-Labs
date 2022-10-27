
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

namespace ChessBoard {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D[] pieces;
        Texture2D board;
        Rectangle boardRect;
        Rectangle[] rects;
        int[] textureIndex;

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
            rects = new Rectangle[32];
            boardRect = new Rectangle();
            textureIndex = new int[] { 2, 4, 1, 6, 3, 1, 4, 2, 5, 5, 5, 5, 5, 5, 5, 5, 11, 11, 11, 11, 11, 11, 11, 11, 8, 10, 7, 0, 9, 7, 10, 8 };
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.

            board = Content.Load<Texture2D>("board");
            boardRect.Width = board.Width;
            boardRect.Height = board.Height;
            graphics.PreferredBackBufferWidth = boardRect.Width;
            graphics.PreferredBackBufferHeight = boardRect.Height;
            graphics.ApplyChanges();

            spriteBatch = new SpriteBatch(GraphicsDevice);
            int x = 0;
            int y = 0;

            pieces = new Texture2D[12];
            for(int i = 0; i < 12; i++) {
                pieces[i] = Content.Load<Texture2D>("" + i);
            }

            for(int i = 0; i < rects.Length; i++) {
                rects[i] = new Rectangle(x, y, pieces[textureIndex[i]].Width / 2, pieces[textureIndex[i]].Height / 2);
                x += (pieces[textureIndex[i]].Width / 2) + 15;
                if(i == 15) {
                    x = 0;
                    y += 320;
                }

                else if(i == 7 || i == 23) {
                    x = 0;
                    y += 75;
                }
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
            spriteBatch.Draw(board, boardRect, Color.White);
            for(int i = 0; i < rects.Length; i++) {
                spriteBatch.Draw(pieces[textureIndex[i]], rects[i], Color.White);
            }
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}

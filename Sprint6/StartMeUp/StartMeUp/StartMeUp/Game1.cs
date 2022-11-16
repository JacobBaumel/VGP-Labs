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

namespace StartMeUp {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    /// 

    public enum GameState {
        START,
        PLAY,
        QUIT
    }

    public class Game1 : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameState state;
        Texture2D[] textures;
        Rectangle screenRect;
        KeyboardState prev;
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
            state = GameState.START;
            prev = Keyboard.GetState();
            screenRect = new Rectangle();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            textures = new Texture2D[3];
            for(int i = 0; i < 3; i++) {
                textures[i] = Content.Load<Texture2D>("" + i);
            }


            screenRect.X = (800 - textures[(int) state].Width) / 2;
            screenRect.Y = (480 - textures[(int) state].Height) / 2;
            screenRect.Width = textures[(int) state].Width;
            screenRect.Height = textures[(int) state].Height;
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

            KeyboardState k = Keyboard.GetState();
            
            if(k.IsKeyDown(Keys.Enter) && prev.IsKeyUp(Keys.Enter)) {
                state = (GameState) (((int) state + 1) % 3);
                screenRect.X = (800 - textures[(int) state].Width) / 2;
                screenRect.Y = (480 - textures[(int) state].Height) / 2;
                screenRect.Width = textures[(int) state].Width;
                screenRect.Height = textures[(int) state].Height;
            }
            
            
            
            
            // TODO: Add your update logic here
            prev = k;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(textures[(int) state], screenRect, Color.White);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}

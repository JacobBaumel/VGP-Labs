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

namespace Breakout {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Bricks b;
        Player p;
        GameState state;

        Texture2D baseRect;
        Texture2D brick;
        SpriteFont font;

        Vector2 text;
        Rectangle[] buttons;

        enum GameState { 
            START,
            LEVEL
        }

        public const int SCREEN_WIDTH = 800;
        public const int SCREEN_HEIGHT = 900;
        

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
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            graphics.ApplyChanges();
            state = GameState.START;
            IsMouseVisible = true;
            text = new Vector2((SCREEN_WIDTH / 2) - 100, 400);
            buttons = new Rectangle[] {new Rectangle((SCREEN_WIDTH - 200) / 2, 500, 200, 50), new Rectangle((SCREEN_WIDTH - 200) / 2, 600, 200, 50), new Rectangle((SCREEN_WIDTH - 200) / 2, 700, 200, 50)};
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            baseRect = new Texture2D(GraphicsDevice, 1, 1);
            baseRect.SetData(new Color[] { Color.White });
            brick = Content.Load<Texture2D>("brick");
            font = Content.Load<SpriteFont>("SpriteFont1");
            p = new Player(Content.Load<Texture2D>("circle"), baseRect);
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
            if(Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            if(state == GameState.START) {
                int x = Mouse.GetState().X;
                int y = Mouse.GetState().Y;
                bool isClicked = Mouse.GetState().LeftButton == ButtonState.Pressed;
                for(int i = 0; i < buttons.Length; i++) {
                    if(isClicked && buttons[i].Contains(x, y)) {
                        b = new Bricks(brick, @"Content/l" + (i + 1) + ".txt");
                        p.reset();
                        state = GameState.LEVEL;
                    }
                }
            }

            else {
                p.update();
                b.update(p);

                if((b.isOver || p.end) && Keyboard.GetState().IsKeyDown(Keys.Space)) {
                    state = GameState.START;
                }
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            if(state == GameState.START) {
                spriteBatch.DrawString(font, "Breakout", text, Color.Orange);
                for(int i = 0; i < buttons.Length; i++) {
                    spriteBatch.DrawString(font, "Level " + (i + 1), new Vector2(buttons[i].X, buttons[i].Y), Color.White);
                }
            }

            else {
                b.draw(spriteBatch);
                p.draw(spriteBatch);

                if(b.isOver)
                    spriteBatch.DrawString(font, "Game over!\nYou win!\n\nPress Space\nto restart", text, Color.Green);

                else if(p.end)
                    spriteBatch.DrawString(font, "Game over!\nYou lose!\n\nPress Space\nto restart", text, Color.Red);
            }
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}

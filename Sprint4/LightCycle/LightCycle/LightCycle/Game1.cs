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

namespace LightCycle {

    

    public class Game1 : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Pixels pixels;
        Cycle c1;
        Cycle c2;
        SpriteFont font;
        Random r;

        Vector2 centertext;
        Vector2 timerText;
        Vector2 endscreenText;

        bool showTimer;
        int timer;
        int frameTimer;

        public const int WINDOW_SIZE = 1000;
        public const int PIXEL_SIZE = 5;
        public const int ARRAY_SIZE = WINDOW_SIZE / PIXEL_SIZE;
        const int UPDATE_PERIOD = 2;
        const string BEGIN_STRING = "PUSH SPACE TO BEGIN";
        const string TIMER_STRING = "$";
        const string END_STRING = "PLAYER $ WINS!\nPUSH SPACE TO PLAY AGAIN!";

        public static State state = State.START;
        public enum State {
            START,
            PLAY,
            P1,
            P2
        }

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
            showTimer = false;
            timer = 0;
            r = new Random((int) DateTime.Now.Ticks);
            frameTimer = 0;
            centertext = new Vector2(175, 235);
            timerText = new Vector2(330, 275);
            graphics.PreferredBackBufferWidth = WINDOW_SIZE;
            graphics.PreferredBackBufferHeight = WINDOW_SIZE;
            Window.AllowUserResizing = false;
            graphics.ApplyChanges();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Texture2D baseRect = new Texture2D(GraphicsDevice, 1, 1);
            baseRect.SetData(new Color[] { Color.White });
            pixels = new Pixels(Content.Load<Texture2D>("blue"), Content.Load<Texture2D>("orange"), baseRect);
            pixels.reset();

            font = Content.Load<SpriteFont>("SpriteFont1");
            Vector2 temp = font.MeasureString(BEGIN_STRING);
            centertext = new Vector2((WINDOW_SIZE - temp.X) / 2, (WINDOW_SIZE - temp.Y) / 2);
            temp = font.MeasureString(TIMER_STRING);
            timerText = new Vector2((WINDOW_SIZE - temp.X) / 2, (WINDOW_SIZE - temp.Y) / 2);
            timerText.Y += 40;
            temp = font.MeasureString(END_STRING);
            endscreenText = new Vector2((WINDOW_SIZE - temp.X) / 2, (WINDOW_SIZE - temp.Y) / 2);
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

            switch(state) {
                case State.START:
                    if(Keyboard.GetState().IsKeyDown(Keys.Space)) {
                        if(!showTimer) {
                            pixels.reset();
                            int pixelPos = r.Next(0, ARRAY_SIZE);
                            if(r.Next(0, 2) == 0) {
                                c1 = new Cycle(pixels, pixelPos, 2, false, 0, 1);
                                c2 = new Cycle(pixels, pixelPos, ARRAY_SIZE - 3, true, 0, -1);
                            }

                            else {
                                c1 = new Cycle(pixels, 2, pixelPos, false, 1, 0);
                                c2 = new Cycle(pixels, ARRAY_SIZE - 3, pixelPos, true, -1, 0);
                            }
                        }
                        showTimer = true;
                    }
                    break;

                case State.PLAY:
                    frameTimer++;
                    if(frameTimer % UPDATE_PERIOD == 0) {
                        c1.update();
                        c2.update();
                    }
                    break;

                case State.P1:
                case State.P2:
                    if(Keyboard.GetState().IsKeyDown(Keys.Space))
                        state = State.START;
                    break;
            }

            if(showTimer && state == State.START) {
                timer++;
                if(timer % 60 == 0 && timer / 60 == 5) {
                    state = State.PLAY;
                    timer = 0;
                    showTimer = false;
                    
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            switch(state) {
                case State.START:
                    spriteBatch.DrawString(font, BEGIN_STRING, centertext, Color.Teal);
                    if(showTimer)
                        spriteBatch.DrawString(font, "" + (5 - (timer / 60)), timerText, Color.Teal);
                    break;

                case State.PLAY:
                    pixels.draw(spriteBatch);
                    break;

                case State.P1:
                    pixels.draw(spriteBatch);
                    spriteBatch.DrawString(font, "PLAYER 1 WINS!\nPUSH SPACE TO PLAY AGAIN!", endscreenText, Color.Cyan);
                    break;

                case State.P2:
                    pixels.draw(spriteBatch);
                    spriteBatch.DrawString(font, "PLAYER 2 WINS!\nPUSH SPACE TO PLAY AGAIN!", endscreenText, Color.Orange);
                    break;
            }
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}

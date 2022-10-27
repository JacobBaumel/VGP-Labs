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

namespace BeamMeUp
{
    /// <summary> 299 168
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D image;
        Rectangle imageRect;
        SoundEffect[] sounds;
        KeyboardState prev;
        string[] soundNames;
        Vector2 menuPos;
        SpriteFont font;
        int currentSound;
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
            imageRect = new Rectangle(0, 100, 299, 168);
            prev = Keyboard.GetState();
            soundNames = new string[] { "TOS Chirp", "Alert", "Borg cut", "TNG Phaser", "TNG Transporter" };
            menuPos = new Vector2(0, 0);
            currentSound = -1;
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
            image = Content.Load<Texture2D>("image");
            sounds = new SoundEffect[5];
            for (int i = 0; i < 5; i++) sounds[i] = Content.Load<SoundEffect>("sound" + i);
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

            KeyboardState current = Keyboard.GetState();
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || current.IsKeyDown(Keys.Escape))
                this.Exit();

            if(currentSound == -1 || time / 60 > sounds[currentSound].Duration.TotalSeconds) {
                if(isKeyEvent(Keys.D1, current)) {
                    sounds[0].Play();
                    currentSound = 0;
                    time = 0;
                }
                else if(isKeyEvent(Keys.D2, current)) {
                    sounds[1].Play();
                    currentSound = 1;
                    time = 0;
                }
                else if(isKeyEvent(Keys.D3, current)) {
                    sounds[2].Play();
                    currentSound = 2;
                    time = 0;
                }
                else if(isKeyEvent(Keys.D4, current)) {
                    sounds[3].Play();
                    currentSound = 3;
                    time = 0;
                }
                else if(isKeyEvent(Keys.D5, current)) {
                    sounds[4].Play();
                    currentSound = 4;
                    time = 0;
                }
            }

            // TODO: Add your update logic here


            time++;
            prev = current;
            base.Update(gameTime);
        }

        bool isKeyEvent(Keys k, KeyboardState state) {
            return state.IsKeyDown(k) && !prev.IsKeyDown(k);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(image, imageRect, Color.White);
            menuPos.X = 300;
            menuPos.Y = 100;
            for(int i = 0; i < soundNames.Length; i++) {
                spriteBatch.DrawString(font, soundNames[i] + ": " + (i + 1), menuPos, Color.White);
                menuPos.Y += 30;
            }
            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}

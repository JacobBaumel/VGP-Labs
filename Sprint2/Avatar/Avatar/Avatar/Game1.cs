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

namespace Avatar {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Rectangle avatarRect;
        Rectangle bigAvatar;
        Texture2D[] avatars;
        int selectedAvatar;
        Texture2D selector;
        Rectangle selectorRect;
        Rectangle selectorRectInside;
        SpriteFont font;
        bool zone;
        GamePadState prev;

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
            IsMouseVisible = true;
            base.Initialize();
            avatarRect = new Rectangle(0, 176, 128, 128);
            bigAvatar = new Rectangle(160, 0, 480, 480);
            selectorRect = new Rectangle(0, 0, 138, 138);
            selectorRectInside = new Rectangle(0, 0, 128, 128);
            selectedAvatar = 0;
            zone = true;
            prev = GamePad.GetState(PlayerIndex.One);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            avatars = new Texture2D[5];
            for(int i = 0; i < avatars.Length; i++) {
                avatars[i] = Content.Load<Texture2D>("avatar" + i);
            }

            selector = new Texture2D(GraphicsDevice, 1, 1);
            selector.SetData(new Color[] { Color.White });
            font = Content.Load<SpriteFont>("SpriteFont1");


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
            GamePadState current = GamePad.GetState(PlayerIndex.One);

            if(current.Buttons.A == ButtonState.Pressed)
                this.Exit();

            if(zone) {
                if(current.IsButtonDown(Buttons.DPadRight) && prev.IsButtonUp(Buttons.DPadRight)) {
                    if((selectedAvatar++) >= 4)
                        selectedAvatar = 0;
                }
                else if(current.IsButtonDown(Buttons.DPadLeft) && prev.IsButtonUp(Buttons.DPadLeft)) {
                    if((selectedAvatar--) <= 0)
                        selectedAvatar = 4;
                }
            }

            if((current.IsButtonDown(Buttons.Start) && prev.IsButtonUp(Buttons.Start)) ||
                (current.IsButtonDown(Buttons.Back) && prev.IsButtonUp(Buttons.Back)))
                zone = !zone;


            // TODO: Add your update logic here

            prev = current;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Use A to exit", Vector2.Zero, Color.White);
            if(zone) {
                avatarRect.X = 55;
                for(int i = 0; i < avatars.Length; i++) {
                    if(i == selectedAvatar % 5) {
                        selectorRect.X = avatarRect.X - 5;
                        selectorRect.Y = avatarRect.Y - 5;
                        spriteBatch.Draw(selector, selectorRect, Color.White);

                        selectorRectInside.X = avatarRect.X;
                        selectorRectInside.Y = avatarRect.Y;
                        spriteBatch.Draw(selector, selectorRectInside, Color.CornflowerBlue);
                    }
                    spriteBatch.Draw(avatars[i], avatarRect, Color.White);
                    avatarRect.X += 138;
                }
            }

            else {
                spriteBatch.Draw(avatars[Math.Abs(selectedAvatar) % 5], bigAvatar, Color.White);
            }
            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}

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

namespace SpaceInvaders {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Invader[,] invaders;
        Texture2D[] textures;
        SoundEffectInstance music;
        Color background;
        int edge;
        bool dir;

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
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 920;
            graphics.ApplyChanges();
            background = new Color(27, 35, 45);
            edge = 0;
            dir = false;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            textures = new Texture2D[8];

            for(int i = 0; i < 8; i++)
                textures[i] = Content.Load<Texture2D>("" + i);

            invaders = new Invader[10, 8];
            for(int i = 0; i < invaders.GetLength(0); i++) {
                for(int j = 0; j < invaders.GetLength(1); j++) {
                    invaders[i, j] = new Invader(i * 40, (j * 40), textures[j]);
                }
            }

            music = Content.Load<SoundEffect>("ost").CreateInstance();
            music.IsLooped = true;
            music.Play();


            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
            music.Stop();
            music.Dispose();
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
            if(dir) {
                edge += 8;
                foreach(Invader i in invaders) {
                    i.update(8, edge >= 400);
                }

                if(edge >= 400)
                    dir = !dir;
            }

            else {
                edge -= 8;
                foreach(Invader i in invaders)
                    i.update(-8, edge <= 0);
                if(edge <= 0)
                    dir = !dir;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(background);

            spriteBatch.Begin();
            foreach(Invader i in invaders)
                i.draw(spriteBatch);
            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}

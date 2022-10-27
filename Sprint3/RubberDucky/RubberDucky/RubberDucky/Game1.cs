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

namespace RubberDucky {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D ducks;
        Rectangle[] sourceRects;
        Rectangle[] screenRects;
        Rectangle[] originalScreens;
        float[] duckRot;
        float[] originalDuckRot;
        Vector2 duckCenter;
        Keys prev;

        Keys[] searchKeys;
        int stage;
        int topDuckSpeed;
        int bottomDuckSpeed;
        int high;
        int low;

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

            
            searchKeys = new Keys[] {Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.A, Keys.E, Keys.O, Keys.NumPad1, Keys.NumPad2, Keys.NumPad3, Keys.NumPad4, Keys.NumPad5, Keys.NumPad6, Keys.Space};
            sourceRects = new Rectangle[6];
            screenRects = new Rectangle[6];
            duckCenter = new Vector2(100, 177 / 2);
            duckRot = new float[6];

            int x = 100;
            int y = 55;

            for(int i = 0; i < 6; i++) {
                duckRot[i] = 0;
                sourceRects[i] = new Rectangle(i * 200, 0, 200, 177);
                screenRects[i] = new Rectangle((int) (x + duckCenter.X), (int) (y + duckCenter.Y), 200, 177);
                x += 200;
                if(i == 2) {
                    y += 200;
                    x = 100;
                }
            }

            originalScreens = new Rectangle[6];
            screenRects.CopyTo(originalScreens, 0);
            duckRot[4] = -180;
            originalDuckRot = new float[6];
            duckRot.CopyTo(originalDuckRot, 0);
            topDuckSpeed = 2;
            bottomDuckSpeed = 4;
            stage = 0;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ducks = Content.Load<Texture2D>("Duckies");

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
            KeyboardState k = Keyboard.GetState();

            if(k.IsKeyDown(Keys.Escape))
                Exit();

            foreach(Keys s in searchKeys) {
                if(k.IsKeyDown(s))
                    prev = s;
            }

            if(prev == Keys.Space) {
                stage = 0;
                originalScreens.CopyTo(screenRects, 0);
                for(int i = 0; i < 6; i++)
                    duckRot[i] = 0;
                duckRot[4] = -180;
            }

            if(stage == 0) {
                switch(prev) {
                    case Keys.A:
                        for(int i = 0; i < 6; i++)
                            duckRot[i] -= 10;
                        break;

                    case Keys.E:
                        for(int i = 1; i < 6; i += 2)
                            duckRot[i] += 30;
                        break;

                    case Keys.O:
                        for(int i = 0; i < 6; i += 2)
                            duckRot[i] -= 30;
                        break;
                    default:
                        if(prev.ToString().Length == 2) {
                            duckRot[Convert.ToInt32(prev.ToString().Substring(1)) - 1] += 30;
                        }

                        else if(prev.ToString().Length == 7) {
                            stage = Convert.ToInt32(prev.ToString().Substring(6));
                            if(stage > 3) {
                                high = stage - 1;
                                low = 6 - stage;
                            }

                            else {
                                low = stage - 1;
                                high = 6 - stage;
                            }

                            screenRects[high].X = 400;
                            screenRects[high].Y = 100;
                            screenRects[low].X = 101;
                            screenRects[low].Y = 240;
                        }
                        break;
                }
            }

            else {
                if(!prev.ToString().StartsWith("NumPad")) {
                    stage = 0;
                    originalScreens.CopyTo(screenRects, 0);
                }

                else if(Convert.ToInt32(prev.ToString().Substring(6)) != stage) {
                    stage = Convert.ToInt32(prev.ToString().Substring(6));
                    if(stage > 3) {
                        high = stage - 1;
                        low = 6 - stage;
                    }

                    else {
                        low = stage - 1;
                        high = 6 - stage;
                    }

                    screenRects[high].X = 400;
                    screenRects[high].Y = 100;
                    screenRects[low].X = 101;
                    screenRects[low].Y = 240;
                }

                if(screenRects[low].X - 100 <= 0 || screenRects[low].X + 100 >= 800)
                    bottomDuckSpeed *= -1;

                if(screenRects[high].X - 100 <= 0 || screenRects[high].X + 100 >= 800)
                    topDuckSpeed *= -1;

                screenRects[low].X += bottomDuckSpeed;
                screenRects[high].X += topDuckSpeed;
            }

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
            if(stage == 0) {
                for(int i = 0; i < 6; i++) {
                    
                        spriteBatch.Draw(ducks, screenRects[i], sourceRects[i], Color.White, MathHelper.ToRadians(duckRot[i]), duckCenter, SpriteEffects.None, 0);
                }
            }

            else {
                spriteBatch.Draw(ducks, screenRects[low], sourceRects[low], Color.White, MathHelper.ToRadians(originalDuckRot[low]), duckCenter, SpriteEffects.None, 0);

                if(screenRects[low].Intersects(screenRects[high])) 
                    spriteBatch.Draw(ducks, screenRects[high], sourceRects[high], Color.Blue, MathHelper.ToRadians(originalDuckRot[high]), duckCenter, SpriteEffects.None, 0);

                else
                    spriteBatch.Draw(ducks, screenRects[high], sourceRects[high], Color.White, MathHelper.ToRadians(originalDuckRot[high]), duckCenter, SpriteEffects.None, 0);
            }
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}

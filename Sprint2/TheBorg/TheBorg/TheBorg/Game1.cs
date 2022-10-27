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

namespace TheBorg {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D turret;
        Texture2D baseRect;
        Texture2D torpedo;
        SpriteFont font;
        Texture2D cube;
        Texture2D explosion;

        Rectangle turretRect;
        Rectangle topRect;
        Rectangle leftRect;
        Rectangle bottomRect;
        Rectangle rightRect;
        Rectangle torpedoRect;
        Rectangle torpedoDrawRect;
        Rectangle cubeRect;

        Keys[] numpad;
        Random r;

        int torpedoSelect;
        KeyboardState prev;
        int timeSinceLaunch;
        bool isTorpeding;
        float torpedoRotation;
        int dXTorpedo;
        int dYTorpedo;
        int mj;
        int selectedMj;
        int globalTime;
        bool isDestroyed;

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
            IsMouseVisible = true; // 19 x 35
            base.Initialize();
            turretRect = new Rectangle(325, 166, 150, 148);
            topRect = new Rectangle(391, 170, 19, 35);
            leftRect = new Rectangle(330, 230, 35, 19);
            bottomRect = new Rectangle(391, 275, 19, 35);
            rightRect = new Rectangle(435, 230, 35, 19);
            torpedoRect = new Rectangle(410, 170, 137, 19);
            torpedoDrawRect = new Rectangle(0, 0, 137, 19);
            cubeRect = new Rectangle(-200, -200, 100, 100);
            torpedoSelect = 0;
            prev = Keyboard.GetState();
            isTorpeding = false;
            timeSinceLaunch = 0;
            dXTorpedo = 0;
            dYTorpedo = -1;
            torpedoRotation = MathHelper.ToRadians(90);
            mj = 0;
            selectedMj = 0;
            globalTime = 0;
            numpad = new Keys[] { Keys.NumPad0, Keys.NumPad1, Keys.NumPad2, Keys.NumPad3, Keys.NumPad4, Keys.NumPad5, Keys.NumPad6, Keys.NumPad7, Keys.NumPad8, Keys.NumPad9 };
            r = new Random();
            isDestroyed = false;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            turret = Content.Load<Texture2D>("StarBase");
            baseRect = new Texture2D(GraphicsDevice, 1, 1);
            baseRect.SetData(new Color[] { Color.White });
            torpedo = Content.Load<Texture2D>("torpedo");
            font = Content.Load<SpriteFont>("SpriteFont1");
            cube = Content.Load<Texture2D>("BorgCube");
            explosion = Content.Load<Texture2D>("Photon Torpedo");
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
            // Allows the game to exit
            if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            globalTime++;
            // TODO: Add your update logic here

            if(isTorpeding) {
                timeSinceLaunch++;
                torpedoDrawRect.X += dXTorpedo;
                torpedoDrawRect.Y += dYTorpedo;
                if(timeSinceLaunch / 60 > 5)
                    isTorpeding = false;
            }

            if(!isTorpeding) {
                if(isEvent(k, Keys.Up)) {
                    torpedoSelect = 0;
                    torpedoRect.X = 410;
                    torpedoRect.Y = 170;
                    torpedoRotation = MathHelper.ToRadians(90);
                    dXTorpedo = 0;
                    dYTorpedo = -1;
                }

                else if(isEvent(k, Keys.Left)) {
                    torpedoSelect = 1;
                    torpedoRect.X = 330;
                    torpedoRect.Y = 230;
                    torpedoRotation = MathHelper.ToRadians(0);
                    dXTorpedo = -1;
                    dYTorpedo = 0;
                }

                else if(isEvent(k, Keys.Down)) {
                    torpedoSelect = 2;
                    torpedoRect.X = 391;
                    torpedoRect.Y = 307;
                    torpedoRotation = MathHelper.ToRadians(-90);
                    dXTorpedo = 0;
                    dYTorpedo = 1;
                }

                else if(isEvent(k, Keys.Right)) {
                    torpedoSelect = 3;
                    torpedoRect.X = 470;
                    torpedoRect.Y = 249;
                    torpedoRotation = MathHelper.ToRadians(180);
                    dXTorpedo = 1;
                    dYTorpedo = 0;
                }

                else if(isEvent(k, Keys.Space) && !isTorpeding) {
                    mj -= selectedMj;

                    isTorpeding = true;
                    timeSinceLaunch = 0;
                }
            }

            for(int i = 0; i < numpad.Length; i++) {
                if(isEvent(k, numpad[i]))
                    selectedMj = i;
            }

            selectedMj = Math.Min(mj, selectedMj);
            if(globalTime % 60 == 0) {
                mj = Math.Min(100, mj + 3);
            }

            if(globalTime % r.Next(180, 600) == 0) {
                isDestroyed = false;
                switch(r.Next(5)) {
                    case 0:
                        cubeRect.Y = 190;
                        cubeRect.X = r.Next(0, 250);
                        break;
                    case 1:
                        cubeRect.Y = 190;
                        cubeRect.X = r.Next(500, 700);
                        break;
                    case 2:
                        cubeRect.X = 350;
                        cubeRect.Y = r.Next(0, 140);
                        break;
                    case 3:
                        cubeRect.X = 350;
                        cubeRect.Y = r.Next(290, 380);
                        break;

                    default:
                        break;
                }
            }

            if (torpedoDrawRect.X > cubeRect.X && torpedoDrawRect.X < cubeRect.X + cubeRect.Width &&
                torpedoDrawRect.Y > cubeRect.Y && torpedoDrawRect.Y < cubeRect.Y + cubeRect.Height) isDestroyed = true;

            if(!isTorpeding) {
                if(Math.Abs(MathHelper.ToDegrees(torpedoRotation) % 180) < 1) {
                    torpedoDrawRect.Y = (int) (torpedoRect.Y + (selectedMj));
                    torpedoDrawRect.X = torpedoRect.X;
                }

                else {
                    torpedoDrawRect.X = (int) (torpedoRect.X + (selectedMj));
                    torpedoDrawRect.Y = torpedoRect.Y;
                }

                torpedoDrawRect.Height = (int) (torpedoRect.Height + (selectedMj * 1.5));

            }

            prev = k;
            base.Update(gameTime);
        }

        bool isEvent(KeyboardState state, Keys k) {
            return state.IsKeyDown(k) && prev.IsKeyUp(k);
        }

        Color torpedoColor(int desired) {
            if(torpedoSelect == desired && !isTorpeding) {
                return Color.Green;
            }
            else
                return Color.Red;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 
        Texture2D isBorgExploaded() {
            if (isDestroyed) return explosion;
            else return cube;
        }
        protected override void Draw(GameTime gameTime) { //gygfegwuyfgwegfuyg
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();
            spriteBatch.Draw(isBorgExploaded(), cubeRect, Color.White);
            spriteBatch.Draw(torpedo, torpedoDrawRect, null, Color.White, torpedoRotation, Vector2.Zero, SpriteEffects.None, 1.0f);
            spriteBatch.Draw(turret, turretRect, Color.White);
            spriteBatch.Draw(baseRect, topRect, torpedoColor(0));
            spriteBatch.Draw(baseRect, leftRect, torpedoColor(1));
            spriteBatch.Draw(baseRect, bottomRect, torpedoColor(2));
            spriteBatch.Draw(baseRect, rightRect, torpedoColor(3));
            spriteBatch.DrawString(font, "" + mj + " MJ", new Vector2(370, 220), Color.Black);
            spriteBatch.DrawString(font, "" + selectedMj + " MJ", new Vector2(370, 235), Color.Black);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}

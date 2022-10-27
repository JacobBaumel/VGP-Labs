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
        Rectangle borgTorp;
        Rectangle turretExplosion;
        Rectangle barsBg;
        Rectangle barLSU;
        Rectangle barSelectedPower;
        Rectangle barSelectedPropulsion;

        Vector2 barSelectedPowerText;
        Vector2 barSelectedPropulsionText;
        Vector2 barLSUText;

        Keys[] listens;
        Random r;
        KeyboardState prev;
        MouseState prevMouse;

        int torpedoSelect;
        int timeSinceLaunch;
        bool isTorpeding;
        float torpedoRotation;
        int dXTorpedo;
        int dYTorpedo;
        int dXBorgTorpedo;
        int dYBorgTorpedo;
        int mj;
        int selectedMj;
        int globalTime;
        bool isDestroyed;
        bool isFiringBack;
        float borgTorpedoRotation;
        int nextSwap;
        bool isHit;
        int propulsion;

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
            borgTorp = new Rectangle(-200, -200, 137, 19);
            turretExplosion = new Rectangle(375, 216, 50, 50);
            torpedoSelect = 0;
            prev = Keyboard.GetState();
            isTorpeding = false;
            timeSinceLaunch = 0;
            dXTorpedo = 0;
            dYTorpedo = -10;
            torpedoRotation = MathHelper.ToRadians(90);
            mj = 0;
            selectedMj = 0;
            globalTime = 0;
            listens = new Keys[] { Keys.NumPad0, Keys.NumPad1, Keys.NumPad2, Keys.NumPad3, Keys.NumPad4, Keys.NumPad5, Keys.NumPad6, Keys.NumPad7, Keys.NumPad8, Keys.NumPad9, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, Keys.Space, Keys.Right, Keys.Down, Keys.Left, Keys.Up };
            r = new Random();
            isDestroyed = false;
            isFiringBack = false;
            borgTorpedoRotation = 0.0f;
            nextSwap = 1;
            dXBorgTorpedo = 0;
            dYBorgTorpedo = 0;
            isHit = false;
            prevMouse = Mouse.GetState();
            propulsion = 1;
            barsBg = new Rectangle(0, 380, 200, 100);
            barLSU = new Rectangle(10, 390, 180, 20);
            barLSUText = new Vector2(210, 390);
            barSelectedPower = new Rectangle(10, 420, 180, 20);
            barSelectedPowerText = new Vector2(210, 420);
            barSelectedPropulsion = new Rectangle(10, 450, 180, 20);
            barSelectedPropulsionText = new Vector2(210, 450);
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

        void setPos() {

            switch(torpedoSelect) {
                case 0: // Up
                    torpedoRect.X = 410;
                    torpedoRect.Y = 170;
                    torpedoRotation = MathHelper.ToRadians(90);
                    dXTorpedo = 0;
                    dYTorpedo = -10;
                    break;

                case 1: // Left
                    torpedoRect.X = 330;
                    torpedoRect.Y = 230;
                    torpedoRotation = MathHelper.ToRadians(0);
                    dXTorpedo = -10;
                    dYTorpedo = 0;
                    break;

                case 2: // Down
                    torpedoRect.X = 391;
                    torpedoRect.Y = 307;
                    torpedoRotation = MathHelper.ToRadians(-90);
                    dXTorpedo = 0;
                    dYTorpedo = 10;
                    break;

                case 3: // Right
                    torpedoRect.X = 470;
                    torpedoRect.Y = 249;
                    torpedoRotation = MathHelper.ToRadians(180);
                    dXTorpedo = 10;
                    dYTorpedo = 0;
                    break;

                default:
                    break;

            }
        }

        void fire() {
            setPos();
            mj -= selectedMj;
            isTorpeding = true;
            timeSinceLaunch = 0;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            KeyboardState board = Keyboard.GetState();
            MouseState m = Mouse.GetState();
            // Allows the game to exit
            if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            globalTime++;
            Console.WriteLine(propulsion);
            // TODO: Add your update logic here

            for(int i = 0; i < listens.Length; i++) {
                if(isEvent(board, listens[i])) {
                    if(i < 10) selectedMj = i;

                    else if(i < 19) {
                        propulsion = i - 9;
                    }

                    else if(!isTorpeding) {
                        if(listens[i] != Keys.Space) {
                            torpedoSelect = listens.Length - i - 1;
                            setPos();
                        }

                        else {
                            fire();
                            
                        }
                    }

                }
            }

            if(isTorpeding) {
                timeSinceLaunch++;
                torpedoDrawRect.X += dXTorpedo;
                torpedoDrawRect.Y += dYTorpedo;
                switch(torpedoSelect) {
                    case 1:
                    case 3:
                        if(Math.Abs(torpedoRect.X - torpedoDrawRect.X) > propulsion * 100)
                            isTorpeding = false;
                        break;
                    case 0:
                    case 2:
                        if(Math.Abs(torpedoRect.Y - torpedoDrawRect.Y) > propulsion * 100)
                            isTorpeding = false;
                        break;

                    default:
                        break;
                }
            }

            if(!isTorpeding) {

                if(isMouseMoveEvent(m)) {
                    if(m.Y < downUpLine(m.X) && m.Y < upDownLine(m.X))
                        torpedoSelect = 0;
                    else if(m.Y < downUpLine(m.X) && m.Y > upDownLine(m.X))
                        torpedoSelect = 3;
                    else if(m.Y > downUpLine(m.X) && m.Y < upDownLine(m.X))
                        torpedoSelect = 1;
                    else
                        torpedoSelect = 2;

                    setPos();
                }

                if(hasClicked(m) && !isTorpeding) {
                    setPos();
                    fire();
                }
                
            }

            selectedMj = Math.Min(mj, selectedMj);
            if(globalTime % 60 == 0) {
                mj = Math.Min(100, mj + 3);
            }

            if(globalTime % nextSwap == 0 && !isFiringBack) {
                isDestroyed = false;
                isFiringBack = true;
                isHit = false;
                switch(r.Next(5)) {
                    case 0:
                        cubeRect.Y = 190;
                        cubeRect.X = r.Next(0, 250);
                        borgTorpedoRotation = MathHelper.ToRadians(180);
                        borgTorp.Y = 250;
                        borgTorp.X = cubeRect.X + cubeRect.Width;
                        dXBorgTorpedo = 1;
                        dYBorgTorpedo = 0;
                        break;
                    case 1:
                        cubeRect.Y = 190;
                        cubeRect.X = r.Next(500, 700);
                        borgTorpedoRotation = MathHelper.ToRadians(0);
                        borgTorp.Y = 250;
                        borgTorp.X = cubeRect.X;
                        dXBorgTorpedo = -1;
                        dYBorgTorpedo = 0;
                        break;
                    case 2:
                        cubeRect.X = 350;
                        cubeRect.Y = r.Next(0, 100);
                        borgTorpedoRotation = MathHelper.ToRadians(-90);
                        borgTorp.X = 400;
                        borgTorp.Y = cubeRect.Y + cubeRect.Height;
                        dXBorgTorpedo = 0;
                        dYBorgTorpedo = 1;
                        break;
                    case 3:
                        cubeRect.X = 350;
                        cubeRect.Y = r.Next(290, 380);
                        borgTorpedoRotation = MathHelper.ToRadians(90);
                        borgTorp.X = 400;
                        borgTorp.Y = cubeRect.Y;
                        dXBorgTorpedo = 0;
                        dYBorgTorpedo = -1;
                        break;

                    default:
                        break;
                }

                nextSwap = r.Next(180, 300);
            }

            if(isFiringBack) {
                borgTorp.X += dXBorgTorpedo;
                borgTorp.Y += dYBorgTorpedo;
                if(borgTorp.X > turretRect.X && borgTorp.X < turretRect.X + turretRect.Width && borgTorp.Y > turretRect.Y && borgTorp.Y < turretRect.Y + turret.Height) {
                    isFiringBack = false;
                    isHit = true;
                }
                if(isDestroyed)
                    isFiringBack = false;

            }

            if(torpedoDrawRect.X > cubeRect.X && torpedoDrawRect.X < cubeRect.X + cubeRect.Width &&
                torpedoDrawRect.Y > cubeRect.Y && torpedoDrawRect.Y < cubeRect.Y + cubeRect.Height)
                isDestroyed = true;

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

            prev = board;
            prevMouse = m;

            barLSU.Width = (int) ((180 / 100.0) * mj);
            barSelectedPower.Width = (int) ((180 / 9.0) * selectedMj);
            barSelectedPropulsion.Width = (int) ((180 / 9.0) * propulsion);

            base.Update(gameTime);
        }

        bool isEvent(KeyboardState state, Keys k) {
            return state.IsKeyDown(k) && prev.IsKeyUp(k);
        }

        bool isMouseMoveEvent(MouseState current) {
            return current != prevMouse;
        }

        bool hasClicked(MouseState current) {
            return current.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed;
        }

        double downUpLine(int x) {
            return 0.6 * x; 
        }

        double upDownLine(int x) {
            return 480 - (0.6 * x);
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
            if(isDestroyed)
                return explosion;
            else
                return cube;
        }
        protected override void Draw(GameTime gameTime) { 
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();
            spriteBatch.Draw(isBorgExploaded(), cubeRect, Color.White);
            spriteBatch.Draw(torpedo, torpedoDrawRect, null, Color.White, torpedoRotation, Vector2.Zero, SpriteEffects.None, 1.0f);
            spriteBatch.Draw(turret, turretRect, Color.White);
            if(isHit) spriteBatch.Draw(explosion, turretExplosion, Color.White);
            spriteBatch.Draw(baseRect, topRect, torpedoColor(0));
            spriteBatch.Draw(baseRect, leftRect, torpedoColor(1));
            spriteBatch.Draw(baseRect, bottomRect, torpedoColor(2));
            spriteBatch.Draw(baseRect, rightRect, torpedoColor(3));
            spriteBatch.DrawString(font, "" + mj + " MJ", new Vector2(370, 220), Color.Black);
            spriteBatch.DrawString(font, "" + selectedMj + " MJ", new Vector2(370, 235), Color.Black);
            spriteBatch.Draw(torpedo, borgTorp, null, Color.White, borgTorpedoRotation, Vector2.Zero, SpriteEffects.None, 1.0f);

            spriteBatch.Draw(baseRect, barsBg, Color.Black);
            spriteBatch.Draw(baseRect, barLSU, Color.Blue);
            spriteBatch.Draw(baseRect, barSelectedPower, Color.Blue);
            spriteBatch.Draw(baseRect, barSelectedPropulsion, Color.Blue);
            spriteBatch.DrawString(font, "LSU: " + mj, barLSUText, Color.Black);
            spriteBatch.DrawString(font, "Selected Power: " + selectedMj, barSelectedPowerText, Color.Black);
            spriteBatch.DrawString(font, "Selected Propulsion: " + propulsion, barSelectedPropulsionText, Color.Black);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}

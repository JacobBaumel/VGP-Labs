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

namespace CastleMania
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D background;

        Texture2D ghost;
        Texture2D herald;
        Texture2D knight;
        Texture2D sword;
        SpriteFont font;

        Rectangle ghostRect;
        Rectangle heraldRect;
        Rectangle knightRect;
        Rectangle knight2Rect;
        Rectangle swordRect;
        Rectangle sword2Rect;
        Rectangle ghostSword;

        string heraldText;
        float globalAlpha;
        int time;
        int zone;
        float sword1Rot;
        float sword2Rot;
        float knightRot;
        float heraldRot;
        float ghostAlpha;
        bool moveGhost;
        bool alphaDir;
        
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
            globalAlpha = 1f;
            ghostAlpha = 0f;
            time = 0;
            ghostRect = new Rectangle(500, 0, 200, 100);
            ghostSword = new Rectangle(ghostRect.X - 30, ghostRect.Y + 25, 50, 49);
            heraldRect = new Rectangle(650, 250, 130, 173);
            knightRect = new Rectangle(100, 250, 86, 150);
            knight2Rect = new Rectangle(400, 250, 86, 150);
            sword2Rect = new Rectangle(knight2Rect.X - 30, knight2Rect.Y + 50, 50, 49);
            swordRect = new Rectangle(knightRect.X + 86, knightRect.Y + 50, 50, 49);
            heraldText = "3";
            zone = 0;
            sword1Rot = 0;
            sword2Rot = 0;
            knightRot = 0;
            heraldRot = 0;
            moveGhost = false;
            alphaDir = false;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("castle");
            ghost = Content.Load<Texture2D>("ghost");
            herald = Content.Load<Texture2D>("herald");
            knight = Content.Load<Texture2D>("knight");
            sword = Content.Load<Texture2D>("sword");
            font = Content.Load<SpriteFont>("font");


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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            time++;
            if(zone == 0) {
                if (time / 60 > 5) zone++;
                else if (time / 60 > 4) heraldText = "You may duel!";
                else if (time / 60 > 3) heraldText = "1";
                else if (time / 60 > 2) heraldText = "2";
                else heraldText = "3";
            }

            else if(zone == 1) {
                sword1Rot = MathHelper.ToRadians(45);
                sword2Rot = -sword1Rot;
                swordRect.Y = knightRect.Y + 75;
                swordRect.X += 15;
                sword2Rect.Y = knight2Rect.Y + 95;
                sword2Rect.X -= 15;
                heraldText = "AAH! They both \nthrew their swords!";
                if (swordRect.X > 850) {
                    zone++;
                    heraldRot = MathHelper.ToRadians(90);
                    heraldRect.X += 100;
                    heraldText = "Oh no! I've been \nstabbed!";
                }
            }

            else if(zone == 2) {
                knightRot += MathHelper.ToRadians(1);
                if (MathHelper.ToDegrees(knightRot) > 50) zone++;
                knight2Rect.Y++;
            }

            else if(zone == 3) {
                heraldText = "Wow! Are they going\nto charge each other?";
                knightRect.X = (int)(knightRect.X + 1);
                knight2Rect.X = (int)(knight2Rect.X - 1);
                if (knightRect.X > 200) zone++;
            }

            else if(zone == 4) {
                heraldText = "Oh no! Its the\nghost knight!";
                ghostAlpha = (float) Math.Min(1, ghostAlpha + 0.005);
                if (ghostAlpha >= 1f) {
                    zone++;
                    ghostAlpha = 1f;
                }
            }

            else if(zone == 5) {
                heraldText = "Oh no, an\neclipse!";
                globalAlpha = (float) Math.Max(0, globalAlpha - 0.005);
                ghostAlpha = (float) Math.Max(0, ghostAlpha - 0.005);
                if(ghostAlpha <= 0) {
                    zone++;
                    ghostAlpha = 0f;
                    globalAlpha = 0f;
                    time = 0;
                }
            }

            else if(zone == 6) {
                heraldText = "Oh no! He's stabbed \neveryone! I'm dead!";
                knightRot = MathHelper.ToRadians(90);
                sword1Rot = MathHelper.ToRadians(90);
                sword2Rot = -sword1Rot;
                swordRect.X = 90;
                swordRect.Y = 250;
                sword2Rect.X = 340;
                sword2Rect.Y = 250;
                globalAlpha = (float) Math.Min(1, globalAlpha + 0.05);
                if(time / 60 > 4) {
                    zone++;
                }
            }

            else if(zone == 7) {
                heraldText = "Wow, now the\n ghost knight escaped!";
                if (moveGhost) {
                    ghostRect.X--;
                    ghostSword.X--;
                }
                moveGhost = !moveGhost;

                if (alphaDir)
                {
                    ghostAlpha = (float)Math.Min(1, ghostAlpha + 0.05);
                }

                else ghostAlpha = (float)Math.Max(0, ghostAlpha - 0.05);

                if (ghostAlpha <= 0 || ghostAlpha >= 1) alphaDir = !alphaDir;
            }



            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            Color alpha = new Color(globalAlpha, globalAlpha, globalAlpha, globalAlpha);

            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), alpha);
            spriteBatch.Draw(herald, heraldRect, null, alpha, heraldRot, Vector2.Zero, SpriteEffects.None, 0f);
            spriteBatch.Draw(knight, knightRect, null, alpha, knightRot, Vector2.Zero, SpriteEffects.FlipHorizontally, 1f);
            spriteBatch.Draw(sword, swordRect, null, alpha, sword1Rot, Vector2.Zero, SpriteEffects.None, 0f);

            spriteBatch.Draw(knight, knight2Rect, null, alpha, -knightRot, Vector2.Zero, SpriteEffects.None, 0f);
            spriteBatch.Draw(sword, sword2Rect, null, alpha, sword2Rot, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
            spriteBatch.DrawString(font, heraldText, new Vector2(550, 425), alpha);

            spriteBatch.Draw(ghost, ghostRect, new Color(ghostAlpha, ghostAlpha, ghostAlpha, ghostAlpha));
            spriteBatch.Draw(sword, ghostSword, null, new Color(ghostAlpha, ghostAlpha, ghostAlpha, ghostAlpha), 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

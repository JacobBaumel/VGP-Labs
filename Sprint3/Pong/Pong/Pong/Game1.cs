using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Pong {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D sprites;
        SpriteFont font;

        Rectangle ballRect;
        Rectangle ballDraw;
        Rectangle screenImage;
        Rectangle ballSourceRect;
        Rectangle squareRect;
        Rectangle squareSourceRect;
        Rectangle paddle1TextureSource;
        Rectangle paddle2TextureSource;
        Rectangle paddle1;
        Rectangle paddle2;

        Vector2 p1pointsVec;
        Vector2 p2pointsVec;
        Vector2 p1gamevec;
        Vector2 p2gamevec;
        Vector2 ballCenter;

        double ballSpeedX;
        double ballSpeedY;

        int p1games;
        int p2games;
        int p1points;
        int p2points;
        double spin;
        double screenRot;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize() {
            int screenWidth = graphics.GraphicsDevice.Viewport.Width;
            int screenHeight = graphics.GraphicsDevice.Viewport.Height;
            ballRect = new Rectangle(50, 50, 20, 20);
            ballDraw = new Rectangle(50, 50, 20, 20);
            ballSpeedX = 4;
            ballSpeedY = 4;

            screenImage = new Rectangle(0, 0, 800, 480);
            squareSourceRect = new Rectangle(867, 714, 16, 16);
            squareRect = new Rectangle(392, 0, 16, 16);
            paddle1TextureSource = new Rectangle(801, 714, 32, 128);
            paddle2TextureSource = new Rectangle(1515, 0, 32, 128);
            paddle1 = new Rectangle(32, 176, 32, 128);
            paddle2 = new Rectangle(736, 176, 32, 128);
            ballCenter = new Vector2(356.5f, 356.5f);
            ballSourceRect = new Rectangle(801, 0, 713, 713);
            p1games = 0;
            p2games = 0;
            p1points = 0; // fixing points
            p2points = 0;

            p1pointsVec = Vector2.Zero;
            p2pointsVec = new Vector2(600, 0);
            p1gamevec = new Vector2(0, 450);
            p2gamevec = new Vector2(600, 450);
            spin = 0;
            screenRot = 0;
            respawnBall();
            base.Initialize();
        }

        void respawnBall() {
            ballRect.X = (800 - ballRect.Width) / 2;
            ballRect.Y = (480 - ballRect.Height) / 2;
            Random r = new Random(DateTime.Now.Second + DateTime.Now.Millisecond);

            do {
                ballSpeedX = r.Next(-3, 3);
            } while(ballSpeedX == 0);
            ballSpeedX *= 2;

            do {
                ballSpeedY = r.Next(-3, 3);
            } while(ballSpeedY == 0);
            ballSpeedY *= 2;

            spin = r.Next(-20, 20);
        }

        void resetPoints() {
            p1points = 0;
            p2points = 0;
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            sprites = Content.Load<Texture2D>("Pong Sprite Sheet");
            font = Content.Load<SpriteFont>("SpriteFont1");
        }

        protected override void UnloadContent() {
        }

        void ySpin() {
            ballSpeedY += 0.1 * spin;
            spin = copySign(Math.Abs(spin * 0.75), spin); 
        }
        void xSpin() {
            ballSpeedX += 0.1 * spin;
            spin = copySign(Math.Abs(spin * 0.75), spin);
        }

        protected override void Update(GameTime gameTime) {
            KeyboardState k = Keyboard.GetState();
            if(k.IsKeyDown(Keys.Escape))
                this.Exit();

            ballRect.Y += (int) ballSpeedY;
            ballRect.X += (int) ballSpeedX;

            if(ballRect.Intersects(paddle1)) {
                if(ballRect.X < paddle1.X + paddle1.Width - Math.Abs(ballSpeedX) - 1) {
                    if(ballRect.Y < paddle1.Y + (paddle1.Height / 2))
                        ballSpeedY = copySign(ballSpeedY, -1);
                    else
                        ballSpeedY = copySign(ballSpeedY, 1);

                    xSpin();
                }

                else {
                    ballSpeedX *= -1;
                    ySpin();
                }
            }

            if(ballRect.Intersects(paddle2)) {
                if(ballRect.X + ballRect.Width > paddle2.X + Math.Abs(ballSpeedX) + 1) {
                    if(ballRect.Y < paddle2.Y + (paddle2.Height / 2))
                        ballSpeedY = copySign(ballSpeedY, -1);
                    else
                        ballSpeedY = copySign(ballSpeedY, 1);

                    xSpin();
                }

                else {
                    ballSpeedX *= -1;
                    ySpin();
                }
            }

            if(ballRect.Y <= 1 || ballRect.Y + ballRect.Height >= 480) {
                ballSpeedY *= -1;
                xSpin();
            }

            if(ballRect.X <= 2) {
                p2points++;
                respawnBall();
            }

            else if(ballRect.X + ballRect.Width >= 800) {
                p1points++;
                respawnBall();
            }


            if(k.IsKeyDown(Keys.W))
                paddle1.Y -= 4;
            if(k.IsKeyDown(Keys.S))
                paddle1.Y += 4;
            if(k.IsKeyDown(Keys.Up))
                paddle2.Y -= 4;
            if(k.IsKeyDown(Keys.Down))
                paddle2.Y += 4;

            paddle1.Y = (int) MathHelper.Clamp(paddle1.Y, 0, 352);
            paddle2.Y = (int) MathHelper.Clamp(paddle2.Y, 0, 352);

            if(p1points >= 11 && p1points - p2points >= 2) {
                respawnBall();
                resetPoints();
                p1games++;
            }

            else if(p2points >= 11 && p2points - p1points >= 2) {
                respawnBall();
                resetPoints();
                p2games++;
            }

            screenRot += spin;

            ballDraw.X = ballRect.X + 10;
            ballDraw.Y = ballRect.Y + 10;


            base.Update(gameTime);
        }

        int copySign(int val, int sign) {
            if(sign < 0)
                return -Math.Abs(val);
            else if(sign == 0)
                return 0;
            else
                return Math.Abs(val);
        } 

        double copySign(double val, double sign) {
            if(sign < 0)
                return -Math.Abs(val);
            else if(sign == 0)
                return 0;
            else
                return Math.Abs(val);
        }
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();


            spriteBatch.Draw(sprites, screenImage, screenImage, Color.White);
            for(squareRect.Y = 8; squareRect.Y < 480; squareRect.Y += 32)
                spriteBatch.Draw(sprites, squareRect, squareSourceRect, Color.White);
            spriteBatch.Draw(sprites, paddle1, paddle1TextureSource, Color.White);
            spriteBatch.Draw(sprites, paddle2, paddle2TextureSource, Color.White);
            spriteBatch.Draw(sprites, ballDraw, ballSourceRect, Color.White, MathHelper.ToRadians((float) screenRot), ballCenter, SpriteEffects.None, 1.0f);
            spriteBatch.DrawString(font, "Points: " + p1points, p1pointsVec, Color.CornflowerBlue);
            spriteBatch.DrawString(font, "Points: " + p2points, p2pointsVec, Color.LightGreen);
            spriteBatch.DrawString(font, "Games: " + p1games, p1gamevec, Color.CornflowerBlue);
            spriteBatch.DrawString(font, "Games: " + p2games, p2gamevec, Color.LightGreen);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

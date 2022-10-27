using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HuffnPuff {
    public class Game1 : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D sprites;
        SpriteFont font;
        
        Rectangle boyrect;
        Rectangle featherRect;

        Rectangle[] boyFrames;
        Rectangle[] featherFrames;

        Random r;


        KeyboardState prev;
        bool dir; // False is left
        int boyFrame;
        int featherFrame;
        int frameChangeCounter;
        int featherFrameCounter;
        int points;
        bool addpoints;
        bool gameover;

        Vector2 featherd;
        float featherTime;

        Vector2 gameoverTextScreen;

        

        void readIn() {
            try {
                using(StreamReader r = new StreamReader(@"Content/HuffNPuff.txt")) {
                    boyFrames = new Rectangle[10];
                    for(int i = 0; i < 10; i++) {
                        string[] line = r.ReadLine().Split(' ');
                        boyFrames[i] = new Rectangle(convert(line[0]), convert(line[1]), convert(line[2]), convert(line[3]));
                    }

                    featherFrames = new Rectangle[4];
                    for(int i = 0; i < 4; i++) {
                        string[] line = r.ReadLine().Split(' ');
                        featherFrames[i] = new Rectangle(convert(line[0]), convert(line[1]), convert(line[2]), convert(line[3]));
                    }
                }
            } catch(Exception e) {
                Console.WriteLine(e.Message);
            }
        }

        int convert(string s) {
            return Convert.ToInt32(s);
        }

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize() {
            readIn();
            boyrect = new Rectangle(0, 344, 96, 136);
            featherRect = new Rectangle(376, 216, 48, 48);
            featherd = new Vector2();
            gameReset();
            base.Initialize();
        }

        void gameReset() {
            boyFrame = 0;
            boyrect.X = (800 - boyrect.Width) / 2;
            dir = false; // False is left
            frameChangeCounter = 0;
            featherFrameCounter = 0;
            featherFrame = 0;
            r = new Random(DateTime.Now.Millisecond);
            featherRect.X = 376;
            featherRect.Y = 216;
            featherTime = 0;
            featherd.Y = 0;
            featherd.X = 0;
            points = 0;
            addpoints = false;
            gameover = false;
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            sprites = Content.Load<Texture2D>("HuffNPuff");
            font = Content.Load<SpriteFont>("SpriteFont1");

            gameoverTextScreen = font.MeasureString("Game over! Your points: $$\nPress R to restart\nPress Escape to exit");
            gameoverTextScreen.X = (800 - gameoverTextScreen.X) / 2;
            gameoverTextScreen.Y = (480 - gameoverTextScreen.Y) / 2;
        }

        protected override void UnloadContent() {
        }

        protected override void Update(GameTime gameTime) {
            KeyboardState k = Keyboard.GetState();
            if(featherRect.Y + featherRect.Height < boyrect.Y && !gameover)
                addpoints = true;

            if(gameover)
                addpoints = false;

            if(k.IsKeyDown(Keys.Escape))
                Exit();

            else if(k.IsKeyDown(Keys.R))
                gameReset();

            if(k.IsKeyDown(Keys.Space)) {
                boyFrame = 4;
                frameChangeCounter = 1;

                if(boyrect.Intersects(featherRect) && addpoints) {
                    featherTime = 0;
                    points++;
                    addpoints = false;
                }
            }

            else if(k.IsKeyDown(Keys.Left)) {
                boyrect.X -= 4;
                dir = false;
                frameChangeCounter++;
            }

             else if(k.IsKeyDown(Keys.Right)) {
                boyrect.X += 4;
                dir = true;
                frameChangeCounter++;
            }
            

            else {
                boyFrame = 0;
            }

            if(frameChangeCounter % 5 == 0) {
                boyFrame++;
                boyFrame %= 4;
            }

            if(++featherFrameCounter % 5 == 0)
                featherFrame = (featherFrame + 1) % 4;

            boyrect.X = (int) MathHelper.Clamp(boyrect.X, 0, 800 - boyrect.Width);
            featherd.Y = -4 + ((float) (2.5 * (featherTime / 60)));
            featherRect.X = (int) MathHelper.Clamp(featherRect.X + featherd.X, 0, 800 - featherRect.Width);
            featherRect.Y = (int) MathHelper.Clamp(featherRect.Y + featherd.Y, 0, 480 - featherRect.Height);

            if(featherTime % 180 == 0)
                featherd.X = (float) ((r.NextDouble() * 8) - 4);

            if(featherRect.Y >= 425)
                gameover = true;
            featherTime++;
            prev = k;
            base.Update(gameTime);
        }

        int getOffset() {
            if(dir)
                return 5;
            else
                return 0;
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(sprites, boyrect, boyFrames[boyFrame + getOffset()], Color.White);
            spriteBatch.Draw(sprites, featherRect, featherFrames[featherFrame], Color.White);
            if(!gameover)
                spriteBatch.DrawString(font, "Points: " + points, Vector2.Zero, Color.White);
            else
                spriteBatch.DrawString(font, "Game over! Your points: " + points + "\nPress R to restart\nPress Escape to exit", gameoverTextScreen, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

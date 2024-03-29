using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

namespace DungeonCrawl {
    public class Game1 : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static int WIDTH = 100;
        public static int HEIGHT = 50;
        int gingerMidX;
        int gingerMidY;

        Tile[,] tiles;

        Texture2D gingerBread;
        Rectangle gingerDraw;

        int xpos;
        int ypos;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize() {
            xpos = 0;
            ypos = 0;
            base.Initialize();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            gingerBread = Content.Load<Texture2D>("Gingerbread Man");
            gingerDraw = new Rectangle((800 - (gingerBread.Width * 2)) / 2, (480 - (gingerBread.Height * 2)) / 2, gingerBread.Width * 2, gingerBread.Height * 2);

            gingerMidX = gingerDraw.X;
            gingerMidY = gingerDraw.Y;

            // Tile reading area
            int totalTiles;

            // Read in special animated tiles
            Dictionary<int, int> framenums = new Dictionary<int, int>();
            using(StreamReader r = new StreamReader(@"Content\meta.txt")) {
                totalTiles = Convert.ToInt32(r.ReadLine());
                while(!r.EndOfStream) {
                    string[] line = r.ReadLine().Split(' ');
                    if(line.Length != 2)
                        break;
                    framenums.Add(Convert.ToInt32(line[0]), Convert.ToInt32(line[1]));
                }
            }

            Texture2D[] availableTiles = new Texture2D[totalTiles];

            for(int i = 0; i < totalTiles; i++) {
                availableTiles[i] = Content.Load<Texture2D>("" + i);
            }

            Random rand = new Random((int) DateTime.Now.Ticks);

            tiles = new Tile[WIDTH, HEIGHT];

            for(int i = 0; i < WIDTH; i++) {
                for(int j = 0; j < HEIGHT; j++) {
                    int tex = rand.Next(0, totalTiles);
                    if(framenums.ContainsKey(tex))
                        tiles[i, j] = new Tile(availableTiles[tex], 32 * i, 32 * j, framenums[tex]);

                    else
                        tiles[i, j] = new Tile(availableTiles[tex], 32 * i, 32 * j, 1);
                }
            }
            

        }

        protected override void UnloadContent() {
        }

        protected override void Update(GameTime gameTime) {
            GamePadState state = GamePad.GetState(PlayerIndex.One);
            if(state.Buttons.Back == ButtonState.Pressed)
                this.Exit();

            int xdir = (int) (state.ThumbSticks.Right.X * 10);

            if(xpos < 1) {
                if(xdir < 0) {
                    gingerDraw.X = (int) MathHelper.Clamp(gingerDraw.X + xdir, 0, gingerMidX);
                }

                else {
                    if(gingerDraw.X < gingerMidX) {
                        gingerDraw.X = (int) MathHelper.Clamp(gingerDraw.X + xdir, 0, gingerMidX);
                    }

                    else {
                        xpos = (int) MathHelper.Clamp(xpos + xdir, 0, (WIDTH * 32) - 800);
                    }
                }
            }

            else if(xpos > (WIDTH * 32) - 801) {
                if(xdir > 0) {
                    gingerDraw.X = (int) MathHelper.Clamp(gingerDraw.X + xdir, gingerMidX, 800 - gingerDraw.Width);
                }

                else {
                    if(gingerDraw.X > gingerMidX)
                        gingerDraw.X = (int) MathHelper.Clamp(gingerDraw.X + xdir, gingerMidX, 800 - gingerDraw.Width);
                    else
                        xpos = (int) MathHelper.Clamp(xpos + xdir, 0, (WIDTH * 32) - 800);
                }
            }

            else {
                xpos = (int) MathHelper.Clamp(xpos + xdir, 0, (WIDTH * 32) - 800);
            }


            int ydir = (int) (-state.ThumbSticks.Right.Y * 10);

            if(ypos < 1) {
                if(ydir < 0) {
                    gingerDraw.Y = (int) MathHelper.Clamp(gingerDraw.Y + ydir, 0, gingerMidY);
                }

                else {
                    if(gingerDraw.Y < gingerMidY) {
                        gingerDraw.Y = (int) MathHelper.Clamp(gingerDraw.Y + ydir, 0, gingerMidY);
                    }

                    else {
                        ypos = (int) MathHelper.Clamp(ypos + ydir, 0, (HEIGHT * 32) - 480);
                    }
                }
            }

            else if(ypos > (HEIGHT * 32) - 481) {
                if(ydir > 0) {
                    gingerDraw.Y = (int) MathHelper.Clamp(gingerDraw.Y + ydir, gingerMidY, 480 - gingerDraw.Height);
                }

                else {
                    if(gingerDraw.Y > gingerMidY) {
                        gingerDraw.Y = (int) MathHelper.Clamp(gingerDraw.Y + ydir, gingerMidY, 480 - gingerDraw.Height);
                    }

                    else {
                        ypos = (int) MathHelper.Clamp(ypos + ydir, 0, (HEIGHT * 32) - 480);
                    }
                }
            }

            else {
                ypos = (int) MathHelper.Clamp(ypos + ydir, 0, (HEIGHT * 32) - 480);
            }

            Console.WriteLine(ypos + "    " + ydir);

            //ypos -= (int) (state.ThumbSticks.Right.Y * 10);

            //ypos = (int) (MathHelper.Clamp(ypos, 0, (HEIGHT * 32) - 800));

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            for(int i = 0; i < WIDTH; i++) {
                for(int j = 0; j < HEIGHT; j ++) {
                    tiles[i, j].draw(spriteBatch, xpos, ypos);
                }
            }
            spriteBatch.Draw(gingerBread, gingerDraw, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

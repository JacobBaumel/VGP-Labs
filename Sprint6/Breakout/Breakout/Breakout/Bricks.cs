using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace Breakout {
    class Bricks {
        public const int BRICK_WIDTH = 40;
        public const int BRICK_HEIGHT = 15;
        public const int BRICK_LAYERS = 40;

        Brick[,] bricks;
        Texture2D baseRect;
        public bool isOver;

        public Bricks(Texture2D _baseRect, string file) {
            baseRect = _baseRect;
            bricks = new Brick[BRICK_LAYERS, Game1.SCREEN_WIDTH / BRICK_WIDTH];
            
            try {
                using(StreamReader r = new StreamReader(file)) {
                    for(int i = 0; i < bricks.GetLength(0); i++) {
                        string line = r.ReadLine();
                        for(int j = 0; j < bricks.GetLength(1); j++) {
                            bricks[i, j] = new Brick(j * BRICK_WIDTH, i * BRICK_HEIGHT, baseRect, enumFromString(line[j]));
                        }
                    }
                }
            } catch(Exception e) {
                Console.WriteLine("Error starting game");
                Console.WriteLine(e.Message);
            }
            isOver = false;
        }

        public void update(Player p) {
            if(isOver)
                return;
            int ay = p.circleRect.Y / (BRICK_HEIGHT);
            int ax = (p.circleRect.X / (Game1.SCREEN_WIDTH / BRICK_WIDTH)) / 2;
            Console.WriteLine("Size: " + bricks.GetLength(1) + "  " + bricks.GetLength(0) + "\n Pos: " + ax + "   " + ay);
            isOver = true;
            foreach(Brick b in bricks)
                if(b.state != BrickState.NONE)
                    isOver = false;
            for(int i = 0; i <= 1; i++) {
                for(int j = 0; j <= 1; j++) {
                    if(ay + i < 0 || ax + j < 0 || ay + i >= bricks.GetLength(0) || ax + j >= bricks.GetLength(1) || bricks[ay + i, ax + j].state == BrickState.NONE)
                        continue;
                    bounceBrick(p, bricks[ay + i, ax + j]);
                }
            }
        }

        void bounceBrick(Player p, Brick b) { // Instead of just checking one brickj at a time, treat multiple bricks as one by just checking 4 edges
            if(b.r.Intersects(p.circleRect)) {
                b.state--;


                if(p.circleRect.X <= b.r.X + p.vx)
                    copySign(ref p.vx, -1);

                else if(p.circleRect.X >= b.r.X + b.r.Width + p.vx)
                    copySign(ref p.vx, 1);

                else if(p.circleRect.Y >= b.r.Y + b.r.Height + p.vy)
                    copySign(ref p.vy, 1);

                else
                    copySign(ref p.vy, -1);

                //if(p.circleRect.Y < b.r.Y) {
                //    if(p.circleRect.X <= b.r.X + p.vx)
                //        p.vx = copySign(p.vx, -1);

                //    else if(p.circleRect.X >= b.r.X + b.r.Width + p.vx)
                //        p.vx = copySign(p.vx, -1);

                //    else
                //        p.vy = copySign(p.vy, -1);
                //}

                //else {
                //    if(p.circleRect.X <= b.r.X + p.vx)
                //        p.vx = copySign(p.vx, -1);

                //    else if(p.circleRect.X >= b.r.X + b.r.Width + p.vx)
                //        p.vx = copySign(p.vx, -1);

                //    else p.vy = copySign(p.vy, 1);
                //}
            }
        }

        private void copySign(ref int val, int sign) {
            if(sign < 0)
                val = -Math.Abs(val);
            else if(sign == 0)
                val = 0;
            else
                val = Math.Abs(val);
        }

        public void draw(SpriteBatch spriteBatch) {
            foreach(Brick b in bricks) {
                b.draw(spriteBatch);
            }
        }

        public enum BrickState { // Game state can also represent HP because enums are integer backed
            NONE,
            RED,
            ORANGE,
            YELLOW,
            GREEN,
            BLUE,
            PURPLE
        }

        private BrickState enumFromString(char s) {
            switch(s) {
                case 'r':
                    return BrickState.RED;
                case 'o':
                    return BrickState.ORANGE;
                case 'y':
                    return BrickState.YELLOW;
                case 'g':
                    return BrickState.GREEN;
                case 'b':
                    return BrickState.BLUE;
                case 'p':
                    return BrickState.PURPLE;
                default:
                    return BrickState.NONE;
            }
        }


        class Brick {
            public Rectangle r;
            Texture2D baseRect;
            public BrickState state;

            public Brick(int x, int y, Texture2D _baseRect, BrickState _state) {
                r = new Rectangle(x, y, BRICK_WIDTH, BRICK_HEIGHT);
                baseRect = _baseRect;
                state = _state;
            }

            public void draw(SpriteBatch spriteBatch) {
                if(state != BrickState.NONE)
                    spriteBatch.Draw(baseRect, r, getColor());
            }

            private Color getColor() {
                switch(state) {
                    case BrickState.RED:
                        return Color.Red;
                    case BrickState.ORANGE:
                        return Color.Orange;
                    case BrickState.YELLOW:
                        return Color.Yellow;
                    case BrickState.GREEN:
                        return Color.Green;
                    case BrickState.BLUE:
                        return Color.Blue;
                    case BrickState.PURPLE:
                        return Color.Purple;

                    default:
                        return Color.Black;
                }
            }
        }
    }
}

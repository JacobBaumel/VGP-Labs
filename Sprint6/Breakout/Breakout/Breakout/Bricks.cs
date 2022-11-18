using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace Breakout {
    class Bricks {
        public const int BRICK_WIDTH = 10;
        public const int BRICK_HEIGHT = 4;
        public const int BRICK_LAYERS = 10;

        Brick[,] bricks;
        Texture2D baseRect;

        public Bricks(Texture2D _baseRect, string file) {
            baseRect = _baseRect;
            bricks = new Brick[BRICK_LAYERS, Game1.SCREEN_WIDTH / BRICK_WIDTH];
            
            try {
                using(StreamReader r = new StreamReader(file)) {
                    for(int i = 0; i < bricks.GetLength(0); i++) {
                        string line = r.ReadLine();
                        for(int j = 0; j < bricks.GetLength(1); j++) {
                            bricks[i, j] = new Brick(i * BRICK_WIDTH, j * BRICK_HEIGHT, baseRect, enumFromString(line[j]));
                        }
                    }
                } 
            } catch(Exception e) {
                Console.WriteLine("Error initializing game!");
                Console.WriteLine(e.Message);
            }
        }

        public void update() {
            foreach(Brick b in bricks) {
                
            }
        }

        public void draw(SpriteBatch spriteBatch) {
            foreach(Brick b in bricks) {
                b.draw(spriteBatch);
            }
        }

        public enum BrickState {
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
            Rectangle r;
            Texture2D baseRect;
            BrickState state;

            public Brick(int x, int y, Texture2D _baseRect, BrickState _state) {
                r = new Rectangle(x, y, BRICK_WIDTH, BRICK_HEIGHT);
                baseRect = _baseRect;
                state = _state;
            }

            public void draw(SpriteBatch spriteBatch) {
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

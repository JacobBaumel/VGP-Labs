using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LightCycle {
    class Pixels {
        protected Texture2D bikeBlue;
        protected Texture2D bikeOrange;
        protected Texture2D baseRect;
        public int p1Rot;
        public int p2Rot;

        public Pixel[,] pixels;

        public Pixels(Texture2D _bikeBlue, Texture2D _bikeOrange, Texture2D _baseRect) {
            bikeBlue = _bikeBlue;
            bikeOrange = _bikeOrange;
            baseRect = _baseRect;
            p1Rot = 90;
            p2Rot = 0;

            pixels = new Pixel[94, 94];
            for(int i = 0; i < pixels.GetLength(0); i++) {
                for(int j = 0; j < pixels.GetLength(1); j++) {
                    pixels[i, j] = new Pixel(i, j, this);
                }
            }

        }

        public void draw(SpriteBatch batch) {
            for(int i = 0; i < pixels.GetLength(0); i++) {
                for(int j = 0; j < pixels.GetLength(1); j++) {
                    pixels[i, j].render(batch);
                }
            } 
        }

        public enum PixelState {
            CLEAR,
            LINE_BLUE,
            LINE_ORANGE,
            LINE_SIDE,
            CYCLE_ORANGE_BACK,
            CYCLE_ORANGE_FRONT,
            CYCLE_BLUE_BACK,
            CYCLE_BLUE_FRONT
        }
        

        public class Pixel {
            const int screenPixelSize = 8;
            static Color orange = new Color(255, 95, 31);
            static Color blue = new Color(35, 225, 233);
            static Color grey = new Color(125, 125, 125);
            static Rectangle cycleBack = new Rectangle(0, 0, 124, 90);
            static Rectangle cycleFront = new Rectangle(125, 0, 124, 90);

            int x;
            int y;
            Rectangle pixelRectangle;
            Rectangle cycleRect;
            Vector2 cycleCenter;
            PixelState state;
            readonly Pixels pixels;
            public Pixel(int _x, int _y, Pixels _pixels) {
                x = _x;
                y = _y;
                pixels = _pixels;
                state = PixelState.CLEAR;
                pixelRectangle = new Rectangle(x * screenPixelSize, y * screenPixelSize, screenPixelSize, screenPixelSize);
                cycleCenter = new Vector2(63, 45);
                cycleRect = new Rectangle(pixelRectangle.X + (screenPixelSize / 2), pixelRectangle.Y + (screenPixelSize / 2), screenPixelSize, screenPixelSize);
            }

            public void set(PixelState p) {
                state = p;
            }

            public void clear() {
                state = PixelState.CLEAR;
            }

            public void render(SpriteBatch spriteBatch) {
                switch(state) {
                    case PixelState.CLEAR:
                        spriteBatch.Draw(pixels.baseRect, pixelRectangle, Color.Black);
                        break;

                    case PixelState.LINE_BLUE:
                        spriteBatch.Draw(pixels.baseRect, pixelRectangle, blue);
                        break;

                    case PixelState.LINE_ORANGE:
                        spriteBatch.Draw(pixels.baseRect, pixelRectangle, orange);
                        break;

                    case PixelState.LINE_SIDE:
                        spriteBatch.Draw(pixels.baseRect, pixelRectangle, grey);
                        break;

                    case PixelState.CYCLE_ORANGE_BACK:
                        spriteBatch.Draw(pixels.bikeOrange, cycleRect, cycleBack, Color.White, MathHelper.ToRadians(pixels.p1Rot), cycleCenter, SpriteEffects.None, 0);
                        break;

                    case PixelState.CYCLE_ORANGE_FRONT:
                        spriteBatch.Draw(pixels.bikeOrange, cycleRect, cycleFront, Color.White, MathHelper.ToRadians(pixels.p1Rot), cycleCenter, SpriteEffects.None, 0);
                        break;

                    case PixelState.CYCLE_BLUE_BACK:
                        spriteBatch.Draw(pixels.bikeBlue, cycleRect, cycleBack, Color.White, MathHelper.ToRadians(pixels.p2Rot), cycleCenter, SpriteEffects.None, 0);
                        break;

                    case PixelState.CYCLE_BLUE_FRONT:
                        spriteBatch.Draw(pixels.bikeBlue, cycleRect, cycleFront, Color.White, MathHelper.ToRadians(pixels.p2Rot), cycleCenter, SpriteEffects.None, 0);
                        break;

                }
            }

            
        }
    }
}

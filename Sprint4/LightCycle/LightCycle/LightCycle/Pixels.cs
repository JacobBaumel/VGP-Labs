using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LightCycle {
    class Pixels {
        Texture2D bikeBlue;
        Texture2D bikeOrange;
        protected Texture2D baseRect;

        Pixel[,] pixels;

        public Pixels(Texture2D _bikeBlue, Texture2D _bikeOrange, Texture2D _baseRect) {
            bikeBlue = _bikeBlue;
            bikeOrange = _bikeOrange;
            baseRect = _baseRect;

            pixels = new Pixel[188, 188];

        }

        public class Pixel {
            const int screenPixelSize = 4;
            static Color orange = new Color(255, 95, 31);
            static Color blue = new Color(35, 225, 233);
            static Color grey = new Color(125, 125, 125);
            static Rectangle cycleBack = new Rectangle(0, 0, 124, 90);
            static Rectangle cycleFront = new Rectangle(125, 0, 124, 90);

            int x;
            int y;
            Rectangle pixelRectangle;
            PixelState state;
            readonly Pixels pixels;
            public Pixel(int _x, int _y, Pixels _pixels) {
                x = _x;
                y = _y;
                pixels = _pixels;
                state = PixelState.CLEAR;
                pixelRectangle = new Rectangle(x, y, screenPixelSize, screenPixelSize);
            }

            public void render(SpriteBatch spriteBatch) {
                switch(state) {
                    case PixelState.CLEAR:
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
                        spriteBatch.Draw(pixels.bikeOrange, pixelRectangle, cycleBack, Color.White);
                        break;

                    case PixelState.CYCLE_ORANGE_FRONT:
                        spriteBatch.Draw(pixels.bikeOrange, pixelRectangle, cycleFront, Color.White);
                        break;

                    case PixelState.CYCLE_BLUE_BACK:
                        spriteBatch.Draw(pixels.bikeBlue, pixelRectangle, cycleBack, Color.White);
                        break;

                    case PixelState.CYCLE_BLUE_FRONT:
                        spriteBatch.Draw(pixels.bikeBlue, pixelRectangle, cycleFront, Color.White);
                        break;

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
        }
    }
}

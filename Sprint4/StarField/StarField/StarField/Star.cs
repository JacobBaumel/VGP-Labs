using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StarField {
    class Star {

        private Texture2D texture;
        private Rectangle pos;
        private Color color;
        private Vector2 vel;
        private static readonly Random r;
        static Star() {
            r = new Random((int) DateTime.Now.Ticks);
        }

        public Star(Texture2D _texture) {
            texture = _texture;
            pos = new Rectangle(0, 0, 5, 5);
            color = new Color(0, 0, 0, 255);
            vel = new Vector2();
            randomizeStuff();
        }

        private void randomizeStuff() {
            
            pos.X = r.Next(0, 795);
            pos.Y = r.Next(0, 475);
            byte[] bytes = new byte[3];
            r.NextBytes(bytes);
            color.R = bytes[0];
            color.G = bytes[1];
            color.B = bytes[2];
            do {
                vel.X = (float) (r.NextDouble() * 10) - 5;
            } while(Math.Abs(vel.X) < 1);

            do {
                vel.Y = (float) (r.NextDouble() * 10) - 5;
            } while(Math.Abs(vel.Y) < 1);
        }

        public void draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, pos, color);
        }

        public void update() {
            pos.X += (int) vel.X;
            pos.Y += (int) vel.Y;

            if(pos.X > 800 || pos.Y > 480 || pos.X + pos.Width < 0 || pos.Y + pos.Height < 0) {
                randomizeStuff();
            }
        }
    }
}

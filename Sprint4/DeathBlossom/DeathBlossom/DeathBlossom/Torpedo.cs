using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DeathBlossom {
    class Torpedo {
        private Texture2D texture;
        private float rot;
        private Rectangle rect;
        private Vector2 center;
        private Vector2 pos;
        private float dx;
        private float dy;

        public Torpedo(Texture2D _texture, float _rot, int x, int y, int _vel) {
            texture = _texture;
            rot = _rot;
            rect = new Rectangle(x, y, texture.Width / 4, texture.Height / 4);
            center = new Vector2(rect.X / 2, rect.Y / 2);
            pos = new Vector2(x, y);
            dy = (float) (_vel * Math.Sin(_rot));
            dx = (float) (_vel * Math.Cos(_rot));
        }

        public bool isOffScreen() {
            return rect.X > 1600 || rect.X + rect.Width < 0 || rect.Y > 960 || rect.Y + rect.Height < 0;
        }

        public void draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, rect, null, Color.White, rot + MathHelper.ToRadians(90), center, SpriteEffects.None, 0);
        }

        public void update() {
            pos.X += dx;
            pos.Y += dy;
            rect.X = (int) pos.X;
            rect.Y = (int) pos.Y;
        }
    }
}

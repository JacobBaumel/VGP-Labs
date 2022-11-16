using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;


namespace BounceHouse {
    class Object {

        public Texture2D texture;
        public Rectangle r;
        public int life;
        public int bCount;
        public bool isColliding;
        public bool hasAdded;
        int rot;
        int vr;
        int vx;
        int vy;
        int lifeTime;
        Rectangle lineRect;
        Vector2 text;
        Vector2 center;

        public Object(Texture2D _texture, Rectangle _lineRect, Random ra) {
            texture = _texture;
            lineRect = _lineRect;
            life = ra.Next(2, 13);
            bCount = 0;
            vx = ra.Next(1, 12);
            vy = ra.Next(1, 12);
            isColliding = true;
            lifeTime = 0;
            hasAdded = false;
            rot = 0;
            life = ra.Next(2, 13);
            text = new Vector2(r.X + (texture.Width / 2), r.Y + (texture.Height / 2));
            center = new Vector2(texture.Width / 2, texture.Height / 2);
            do {
                vr = ra.Next(-5, 5);
            } while(vr == 0);
            do {
                r = new Rectangle(ra.Next(0, 999 - texture.Width), ra.Next(0, 799 - texture.Height), (int) (texture.Width / 5), (int) (texture.Height / 5));
            } while(r.Intersects(lineRect));
        }

        public void update() {
            if(r.Intersects(lineRect)) {
                vx *= -1;
                bCount++;
                hasAdded = false;
            }
            if(r.X< 0 || r.X + r.Width > 1000) {
                bCount++;
                vx *= -1;
                hasAdded = false;
                vr *= -1;
            }

            if(r.Y < 0 || r.Y + r.Height > 800) {
                bCount++;
                vy *= -1;
                vr *= -1;
                hasAdded = false;
            }

            lifeTime++;
            if(lifeTime % 60 == 0)
                life--;

            rot += vr;

            r.X += vx;
            r.Y += vy;

            text.X = r.X - (texture.Width / 5) + 15;
            text.Y = r.Y - (texture.Height / 5) + 15;
        }

        public void draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, r, null, Color.White, MathHelper.ToRadians(rot), Vector2.Zero, SpriteEffects.None, 0);
        }

        public void drawLife(SpriteBatch spriteBatch, SpriteFont font) {
            spriteBatch.DrawString(font, "" + life, text, Color.Black, MathHelper.ToRadians(rot), Vector2.Zero, 1, SpriteEffects.None, 0);
        }

        public void drawBounce(SpriteBatch spriteBatch, SpriteFont font) {
            spriteBatch.DrawString(font, "" + bCount, text, Color.Black, MathHelper.ToRadians(rot), Vector2.Zero, 1, SpriteEffects.None, 0);
        }
    }
}

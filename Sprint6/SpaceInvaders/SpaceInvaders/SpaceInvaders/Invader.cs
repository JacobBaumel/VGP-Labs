using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders {
    class Invader {

        public Rectangle rect;
        public Texture2D texture;

        public Invader(int x, int y, Texture2D _texture) {
            rect = new Rectangle(x, y, _texture.Width / 2, _texture.Height / 2);
            texture = _texture;
        }

        public void update(int vx, bool line) {
            rect.X += vx;
            if(line) {
                rect.Y = rect.Y + 40;
                if(rect.Y > 920-40)
                    rect.Y = 0;
            }
        }

        public void draw(SpriteBatch sprites) {
            sprites.Draw(texture, rect, Color.White);
        }
    }
}

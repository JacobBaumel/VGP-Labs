using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonCrawl {
    public class Tile {
        Texture2D texture;
        Rectangle animationFrame;
        Rectangle drawRect;
        int totalFrames;
        int xpos;
        int ypos;
        int frame;

        public Tile(Texture2D texture, int xpos, int ypos, int totalFrames = 1) {
            this.texture = texture;
            this.totalFrames = totalFrames;
            this.xpos = xpos;
            this.ypos = ypos;
            this.frame = 0;
            animationFrame = new Rectangle(0, 0, 32, 32);
            drawRect = new Rectangle(0, 0, 32, 32);
        }

        public void draw(SpriteBatch spriteBatch, int x, int y) {
            drawRect.X = xpos - x;
            drawRect.Y = ypos - y;
            animationFrame.X = (((frame++) / 10) % totalFrames) * 32;
            spriteBatch.Draw(texture, drawRect, animationFrame, Color.White);
        }
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Stargate {
    public abstract class Alien {
        public Rectangle rect;
        public Texture2D texture;
        public Rectangle[] sourceList;
        public Rectangle source;
        public double vx;
        public double vy;

        // To be called 
        public abstract void update();
        public abstract void kill();

        //draw the alien id
        public void draw(SpriteBatch spritebatch)
        {
            if (rect.X > Game1.SCREENPOS && rect.X < Game1.SCREENPOS + Game1.SCREEN_WIDTH)
            {
                Rectangle draw = rect;
                draw.X -= Game1.SCREENPOS;
                spritebatch.Draw(texture, draw, source, Color.White);
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Stargate {
    public class Humanoid {
        public Rectangle rect;
        public Point position;
        Texture2D texture;
        Rectangle humanoidSource;
        public double vy;
        public bool targeted;

        public Humanoid(Texture2D texture, Rectangle humanoidSource, Point p)
        {
            this.texture = texture;
            this.humanoidSource = humanoidSource;
            rect = new Rectangle(p.X, p.Y, 15, 16);
            position = p;
            vy = 0;
            targeted = false;
        }

        public void update()
        {
            //if humanoids are not on the ground they fall
            position.Y += (int) vy;
            if (position.Y <= 40) {
                vy = 0;
            }
        }

        public void draw(SpriteBatch spritebatch)
        {
            if (rect.X > Game1.SCREENPOS && rect.X < Game1.SCREENPOS + Game1.SCREEN_WIDTH)
            {
                Rectangle draw = rect;
                draw.X -= Game1.SCREENPOS;
                spritebatch.Draw(texture, draw, humanoidSource, Color.White);
            }

        }
    }
}
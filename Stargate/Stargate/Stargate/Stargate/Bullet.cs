using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
namespace Stargate
{
    class Bullet
    {
        public Rectangle rect; //access this value to tell if its intersecting
        public int bulletLife; //keeps track of how long bullet has been on the screen, if too long the bullet is killed
        public int xVelocity;
        Texture2D texture;

        public Bullet(Texture2D texture, Rectangle r, double xv) 
        {
            this.texture = texture;
            bulletLife = 30;
            if(xv >= 0) 
            {
                xVelocity = 30;
                rect = new Rectangle(r.X + r.Width + 5, r.Y + (r.Height - 5)/2, 30, 5);
            }
            else {
                xVelocity = -30;
                rect = new Rectangle(r.X - 30 - 5, r.Y + (r.Height - 5) / 2, 30, 5);
            }
        }

        //returns true if bullet needs to be removed
        //this method is only called by bullets class (found in spaceship.cs)
        public bool update() 
        {
            bulletLife--;
            if(bulletLife < 0)
            {
                return true;
            }
            if (xVelocity > 0) 
            {
                rect.X+=3;
                rect.Width  +=  xVelocity;
            }
            else 
            {
                rect.X-=3;
                rect.X  +=  xVelocity;
                rect.Width += Math.Abs(xVelocity);
            }
            return false;
        }

        //call this method if you need to destroy a bullet, bullet needs to be destroyed if it hits an alien
        public void destroyBullet() 
        {
            bulletLife = 0;
        }

        public void draw(SpriteBatch spritebatch, Color color)
        {
            Rectangle draw = rect;
            draw.X -= Game1.SCREENPOS;
            spritebatch.Draw(texture, draw, color);
        }

    }
}

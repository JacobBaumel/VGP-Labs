using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Stargate {
    public class Spaceship {
        const double DVELOCITY = 0.125;
        const double DECELARATION = 0.875;
        const float MAX_VEL = 15f;

        public Texture2D texture;
        public Rectangle rect;
        public Rectangle[] sourceList;
        public Rectangle source;
        KeyboardState old;
        public double vx;
        double vy;
        double timer;
        Bullets bullets;



        public Spaceship(Texture2D spriteSheet, Rectangle[] sourceList, Texture2D bulletsTexture) {
            old = Keyboard.GetState();
            texture = spriteSheet;
            rect = new Rectangle(500, 0, 100, 24);
            vy = 0;
            vx = 0;
            this.sourceList = sourceList;
            source = sourceList[0];
            timer = 0;
            bullets = new Bullets(bulletsTexture, this);
        }

        public void update() {
            KeyboardState k = Keyboard.GetState();

            if(k.IsKeyDown(Keys.A)) {
                vx -= Math.Abs(vx * DVELOCITY);
                if(Math.Abs(vx) < 1)
                    vx = -1;
            }

            else if(k.IsKeyDown(Keys.D)) {
                vx += Math.Abs(vx * DVELOCITY);
                if(Math.Abs(vx) < 1)
                    vx = 1;
            }

            else
                vx *= DECELARATION;

            if(k.IsKeyDown(Keys.W)) {
                rect.Y += -4;
            }

            else if(k.IsKeyDown(Keys.S)) {
                rect.Y += 4;
            }

            vx = MathHelper.Clamp((float) vx, -MAX_VEL, MAX_VEL);
            vy = MathHelper.Clamp((float) vy, -MAX_VEL, MAX_VEL);

            rect.X += (int) vx;
            rect.Y += (int) vy;

            //animations
            if (k.IsKeyDown(Keys.S)) {
                source = sourceList[6 + (int) (timer % 2)];
            }
            else if (k.IsKeyDown(Keys.W))
            {
                source = sourceList[4 + (int)(timer % 2)];
            }
            else if (k.IsKeyDown(Keys.D) || k.IsKeyDown(Keys.A))
            {
                source = sourceList[2 + (int)(timer % 2)];
            }
            else
            {
                source = sourceList[(int)(timer % 2)];
            }
            timer += .2;


            rect.X = (int) MathHelper.Clamp(rect.X, 0, 2000);
            rect.Y = (int) MathHelper.Clamp(rect.Y, 0, 2000);
            if(rect.X < Game1.SCREENPOS + 200 || rect.X > Game1.SCREENPOS + 800) Game1.SCREENPOS += (int) vx;

            Console.WriteLine(rect.X + "   " + Game1.SCREENPOS);

            if(k.IsKeyDown(Keys.Space) && old.IsKeyUp(Keys.Space))
                bullets.addBullet();

            bullets.update();

            old = k;
        }

        public void draw(SpriteBatch spritebatch) 
        {
            if(rect.X > Game1.SCREENPOS && rect.X < Game1.SCREENPOS + Game1.SCREEN_WIDTH) {
                Rectangle draw = rect;
                draw.X -= Game1.SCREENPOS;
                if(vx < 0)
                    spritebatch.Draw(texture, draw, source, Color.White, 0, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0);
                else
                    spritebatch.Draw(texture, draw, source, Color.White);
            }

            bullets.draw(spritebatch);
        
        }

        public bool areBulletsIntersecting(Rectangle rect) {
            return bullets.checkIntersecting(rect);
        }
    }

    class Bullets
    {
        public List<Bullet> bullets;
        public Color color;
        Texture2D texture;
        Spaceship ship;
        int rChange, gChange, bChange;

        public Bullets(Texture2D text, Spaceship ship)
        {
            this.texture = text;
            bullets = new List<Bullet>();
            this.ship = ship;
            color = new Color(255, 0, 0);
            rChange = 0;
            gChange =  1;
            bChange = 0;
        }

        public void addBullet() 
        {
            bullets.Add(new Bullet(texture, ship.rect, ship.vx));
        }

        public void update()
        {
            if(gChange > 0) 
            {
                color.G += 5;
                if (color.G == 255) 
                {
                    gChange = 0;
                    rChange = -1;
                }
            }
            else if(rChange < 0) 
            {
                color.R  -=  5;
                if(color.R == 0) 
                {
                    rChange = 0;
                    bChange = 1;
                }
            }
            else if(bChange > 0)
            {
                color.B += 5;
                if(color.B == 255) 
                {
                    bChange = 0;
                    gChange = -1;
                }
            }
            else if(gChange < 0)
            {
                color.G  -=  5;
                if(color.G == 0)
                {
                    gChange = 0;
                    rChange = 1;
                }
            }
            else if(rChange > 0) {
                color.R  +=  5;
                if(color.R == 255) {
                    rChange = 0;
                    bChange = -1;
                }
            }
            else if(bChange < 0) {
                color.B -=  5;
                if(color.B == 0) {
                    bChange = 0;
                    gChange =  1;
                }
            }
            for(int i = 0; i < bullets.Count; i++) 
            {
                if (bullets[i].update()) 
                {
                    bullets.RemoveAt(i);
                    i--;
                }
            }
        }

        //this will check all bullets and return true if rect is intersecting any
        //any bullets intersecting will be destroyed
        public bool checkIntersecting(Rectangle rect) {
            bool isIntersecting = false;
            for(int i = 0; i < bullets.Count; i++) 
            {
                if (rect.Intersects(bullets[i].rect)) 
                {
                    bullets[i].destroyBullet();
                    isIntersecting = true;
                }
            }
            return isIntersecting;
        }

        public void draw(SpriteBatch spritebatch)
        {
            for(int i = 0; i < bullets.Count; i++) 
            {
                bullets[i].draw(spritebatch, color);
            }
        }

    }
}

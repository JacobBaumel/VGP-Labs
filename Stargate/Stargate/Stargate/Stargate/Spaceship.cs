using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Stargate {
    class Spaceship {
        const double DVELOCITY = 0.125;
        const double DECELARATION = 0.875;
        const float MAX_VEL = 15f;

        public Texture2D texture;
        public Rectangle rect;
        public Rectangle[] sourceList;
        public Rectangle source;
        KeyboardState old;
        double vx;
        double vy;
        double timer;
        


        public Spaceship(Texture2D spriteSheet, Rectangle[] sourceList) {
            old = Keyboard.GetState();
            texture = spriteSheet;
            rect = new Rectangle(500, 0, 100, 24);
            vy = 0;
            vx = 0;
            this.sourceList = sourceList;
            source = sourceList[0];
            timer = 0;
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


        }

        public void draw(SpriteBatch spritebatch) {
            Rectangle draw = rect;
            draw.X -= Game1.SCREENPOS;
            if (vx < 0)
                spritebatch.Draw(texture, draw, source, Color.White, 0, new Vector2(0,0), SpriteEffects.FlipHorizontally, 0);
            else
                spritebatch.Draw(texture, draw, source, Color.White);
        }
        
    }
}

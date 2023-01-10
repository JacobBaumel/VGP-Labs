
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Stargate {
    class Lander : Alien {
        public bool abducting;
        public Humanoid target;
        List<Humanoid> humans;
        double timer;

        public Lander(Texture2D texture, Rectangle[] sourceList, List<Humanoid> humans)
        {
            //Randomly generate a spawn point above ground(y value 40)
            this.humans = humans;
            Random rand = new Random();
            this.rect = new Rectangle(rand.Next(0, Game1.GAME_WIDTH - 32), rand.Next(40, Game1.SCREEN_HEIGHT - 36), 32, 36); //lander has dimensions of 8x9 i think but we make it bigger x3
            this.texture = texture;
            this.sourceList = sourceList;
            this.source = sourceList[0];
            abducting = false;

            this.vx = rand.Next(-4,  4);
            this.vy = rand.Next(-4, 4);
            timer = 0;
            target = null;
        }

        public override void kill() {
            if (target != null) {
                target.vy = -5;
            }
            
        }

        public override void update() {
            if (abducting && this.rect.Y < 30) { //some y-value that counts as the top of the screen
                //transform into mutant
            }
            if(this.rect.X > 0 && this.rect.Right < Game1.GAME_WIDTH) {
                this.rect.X += (int) vx;
            }
            if (this.rect.Y > 0 && this.rect.Bottom < Game1.SCREEN_HEIGHT)
            {
                this.rect.Y += (int)vy;
            }

            source = sourceList[1 + (int)(timer % 4)];
            timer += .2;
        }
        public void findTarget() {
            for(int i = 0; i < humans.Count; i++)
            {
                if (!humans[i].targeted)
                {
                    target = humans[i];
                    
                    break;
                }
            }
        }

        //Lander picks up a Humanoid, both move to top of screen
        public void abduct(Humanoid humanoid)
        {
            //Standard upward velocity
            this.vy = -4;
            this.vx = 0;

            humanoid.position = new Point(this.rect.X, this.rect.Bottom);
            humanoid.vy = this.vy;
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DeathBlossom {
    class Gunstar {
        Texture2D gunstarTex;
        Texture2D torpedoTexture;
        Rectangle gunstarRect;
        Vector2 gunstarCenter;
        bool isFiring;
        double heading;
        int fireCounter;
        Random rand;
        List<Torpedo> torpedos;

        public double Heading {
            get {
                return heading;
            }
        }
        public bool IsFiring {
            get {
                return isFiring;
            }
        }
        public Vector2 Location {
            get {
                return gunstarCenter;
            }
        }

        public Gunstar(Texture2D texture, Rectangle location, List<Torpedo> _torpedos, Texture2D _torpedoTexture) {
            gunstarTex = texture;
            gunstarRect = location;
            gunstarCenter = new Vector2(gunstarRect.X + gunstarRect.Width / 2,
                                        gunstarRect.Y + gunstarRect.Height / 2);
            torpedoTexture = _torpedoTexture;
            isFiring = false;
            heading = 0;
            fireCounter = 0;
            rand = new Random();
            torpedos = _torpedos;
        }

        public void fire() {
            isFiring = true;
        }

        public void Update(GameTime gameTime) {
            if(isFiring) {
                fireCounter++;
                if(fireCounter < 600) {
                    double multiplier = (fireCounter / 600.0) * 3;
                    heading = (heading + rand.NextDouble() * multiplier) % 360;
                    torpedos.Add(new Torpedo(torpedoTexture, (float) heading, (int) gunstarCenter.X, (int) gunstarCenter.Y, 5));
                }
                else {
                    isFiring = false;
                    fireCounter = 0;
                    heading = 0;
                }
            }

            for(int i = 0; i < torpedos.Count(); i++) {
                torpedos.ElementAt(i).update();
                if(torpedos.ElementAt(i).isOffScreen()) {
                    torpedos.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
            spriteBatch.Draw(gunstarTex, gunstarCenter, null, Color.White,
                (float) heading,
                new Vector2(gunstarTex.Width / 2, gunstarTex.Height / 2), 0.2f,
                SpriteEffects.None, 0);

            foreach(Torpedo t in torpedos)
                t.draw(spriteBatch);
        }
    }
}

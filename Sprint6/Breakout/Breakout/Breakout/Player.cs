using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Breakout {
    class Player {
        const int BUMPER_WIDTH = 50;
        const int BUMPER_HEIGHT = 10;
        const int BUMPER_Y = 40;
        const int CIRCLE_R = 20;
        const int CIRCLE_Y = 60;
        Keys[] watchKeys = {Keys.A, Keys.D, Keys.Space};
        Random r;

        public int vx;
        public int vy;
        
        Texture2D circle;
        Texture2D baseRect;

        public Rectangle circleRect;
        Rectangle rect;

        public Player(Texture2D _circle, Texture2D _baseRect) {
            r = new Random((int) DateTime.Now.Ticks);
            circle = _circle;
            baseRect = _baseRect;
            vx = 0;
            vy = 0;
            circleRect = new Rectangle((Game1.SCREEN_WIDTH - CIRCLE_R) / 2, Game1.SCREEN_HEIGHT - CIRCLE_Y, CIRCLE_R, CIRCLE_R);
            rect = new Rectangle((Game1.SCREEN_WIDTH - BUMPER_WIDTH) / 2, Game1.SCREEN_HEIGHT - BUMPER_Y, BUMPER_WIDTH, BUMPER_HEIGHT);
        }

        public void draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(circle, circleRect, Color.White);
            spriteBatch.Draw(baseRect, rect, Color.White);
        }

        public void update() {
            KeyboardState k = Keyboard.GetState();

            if(k.IsKeyUp(Keys.Q)) { // TODO take this out
                circleRect.X += vx;
                circleRect.Y += vy;

                if(circleRect.X < 0 || circleRect.X > Game1.SCREEN_WIDTH - CIRCLE_R)
                    vx *= -1;

                if(circleRect.Y < 0 || circleRect.Y > Game1.SCREEN_HEIGHT - CIRCLE_R)
                    vy *= -1;

                circleRect.Y = (int) MathHelper.Clamp(circleRect.Y, 0, Game1.SCREEN_HEIGHT - circleRect.Height);
                circleRect.X = (int) MathHelper.Clamp(circleRect.X, 0, Game1.SCREEN_WIDTH - circleRect.Width);

                foreach(Keys s in watchKeys) {
                    if(k.IsKeyDown(s))
                        switch(s) {
                            case Keys.A:
                                rect.X -= 3;
                                break;

                            case Keys.D:
                                rect.X += 3;
                                break;

                            case Keys.Space:
                                if(vy == 0) {
                                    while(vy == 0) {
                                        vy = r.Next(-6, -1);
                                    }

                                    do {
                                        vx = r.Next(-4, 4);
                                    } while(vx == 0);

                                    circleRect.X = ((BUMPER_WIDTH - CIRCLE_R) / 2) + rect.X;
                                    circleRect.Y = rect.Y - CIRCLE_R - CIRCLE_R;
                                }
                                break;

                            default:
                                break;
                        }
                }

                rect.X = (int) MathHelper.Clamp(rect.X, 0, Game1.SCREEN_WIDTH - BUMPER_WIDTH);

                if(rect.Intersects(circleRect)) {
                    if(circleRect.Y + circleRect.Width > rect.Y + Math.Abs(vy) + 1) {
                        if(circleRect.X < rect.X + (rect.Width / 2)) {
                            vx = copySign(vy, -1);
                        }

                        else
                            vx = copySign(vy, 1);
                    }

                    else
                        vy *= -1;
                }
            }
        }

        private int copySign(int val, int sign) {
            if(sign < 0)
                return -Math.Abs(val);
            else if(sign == 0)
                return 0;
            else
                return Math.Abs(val);
        }


    }
}

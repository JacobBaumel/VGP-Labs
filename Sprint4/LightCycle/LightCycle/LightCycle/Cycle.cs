using Microsoft.Xna.Framework.Input;

namespace LightCycle {
    class Cycle {

        int vx;
        int vy;
        int fx;
        int fy;
        Pixels p;
        bool playerNum;

        public Cycle(Pixels pixels, int x, int y, bool player) {
            vx = 0;
            vy = 0;
            fx = x;
            fy = y;
            p = pixels;
            playerNum = player;
        }

        public void update() {
            p.pixels[fx, fy].clear();
            p.pixels[fx + (vx * -1), fy + (vy * -1)].set(getLine());

            KeyboardState k = Keyboard.GetState();
            bool e = false;
            if(k.IsKeyDown(getUp())) {
                vx = 0;
                vy = -1;
                e = true;
                setRot(270);
            }

            if(k.IsKeyDown(getDown())) {
                vx = 0;
                vy = 1;
                e = true;
                setRot(90);
            }

            if(k.IsKeyDown(getLeft())) {
                vx = -1;
                vy = 0;
                e = true;
                setRot(180);
            }

            if(k.IsKeyDown(getRight())) {
                vx = 1;
                vy = 0;
                e = true;
                setRot(0);
                
            }

            fx += vx;
            fy += vy;

            p.pixels[fx, fy].set(getFront());
            p.pixels[fx + (vx * -1), fy + (vy * -1)].set(getBack());
            
        }

        Keys getUp() {
            if(playerNum)
                return Keys.W;
            return Keys.Up;
        }

        Keys getDown() {
            if(playerNum)
                return Keys.S;
            return Keys.Down;
        }

        Keys getLeft() {
            if(playerNum)
                return Keys.A;
            return Keys.Left;
        }

        Keys getRight() {
            if(playerNum)
                return Keys.D;
            return Keys.Right;
        }

        Pixels.PixelState getLine() {
            if(playerNum)
                return Pixels.PixelState.LINE_ORANGE;
            return Pixels.PixelState.LINE_BLUE;
        }

        Pixels.PixelState getBack() {
            if(playerNum)
                return Pixels.PixelState.CYCLE_ORANGE_BACK;
            return Pixels.PixelState.CYCLE_BLUE_BACK;
        }

        Pixels.PixelState getFront() {
            if(playerNum)
                return Pixels.PixelState.CYCLE_ORANGE_FRONT;
            return Pixels.PixelState.CYCLE_BLUE_FRONT;
        }

        void setRot(int rot) {
            if(playerNum)
                p.p1Rot = rot;
            else
                p.p2Rot = rot;
        }

        void win() {
            if(playerNum)
                Game1.state = Game1.State.P1;
            else
                Game1.state = Game1.State.P2;
        }

    }
}

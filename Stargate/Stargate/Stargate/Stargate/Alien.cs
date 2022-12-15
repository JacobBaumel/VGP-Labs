using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Stargate {
    abstract class Alien {
        public Rectangle rect;
        public Texture2D texture;

        // To be called 
        public abstract void update();
        public abstract void kill();

        public abstract void draw(SpriteBatch spriteBatch);
    }
}

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ScribblePlatformer {
    class Collectable : AnimatedSprite {
        private string currentAnim = "Idle";
        private Rectangle localBounds;
        bool isAlive;
        Level level;
        Vector2 position;

    }
}

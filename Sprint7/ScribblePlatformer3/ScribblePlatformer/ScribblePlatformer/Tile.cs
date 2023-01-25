using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScribblePlatformer {

    public enum TileCollision {
        Passable = 0,
        Impassable = 1,
        Platform = 2
    }

    class Tile {
        public string TileSheetName;
        public int TileSheetIndex;
        public TileCollision Collision;

        public const int Width = 64;
        public const int Height = 64;
        public static readonly Vector2 Size = new Vector2(Width, Height);

        public Tile(string tileSheetname, int tileSheetIndex, TileCollision _collision) {
            TileSheetIndex = tileSheetIndex;
            TileSheetName = tileSheetname;
            Collision = _collision;
        }
    }
}

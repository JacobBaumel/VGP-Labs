using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScribblePlatformer {
    class Tile {
        public string TileSheetName;
        public int TileSheetIndex;

        public const int Width = 64;
        public const int Height = 64;
        public static readonly Vector2 Size = new Vector2(Width, Height);

        public Tile(string tileSheetname, int tileSheetIndex) {
            TileSheetIndex = tileSheetIndex;
            TileSheetName = tileSheetname;
        }
    }
}

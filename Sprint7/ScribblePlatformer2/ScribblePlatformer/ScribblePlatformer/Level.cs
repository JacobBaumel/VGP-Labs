using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace ScribblePlatformer {
    class Level : IDisposable {
        private Tile[,] tiles;
        private Dictionary<string, Texture2D> tileSheets;
        private Dictionary<int, Rectangle> TileSourceRecs;

        ContentManager content;

        public ContentManager Content {
            get {
                return content;
            }
        }

        private const int TileWidth = 64;
        private const int TileHeight = 64;
        private const int TilesPerRow = 5;
        private const int NumRowsPerSheet = 5;

        private Random random = new Random(1337);

        public int Width {
            get {
                return tiles.GetLength(0);
            }
        }

        public int Height {
            get {
                return tiles.GetLength(1);
            }
        }

        public Level(IServiceProvider provider, string path) {
            content = new ContentManager(provider, "Content");

            tileSheets = new Dictionary<string, Texture2D>();
            tileSheets.Add("Blocks", Content.Load<Texture2D>("Tiles/Blocks"));
            tileSheets.Add("Platforms", Content.Load<Texture2D>("Tiles/Platforms"));

            TileSourceRecs = new Dictionary<int, Rectangle>();
            for(int i = 0; i < TilesPerRow * NumRowsPerSheet; i++) {
                Rectangle rectTile = new Rectangle(
                    (i % TilesPerRow) * TileWidth,
                    (i / TilesPerRow) * TileHeight,
                    TileWidth,
                    TileHeight);
                TileSourceRecs.Add(i, rectTile);
            }

            LoadTiles(path);
        }

        public void Draw(GameTime time, SpriteBatch spriteBatch) {
            DrawTiles(spriteBatch);
        }

        private void DrawTiles(SpriteBatch spriteBatch) {
            for(int y = 0; y < Height; ++y) {
                for(int x = 0; x < Width; ++x) {
                    if(tileSheets.ContainsKey(tiles[x, y].TileSheetName)) {
                        Vector2 position = new Vector2(x, y) * Tile.Size;
                        spriteBatch.Draw(tileSheets[tiles[x, y].TileSheetName], position, TileSourceRecs[tiles[x, y].TileSheetIndex], Color.White);
                    }
                }
            }
        }

        private void LoadTiles(string path) {
            int numOfTilesAcross = 0;
            List<string> lines = new List<string>();

            try {
                using(StreamReader reader = new StreamReader(path)) {
                    string line = reader.ReadLine();
                    numOfTilesAcross = line.Length;
                    while(line != null) {
                        lines.Add(line);
                        int nextLineWidth = line.Length;
                        if(nextLineWidth != numOfTilesAcross) {
                            throw new Exception(String.Format("The length of line {0} is different from all preceeding lines.", lines.Count));
                        }
                        line = reader.ReadLine();
                    }
                }
            } catch(Exception e) {
                Console.WriteLine("File could not be read!");
                Console.WriteLine(e.Message);
            }

            tiles = new Tile[numOfTilesAcross, lines.Count];

            for(int y = 0; y < Height; ++y) {
                for(int x = 0; x < Width; ++x) {
                    string currentRow = lines[y];
                    char tileType = currentRow[x];
                    tiles[x, y] = LoadTile(tileType, x, y);
                }
            }
        }

        private Tile LoadTile(char type, int x, int y) {
            switch(type) {
                case '.':
                    return new Tile(String.Empty, 0);

                case 'B':
                    return LoadVarietyTile("Platforms", 0, 5);

                case 'G':
                    return LoadVarietyTile("Platforms", 5, 5);

                case 'O':
                    return LoadVarietyTile("Platforms", 10, 5);

                case 'R':
                    return LoadVarietyTile("Platforms", 15, 5);

                case 'Y':
                    return LoadVarietyTile("Platforms", 20, 5);


                case 'b':
                    return LoadVarietyTile("Blocks", 0, 5);

                case 'g':
                    return LoadVarietyTile("Blocks", 5, 5);

                case 'o':
                    return LoadVarietyTile("Blocks", 10, 5);

                case 'r':
                    return LoadVarietyTile("Blocks", 15, 5);

                case 'y':
                    return LoadVarietyTile("Blocks", 20, 5);


                default:
                    throw new NotSupportedException(String.Format("Unsupported tile type character '{0}' at positoin {1}, {2}", type, x, y));
            }
        }

        private Tile LoadVarietyTile(string sheetName, int colorRow, int variationCount) {
            int index = random.Next(variationCount);
            int tileSheetIndex = colorRow + index;
            return new Tile(sheetName, tileSheetIndex);
        }


        public void Dispose() {
            Content.Unload();
        }
    }
}

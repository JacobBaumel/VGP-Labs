using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PickAPeck {

    public class Game1 : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D sheet;
        SpriteFont font;

        Rectangle[] sourceRects;
        Rectangle[] screenRects;
        Vector2[] labelTextPos;
        Keys[] keys;

        KeyboardState prev;

        int selection;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        
        protected override void Initialize() {
            parseRects(@"Content/rects.txt");
            screenRects = new Rectangle[sourceRects.Length];
            labelTextPos = new Vector2[sourceRects.Length];
            for(int i = 0; i < sourceRects.Length; i++) {
                if(i == 0)
                    screenRects[0] = new Rectangle(0, 0, sourceRects[0].Width, sourceRects[0].Height);
                else 
                    screenRects[i] = new Rectangle(screenRects[i - 1].X + screenRects[i - 1].Width + 5, 0, sourceRects[i].Width, sourceRects[i].Height);
            }
            Window.AllowUserResizing = true;
            selection = -1;
            prev = Keyboard.GetState();
            keys = new Keys[] { Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5 };
            base.Initialize();
        }

       
        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            sheet = Content.Load<Texture2D>("SpriteSheet");
            font = Content.Load<SpriteFont>("SpriteFont1");

            for(int i = 0; i < sourceRects.Length; i++) {
                labelTextPos[i] = new Vector2(screenRects[i].X + ((screenRects[i].Width - font.MeasureString("" + (i + 1)).X) / 2), screenRects[i].Height + 10);
            }
        }

        
        protected override void UnloadContent() {
        }

        
        protected override void Update(GameTime gameTime) {
            KeyboardState k = Keyboard.GetState();
            if(k.IsKeyDown(Keys.Escape))
                this.Exit();

            for(int i = 0; i < keys.Length; i++) {
                if(isEvent(k, keys[i])) {
                    if(selection == i)
                        selection = -1;
                    else
                        selection = i;
                }
            }

            for(int i = 0; i < screenRects.Length; i++) {
                if(i == selection) {
                    screenRects[i].Width = sourceRects[i].Width * 2;
                    screenRects[i].Height = sourceRects[i].Height * 2;
                    screenRects[i].Y = 200;
                }

                else {
                    screenRects[i].Width = sourceRects[i].Width;
                    screenRects[i].Height = sourceRects[i].Height;
                    screenRects[i].Y = 0;
                }
            } 
            prev = k;

            base.Update(gameTime);
        }

        bool isEvent(KeyboardState s, Keys k) {
            return s.IsKeyDown(k) && prev.IsKeyUp(k);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            for(int i = 0; i < sourceRects.Length; i++) {
                spriteBatch.Draw(sheet, screenRects[i], sourceRects[i], Color.White);
                spriteBatch.DrawString(font, "" + (i + 1), labelTextPos[i], Color.White);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        void parseRects(string path) {
            try {
                using(StreamReader reader = new StreamReader(path)) {
                    sourceRects = new Rectangle[Convert.ToInt32(reader.ReadLine())];
                    int index = 0;
                    while(!reader.EndOfStream) {
                        string[] parts = reader.ReadLine().Split(' ');
                        sourceRects[index++] = new Rectangle(Convert.ToInt32(parts[0]), Convert.ToInt32(parts[1]), Convert.ToInt32(parts[2]), Convert.ToInt32(parts[3]));
                    }
                }
            } catch(Exception e) {
                Console.WriteLine("Could not parse rectangles file");
                Console.WriteLine(e.Message);
            }
        }
    }
}

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

namespace JourneyToBabel {
    public enum GameState {
        START,
        SAVE,
        END
    }

    public enum Language {
        ENGLISH,
        SPANISH,
        GERMAN
    }
    
    public class Game1 : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Dictionary<GameState, Dictionary<Language, string>> phrases;
        Language currLang;
        GameState state;
        List<string> statePhrases;

        SpriteFont font;

        string welcomePhrase;
        Vector2 wordPos;

        KeyboardState old;
        Keys[] watchkeys;


        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        
        protected override void Initialize() {
            currLang = Language.ENGLISH;
            state = GameState.START;
            statePhrases = (new string[] {"start", "save", "done"}).ToList();
            phrases = new Dictionary<GameState, Dictionary<Language, string>>();
            welcomePhrase = "Use spacebar to change game state, use number keys to change language\n1: English\n2: Spanish\n3: German";
            wordPos = new Vector2(200, 200);
            old = Keyboard.GetState();
            watchkeys = new Keys[] { Keys.D1, Keys.D2, Keys.D3 };

            GameState whichState;
            using(StreamReader r = new StreamReader(@"Content\phrases.txt")) {
                while(!r.EndOfStream) {
                    // String read from file has extra characters, so truncate them off
                    string line = r.ReadLine();
                    line = line.Substring(0, line.Length - 1);

                    whichState = (GameState) statePhrases.IndexOf(line);
                    phrases[whichState] = new Dictionary<Language, string>();
                    for(int i = 0; i < 3; i++) {
                        phrases[whichState][(Language) i] = r.ReadLine();
                    }

                    // Skip empty line
                    r.ReadLine();
                }
            }

            base.Initialize();
        }

       
        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("SpriteFont1");
        }

       
        protected override void UnloadContent() {
        }

        
        protected override void Update(GameTime gameTime) {
            KeyboardState k = Keyboard.GetState();
            if(k.IsKeyDown(Keys.Escape))
                this.Exit();

            if(k.IsKeyDown(Keys.Space) && old.IsKeyUp(Keys.Space)) {
                state = (GameState) ((((int) state) + 1) % 3);
            }

            for(int i = 0; i < watchkeys.Length; i++) {
                if(k.IsKeyDown(watchkeys[i]))
                    currLang = (Language) i;
            }

            old = k;


            base.Update(gameTime);
        }

       
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.DrawString(font, welcomePhrase, Vector2.Zero, Color.White);
            spriteBatch.DrawString(font, phrases[state][currLang], wordPos, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

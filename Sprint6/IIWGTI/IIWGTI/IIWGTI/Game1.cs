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

namespace IIWGTI {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Subject[,] subjects;
        Subjects s;
        KeyboardState old;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            // TODO: Add your initialization logic here
            s = Subjects.CompSci;
            subjects = new Subject[4, 4];
            old = Keyboard.GetState();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Texture2D texture = Content.Load<Texture2D>("Sprint 6 Test Image");
            SpriteFont font = Content.Load<SpriteFont>("SpriteFont1");
            int i = 0;
            int j = -1;

            char[] nums = new char[] {'1', '2', '3', '4' };

            using(StreamReader names = new StreamReader(@"Content/Sprint 6 Test Image - Names.txt")) {
                using(StreamReader poss = new StreamReader(@"Content/Sprint 6 Test Image - Points.txt")) {
                    while(!names.EndOfStream) {
                        string name = names.ReadLine();
                        string[] pos = poss.ReadLine().Split(' ');

                        j++;
                        if(j >= 4) {
                            j = 0;
                            i++;
                        }

                        int index = name.IndexOfAny(nums);
                        int value = Convert.ToInt32("" + name[index]);
                        Subjects sub = Subject.subjectFromString(name.Substring(0, index - 1));

                        subjects[i, j] = new Subject(new Rectangle(Convert.ToInt32(pos[0]), Convert.ToInt32(pos[1]), Convert.ToInt32(pos[2]), Convert.ToInt32(pos[3])),
                            texture, value, sub, font);
                    }
                }
            }

            for(int ii = 0; ii < 4; ii++) {
                bool cont = false;
                do {
                    cont = false;
                    for(int jj = 0; jj < subjects.GetLength(1) - 1; jj++) {
                        if(subjects[ii, jj].value > subjects[ii, jj + 1].value) {
                            Subject temp = subjects[ii, jj];
                            subjects[ii, jj] = subjects[ii, jj + 1];
                            subjects[ii, jj + 1] = temp;
                            cont = true;
                        }
                    }

                } while(cont);

                
            }

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            // Allows the game to exit
            KeyboardState k = Keyboard.GetState();
            if(k.IsKeyDown(Keys.Escape))
                this.Exit();

            if(k.IsKeyDown(Keys.Space) && old.IsKeyUp(Keys.Space)) {
                s = (Subjects) (((int) s + 1) % 4);
            }

            if(k.IsKeyDown(Keys.Enter) && old.IsKeyUp(Keys.Enter)) {
                for(int i = 0; i < subjects.GetLength(1); i++) {
                    for(int j = 0; j < subjects.GetLength(1) / 2; j++) {
                        Subject temp = subjects[i, j];
                        subjects[i, j] = subjects[i, subjects.GetLength(1) - j - 1];
                        subjects[i, subjects.GetLength(1) - j - 1] = temp;
                    }
                }
            }

            old = k;

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Rectangle pos = new Rectangle(10, 10, 0, 0);
            spriteBatch.Begin();
            for(int i = 0; i < 4; i++) {
                subjects[(int) s, i].draw(spriteBatch, ref pos);
            }
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}

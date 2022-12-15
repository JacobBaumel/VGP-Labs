using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Stargate {
    public enum GameState
    {
        Menu, Playing, GameOver
    }

    public class Game1 : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public const int SCREEN_WIDTH = 1000;
        public const int SCREEN_HEIGHT = 550;
        public static int SCREENPOS = 0;

        Texture2D DELTEMELATER;
        Rectangle REMOVEMELATER = new Rectangle(500, 500, 20, 20);

        Spaceship s;
        Texture2D ship;
        Rectangle[] shipSource;

        //keeps track of whether we are on the menu or playing the game or on the game over screen
        public GameState currentState;

        //velocity that affects everything because of the screen moving
        public int screenVelocity;

        public Game1() 
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content"; 

            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            graphics.ApplyChanges();
        }

        protected override void Initialize() 
        {
            currentState = GameState.Menu;
            
            base.Initialize();
        }

        public void ParseSheet(string file, ref Rectangle[] output) 
        {
            try
            {
                using(StreamReader r = new StreamReader(file)) 
                {
                    output = new Rectangle[Convert.ToInt32(r.ReadLine())];
                    for (int i = 0; i < output.Length; i++)
                    {
                        string temp = r.ReadLine();
                        string[] cur = temp.Split(' ');

                        output[i] = new Rectangle(Convert.ToInt32(cur[0]), Convert.ToInt32(cur[1]), Convert.ToInt32(cur[2]), Convert.ToInt32(cur[3]));
                    }
                }
            }
            catch (Exception e) { Console.WriteLine($"Could not find file\n{e}"); }
        }

        protected override void LoadContent() 
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ship = Content.Load<Texture2D>("Ship");

            ParseSheet(@"Content/ShipSource.txt", ref shipSource);
            s = new Spaceship(ship, shipSource);

            DELTEMELATER = new Texture2D(GraphicsDevice, 1, 1);
            DELTEMELATER.SetData(new Color[] { Color.White });

        }

        protected override void UnloadContent() {
            
        }

        protected override void Update(GameTime gameTime) 
        {
            if(Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            s.update();

            if(currentState == GameState.Menu) 
            {
                //MENU HERE

            }
            else if(currentState == GameState.Playing) 
            {
                //GAME RUNNING HERE

            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
            s.draw(spriteBatch);
            Rectangle temp = REMOVEMELATER;
            temp.X -= SCREENPOS;
            spriteBatch.Draw(DELTEMELATER, temp, Color.White);
            if(currentState == GameState.Menu) 
            {
                //DRAW MENU HERE

            }
            else if (currentState == GameState.Playing)
            {
                //DRAW GAME HERE

            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

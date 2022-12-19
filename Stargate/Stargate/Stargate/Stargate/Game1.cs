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
        public GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public const int SCREEN_WIDTH = 1000;
        public const int SCREEN_HEIGHT = 550;
        public const int GAME_WIDTH = 2000;
        public static int SCREENPOS = 0;

        public Texture2D baseRect;

        public Spaceship s;
        public List<Alien> aliens;
        public List<Humanoid> humans;

        Random rand;

        int wave;

        public Spaceship GetSpaceship() {
            return s;
        }

        //textures
        Texture2D ship;
        Rectangle[] shipSource;
        Texture2D mobs;
        Rectangle[] mobSource;

        Texture2D bulletsTexture;

        //keeps track of whether we are on the menu or playing the game or on the game over screen
        public GameState currentState;

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
            wave = 0;
            humans = new List<Humanoid>();
            rand = new Random();
            aliens = new List<Alien>();
            currentState = GameState.Playing;
            
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
            //ship textures
            ship = Content.Load<Texture2D>("Ship");
            ParseSheet(@"Content/ShipSource.txt", ref shipSource);
            //mob textures
            mobs = Content.Load<Texture2D>("Mobs");
            ParseSheet(@"Content/MobSource.txt", ref mobSource);

            for (int i = 0; i < 1; i++)
            {
                humans.Add(new Humanoid(mobs, mobSource[0], new Point(rand.Next(0,GAME_WIDTH - 10),0)));
            }

            bulletsTexture = Content.Load<Texture2D>("Bullet");
            s = new Spaceship(ship, shipSource, bulletsTexture);

            baseRect = new Texture2D(GraphicsDevice, 1, 1);
            baseRect.SetData(new Color[] { Color.White });

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
                foreach(Alien a in aliens) {
                    a.update();
                }

                for(int i = 0; i < aliens.Count; i++) 
                {
                    if (s.areBulletsIntersecting(aliens[i].rect)) {
                        aliens.RemoveAt(i);
                        i--;
                    }
                }

                s.update();

                if(aliens.Count() == 0) {
                    wave++;
                    for(int i = 0; i < 10; i++) {
                        aliens.Add(new Lander(mobs, mobSource, humans));// TODO: Replace with a randomizer to spawn other types of aliens
                    }
                }                

            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
            s.draw(spriteBatch);
            humans[0].draw(spriteBatch);


            
                foreach(Alien a in aliens)
                    a.draw(spriteBatch);

            
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

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

namespace BounceHouse {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        List<Object> objs;
        Texture2D[] textures;
        Texture2D baseRect; // Texture for line
        Rectangle lineRect;
        int lineChangeTimer;
        int lineChangerSeconds;
        Random r;
        bool show;
        int doubleCooldown;
        int showTime;
        SpriteFont font;

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
            r = new Random((int) DateTime.Now.Ticks);
            graphics.PreferredBackBufferHeight = 800;
            graphics.PreferredBackBufferWidth = 1000;
            graphics.ApplyChanges();
            showTime = 0;
            lineChangeTimer = 0;
            show = false;
            lineChangerSeconds = r.Next(1, 6);
            lineRect = new Rectangle(500, 0, 1, 800);
            objs = new List<Object>();
            doubleCooldown = 0;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            textures = new Texture2D[8];
            for(int i = 0; i < 8; i++) {
                textures[i] = Content.Load<Texture2D>("" + i);
            }

            font = Content.Load<SpriteFont>("SpriteFont1");


            baseRect = new Texture2D(GraphicsDevice, 1, 1);
            
            baseRect.SetData(new Color[] { Color.White }); // Create 1x1 square for line



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

            while(objs.Count < 3)
                addObj();

            while(objs.Count > 1000)
                objs.RemoveAt(objs.Count - 1);

            lineChangeTimer++;

            if(lineChangeTimer % (60 * lineChangerSeconds) == 0) {
                lineChangerSeconds = r.Next(1, 6);
                double percentage = ((r.Next(0, 10) * 5) + 50) / 100.0;
                lineRect.Height = (int) (800 * percentage);
                lineRect.Y = (int) (((1 - percentage) / 2) * 800);
            }

            for(int i = 0; i < objs.Count; i++) {
                objs[i].update();
                if(objs[i].bCount > 0 && objs[i].bCount % 5 == 0 && !objs[i].hasAdded) {
                    addObj();
                    objs[i].hasAdded = true;
                }

                if(objs[i].life <= 0) {
                    objs.RemoveAt(i);
                    i--;
                }
            }

            for(int i = 0; i < objs.Count; i++) {
                bool hasIntersected = false;
                for(int j = i + 1; j < objs.Count; j++) {
                    if(objs[i].r.Intersects(objs[j].r) && objs[i].texture == objs[j].texture && (!objs[i].isColliding && !objs[j].isColliding)) {
                        objs[i].isColliding = true;
                        objs[j].isColliding = true;
                        addObj();
                        hasIntersected = true;
                    }
                        
                }

                if(!hasIntersected)
                    objs[i].isColliding = false;
            }

            if((k.IsKeyDown(Keys.R) || k.IsKeyDown(Keys.B) || k.IsKeyDown(Keys.P) || k.IsKeyDown(Keys.Y)) && doubleCooldown == 0) {
                int size = objs.Count;
                for(int i = 0; i < size; i++) {
                    addObj(objs[i].texture);
                }

                doubleCooldown = 5;
            }

            if(lineChangeTimer % 60 == 0 && doubleCooldown > 0)
                doubleCooldown--;

            if(showTime <= 0) {
                
                if(k.IsKeyDown(Keys.A)) {
                    show = true;
                    showTime = 1;
                }
                else if(k.IsKeyDown(Keys.X)) {
                    showTime = 1;
                    show = false;
                }
            }

            else if(lineChangeTimer % 60 == 0)
                showTime--;
            
            base.Update(gameTime);
        }

        void addObj() {
            addObj(textures[r.Next(0, textures.Length)]);
        }

        void addObj(Texture2D texture) {
            objs.Add(new Object(texture, lineRect, r));
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(baseRect, lineRect, Color.White);
            foreach(Object o in objs)
                o.draw(spriteBatch);

            if(showTime > 0) {
                foreach(Object o in objs) {
                    if(show)
                        o.drawLife(spriteBatch, font);
                    else
                        o.drawBounce(spriteBatch, font);
                }
            }
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}

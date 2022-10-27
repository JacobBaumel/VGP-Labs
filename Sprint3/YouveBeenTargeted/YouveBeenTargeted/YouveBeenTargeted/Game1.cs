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

namespace YouveBeenTargeted {
    public class Game1 : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D tank;
        Texture2D baseRect;

        Rectangle tankRect;
        Rectangle projectileRect;
        Vector2 projectileOrigin;
        Vector2 projectilePos;
        Vector2 tankOrigin;

        MouseState prev;

        float dX;
        float dY;
        float tankRot;

        const float SPEED = 3;
        


        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize() {
            tankRect = new Rectangle(400, 240, 110, 110);
            tankOrigin = new Vector2(110, 110);
            projectileOrigin = new Vector2(1, 1);
            projectileRect = new Rectangle(400, 240, 5, 5);
            projectilePos = new Vector2(projectileRect.X, projectileRect.Y);
            dX = 0;
            dY = 0;
            IsMouseVisible = true;
            tankRot = 0;
            prev = Mouse.GetState();
            base.Initialize();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            tank = Content.Load<Texture2D>("tank");
            baseRect = new Texture2D(GraphicsDevice, 1, 1);
            baseRect.SetData(new Color[] { Color.White });
        }

        protected override void UnloadContent() {
        }

        protected override void Update(GameTime gameTime) {
            if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            MouseState m = Mouse.GetState();
            
            tankRot = (float) (Math.Atan2(240 - m.Y, 400 - m.X) - MathHelper.ToRadians(90));

            if(m.LeftButton == ButtonState.Pressed && prev.LeftButton == ButtonState.Released) {
                projectilePos.X = 400;
                projectilePos.Y = 240;
                double speed = Math.Sqrt(Math.Pow(400 - m.X, 2) + Math.Pow(240 - m.Y, 2)) / SPEED;
                dX = (float) (-((400 - m.X) / speed));
                dY = (float) (-((240- m.Y) / speed));
            }

            projectilePos.X += dX;
            projectilePos.Y += dY;

            projectileRect.X = (int) projectilePos.X;
            projectileRect.Y = (int) projectilePos.Y;
            prev = m;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(baseRect, projectileRect, null, Color.White, 0, projectileOrigin, SpriteEffects.None, 0);
            spriteBatch.Draw(tank, tankRect, null, Color.White, tankRot, tankOrigin, SpriteEffects.None, 0);
            ;
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

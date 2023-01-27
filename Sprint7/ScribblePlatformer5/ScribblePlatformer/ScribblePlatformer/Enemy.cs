using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ScribblePlatformer {
    class Enemy : AnimatedSprite {
        private SpriteEffects flip = SpriteEffects.None;
        private string currentAnim = "Walk";
        private const float MoveSpeed = 128.0f;
        private Rectangle localBounds;
        bool isAlive;
        bool isCompletelyDead;
        Level level;
        Vector2 position;
        Vector2 velocity;

        public bool IsAlive {
            get {
                return isAlive;
            }
        }
        public bool IsCompletelyDead {
            get {
                return isCompletelyDead;
            }
        }
        public Level Level {
            get {
                return level;
            }
        }

        public Vector2 Position {
            get {
                return position;
            }

            set {
                position = value;
            }
        }

        public Vector2 Velocity {
            get {
                return velocity;
            }

            set {
                velocity = value;
            }
        }

        public Rectangle BoundingRectangle {
            get {
                int left = (int) Math.Round(Position.X - Origin.X) + localBounds.X;
                int top = (int) Math.Round(Position.Y - Origin.Y) + localBounds.Y;
                return new Rectangle(left, top, localBounds.Width, localBounds.Height);
            }
        }

        public Enemy(Level _level, Vector2 _position, string _enemy) : base(96, 96, 4) {
            level = _level;
            position = _position;
            isAlive = true;
            isCompletelyDead = false;
            LoadContent(_enemy);
        }

        public void LoadContent(string _enemy) {
            string enemyString = string.Empty;
            switch(_enemy) {
                case "e":
                    enemyString = "Sprites/Enemies/muffinman";
                    break;

                default:
                    enemyString = "Sprites/Enemies/muffinman";
                    break;
            }

            Texture2D tex = Level.Content.Load<Texture2D>(enemyString);
            if(!SpriteTextures.Contains(tex)) {
                SpriteTextures.Add(tex);
                Animation anim = new Animation();
                anim.LoadAnimation("Walk", 0, new List<int> {0, 1}, 4, true);
                SpriteAnimations.Add("Walk", anim);

                anim = new Animation();
                anim.LoadAnimation("Dead", 0, new List<int> {2, 3, 4, 5}, 36, false);
                anim.AnimationCallBack(DeadAnimEnd);
                SpriteAnimations.Add("Dead", anim);

                int width = FrameWidth - 10;
                int left = (FrameWidth - width) / 2;
                int height = FrameHeight - 10;
                int top = FrameHeight - height;
                localBounds = new Rectangle(left, top, width, height);
                SpriteAnimations[currentAnim].ResetPlay();
            }
        }

        public void OnKilled() {
            isAlive = false;
            SpriteAnimations[currentAnim].Stop();
            currentAnim = "Dead";
            SpriteAnimations[currentAnim].ResetPlay();
        }

        public void DeadAnimEnd() {
            isCompletelyDead = true;
        }
        
        public void Update(GameTime _gameTime) {
            SpriteAnimations[currentAnim].Update(_gameTime);
            if(isAlive) {
                float elapsed = (float) _gameTime.ElapsedGameTime.TotalSeconds;
                int direction = 1;
                if(Velocity.X < 0)
                    direction = -1;
                float posX = Position.X + localBounds.Width / 2 * direction;
                int tileX = (int) Math.Floor(posX / Tile.Width) - direction;
                int tileY = (int) Math.Floor(Position.Y / Tile.Height);
                if(Level.GetCollision(tileX + direction, tileY - 1) == TileCollision.Impassable || Level.GetCollision(tileX + direction, tileY + 1) == TileCollision.Passable ||
                    Level.GetCollision(tileX + direction, tileY) == TileCollision.Impassable) {
                    direction *= -1;
                }

                velocity = new Vector2(direction * MoveSpeed * elapsed, 0.0f);
                position += velocity;
            }
        }

        public void Draw(GameTime _gameTime, SpriteBatch _spriteBatch) {
            Rectangle source = GetFrameRectangle(SpriteAnimations[currentAnim].FrameToDraw);
            if(Velocity.X > 0)
                flip = SpriteEffects.FlipHorizontally;
            else if(Velocity.X < 0)
                flip = SpriteEffects.None;
            _spriteBatch.Draw(SpriteTextures[0], position, source, Color.White, 0.0f, Origin, 1.0f, flip, 0.0f);
        }

    }
}

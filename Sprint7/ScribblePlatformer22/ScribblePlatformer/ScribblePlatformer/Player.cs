using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace ScribblePlatformer {
    class Player {
        private Texture2D playerSprite;

        Level level;
        public Level Level {
            get {
                return level;
            }
        }

        bool isAlive;
        public bool IsAlive {
            get {
                return isAlive;
            }
        }

        Vector2 position;
        public Vector2 Position {
            get {
                return position;
            }

            set {
                position = value;
            }
        }

        private float previousBottom;

        Vector2 velocity;
        public Vector2 Velocity {
            get {
                return velocity;
            }

            set {
                velocity = value;
            }
        }

        private const float MoveAccelaration = 14000.0f;
        private const float MaxMoveSpeed = 2000.0f;
        private const float GroundDragFactor = 0.58f;
        private const float AirDragFactor = 0.65f;

        private const float MaxJumpTime = 0.35f;
        private const float JumpLaunchVelocity = -4000.0f;
        private const float GravityAccelaration = 3500.0f;
        private const float MaxFallSpeed = 600.0f;
        private const float JumpControlPower = 0.14f;

        private const float MoveStickScale = 1.0f;
        private const Buttons JumpButton = Buttons.A;

        bool isOnground;
        public bool IsOnGround {
            get {
                return isOnground;
            }
        }

        private float movement;

        private bool isJumping;
        private bool wasJumping;
        private float jumpTime;

        private Rectangle localBounds;

        public Rectangle BoundingRectangle {
            get {
                int left = (int) Math.Round(Position.X - Origin.X) + localBounds.X;
                int top = (int) Math.Round(Position.Y - Origin.Y) + localBounds.Y;
                return new Rectangle(left, top, localBounds.Width, localBounds.Height);
            }
        }

        public Vector2 Origin {
            get {
                return new Vector2(playerSprite.Width / 2.0f, playerSprite.Height / 2.0f);
            }
        }

        public Player(Level _level, Vector2 _position) {
            level = _level;
            LoadContent();
            Reset(_position);
        }

        public void LoadContent() {
            playerSprite = Level.Content.Load<Texture2D>("Sprites/Player/player-1");
            int width = playerSprite.Width - 4;
            int left = (playerSprite.Width - width) / 2;
            int height = playerSprite.Height;
            int top = playerSprite.Height - height;
            localBounds = new Rectangle(left, top, width, height);
        }

        public void Reset(Vector2 _position) {
            Position = _position;
            Velocity = Vector2.Zero;
            isAlive = true;
        }

        private void GetInput() {
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();

            movement = gamePadState.ThumbSticks.Left.X * MoveStickScale;

            if(Math.Abs(movement) < 0.5f)
                movement = 0.0f;

            if(gamePadState.IsButtonDown(Buttons.DPadLeft) ||
                keyboardState.IsKeyDown(Keys.Left) ||
                keyboardState.IsKeyDown(Keys.A)) {
                movement = -1.0f;
            }

            else if(gamePadState.IsButtonDown(Buttons.DPadRight) || 
                keyboardState.IsKeyDown(Keys.Right) ||
                keyboardState.IsKeyDown(Keys.D)) {
                movement = 1.0f;
            }

            isJumping = gamePadState.IsButtonDown(JumpButton) ||
                keyboardState.IsKeyDown(Keys.Space) ||
                keyboardState.IsKeyDown(Keys.W) ||
                keyboardState.IsKeyDown(Keys.Up);
        }

        public void update(GameTime _gameTime) {
            GetInput();
            ApplyPhysics(_gameTime);
            movement = 0.0f;
            isJumping = false;
        }

        public void ApplyPhysics(GameTime _gameTime) {
            float elapsed = (float) _gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 previousPosition = Position;

            velocity.X += movement * MoveAccelaration * elapsed;
            velocity.Y = MathHelper.Clamp(velocity.Y + GravityAccelaration * elapsed, -MaxFallSpeed, MaxFallSpeed);

            velocity.Y = DoJump(velocity.Y, _gameTime);

            if(IsOnGround)
                velocity.X *= GroundDragFactor;
            else
                velocity.X *= AirDragFactor;

            velocity.X = MathHelper.Clamp(velocity.X, -MaxMoveSpeed, MaxMoveSpeed);

            Position += velocity * elapsed;
            Position = new Vector2((float) Math.Round(Position.X), (float) Math.Round(Position.Y));

            HandleCollisions();

            if(Position.X == previousPosition.X)
                velocity.X = 0;
            if(Position.Y == previousPosition.Y)
                velocity.Y = 0;
        }

        private float DoJump(float _velocityY, GameTime _gameTime) {
            if(isJumping) {
                if((!wasJumping & IsOnGround) || jumpTime > 0.0f) {
                    jumpTime += (float) _gameTime.ElapsedGameTime.TotalSeconds;
                }

                if(0.0f < jumpTime && jumpTime <= MaxJumpTime) {
                    _velocityY = JumpLaunchVelocity * (1.0f - (float) Math.Pow(jumpTime / MaxJumpTime, JumpControlPower));
                }

                else {
                    jumpTime = 0.0f;
                }
            }

            else {
                jumpTime = 0.0f;
            }

            wasJumping = isJumping;
            return _velocityY;
        }

        private void HandleCollisions() {
            Rectangle bounds = BoundingRectangle;
            int leftTile = (int) Math.Floor((float) bounds.Left / Tile.Width);
            int rightTile = (int) Math.Ceiling(((float) bounds.Right / Tile.Width)) - 1;
            int topTile = (int) Math.Floor((float) bounds.Top / Tile.Height);
            int bottomTile = (int) Math.Ceiling(((float) bounds.Bottom / Tile.Height)) - 1;

            isOnground = false;

            for(int y = topTile; y <= bottomTile; ++y) {
                for(int x = leftTile; x <= rightTile; ++x) {
                    TileCollision collision = Level.GetCollision(x, y);
                    if(collision != TileCollision.Passable) {
                        Rectangle tileBounds = Level.GetBounds(x, y);
                        Vector2 depth = bounds.GetIntersectionDepth(tileBounds);
                        if(depth != Vector2.Zero) {
                            float absDepthX = Math.Abs(depth.X);
                            float absDepthY = Math.Abs(depth.Y);

                            if(absDepthY < absDepthX || collision == TileCollision.Platform) {
                                if(previousBottom < tileBounds.Top)
                                    isOnground = true;

                                if(collision == TileCollision.Impassable || IsOnGround) {
                                    Position = new Vector2(Position.X, Position.Y + depth.Y);
                                    bounds = BoundingRectangle;
                                }
                            }

                            else if(collision == TileCollision.Impassable) {
                                Position = new Vector2(Position.X + depth.X, Position.Y);
                                bounds = BoundingRectangle;
                            }
                        }
                    }
                }
            }
            previousBottom = bounds.Bottom;
        }

        public void draw(GameTime _gameTime, SpriteBatch _spriteBatch) {
            Rectangle source = new Rectangle(0, 0, playerSprite.Width, playerSprite.Height);

            _spriteBatch.Draw(playerSprite, position, source, Color.White, 0.0f, Origin, 1.0f, SpriteEffects.None, 0.0f);
        }


    }
}

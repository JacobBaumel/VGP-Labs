using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;



namespace ScribblePlatformer {
    
    class Player : AnimatedSprite {
        private const float MoveAccelaration = 14000f;
        private const float MaxMoveSpeed = 2000f;
        private const float GroundDragFactor = 0.58f;
        private const float AirDragFactor = 0.65f;

        private const float MaxJumpTime = 0.35f;
        private const float JumpLaunchVelocity = -4000f;
        private const float GravityAccelaration = 3500f;
        private const float MaxFallSpeed = -100f;
        private const float JumpControlPower = 0.14f;

        private const float MoveStickScale = 1f;
        private const Buttons JumpButton = Buttons.A;

        public Level Level {
            get {
                return level;
            }
        }

        Level level;

        public bool IsAlive {
            get {
                return IsAlive;
            }
        }

        bool isAlive;

        public Vector2 Position {
            get {
                return position;
            }

            set {
                position = value;
            }
        }

        Vector2 position;

        private float previousBottom;

        public Vector2 Velocity {
            get {
                return velocity;
            }

            set {
                velocity = value;
            }
        }

        Vector2 velocity;

        public bool IsOnGround {
            get {
                return isOnGround;
            }
        }

        bool isOnGround;

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
                return new Vector2(FrameWidth / 2.0f, FrameHeight / 2.0f);
            }
        }

        public Player(Level _level, Vector2 _position) {
            level = _level;
            LoadContent();
            Reset(_position);
        }

        private string currentAnim = "Idle";
        private SpriteEffects flip = SpriteEffects.None;
        public void LoadContent() {
            SpriteTextures.Add(Level.Content.Load<Texture2D>("Sprites/Player/player"));
            Animation anim = new Animation();
            anim.LoadAnimation("Idle", 0, new List<int> {0, 11, 0, 12}, 7, true);
            SpriteAnimations.Add("Idle", anim);

            anim = new Animation();
            anim.LoadAnimation("Walking", 0, new List<int> {0, 1, 2, 3, 4, 5, 6}, 7, true);
            SpriteAnimations.Add("Walking", anim);

            anim = new Animation();
            anim.LoadAnimation("Jump", 0, new List<int> {7, 8, 9, 10, 9, 8, 7}, 20, false);
            anim.AnimationCallBack(JumpAnimEnd);
            SpriteAnimations.Add("Jump", anim);

            int width = FrameWidth - 4;
            int left = (FrameWidth - width) / 2;
            int height = FrameHeight - 4;
            int top = FrameHeight - height;
            localBounds = new Rectangle(left, top, width, height);
        }

        public void JumpAnimEnd() {
            currentAnim = "Idle";
            SpriteAnimations[currentAnim].Play();
        }

        public void Reset(Vector2 _position) {
            Position = _position;
            Velocity = Vector2.Zero;
            isAlive = true;
            SpriteAnimations[currentAnim].Stop();
            currentAnim = "Idle";
            SpriteAnimations[currentAnim].ResetPlay();
        }

        private void GetInput() {
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();

            movement = gamePadState.ThumbSticks.Left.X * MoveStickScale;

            if(Math.Abs(movement) < 0.5f)
                movement = 0;

            if(gamePadState.IsButtonDown(Buttons.DPadLeft) || keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A)) {
                movement = -1.0f;
            }

            else if(gamePadState.IsButtonDown(Buttons.DPadRight) || keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D)) {
                movement = 1.0f;
            }

            isJumping =
                gamePadState.IsButtonDown(JumpButton) ||
                keyboardState.IsKeyDown(Keys.Space) ||
                keyboardState.IsKeyDown(Keys.Up) ||
                keyboardState.IsKeyDown(Keys.W);

            if(isJumping && currentAnim != "Jump") {
                SpriteAnimations[currentAnim].Stop();
                currentAnim = "Jump";
                SpriteAnimations[currentAnim].ResetPlay();
            }

            if(movement != 0 && currentAnim != "Jump" && currentAnim != "Walking") {
                SpriteAnimations[currentAnim].Stop();
                currentAnim = "Walking";
                SpriteAnimations[currentAnim].ResetPlay();
            }

            else if(currentAnim != "Jump" && movement == 0 && currentAnim == "walking") {
                SpriteAnimations[currentAnim].Stop();
                currentAnim = "Idle";
                SpriteAnimations[currentAnim].ResetPlay();
            }
        }

        public void Update(GameTime time) {
            GetInput();
            ApplyPhysics(time);
            SpriteAnimations[currentAnim].Update(time);
            movement = 0;
            isJumping = false;
        }

        public void draw(GameTime time, SpriteBatch spriteBatch) {
            Rectangle source = GetFrameRectangle(SpriteAnimations[currentAnim].FrameToDraw);
            if(Velocity.X < 0)
                flip = SpriteEffects.FlipHorizontally;
            else if(Velocity.X > 0)
                flip = SpriteEffects.None;

            spriteBatch.Draw(SpriteTextures[0], position, source, Color.White, 0.0f, Origin, 1.0f, flip, 0.0f);
        }

        private void ApplyPhysics(GameTime time) {
            float elapsed = (float) time.ElapsedGameTime.TotalSeconds;
            Vector2 previousPosition = Position;

            velocity.X += movement * MoveAccelaration * elapsed;
            velocity.Y += MathHelper.Clamp(velocity.Y + GravityAccelaration * elapsed, -MaxFallSpeed, MaxFallSpeed);
            velocity.Y = DoJump(velocity.Y, time);

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

        private float DoJump(float vY, GameTime time) {
            if(isJumping) {
                if((!wasJumping && IsOnGround) || jumpTime > 0.0f) {
                    jumpTime += (float) time.ElapsedGameTime.TotalSeconds;
                }

                if(0.0f < jumpTime && jumpTime <= MaxJumpTime) {
                    vY = JumpLaunchVelocity *
                        (1.0f - (float) Math.Pow(jumpTime / MaxJumpTime, JumpControlPower));
                }

                else {
                    jumpTime = 0.0f;
                }
            }
           
            else {
                jumpTime = 0.0f;
            }

            wasJumping = isJumping;
            return vY;
        }


        private void HandleCollisions() {
            Rectangle bounds = BoundingRectangle;
            int leftTile = (int) Math.Floor((float) bounds.Left / Tile.Width);
            int rightTile = (int) Math.Ceiling(((float) bounds.Right / Tile.Width)) - 1;
            int topTile = (int) Math.Floor((float) bounds.Top / Tile.Height);
            int bottomTile = (int) Math.Ceiling(((float) bounds.Bottom / Tile.Height)) - 1;

            isOnGround = false;

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
                                if(previousBottom <= tileBounds.Top)
                                    isOnGround = true;

                                if(collision == TileCollision.Impassable || isOnGround) {
                                    Position = new Vector2(Position.X, Position.Y + depth.Y);
                                    bounds = BoundingRectangle;
                                }
                            }

                            else if(collision == TileCollision.Impassable) {
                                Position = new Vector2((int) (Position.X + (depth.X * 0.75)), Position.Y);
                                bounds = BoundingRectangle;
                            }
                        }

                    }
                }
            }

            previousBottom = bounds.Bottom;
        }
    }
}

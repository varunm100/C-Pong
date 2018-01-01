using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace pong
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Paddle paddleLeft;
        Paddle paddleRight;
        Paddle paddleTop;
        Paddle paddleBottom;

        Texture2D paddleTexture;
        Vector2 paddleScale;
        float paddleSca = 25f;

        Texture2D ballTexture;
        Vector2 ballScale;
        Vector2 ballPosition;
        Vector2 ballVelocity = new Vector2(125,50);
        public static Vector2 ballInitialVelocity = new Vector2(125, 50);
        float PaddleSpeedScaleFac = 1.5f;
        
        float ballSca = 25f;
        Rectangle ballCollider;

        float ScreenWidth;
        float ScreenHeight;

        public static int TurnCount = 0;

        Texture2D pongTable;
        Vector2 tableScale;

        SpriteFont font;
        bool gameOver = false;
        bool LeftWin;
        bool displayWinner = false;
        String winningString;
        
        public static float count = 1;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            ScreenWidth = GraphicsDevice.PresentationParameters.Bounds.Width;
            ScreenHeight = GraphicsDevice.PresentationParameters.Bounds.Height;
            ballPosition = new Vector2(ScreenWidth/2, ScreenHeight/2);
            ballVelocity *= PaddleSpeedScaleFac;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            paddleTexture = this.Content.Load<Texture2D>("pong-paddle");
            paddleScale = new Vector2(paddleSca / paddleTexture.Width, paddleSca / paddleTexture.Width);

            ballTexture = this.Content.Load<Texture2D>("pong-ball");
            ballScale = new Vector2(ballSca / ballTexture.Width, ballSca / paddleTexture.Width);

            paddleLeft = new Paddle(paddleTexture, new Vector2(0, 0), paddleScale, 300, 0);
            paddleRight = new Paddle(paddleTexture, new Vector2(ScreenWidth-(paddleTexture.Bounds.Width*paddleScale.X), 0), paddleScale, 300, 0);

            pongTable = this.Content.Load<Texture2D>("pong-table");
            tableScale = new Vector2(ScreenWidth / pongTable.Width, ScreenHeight / pongTable.Height);

            font = this.Content.Load<SpriteFont>("pong-score-font");
        }

        protected override void UnloadContent()
        {
            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            ballCollider = new Rectangle((int)ballPosition.X, (int)ballPosition.Y, (int)ballTexture.Width*(int)ballScale.X, (int)ballTexture.Height*(int)ballScale.Y);

            paddleLeft.MoveUpADown(Keys.A,Keys.D,gameTime);
            paddleLeft.CheckOutBounds(GraphicsDevice.PresentationParameters.Bounds.Width, GraphicsDevice.PresentationParameters.Bounds.Height);
            ballVelocity = paddleLeft.HandleBallCollision(ballCollider, ballVelocity, ballPosition, 1, -1);

            paddleRight.MoveUpADown(Keys.NumPad4, Keys.NumPad6, gameTime);
            paddleRight.CheckOutBounds(GraphicsDevice.PresentationParameters.Bounds.Width, GraphicsDevice.PresentationParameters.Bounds.Height);
            ballVelocity = paddleRight.HandleBallCollision(ballCollider, ballVelocity, ballPosition, -1, 1);

            ballPosition += ballVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (ballPosition.Y <= 0)
            {
                ballVelocity.Y *= (float)-0.9;
                ballVelocity.X *= (float)1.05;
            }
            else if (ballPosition.Y >= ScreenHeight)
            {
                ballVelocity.Y *= (float)-0.9;
                ballVelocity.X *= (float)1.05;
            }
            if (ballPosition.X < 0)
            {
                Console.WriteLine("Game Over! Right Pong Player Won In " + TurnCount.ToString() + " turns");
                winningString = "Right Pong Player Wins!";
                gameOver = true;
                LeftWin = false;
                TurnCount = 0;
                displayWinner = true;
            } else if (ballPosition.X > ScreenWidth)
            {
                Console.WriteLine("Game Over! Left Pong Player Won In " + TurnCount.ToString() + " turns");
                winningString = "Left Pong Player Wins!";
                gameOver = true;
                LeftWin = true;
                TurnCount = 0;
                displayWinner = true;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.R) && gameOver)
            {
                gameOver = false;
                ballPosition = new Vector2(ScreenWidth / 2, ScreenHeight / 2);
                ballVelocity = ballInitialVelocity;
                ballVelocity *= PaddleSpeedScaleFac;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            
            spriteBatch.Begin();
            spriteBatch.Draw(pongTable, position: Vector2.Zero, scale: tableScale);
            spriteBatch.DrawString(font, TurnCount.ToString(), new Vector2(paddleScale.X*paddleTexture.Width+20, ScreenHeight-45), Color.Black);
            spriteBatch.Draw(paddleRight.paddleTexture, position: paddleRight.position, scale: paddleRight.scale, rotation: paddleRight.rotation, origin: paddleRight.origin);
            spriteBatch.Draw(paddleLeft.paddleTexture, position: paddleLeft.position, scale: paddleLeft.scale, rotation: paddleLeft.rotation, origin: paddleLeft.origin);
            if (displayWinner) { spriteBatch.DrawString(font, winningString, new Vector2(ScreenWidth / 2, ScreenHeight / 2), Color.Black); displayWinner = false; }
            if (!gameOver) { spriteBatch.Draw(ballTexture, position: ballPosition, scale: ballScale); }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

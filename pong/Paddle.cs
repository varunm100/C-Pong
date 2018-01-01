using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace pong
{
    class Paddle
    {
        public Texture2D paddleTexture;
        public Vector2 position;
        public Vector2 scale;
        public Vector2 origin;
        public float moveSpeed;
        public float rotation;
        public float paddleHeight;
        public Vector2 paddleVelocity;

        public Paddle(Texture2D _paddleTexture, Vector2 _position, Vector2 _scale, float _moveSpeed, float _rotation)
        {
            this.moveSpeed = _moveSpeed;
            this.paddleTexture = _paddleTexture;
            this.position = _position;
            this.scale = _scale;
            this.rotation = _rotation;
        }

        public void UpdateOrigin()
        {
            origin = new Vector2((position.X + (paddleTexture.Bounds.Width / 2)), (position.Y + (paddleTexture.Bounds.Height/2)));
        }

        public void MoveUpADown(Keys up, Keys down, GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(up))
            {
                position += new Vector2(0, -moveSpeed) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                paddleVelocity = new Vector2(0, -moveSpeed) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            } else if (state.IsKeyDown(down))
            {
                position += new Vector2(0, moveSpeed) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                paddleVelocity = new Vector2(0, moveSpeed) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public void MoveLeftARight(Keys left, Keys right, GameTime gameTime) {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(left))
            {
                position += new Vector2(-moveSpeed, 0) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                paddleVelocity = new Vector2(-moveSpeed, 0) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            } else if (state.IsKeyDown(right))
            {
                position += new Vector2(moveSpeed, 0) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                paddleVelocity = new Vector2(moveSpeed, 0) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public void CheckOutBounds(float windowWidth, float windowHeight)
        {
            if (position.X < 0)
            {
                position.X = 0;
            }
            if (position.X > windowWidth)
            {
                position.X = windowWidth;
            }
            if (position.Y < 0)
            {
                position.Y = 0;
            }
            if (position.Y > windowHeight-(scale.Y*paddleTexture.Height))
            {
                position.Y = windowHeight - (scale.Y * paddleTexture.Height);
            }
        }

        public Vector2 HandleBallCollision(Rectangle Ball, Vector2 ballVelocity, Vector2 ballPosition, int ScaleCos, int ScaleSin)
        {
            Rectangle PaddleBoundingBox = new Rectangle((int)position.X, (int)position.Y, (int)(scale.X * paddleTexture.Width), (int)(scale.Y * paddleTexture.Height));
            paddleHeight = paddleTexture.Height * scale.Y;
            if (PaddleBoundingBox.Intersects(Ball))
            {
                //Console.WriteLine("Collision Detected!");
                float YIntersection = (position.Y + (paddleHeight / 2)) - ballPosition.Y;
                float NormalizedYIntersect = (YIntersection / (paddleHeight / 2));
                float bounceAngle = NormalizedYIntersect * 75;
                ballVelocity = new Vector2((float)(ScaleCos*600*Game1.count*Math.Cos(ConvertDegToRad(bounceAngle))), (float)(ScaleSin*600*Math.Sin(ConvertDegToRad(bounceAngle))));
                Game1.TurnCount += 1;
                //Game1.count *= 1.5f;
                Console.WriteLine(bounceAngle);
                return ballVelocity;
            } else
            {
                return ballVelocity;
            }
        }

        public static float ConvertDegToRad(float deg)
        {
            return (float)Math.PI * (float)deg / 180.0f;
        }
    }
}

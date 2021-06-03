﻿using System.Collections.Generic;
using System.Linq;
using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Factories;
using IPCA.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ZeldaMario
{
    public class Bullet : Sprite
    {
        public float speed = 3000f;
        private Game1 _game;
        int direction;
        public bool morte = false;
       

        float dieTimer = 3f;
        public Bullet(Game1 game, string texName, float x , float y , Direction dir = Direction.Right) :
            base("Bullet",
                game.Content.Load<Texture2D>(texName), new Vector2(x, y), false 
                )
        {
            
            _game = game;
            if (dir == Direction.Right)
            {
                direction = -1;
                _direction = Direction.Right;
            }
            else
            {
                direction = +1;
                _direction = Direction.Left;
            }

            AddRectangleBody(
                game.Services.GetService<World>(),
                isKinematic: false
            ); // kinematic is false by default
            //Body.IsSensor = true;
            Body.IgnoreGravity = true;
            Body.Friction = 0;
            //Body.OnCollision = (a, b, c) => { morte = true; };
        }

        public override void Update(GameTime gameTime)
        {

            moverBala(gameTime, 2500f);
            CountDown(ref dieTimer, gameTime);
            if (dieTimer <= 0) morte = true;
            base.Update(gameTime);
        }
        //Conta o tempo para a morte da bala
        private void CountDown(ref float _dieTimer, GameTime gameTime)
        {
            _dieTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void moverBala(GameTime gameTime,float speed) 
        {
            if (_direction == Direction.Left)  // A planta é com lado invertido, ou seja, quando estamos a olhar para a direita na verdade estamos a calcular pro lado esquerdo 
            {
                _position.X -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else _position.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
        }
    }
}

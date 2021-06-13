using System;
using System.Collections.Generic;
using System.Linq;
using Genbox.VelcroPhysics.Collision.RayCast;
using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Factories;
using IPCA.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace ZeldaMario 
{
    public class Prantinha : AnimatedSprite
    {
        
        private Game1 _game;
        private float attackDist=3f;
        public List<Bullet> tiro = new List<Bullet>();
        private float timer, resetTimer = 4f;

        public Prantinha(Game1 game, float x, float y) : base
            ("prantinha", new Vector2(x,y),
            Enumerable.Range(1,2).Select(n => game.Content.Load<Texture2D>($"planta-idle-export{n}")
            ).ToArray())
        {
            timer = resetTimer;
            _game = game;

            AddRectangleBody(
            _game.Services.GetService<World>(),
                width: _size.X/1.8f ,
                height: _size.Y/0.8f ,
                true
          ); // kinematic is false by default - kinematic : atributo q define so o body vai ser afetado pela fisica ou nao

            Body.Friction = 0f;
           
        }
        public bool distToPlayer(Player p)
        {
            float distX, h, distY,direction;
            
            direction=p.Position.X - Position.X;
            if (direction < 0)
            {
                _direction = Direction.Right;
            }
            else _direction = Direction.Left;
            distX = MathF.Abs(direction);
            distY = MathF.Abs(p.Position.Y - Position.Y);
            h = MathF.Abs ( MathF.Sqrt(distX * distX + distY * distY)); //calculo da distancia para o player 
            
            if (h <= attackDist) return true;
            else return false;
           
        }


        public override void Update(GameTime gameTime)
        {
            foreach (Bullet b in tiro.ToArray())
            {
                if (b.morte)
                {
                    World world = _game.Services.GetService<World>();
                    world.RemoveBody(b.Body);
                    tiro.Remove(b);
                }
            }
            if (distToPlayer(_game._player))
            {
                if (tiro.Count > 0)
                {
                    foreach (Bullet b in tiro.ToArray())
                    {
                        b.Update(gameTime);
                        if (b.morte)
                        {
                            World world = _game.Services.GetService<World>();
                            world.RemoveBody(b.Body);
                            tiro.Remove(b);
                        }
                    }
                }
                timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (timer <= 0)
                {
                    Bullet bala;

                    //Imagem invertida, então quando ta pra direita temos que virar pra esquerda
                    if (_direction == Direction.Right)
                    {
                        bala = new Bullet(_game, "p_tiro", this._position.X - 0.25f, this._position.Y + 0.1f, _direction);
                        _game._plantatiroSound.Play();
                        tiro.Add(bala);

                    }
                    else
                    {
                        bala = new Bullet(_game, "p_tiro", this._position.X +0.25f, this._position.Y + 0.1f, _direction);
                        _game._plantatiroSound.Play();
                        tiro.Add(bala);
                    }
                    timer = resetTimer;
                }

            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach(Bullet b in tiro.ToArray()) 
            {
                b.Draw(spriteBatch, gameTime);
            }
            base.Draw(spriteBatch, gameTime);
        }

    }
}

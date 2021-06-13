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

    public class Gumba : AnimatedSprite
    {
       
        private Game1 _game;
        public bool walk = false;
        public float speed = 15f;
        // hit do gumba 
        public float timer;
        public float resetDoTempo = 0.5f;
        public int life = 100;
        public bool hit = false;

        private int patrolOffset = 3; // raio que o gumba pode andar

        private List<Texture2D> _runFrames;

        public Gumba(Game1 game, float x = 0, float y = 0) :
            base("gumba",
                new Vector2(x, y),
                Enumerable.Range(1, 4).Select<int, Texture2D>(
                    n => game.Content.Load<Texture2D>($"gumba-walking-export{n}")).ToArray()
                )
        {
            _runFrames = _textures;
            _direction = Direction.Left;
            timer = resetDoTempo;
            _game = game;

            AddCircleBody(
                game.Services.GetService<World>(),
                raids: _size.X / 4f
                );
            Body.Restitution = 0;
            Body.Friction = 0;

            Fixture sensor = FixtureFactory.AttachRectangle(
                _size.X / 3f, _size.Y * 0.2f,
                4, new Vector2(0, -_size.Y / 2f),
                Body);

            sensor.IsSensor = true;

            sensor.OnCollision = (a, b, contact) => walk = true;
            sensor.OnSeparation = (a, b, contact) =>
            {
                patrolOffset -= 1;
                if (patrolOffset <= 0)
                {
                    if (_direction == Direction.Right) _direction = Direction.Left;
                    else if (_direction == Direction.Left) _direction = Direction.Right;
                    walk = false;
                    patrolOffset = 3;
                }
            };

        }

        public override void Update(GameTime gameTime)
        {
            if (hit && timer > 0)
            {
                corDoDesenho = Color.Red;
                tempo(gameTime, ref timer);
               // Console.WriteLine("entreiNoHit");
            }
            else corDoDesenho = Color.White;

            if (Body != null)
            {
                if (walk && _direction == Direction.Right)
                {
                    Body.LinearVelocity = new Vector2(-speed, Body.LinearVelocity.Y) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else if (walk && _direction == Direction.Left)
                {
                    Body.LinearVelocity = new Vector2(speed, Body.LinearVelocity.Y) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
            base.Update(gameTime);
        }

        // Função para verificação para o decremento de vida do gumba
        // ref = endereço de memoria 
        public void tempo(GameTime gameTime, ref float timer)
        {
            if (timer > 0) timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
        }
    }
}

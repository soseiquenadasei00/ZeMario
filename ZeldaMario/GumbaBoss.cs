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
    public class GumbaBoss : AnimatedSprite
    {
        enum Status
        {
            Walk,Jumping
        }
        private Status _status = Status.Walk;
        private Game1 _game;
        public float resetDoTempo = 4f;
        public float resetACor = 0.5f;
        public float timer;
        public float timerdaCor;
        public bool walk = false;
        public float speed = 15f;
        public int life = 500;
        public bool hit = false;
        bool jump = false;
        private int patrolOffset = 5;
        private List<Texture2D> _puloInicial=new List<Texture2D>();
        private List<Texture2D> _salto=new List<Texture2D>();
        private List<Texture2D> _aterrar= new List<Texture2D>();
        private List<Texture2D> _walking= new List<Texture2D>();
        
        private bool _isGrounded = false;
       
        public GumbaBoss(Game1 game, float x = 0, float y = 0) :
           base("GumbaBoss",
               new Vector2(x, y),
               Enumerable.Range(0, 1).Select<int, Texture2D>(
                   n => game.Content.Load<Texture2D>("assets/orig/images/idle")).ToArray()
               )
        {
            timer = resetDoTempo;
            _walking = _textures;
            _puloInicial = Enumerable.Range(1, 3)
                .Select(
                    n => game.Content.Load<Texture2D>($"salto incio/preparas_para_saltar{n}")
                ).ToList();
            _salto = Enumerable.Range(1, 3)
                .Select(
                    n => game.Content.Load<Texture2D>($"salto/inicio_salto_e_ar{n}")
                ).ToList();
            _direction = Direction.Left;
           
            _game = game;

            AddCircleBody(
                 game.Services.GetService<World>(),
                 raids: _size.X / 2f
                 );
            Body.Restitution = 0;
            Body.Friction = 0.01f;

            Fixture sensor = FixtureFactory.AttachRectangle(
                _size.X / 3f, _size.Y * 0.2f,
                4, new Vector2(0, -_size.Y / 2f),
                Body);

            sensor.IsSensor = true;

          

            sensor.OnCollision = (a, b, contact) =>
            {
               _isGrounded = true;
                if(_isGrounded)
                walk = true;
            };
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
            sensor.IsSensor = true;
        }
        public void tempo(GameTime gameTime, ref float timer)
        {
            if (timer > 0)
            {
                jump = false;
                timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                jump = true;
                Body.LinearVelocity = new Vector2(Body.LinearVelocity.X, 4f);
            }
        }
        public void tempoDaCor(GameTime gameTime, ref float timer)
        {
            if (timerdaCor > 0)
            {
                corDoDesenho = Color.Red;
                timerdaCor -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            
        }
        public override void Update(GameTime gameTime)
        {
            if (hit && timerdaCor > 0)
            {
                tempoDaCor(gameTime, ref timerdaCor);
                hit = false;
            }
            else
            {
                timerdaCor = resetACor;
                corDoDesenho = Color.White;
            }

            if (Body != null && timer > 0 && jump == false)
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


            if (_isGrounded)
            {
                tempo(gameTime, ref timer);
            }
            else timer = resetDoTempo;

            if (_status == Status.Walk && Body.LinearVelocity.Y > 0.001f)
            {
                _status = Status.Jumping;
                if (_isGrounded)
                {
                    _textures = _puloInicial;
                    timer = resetDoTempo;
                    _currentTexture = 0;
                    if(_currentTexture == 3)
                    {
                        _textures = _salto;
                        _currentTexture = 0;
                    }
                }
                else
                {
                    _textures = _aterrar;
                    _currentTexture = 0;
                }
            }
            if (_status == Status.Jumping && Body.LinearVelocity.Y < 0.001f)
            {
                _status = Status.Walk;
                _textures = _walking;
                _currentTexture = 0;
            }

            

            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
        }
    }
}

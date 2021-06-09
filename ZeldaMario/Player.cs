using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using IPCA.MonoGame;
using System.Linq;
using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Factories;

namespace ZeldaMario
{
    public class Player : AnimatedSprite
    {
        enum Status
        {
            Idle, Walk,
        }
        private Status _status = Status.Idle;

        private Game1 _game;
        private bool _isGrounded = false;
        private bool attack = false;
        public int vidas = 3;
        public Vector2 offset = new Vector2(0, -0.05f);
        public bool morte = false;
        public bool toque = false;
        private Vector2 posicaoInicial;
        
        public int countCoin;
        public int direçãoPlayer = 1;
        private List<ITempObject> _objects;
        private List<Texture2D> _idleFrames = new List<Texture2D>();
        private List<Texture2D> _walkFrames = new List<Texture2D>();
        private List<Texture2D> _attackFrames = new List<Texture2D>();

        public Player(Game1 game, float x, float y) :
            base("player",
                new Vector2(x, y),
                Enumerable.Range(1, 22)
                    .Select(
                        n => game.Content.Load<Texture2D>($"Wulfricidle/idle{n}")
                        )
                    .ToArray())
        {
            posicaoInicial = new Vector2(x, y);
            _idleFrames = _textures; // loaded by the base construtor

            _walkFrames = Enumerable.Range(1, 10)
                .Select(
                    n => game.Content.Load<Texture2D>($"Wulfricwalking/andar-d{n}")
                )
                .ToList();
            _attackFrames = Enumerable.Range(1, 16)
                .Select(
                    n => game.Content.Load<Texture2D>($"Wulfricatack/atack{n}")
                )
                .ToList();
            _game = game;

            _objects = new List<ITempObject>();

            AddCircleBody(
                 _game.Services.GetService<World>(),
                 raids: 0.06f
                );
            Fixture fixtureCorpo = FixtureFactory.AttachCircle(
                radius: 0.035f, 0, Body, offset
                );
            Body.Friction = 0.01f;
            Fixture sensor = FixtureFactory.AttachRectangle(
                _size.X / 4f, _size.Y / 16f,
                4, new Vector2(0, -_size.Y / 2.5f),
                Body);
            sensor.IsSensor = true;

            sensor.OnCollision = (a, b, contact) =>
            {
                _isGrounded = true;
            };
            sensor.OnSeparation = (a, b, contact) => _isGrounded = false;

            KeyboardManager.Register(
                Keys.Space,
                KeysState.GoingDown,
                () =>
                {
                    if (_isGrounded) Body.LinearVelocity = new Vector2(0, 4); //alcance do salto
                });
            KeyboardManager.Register(
                Keys.A,
                KeysState.Down,
                () => { Body.LinearVelocity = new Vector2(-1, Body.LinearVelocity.Y); });
            KeyboardManager.Register(
                Keys.D,
                KeysState.Down,
                () => { Body.LinearVelocity = new Vector2(1, Body.LinearVelocity.Y); });

            KeyboardManager.Register(
                Keys.A,
                KeysState.GoingUp,
                () => { Body.LinearVelocity = new Vector2(0, 0); });
            KeyboardManager.Register(
                Keys.D,
                KeysState.GoingUp,
                () => { Body.LinearVelocity = new Vector2(0, 0); });
            KeyboardManager.Register(Keys.F, KeysState.Down, () => {attack = true; Attack(); });
            KeyboardManager.Register(Keys.F, KeysState.Up, () => { AbortAttack(); });

        }
        public override void Update(GameTime gameTime)
        {
            if (_direction == Direction.Right) direçãoPlayer = 1;
            else direçãoPlayer = -1;

            if (vidas == 0)
            {
                resetar();
            }
            //Verficação dos colliders com inimigos ou com a moeda 
            Body.OnCollision = (a, b, c) =>
            {
                Sprite temp;
                temp = (Sprite)b.Body.UserData;
                if (b.Body.UserData != null)
                {
                    if (temp.Name == "Coin")
                    {
                        foreach (Coin coin in _game._coin.ToArray())
                        {
                            if (temp.Position == coin.Position)
                            {
                                countCoin++;
                                _game._coin.Remove(coin);

                            }
                        }
                    }
                    if (temp.Name == "Bullet")
                    {
                        foreach (Prantinha p in _game._prantinha.ToArray())
                        {
                            foreach (Bullet bala in p.tiro.ToArray())
                            {
                                if (bala.Position == temp.Position)
                                {
                                    World world = _game.Services.GetService<World>();
                                    world.RemoveBody(bala.Body);
                                    p.tiro.Remove(bala);
                                    break;
                                }
                            }
                        }
                        vidas -= 1;
                    }
                    if (temp.Name == "gumba" && attack == false)
                    {
                        if (toque == false)
                            removerVida(gameTime);
                    }
                    if (temp.Name == "assets/orig/images/tile240" || temp.Name == "assets/orig/images/tile241")
                    {
                        resetar();
                    }
                }
            };

            if (_isGrounded) toque = false; // Para o gumba 

            foreach (ITempObject obj in _objects)
                obj.Update(gameTime);

            if (_status == Status.Idle && Body.LinearVelocity.LengthSquared() > 0.001f)
            {
                _status = Status.Walk;
                _textures = _walkFrames;
                _currentTexture = 0;
            }

            if (_status == Status.Walk && Body.LinearVelocity.LengthSquared() <= 0.001f)
            {
                _status = Status.Idle;
                _textures = _idleFrames;
                _currentTexture = 0;
            }

            if (Body.LinearVelocity.X < 0f) _direction = Direction.Left;
            else if (Body.LinearVelocity.X > 0f) _direction = Direction.Right;

            if (attack )
            {
                _textures = _attackFrames;
            }
            else if (_status == Status.Idle) _textures = _idleFrames;
            else
            {
                _textures = _walkFrames;
            }

            base.Update(gameTime);
            Camera.LookAt(_position);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
            foreach (ITempObject obj in _objects)
                obj.Draw(spriteBatch, gameTime);
        }

        //Funções auxiliares

        //Reseta o player para a posição inicial 
        public void resetar()
        {
            Body.Position = posicaoInicial;
            Body.LinearVelocity = Vector2.Zero;
            morte = false;
            if (vidas == 0) vidas = 3;

        }
        public void removerVida(GameTime gameTime)
        {
            Console.WriteLine(vidas);
            if (_direction == Direction.Right) Body.LinearVelocity = new Vector2(-1f, 2.5f);
            else Body.LinearVelocity = new Vector2(1f, 2.5f);
            vidas--;
            toque = true;
        }

        public void Attack()
        {
            Fixture fixture;
           
            if (_currentTexture == 9)
            {
                fixture = FixtureFactory.AttachRectangle(
                    _size.X / 8f, _size.Y / 6f,
                    1,
                    new Vector2((direçãoPlayer * _size.X / 2), _size.Y / 15f),
                    Body);

                fixture.IsSensor = true;
                fixture.OnCollision = (a, b, c) =>
                {
                    Sprite temp;
                    temp = (Sprite)b.Body.UserData;

                    if (temp.Name == "gumba")
                    {
                        foreach (Gumba g in _game._gumba.ToArray())
                        {
                            if (g.Position == temp.Position)
                            {

                                if (g.life > 0)
                                {
                                    g.hit = true;
                                    g.timer = g.resetDoTempo;
                                    g.life--;
                                    break;
                                }
                                else
                                {
                                    World world = _game.Services.GetService<World>();
                                    world.RemoveBody(g.Body);
                                    _game._gumba.Remove(g);
                                    break;
                                }

                            }
                        }
                    }
                    else if (temp.Name == "prantinha")
                    {
                        foreach (Prantinha prantinha in _game._prantinha.ToArray())
                        {
                            if (prantinha.Position == temp.Position)
                            {
                                World world = _game.Services.GetService<World>();
                                world.RemoveBody(prantinha.Body);
                                _game._prantinha.Remove(prantinha);
                                break;
                            }
                        }
                    }
                };
            }
        }

        public void StopAttack()
        {

            if (Body.FixtureList.Count > 3)
            {
                Body.DestroyFixture(Body.FixtureList[3]);
                
            }
        }
        public void AbortAttack()//usado para quando parar de clicar na tecla
        {
            StopAttack();

            if (attack)
            {
                attack = false;
                _textures = _idleFrames;
            }
        }
    }

}

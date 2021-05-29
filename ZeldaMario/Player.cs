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

        public int countCoin;
        private List<ITempObject> _objects;

        private List<Texture2D> _idleFrames=new List<Texture2D>();
        private List<Texture2D> _walkFrames;

        public Player(Game1 game, float x, float y) :
            base("player",
                new Vector2(x, y),
                Enumerable.Range(0,3)
                    .Select(
                        n => game.Content.Load<Texture2D>($"WulfricWalk{n}")
                        )
                    .ToArray())
        {
            _walkFrames = _textures; // loaded by the base construtor
            countCoin = 0;
            _idleFrames.Add(game.Content.Load<Texture2D>("Wulfric0"));
                //= Enumerable.Range(0, 4)
                //.Select(
                //    n => game.Content.Load<Texture2D>($"WulfricWalk{n}")
                //)
                //.ToList();

            _game = game;


            _objects = new List<ITempObject>();

            AddRectangleBody(
                _game.Services.GetService<World>(),
                width: _size.X /1.8f,
                height: _size.Y /1.25f
            ); // kinematic is false by default

            Fixture sensor = FixtureFactory.AttachRectangle(
                _size.X / 4f, _size.Y / 16f,
                4, new Vector2(0, -_size.Y / 2.2f),
                Body);
            sensor.IsSensor = true;

            sensor.OnCollision = (a, b, contact) =>
            {
                if (b.GameObject().Name != "bullet")
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
            

        }

        public override void Update(GameTime gameTime)
        {
            Body.OnCollision = (a, b, c) =>
            {
                Sprite temp;
                temp = (Sprite)b.Body.UserData;
                if (temp.Name == "Coin")
                {
                    foreach (Coin coin in _game._coin.ToArray())
                    {
                        if (temp.Position == coin.Position)
                        {
                            countCoin++;
                            _game._coin.Remove(coin);
                            Console.WriteLine(_game._coin.Count);
                        }
                    }
                }
            };
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



            base.Update(gameTime);
            Camera.LookAt(_position);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
            foreach (ITempObject obj in _objects)
                obj.Draw(spriteBatch, gameTime);
        }
    }
}

﻿using System;
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
      

        private List<ITempObject> _objects;

        private List<Texture2D> _idleFrames;
        private List<Texture2D> _walkFrames;

        public Player(Game1 game, float x, float y) :
            base("player",
                new Vector2(x, y),
                Enumerable.Range(1, 5)
                    .Select(
                        n => game.Content.Load<Texture2D>($"Wulfric{n}")
                        )
                    .ToArray())
        {
            _idleFrames = _textures; // loaded by the base construtor

            _walkFrames = Enumerable.Range(1, 5)
                .Select(
                    n => game.Content.Load<Texture2D>($"walk{n}")
                )
                .ToList();

            _game = game;


            _objects = new List<ITempObject>();

            AddRectangleBody(
                _game.Services.GetService<World>(),
                width: _size.X * 5.8f,
                height: _size.Y * 8f
            ); // kinematic is false by default

            Fixture sensor = FixtureFactory.AttachRectangle(
                _size.X * 6 , _size.Y,
                4, new Vector2(0, -_size.Y * 4f),
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
                    if (_isGrounded) Body.LinearVelocity = new Vector2(0, 10); //alcance do salto
                });
            KeyboardManager.Register(
                Keys.A,
                KeysState.Down,
                () => { Body.LinearVelocity = new Vector2(-5, Body.LinearVelocity.Y); });
            KeyboardManager.Register(
                Keys.D,
                KeysState.Down,
                () => { Body.LinearVelocity = new Vector2(5, Body.LinearVelocity.Y); });

            KeyboardManager.Register(
                Keys.A,
                KeysState.GoingUp,
                () => { Body.LinearVelocity = new Vector2(0, 0); });
            KeyboardManager.Register(
                Keys.D,
                KeysState.GoingUp,
                () => { Body.LinearVelocity = new Vector2(0, 0); });
            //BALAAA
            //KeyboardManager.Register(
            //    Keys.F, KeysState.GoingDown,
            //    () =>
            //    {
            //        Vector2 pixelClick = Mouse.GetState().Position.ToVector2();
            //        Vector2 pixelDyno = Camera.Position2Pixels(_position);
            //        Vector2 delta = pixelClick - pixelDyno;
            //        delta.Normalize();
            //        delta.Y = -delta.Y; // Invert for "virtual" world
            //        Vector2 dir = 5f * delta;

            //        Bullet bullet = new Bullet(_fireBall, _position,
            //            dir, game.Services.GetService<World>());
            //        _objects.Add(bullet);
            //    }
            //    );

        }

        public override void Update(GameTime gameTime)
        {
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

            //UPDATE DA BALA
            //_objects.AddRange(_objects
            //    .Where(obj => obj is Bullet)
            //    .Cast<Bullet>()
            //    .Where(b => b.Collided)
            //    .Select(b => new Explosion(_game, b.ImpactPos))
            //    .ToArray()
            //);
            //_objects = _objects.Where(b => !b.IsDead()).ToList();
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
            foreach (ITempObject obj in _objects)
                obj.Draw(spriteBatch, gameTime);
        }
    }
}

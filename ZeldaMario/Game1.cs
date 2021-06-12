﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using IPCA.MonoGame;
using Genbox.VelcroPhysics.Dynamics;
using System.Collections.Generic;

namespace ZeldaMario
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private World _world;
        private Scene _scene;
        public Player _player;
        public Bandeira _bandeira;
        public Texture2D background;
       
        public List<Prantinha> _prantinha = new List<Prantinha>();
        public List<Gumba> _gumba = new List<Gumba>(); 
        public List<Coin> _coin = new List<Coin>();

        private Texture2D _vida0;
        private Texture2D _vida1;
        private Texture2D _vida2;
        private Texture2D _vida3;
        Vector2 _posicaoVida;



        //  public Player Player => _player;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _world = new World(new Vector2(0, -10f)); //gravidade 
            new KeyboardManager(this);
            Services.AddService(_world);
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = 800;
            _graphics.PreferredBackBufferWidth = 1600;
            _graphics.ApplyChanges();

            Debug.SetGraphicsDevice(GraphicsDevice);

            new Camera(GraphicsDevice, height: 2.5f);//zoom da cam
            Camera.LookAt(Camera.WorldSize / 4f);
          

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _scene = new Scene(this, "MainScene");
            background = Content.Load<Texture2D>("background");
            _vida0 = Content.Load<Texture2D>("0hearts");
            _vida1 = Content.Load<Texture2D>("1heart");
            _vida2 = Content.Load<Texture2D>("2hearts");
            _vida3 = Content.Load<Texture2D>("3hearts");

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _world.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);
            _player.Update(gameTime);
            _bandeira.Update(gameTime);
            foreach (Prantinha p in _prantinha.ToArray())
            {
                foreach (Bullet b in p.tiro.ToArray()) b.Update(gameTime);
                p.Update(gameTime);
            }
            foreach (Gumba g in _gumba.ToArray())
            {
                g.Update(gameTime);
            }

            foreach (Coin c in _coin)
            {
                c.Update(gameTime);
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
            _spriteBatch.Draw(background,new Rectangle(0,0,1600,800),Color.White);
            _bandeira.Draw(_spriteBatch, gameTime);
            _scene.Draw(_spriteBatch, gameTime);
           _player.Draw(_spriteBatch, gameTime);


            //desenhar vidas
            Vector2 anchor = new Vector2(_vida3.Width / 32f, _vida3.Height / 16f);
            Vector2 scale = Camera.Length2Pixels(_vida0.Bounds.Size.ToVector2() / 4048f);
            scale.X = scale.X / 2f;
            _posicaoVida = new Vector2(20, 750);

            //
            if (_player.vidas == 0) _spriteBatch.Draw(_vida0, _posicaoVida, null, Color.White,
                        0, anchor, scale * 2f, 0, 0);
            if (_player.vidas == 1) _spriteBatch.Draw(_vida1, _posicaoVida, null, Color.White,
                        0, anchor, scale * 2f, 0, 0);
            if (_player.vidas == 2) _spriteBatch.Draw(_vida2, _posicaoVida, null, Color.White,
                        0, anchor, scale * 2f, 0, 0);
            if (_player.vidas == 3) _spriteBatch.Draw(_vida3, _posicaoVida, null, Color.White,
                        0, anchor, scale * 2f, 0, 0);




            foreach (Prantinha p in _prantinha)
            {
                p.Draw(_spriteBatch, gameTime);
            }
            foreach (Gumba g in _gumba)
            {
                g.Draw(_spriteBatch, gameTime);
            }
            foreach (Coin c in _coin)
            {
                c.Draw(_spriteBatch, gameTime);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

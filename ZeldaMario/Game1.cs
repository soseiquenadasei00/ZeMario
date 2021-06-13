using Microsoft.Xna.Framework;
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
        public World _world;
        public Scene _scene;
        public Player _player;
        public GumbaBoss _boss;
        public Texture2D background;
        private SpriteFont arial12;
        private int countMoeda = 0;

        public List<Prantinha> _prantinha = new List<Prantinha>();
        public List<Gumba> _gumba = new List<Gumba>(); 
        public List<Coin> _coin = new List<Coin>();
        
        public bool changeScene = false;
        private Texture2D _vida0, _vida1, _vida2, _vida3;
        private Texture2D _moedinha;
        private Texture2D backgroudMenu;
        Vector2 _posicaoVida;
        Vector2 _posicaoMoedinha;
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
            _scene = new Scene(this, "MenuScene");
            arial12 = Content.Load<SpriteFont>("arial12");
            background = Content.Load<Texture2D>("background");
            _vida0 = Content.Load<Texture2D>("0hearts");
            _vida1 = Content.Load<Texture2D>("1heart");
            _vida2 = Content.Load<Texture2D>("2hearts");
            _vida3 = Content.Load<Texture2D>("3hearts");
            _moedinha = Content.Load<Texture2D>("moedinha");
            backgroudMenu = Content.Load<Texture2D>("cave");

        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
         
                _world.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);
            if (Keyboard.GetState().IsKeyDown(Keys.R)) Initialize();
            _player.Update(gameTime);
            if (_scene.filename == "Content/scenes/MainScene.dt" && _boss !=null)
            {
                
                _boss.Update(gameTime);
            }

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
            if (_scene.filename == "Content/scenes/MenuScene.dt")
                _spriteBatch.Draw(backgroudMenu, new Rectangle(0, 0, 1600, 800), Color.White);
            else _spriteBatch.Draw(background, new Rectangle(0, 0, 1600, 800), Color.White);

            _player.Draw(_spriteBatch, gameTime);

            _scene.Draw(_spriteBatch, gameTime);

            //desenhar vidas e moeda 
            if (_scene.filename == "Content/scenes/MainScene.dt")
            {
               if(_boss != null)
                _boss.Draw(_spriteBatch, gameTime);
                
                Vector2 anchor = new Vector2(_vida3.Width / 32f, _vida3.Height / 16f);
                Vector2 scale = Camera.Length2Pixels(_vida0.Bounds.Size.ToVector2() / 4048f);
                scale.X = scale.X / 2f;
                _posicaoVida = new Vector2(20, 750);
                _posicaoMoedinha = new Vector2(0, 650);

                //desenhar vidas
                if (_player.vidas == 0) _spriteBatch.Draw(_vida0, _posicaoVida, null, Color.White,
                            0, anchor, scale * 2f, 0, 0);
                if (_player.vidas == 1) _spriteBatch.Draw(_vida1, _posicaoVida, null, Color.White,
                            0, anchor, scale * 2f, 0, 0);
                if (_player.vidas == 2) _spriteBatch.Draw(_vida2, _posicaoVida, null, Color.White,
                            0, anchor, scale * 2f, 0, 0);
                if (_player.vidas == 3) _spriteBatch.Draw(_vida3, _posicaoVida, null, Color.White,
                            0, anchor, scale * 2f, 0, 0);



                Vector2 posicaoMoeda = new Vector2(85, 660);
                countMoeda = _player.countCoin;
                this.drawCoinsText(posicaoMoeda, $"{countMoeda}");

                _spriteBatch.Draw(_moedinha, _posicaoMoedinha, null, Color.White, 0, anchor, scale * 1.5f, 0, 0);

            }
           
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
        public void drawCoinsText(Vector2 _position, string _text)
        {
            _spriteBatch.DrawString(
                arial12,
                 _text,
                _position,
                Color.White);
        }
    }
}

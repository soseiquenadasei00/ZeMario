using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using IPCA.MonoGame;
using Genbox.VelcroPhysics.Dynamics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

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
        public Flag _flag;
        public bool win = false;

        public List<Prantinha> _prantinha = new List<Prantinha>();
        public List<Gumba> _gumba = new List<Gumba>(); 
        public List<Coin> _coin = new List<Coin>();
        public List<AnimatedSprite> _extra= new List<AnimatedSprite>();

        //som
        public  SoundEffect _coinSound;
        public  SoundEffect _gameOverSound;
        public  SoundEffectInstance _gameOverSoundInstance;
        public  SoundEffect _gameWinSound;
        public  SoundEffectInstance _gameWinSoundInstance;
        public  SoundEffect _jumpSound;
        public  SoundEffectInstance _jumpSoundInstance;
        
        public  SoundEffect _playerAttackSound;
        public  SoundEffectInstance _playerAttackSoundInstance;
        public  SoundEffect _plantatiroSound;

        public SoundEffect _playerGetHit;
        public SoundEffectInstance _playerGetHitInstance;


        public Song[] _ambienteSound = new Song[2];
        public Song _bossMusic;
        public bool _sonido = true;

        private float _volume = 0.5f;


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

            new Camera(GraphicsDevice, height: 5f);//zoom da cam
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
            backgroudMenu = Content.Load<Texture2D>("cave_2.0");

            _ambienteSound[0] = Content.Load<Song>("music");
            _ambienteSound[1] = Content.Load<Song>("boss-music");




            _coinSound = Content.Load<SoundEffect>("coin");
            _gameOverSound = Content.Load<SoundEffect>("game_over");
            _gameOverSoundInstance = _gameOverSound.CreateInstance();
            _gameWinSound = Content.Load<SoundEffect>("game_win");
            _gameWinSoundInstance = _gameWinSound.CreateInstance();
            _jumpSound = Content.Load<SoundEffect>("jump");
            _jumpSoundInstance = _jumpSound.CreateInstance();
            _playerAttackSound = Content.Load<SoundEffect>("player_atack");
            _playerAttackSoundInstance = _playerAttackSound.CreateInstance();
            _playerGetHit = Content.Load<SoundEffect>("player_atack");
            _playerGetHitInstance = _playerGetHit.CreateInstance();

            _plantatiroSound = Content.Load<SoundEffect>("planta_shot");

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.25f;
            MediaPlayer.Play(_ambienteSound[0]);
                 

        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
         
             _world.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);
            _player.Update(gameTime);

            //som
            if (!win)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.K)) _volume += 0.1f; //aumenta volume
                if (Keyboard.GetState().IsKeyDown(Keys.J)) _volume -= 0.1f; //diminui volume
            }
                if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit(); //sair do jogo
            
             // MediaPlayer.Play(_bossMusic);

            _volume = (float)System.Math.Clamp(_volume, 0.0, 1.0);
            MediaPlayer.Volume = _volume;
           

            //carregar o boss e flag apenas na scene principal
            if (_scene.filename == "Content/scenes/MainScene.dt" && _boss !=null)
            {
                _boss.Update(gameTime);
                _flag.Update(gameTime);
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

                _flag.Draw(_spriteBatch, gameTime);
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
                
                if (this.win)
                {
                    Vector2 windowSize = new Vector2(
                           _graphics.PreferredBackBufferWidth,
                           _graphics.PreferredBackBufferHeight);
                    // Transparent Layer
                    Texture2D pixel = new Texture2D(GraphicsDevice, 1, 1);
                    pixel.SetData(new[] { Color.White });
                    _spriteBatch.Draw(pixel,
                        new Rectangle(Point.Zero, windowSize.ToPoint()),
                        new Color(Color.Green, 0.5f));

                    // Draw Win Message
                    string win = $" GANHAMOS!!!";
                    Vector2 winMeasures = arial12.MeasureString(win) / 2f;
                    Vector2 windowCenter = windowSize / 2f;
                    Vector2 pos = windowCenter - winMeasures;
                    _spriteBatch.DrawString(arial12, win, pos, Color.HotPink);
                }


                //texto das moedas
                Vector2 posicaoMoeda = new Vector2(85, 660);
                
                this.drawCoinsText(posicaoMoeda, $"{_player.countCoin}");
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

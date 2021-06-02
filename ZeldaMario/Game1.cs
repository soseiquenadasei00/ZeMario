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
        private World _world;
        private Scene _scene;
        public Player _player;
       
        public List<Prantinha> _prantinha= new List<Prantinha>();
        public List<Gumba> _gumba=new List<Gumba>(); 
        public List<Coin> _coin=new List<Coin>();
        

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

            new Camera(GraphicsDevice, height: 10.5f);//zoom da cam
            Camera.LookAt(Camera.WorldSize / 2f);
          

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _scene = new Scene(this, "MainScene");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _world.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);
            _player.Update(gameTime);
            foreach (Prantinha p in _prantinha)
            {
                p.Update(gameTime);
            }
            foreach (Gumba g in _gumba)
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
            _scene.Draw(_spriteBatch, gameTime);
           _player.Draw(_spriteBatch, gameTime);
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

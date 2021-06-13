using System.Linq;
using Genbox.VelcroPhysics.Dynamics;
using IPCA.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace ZeldaMario
{
    public class Flag : AnimatedSprite
    {
        Game1 _game;
        public Flag(Game1 game, float x, float y) : base
           ("flag", new Vector2(x, y),
           Enumerable.Range(0, 4).Select(n => game.Content.Load<Texture2D>($"flag{n}")
           ).ToArray())
        {
            _game = game;
            AddRectangleBody(
           _game.Services.GetService<World>(),
               width: _size.X / 2f,
               height: _size.Y / 2f,
               true
         ); // kinematic is false by default - kinematic : atributo q define so o body vai ser afetado pela fisica ou nao
            Body.IsSensor = true;
            
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
        }
    }
}

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
    public class Bandeira : Sprite
    {
        private Game1 _game;
        public Bandeira(Game1 game, float x = 0, float y = 0) :
           base("Bandeira",game.Content.Load<Texture2D>("assets/orig/images/banner"),new Vector2(x,y),false)
               
        {
            _game = game;
            AddRectangleBody(
            _game.Services.GetService<World>(),
                width: _size.X / 1.8f,
                height: _size.Y / 0.8f,
                true
          ); // kinematic is false by default - kinematic : atributo q define so o body vai ser afetado pela fisica ou nao
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

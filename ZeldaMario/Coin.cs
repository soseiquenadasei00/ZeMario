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
    public class Coin : AnimatedSprite
    {
        private Game1 _game;

        private List<Texture2D> _idleFrames;
        private List<Texture2D> _walkFrames;
        private Vector2 _startingPoint;

        

        private HashSet<Fixture> _collisions;

        public Coin(Game1 game, float x, float y) :
           base("Coin",
                new Vector2(x, y),
                Enumerable.Range(1, 4).Select<int, Texture2D>(
                    n => game.Content.Load<Texture2D>($"tileset{n}")).ToArray()
                )

        {

            _collisions = new HashSet<Fixture>();
            _idleFrames = _textures; // loaded by the base construtor

            _game = game;

            AddCircleBody(  // Collider
                _game.Services.GetService<World>(),
                raids: _size.X / 4f, true



            ); // kinematic is false by default

            Body.IsSensor = true;
            
         }
        public override void Update(GameTime gameTime)
        {


            base.Update(gameTime);
        }

    }
}


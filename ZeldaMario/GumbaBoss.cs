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
    class GumbaBoss : AnimatedSprite
    {

        public GumbaBoss(Game1 game, float x = 0, float y = 0) :
           base("GumbaBoss",
               new Vector2(x, y),
               Enumerable.Range(0, 1).Select<int, Texture2D>(
                   n => game.Content.Load<Texture2D>("idle")).ToArray()
               )
        {

        }
    }
}

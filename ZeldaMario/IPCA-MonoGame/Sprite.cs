using System;
using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace IPCA.MonoGame
{
    // Game Object child
    // represents a game object that has a texture
    public class Sprite : GameObject
    {
        
        public enum Direction
        {
            Left,
            Right,
        }
        protected Direction _direction = Direction.Right;

        // TODO: we should not duplicate textures on each instance
        protected Texture2D _texture;
        public Sprite(string name, Texture2D texture, Vector2 position,
            bool offset = false) : base(name, position)
        {
            _texture = texture;
            _size = _texture.Bounds.Size.ToVector2() / 128f;  // TODO: HARDCODED!
            if (offset)
                _position = position + new Vector2(_size.X , _size.Y) / 2f; // Anchor in the middle
        }

       
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Vector2 pos = Camera.Position2Pixels(_position);
            Vector2 anchor = new Vector2(_texture.Width / 2f, _texture.Height/2f);
           
            Vector2 scale = Camera.Length2Pixels(_size) /16f; // TODO: HARDCODED!
            Vector2 scalePlayer = Camera.Length2Pixels(_size) / 40f; 
            
            scale.Y = scale.X;  // FIXME! TODO: HACK HACK HACK
            
            if(Name == "player") {
                Vector2 anchor2 = new Vector2(_texture.Width / 2f, _texture.Height / 2f);
                spriteBatch.Draw(_texture, pos, null, Color.White,
                _rotation, anchor2, (scalePlayer), 
                _direction == Direction.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                0);
            }

            else if (Name == "prantinha")
            {
                Vector2 anchor2 = new Vector2(_texture.Width / 2f, _texture.Height / 1.4f);
                spriteBatch.Draw(_texture, pos, null, Color.White,
                        _rotation, anchor2, scale/1.7f,
                        _direction == Direction.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                        0);

                 }
           
            else if (Name == "gumba")
            {
                Vector2 anchor2 = new Vector2(_texture.Width / 2f, _texture.Height/1.5f);

                spriteBatch.Draw(_texture, pos, null, corDoDesenho,
                    _rotation, anchor2, scale / 2f,
                    _direction == Direction.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                    0);

            }
            else if(Name == "Coin") 
            {

                spriteBatch.Draw(_texture, pos, null, Color.White,
                   _rotation, anchor, scale / 2f,
                   _direction == Direction.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                   0);
            }
            else if(Name == "assets/orig/images/banner") 
            {
                Vector2 anchor2 = new Vector2(_texture.Width / 2f, _texture.Height / 1.5f);
                spriteBatch.Draw(_texture, pos, null, Color.White,
                  _rotation, anchor, scale / 4f,
                  _direction == Direction.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                  0);
            }
            else if (Name == "assets/orig/images/Start")
            {
                Vector2 anchor2 = new Vector2(_texture.Width / 2f, _texture.Height / 1.5f);
                spriteBatch.Draw(_texture, pos, null, Color.White,
                  _rotation, anchor, scale / 4f,
                  _direction == Direction.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                  0);

            }

            else
            {
                spriteBatch.Draw(_texture, pos, null, Color.White,
                                _rotation, anchor, scale ,
                                _direction == Direction.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                                0);
            }

            base.Draw(spriteBatch, gameTime);
        }
    }
}
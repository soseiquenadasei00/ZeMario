using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Genbox.VelcroPhysics.Dynamics;
using Newtonsoft.Json.Linq;
using ZeldaMario;
using System;

namespace IPCA.MonoGame
{
    public class Scene
    {
        private List<Sprite> _sprites;

        public Scene(Game1 game, string name)
        {
            string filename = $"Content/scenes/{name}.dt";
            _sprites = new List<Sprite>();
            using (StreamReader reader = File.OpenText(filename))
            {
                JObject sceneJson = (JObject) JToken.ReadFrom(new JsonTextReader(reader));
                JArray spriteJson = (JArray) sceneJson["composite"]["sImages"];
                foreach (JObject image in spriteJson)
                {
                    float x = (float) (image["x"] ?? 0);
                    float y = (float) (image["y"] ?? 0);
                    string imageName = (string) image["imageName"];
                    string imageFilename = $"assets/orig/images/{imageName}";

                    // Load texture here, and send it to the sprite object
                    Texture2D texture = game.Content.Load<Texture2D>(imageFilename);
                    Sprite sprite = new Sprite(imageFilename, texture, new Vector2(x/128, y/128), true);
                    Console.WriteLine(imageFilename);

                    if (imageName == "prantinha1")
                    {
                        game._prantinha = new Prantinha(game, x, y);
                        
                    }
                    else if (imageName == "Wulfric1")
                    {
                        game._player = new Player(game, x/16, y+2f);
                    }
                    else 
                    {
                        _sprites.Add(sprite);
                        sprite.AddRectangleBody(game.Services.GetService<World>(),
                            isKinematic: true);

                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (Sprite sprite in _sprites)
            {
                sprite.Draw(spriteBatch, gameTime);
            }
        }
    }
}
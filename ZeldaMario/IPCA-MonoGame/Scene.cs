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
using System.Linq;

namespace IPCA.MonoGame
{
    public class Scene
    {
        public List<Sprite> _sprites;
        public string filename;

        public Scene(Game1 game, string name)
        {
            filename = $"Content/scenes/{name}.dt";
            _sprites = new List<Sprite>();
            using (StreamReader reader = File.OpenText(filename))
            {
                JObject sceneJson = (JObject)JToken.ReadFrom(new JsonTextReader(reader));
                JArray spriteJson = (JArray)sceneJson["composite"]["sImages"];
                foreach (JObject image in spriteJson)
                {
                    float x = (float)(image["x"] ?? 0);
                    float y = (float)(image["y"] ?? 0);
                    string imageName = (string)image["imageName"];
                    string imageFilename = $"assets/orig/images/{imageName}";
                    string LayerName = (string)image["layerName"];


                    // Load texture here, and send it to the sprite object
                    Texture2D texture = game.Content.Load<Texture2D>(imageFilename);
                    Sprite sprite = new Sprite(imageFilename, texture, new Vector2(x / 8, y / 8), true);
                    //Console.WriteLine(imageFilename);
                    if (LayerName == "Mapa1")
                    {
                        _sprites.Add(sprite);
                    }

                    else if (LayerName == "morte")
                    {

                        _sprites.Add(sprite);
                        sprite.AddRectangleBody(game.Services.GetService<World>(),

                            height: (sprite.Size.Y / 2f), isKinematic: true);
                    }
                    else if (imageName == "planta-idle-export1")
                    {
                        game._prantinha.Add(new Prantinha(game, (x / 8) + 0.2f, (y / 8) + 0.07f));

                    }
                    else if (imageName == "Wulfric0")
                    {
                        game._player = new Player(game, (x / 8) + 0.2f, (y / 2f) - 0.25f); // y+2 para ajustar a altura
                    }
                    else if (imageName == "gumba-idle")
                    {
                        if (y == 0) game._gumba.Add(new Gumba(game, (x / 8) + 0.2f, (y / 8) + 0.12f));
                        else game._gumba.Add(new Gumba(game, (x / 8) + 0.2f, (y / 8) + 0.12f));

                    }
                    else if (imageName == "tileset1")
                    {
                        game._coin.Add(new Coin(game, (x / 8) + 0.12f, (y / 8) + 0.12f));

                    }
                    else if (imageName == "idle")
                    {

                        game._boss = new GumbaBoss(game, x / 8f, (y / 8f) + 1.2f);
                    }
                    else if (imageName == "flag0")
                    {
                        game._flag = new Flag(game,(x / 8f) + 0.3f, (y / 8f) + 0.22f);
                    }
                    else 
                    {
                        
                        _sprites.Add(sprite);
                        sprite.AddRectangleBody(game.Services.GetService<World>(),
                            isKinematic: true);
                        sprite.Body.Friction = 110f;

                    }
                }
            }
            game.changeScene = false;
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
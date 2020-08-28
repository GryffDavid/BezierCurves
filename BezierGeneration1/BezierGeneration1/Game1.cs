using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace BezierGeneration1
{
    
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;       
        Matrix Projection = Matrix.CreateOrthographicOffCenter(0, 1280, 720, 0, -10, 10);
        BasicEffect BasicEffect;
        Texture2D Texture;
        List<Bezier> BezList = new List<Bezier>();

        int index = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        
        protected override void Initialize()
        {
            //Bez = new Bezier();
            BezList.Add(new Bezier(new Vector2(600, 585), new Vector2(560, 400)));
            base.Initialize();
        }

        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            BasicEffect = new BasicEffect(GraphicsDevice);

            BasicEffect.Projection = Projection;
            BasicEffect.VertexColorEnabled = true;

            Texture = Content.Load<Texture2D>("Bg");

            foreach (Bezier bez in BezList)
            {
                bez.LoadContent(Content);
            }
        }

        
        protected override void UnloadContent()
        {

        }

        
        protected override void Update(GameTime gameTime)
        {
            BezList[index].Update(gameTime);
            //foreach (Bezier bez in BezList)
            //{
            //    bez.Update(gameTime);
            //}

            base.Update(gameTime);
        }

        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.Draw(Texture, new Vector2(1280/2 - Texture.Width/2, 100), Color.White);
            spriteBatch.End();

            foreach (Bezier bez in BezList)
            {
                bez.Draw(GraphicsDevice, BasicEffect);
            }

            base.Draw(gameTime);
        }
    }
}

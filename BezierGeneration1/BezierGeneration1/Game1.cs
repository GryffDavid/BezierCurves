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

        KeyboardState CurrentKeyboardState, PreviousKeyboardState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            Content.RootDirectory = "Content";
        }

        
        protected override void Initialize()
        {
            BezList.Add(new Bezier(new Vector2(600, 585), new Vector2(560, 400), new Vector2(661, 550), new Vector2(495, 558)));
            BezList.Add(new Bezier(new Vector2(560, 400), new Vector2(530, 380), new Vector2(581, 443), new Vector2(604, 336)));
            BezList.Add(new Bezier(new Vector2(530, 380), new Vector2(600, 300), new Vector2(512, 379), new Vector2(522, 273)));
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
            CurrentKeyboardState = Keyboard.GetState();

            if (CurrentKeyboardState.IsKeyUp(Keys.T) &&
                PreviousKeyboardState.IsKeyDown(Keys.T))
            {
                index++;
            }

            if (CurrentKeyboardState.IsKeyUp(Keys.G) &&
                PreviousKeyboardState.IsKeyDown(Keys.G))
            {
                index--;
            }

            if (CurrentKeyboardState.IsKeyDown(Keys.Space))
            {
                int p = 0;
            }

            BezList[index].Update(gameTime);


            PreviousKeyboardState = CurrentKeyboardState;
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

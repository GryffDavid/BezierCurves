using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BezierGeneration1
{
    public class Bezier
    {
        Texture2D Block;
        List<Vector2> BezierPoints = new List<Vector2>();
        List<Vector2> ActualPoints = new List<Vector2>();
        List<Vector2> CurvePoints = new List<Vector2>();
        SpriteFont Font;
        MouseState CurrentMouseState;
        Vector2 ControlPoint1, ControlPoint2;

        float offset = 150;

        public Bezier()
        {
            ActualPoints.Add(new Vector2(450, 450)); //Start Point
            ActualPoints.Add(new Vector2(350, 250)); //Control Point
            ActualPoints.Add(new Vector2(350, 350)); //Control Point 2
            ActualPoints.Add(new Vector2(450, 200)); //End Point

            ControlPoint1 = ActualPoints[1];
            ControlPoint2 = ActualPoints[2];

            UpdateCurve();
        }

        public void Update(GameTime gameTime)
        {
            CurrentMouseState = Mouse.GetState();

            if (CurrentMouseState.LeftButton == ButtonState.Pressed)
            {
                ControlPoint1 = new Vector2(CurrentMouseState.X, CurrentMouseState.Y) + new Vector2(0, offset);
                UpdateCurve();
            }

            if (CurrentMouseState.RightButton == ButtonState.Pressed)
            {
                ControlPoint2 = new Vector2(CurrentMouseState.X, CurrentMouseState.Y) + new Vector2(0, -offset);
                UpdateCurve();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                int p = 0;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                offset++;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                offset--;
            }
        }

        public void LoadContent(ContentManager contentManager)
        {
            Block = contentManager.Load<Texture2D>("Block");
            Font = contentManager.Load<SpriteFont>("Font");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < CurvePoints.Count; i++)
            {
                spriteBatch.DrawString(Font, i.ToString(), CurvePoints[i], Color.Red);
            }

            for (int p = 0; p < CurvePoints.Count - 2; p++)
            {
                Vector2 dir1 = CurvePoints[p] - CurvePoints[p + 2];
                for (float i = -1; i < 0; i += 0.01f)
                {
                    spriteBatch.Draw(Block, new Rectangle((int)CurvePoints[p].X + (int)(dir1.X * i), (int)CurvePoints[p].Y + (int)(dir1.Y * i), 1, 1), Color.White);
                }
            }

            foreach (Vector2 vec in CurvePoints)
            {
                spriteBatch.Draw(Block, new Rectangle((int)vec.X, (int)vec.Y, 4,4), Color.White);
            }

            foreach (Vector2 vec in ActualPoints)
            {
                spriteBatch.Draw(Block, new Rectangle((int)vec.X, (int)vec.Y, 4, 4), Color.HotPink);
            }


            spriteBatch.Draw(Block, new Rectangle((int)ControlPoint1.X, (int)ControlPoint1.Y, 4, 4), Color.Yellow);
            spriteBatch.Draw(Block, new Rectangle((int)ControlPoint2.X, (int)ControlPoint2.Y, 4, 4), Color.Purple);
        }

        public void UpdateCurve()
        {
            BezierPoints.Clear();
            CurvePoints.Clear();   

            BezierPoints.AddRange(ActualPoints);
            
            for (float t = 0f; t < 1.0; t += 0.025f)
            {
                float nx = (float)Math.Pow(1 - t, 3) * ActualPoints[0].X + (3 * t) * ((float)Math.Pow(1 - t, 2)) * ControlPoint1.X + 
                           3 * (float)Math.Pow(t, 2) * (1 - t) * ControlPoint2.X + (float)Math.Pow(t, 3) * ActualPoints[3].X;

                float nY = (float)Math.Pow(1 - t, 3) * ActualPoints[0].Y + (3 * t) * ((float)Math.Pow(1 - t, 2)) * ControlPoint1.Y +
                           3 * (float)Math.Pow(t, 2) * (1 - t) * ControlPoint2.Y + (float)Math.Pow(t, 3) * ActualPoints[3].Y;
                
                Vector2 newPoint1 = new Vector2(nx, nY); 

                CurvePoints.Add(newPoint1);
            }
        }
    }
}

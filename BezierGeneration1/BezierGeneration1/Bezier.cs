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
                ControlPoint1 = new Vector2(CurrentMouseState.X, CurrentMouseState.Y);
                UpdateCurve();
            }

            if (CurrentMouseState.RightButton == ButtonState.Pressed)
            {
                ControlPoint2 = new Vector2(CurrentMouseState.X, CurrentMouseState.Y);
                UpdateCurve();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                int p = 0;
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
            
            for (float t = 0f; t < 1.05; t += 0.05f)
            {
                //B(t) = 
                //(1 - t)^2*P0 +  

                //2(1-t)tP1 + 

                //t^2 * P2

                #region Quadratic
                //float term1x = (float)Math.Pow(1 - t, 2) * ActualPoints[0].X;
                //float term2x = (2 * t * (1 - t)) * (ControlPoint1.X);
                //float term3x = (float)Math.Pow(t, 2) * ActualPoints[2].X;

                //float x = term1x + term2x + term3x;
                //float y = (float)Math.Pow(1 - t, 2) * ActualPoints[0].Y + (2 * t * (1 - t)) * (ControlPoint1.Y) + (float)Math.Pow(t, 2) * ActualPoints[2].Y;
                //Vector2 newPoint1 = new Vector2(x, y);
                #endregion


                #region Cubic
                //float term = (3 * t * (1 - t));

                float term = (3 * t) * ((float)Math.Pow(1 - t, 2));
                float term2 = 3 * (float)Math.Pow(t, 2) * (1 - t);

                float nx1 = (float)Math.Pow(1 - t, 3) * ActualPoints[0].X;
                float nx2 = term * ControlPoint1.X;
                float nx3 = term2 * ControlPoint2.X;
                float nx4 = (float)Math.Pow(t, 3) * ActualPoints[3].X;

                float nx = nx1 + nx2 + nx3 + nx4;


                
                float nY1 = (float)Math.Pow(1 - t, 3) * ActualPoints[0].Y;
                float nY2 = term * ControlPoint1.Y;
                float nY3 = term2 * ControlPoint2.Y;
                float nY4 = (float)Math.Pow(t, 3) * ActualPoints[3].Y;

                float nY = nY1 + nY2 + nY3 + nY4;

                Vector2 newPoint1 = new Vector2(nx, nY); 
                #endregion


                CurvePoints.Add(newPoint1);
            }
        }
    }
}

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

        public Bezier()
        {
            ActualPoints.Add(new Vector2(450, 450));
            ActualPoints.Add(new Vector2(350, 300));
            ActualPoints.Add(new Vector2(450, 200));

            UpdateCurve();
        }

        public void Update(GameTime gameTime)
        {
            CurrentMouseState = Mouse.GetState();

            if (CurrentMouseState.LeftButton == ButtonState.Pressed)
            {
                ActualPoints[1] = new Vector2(CurrentMouseState.X, CurrentMouseState.Y);
                UpdateCurve();
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
        }

        public void UpdateCurve()
        {
            BezierPoints.Clear();
            CurvePoints.Clear();   

            BezierPoints.AddRange(ActualPoints);
            
            for (float t = 0f; t < 1.05; t += 0.05f)
            {
                Vector2 newPoint1 = new Vector2(
                    (float)Math.Pow(1 - t, 2) * ActualPoints[0].X + (2 * t * (1 - t)) * (ActualPoints[1].X) + (float)Math.Pow(t, 2) * ActualPoints[2].X,
                    (float)Math.Pow(1 - t, 2) * ActualPoints[0].Y + (2 * t * (1 - t)) * (ActualPoints[1].Y) + (float)Math.Pow(t, 2) * ActualPoints[2].Y);
                CurvePoints.Add(newPoint1);
            }
        }
    }
}

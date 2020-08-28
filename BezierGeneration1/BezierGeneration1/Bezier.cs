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

        VertexPositionColor[] vertices = new VertexPositionColor[180];

        float offset = 100;
        float increment = 0.05f;

        Vector2 pattern, pattern2;

        public Bezier(Vector2 startPoint, Vector2 endPoint, Vector2? cp1 = null, Vector2? cp2 = null)
        {
            ActualPoints.Add(startPoint); //Start Point
            ActualPoints.Add(endPoint); //End Point

            if (cp1 != null)
                ControlPoint1 = cp1.Value;

            if (cp2 != null)
                ControlPoint2 = cp2.Value;

            pattern = new Vector2(startPoint.X - ControlPoint1.X, startPoint.Y - ControlPoint1.Y);
            pattern2 = new Vector2(startPoint.X - ControlPoint2.X, startPoint.Y - ControlPoint2.Y);

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

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                offset++;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                offset--;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                increment += 0.05f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                increment -= 0.05f;
            }
        }

        public void LoadContent(ContentManager contentManager)
        {
            Block = contentManager.Load<Texture2D>("Block");
            Font = contentManager.Load<SpriteFont>("Font");
        }

        public void Draw(GraphicsDevice graphicsDevice, BasicEffect basicEffect)
        {
            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, vertices, 0, CurvePoints.Count -2);
            }
        }

        public void UpdateCurve()
        {
            BezierPoints.Clear();
            CurvePoints.Clear();   

            BezierPoints.AddRange(ActualPoints);

            CurvePoints.Add(ActualPoints[0]);
                        
            for (float t = 0f; t < 1.05; t += 0.05f)
            {
                float nx = (float)Math.Pow(1 - t, 3) * ActualPoints[0].X + (3 * t) * ((float)Math.Pow(1 - t, 2)) * ControlPoint1.X + 
                           3 * (float)Math.Pow(t, 2) * (1 - t) * ControlPoint2.X + (float)Math.Pow(t, 3) * ActualPoints[1].X;

                float nY = (float)Math.Pow(1 - t, 3) * ActualPoints[0].Y + (3 * t) * ((float)Math.Pow(1 - t, 2)) * ControlPoint1.Y +
                           3 * (float)Math.Pow(t, 2) * (1 - t) * ControlPoint2.Y + (float)Math.Pow(t, 3) * ActualPoints[1].Y;
                
                Vector2 newPoint1 = new Vector2(nx, nY); 

                CurvePoints.Add(newPoint1);               
            }

            CurvePoints.Add(ActualPoints[1]);

            for (int i = 0; i < CurvePoints.Count - 1; i++)
            {
                vertices[i] = new VertexPositionColor(new Vector3(CurvePoints[i].X, CurvePoints[i].Y, 0), Color.White);
            }
        }
    }
}

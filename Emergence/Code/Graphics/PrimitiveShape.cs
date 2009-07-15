using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Emergence
{
    public enum DrawType
    {
        LineStrip,
        LineLoop
    }

	public class PrimitiveShape
	{
		public Color ShapeColor = Color.White;

		Vector2[] vertices;
		Vector2[] transformedVertices;
		BoundingRectangle bounds;
		Vector2 position = Vector2.Zero;
		float rotation = 0f;
        DrawType type;

		public Vector2 Position
		{
			get { return position; }
			set
			{
				if (!position.Equals(value))
				{
					position = value;
					CalculatePointsAndBounds();
				}
			}
		}

		public float Rotation
		{
			get { return rotation; }
			set
			{
				if (rotation != value)
				{
					rotation = value;
					CalculatePointsAndBounds();
				}
			}
		}

		public BoundingRectangle Bounds
		{
			get { return bounds; }
		}

        public PrimitiveShape(Vector2[] vertices, DrawType type)
		{
			this.vertices = (Vector2[])vertices.Clone();
			this.transformedVertices = new Vector2[vertices.Length];
            this.type = type;
			CalculatePointsAndBounds();
		}

		public void Draw(PrimitiveBatch batch)
		{
			batch.Begin(PrimitiveType.LineList);

            switch (type)
            {
                case DrawType.LineStrip:
                    for (int i = 0; i < transformedVertices.Length -1; i++)
                    {
                        batch.AddVertex(transformedVertices[i], ShapeColor);
                        batch.AddVertex(transformedVertices[(i + 1)], ShapeColor);
                    }
                    break;
                case DrawType.LineLoop:
                    for (int i = 0; i < transformedVertices.Length; i++)
                    {
                        batch.AddVertex(transformedVertices[i], ShapeColor);
                        batch.AddVertex(transformedVertices[(i + 1) % transformedVertices.Length], ShapeColor);
                    }
                    break;
                default:
                    break;
            }
			
			batch.End();
		}

		private void CalculatePointsAndBounds()
		{
			for (int i = 0; i < vertices.Length; i++)
				transformedVertices[i] = Vector2.Transform(vertices[i], Matrix.CreateRotationZ(rotation)) + position;

			bounds = new BoundingRectangle(transformedVertices);
		}

		/* using the algorithm written by Darel Rex Finley at
		 * http://alienryderflex.com/polygon/
		 * 
		 * we take a given point and draw a horizontal line, counting the
		 * intersections with sides of the polygon. if the number of intersections
		 * is odd, the point is inside the polygon.
		 */
		public bool ContainsPoint(Vector2 point)
		{
			if (!bounds.Contains(point))
				return false;

			bool oddNodes = false;

			int j = vertices.Length - 1;
			float x = point.X;
			float y = point.Y;

			for (int i = 0; i < transformedVertices.Length; i++)
			{
				Vector2 tpi = transformedVertices[i];
				Vector2 tpj = transformedVertices[j];

				if (tpi.Y < y && tpj.Y >= y || tpj.Y < y && tpi.Y >= y)
					if (tpi.X + (y - tpi.Y) / (tpj.Y - tpi.Y) * (tpj.X - tpi.X) < x)
						oddNodes = !oddNodes;

				j = i;
			}

			return oddNodes;
		}

		public static bool TestCollision(PrimitiveShape shape1, PrimitiveShape shape2)
		{
			if (shape1.Bounds.Intersects(shape2.Bounds))
			{
				//simple check if the first polygon contains any points from the second
				for (int i = 0; i < shape2.transformedVertices.Length; i++)
					if (shape1.ContainsPoint(shape2.transformedVertices[i]))
						return true;

				//switch around and test the other way
				for (int i = 0; i < shape1.transformedVertices.Length; i++)
					if (shape2.ContainsPoint(shape1.transformedVertices[i]))
						return true;

				//now we have to check for line segment intersections
				for (int i = 0; i < shape1.transformedVertices.Length; i++)
				{
					//get the two points from a segment on shape 1
					Vector2 a = shape1.transformedVertices[i];
					Vector2 b = shape1.transformedVertices[(i + 1) % shape1.transformedVertices.Length];

					for (int j = 0; j < shape2.transformedVertices.Length; j++)
					{
						//get two points from a segment on shape 2
						Vector2 c = shape2.transformedVertices[j];
						Vector2 d = shape2.transformedVertices[(j + 1) % shape2.transformedVertices.Length];

						//figure out of we have an intersection
						if (segmentsIntersect(a, b, c, d))
							return true;
					}
				}
			}

			return false;
		}

		//thanks to Joseph Duchesne for this method
		//http://www.idevgames.com/forum/showthread.php?t=7458
		private static bool segmentsIntersect(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
		{
			float[,] tarray = new float[4, 2];  //<===== Find the inner bounding of rect ABCD

			float ax = a.X;
			float ay = a.Y;
			float bx = b.X;
			float by = b.Y;
			float cx = c.X;
			float cy = c.Y;
			float dx = d.X;
			float dy = d.Y;

			if (ax < bx)
			{
				tarray[0, 0] = ax;
				tarray[1, 0] = bx;
			}
			else
			{
				tarray[0, 0] = bx;
				tarray[1, 0] = ax;
			}
			if (ay < by)
			{
				tarray[0, 1] = ay;
				tarray[1, 1] = by;
			}
			else
			{
				tarray[0, 1] = by;
				tarray[1, 1] = ay;
			}
			if (cx < dx)
			{
				tarray[2, 0] = cx;
				tarray[3, 0] = dx;
			}
			else
			{
				tarray[2, 0] = dx;
				tarray[3, 0] = cx;
			}
			if (cy < dy)
			{
				tarray[2, 1] = cy;
				tarray[3, 1] = dy;
			}
			else
			{
				tarray[2, 1] = dy;
				tarray[3, 1] = cy;
			}

			float[,] tarray2 = new float[2, 2];

			if (tarray[0, 0] < tarray[2, 0])
				tarray2[0, 0] = tarray[2, 0];
			else
				tarray2[0, 0] = tarray[0, 0];
			if (tarray[0, 1] < tarray[2, 1])
				tarray2[0, 1] = tarray[2, 1];
			else
				tarray2[0, 1] = tarray[0, 1];
			if (tarray[1, 0] < tarray[3, 0])
				tarray2[1, 0] = tarray[1, 0];
			else
				tarray2[1, 0] = tarray[3, 0];
			if (tarray[1, 1] < tarray[3, 1])
				tarray2[1, 1] = tarray[1, 1];
			else
				tarray2[1, 1] = tarray[3, 1];

			float mab = (ay - by) / (ax - bx); //<===== Find Slopes of Lines
			float mcd = (cy - dy) / (cx - dx);

			if (mab == mcd)
				return false;  //the lines are parallel

			float yiab = ((ax - bx) * ay - ax * (ay - by)) / (ax - bx);
			float yicd = ((cx - dx) * cy - cx * (cy - dy)) / (cx - dx);
			float x = (yicd - yiab) / (mab - mcd);
			float y = mab * x + yiab;

			return (
				x > tarray2[0, 0] &&
				x < tarray2[1, 0] &&
				y > tarray2[0, 1] &&
				y < tarray2[1, 1]);
		}
	}
}

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Emergence
{
	public struct BoundingRectangle
	{
		public float l, b, r, t;

		public BoundingRectangle(float l, float b, float r, float t)
		{
			this.l = l;
			this.b = b;
			this.r = r;
			this.t = t;
		}

		public BoundingRectangle(params Vector2[] points)
		{
			if (points.Length < 2)
				throw new Exception("BoundingRectangle can only be made for two or more points");

			l = float.PositiveInfinity;
			r = float.NegativeInfinity;
			b = float.PositiveInfinity;
			t = float.NegativeInfinity;

			for (int i = 1; i < points.Length; i++)
			{
				Vector2 v = points[i];

				l = (float)Math.Min(l, v.X);
				r = (float)Math.Max(r, v.X);
				b = (float)Math.Min(b, v.Y);
				t = (float)Math.Max(t, v.Y);
			}
		}

		public bool Intersects(BoundingRectangle other)
		{
			return (l <= other.r && other.l <= r && b <= other.t && other.b <= t);
		}

		public bool Contains(Vector2 v)
		{
			return (l < v.X && r > v.X && b < v.Y && t > v.Y);
		}
	}
}

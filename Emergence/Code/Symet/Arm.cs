using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Emergence
{
    class Arm
    {
        // Attributes

        int id;
        Dictionary<int, Segment> segments;
        Vector2 position;
        double rotation;

        // Properties

        public int ID
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }
        public Vector2 Position
        {
            get
            {
                return this.position;
            }
            set
            {
                this.position = value;

                foreach (Segment segment in segments.Values)
                    segment.Position = value;
            }
        }
        public double Rotation
        {
            get
            {
                return this.rotation;
            }
            set
            {
                this.rotation = value;

                foreach (Segment segment in segments.Values)
                    segment.Rotation = value;
            }
        }

        // Functions

        public Arm(Dictionary<int, Segment> segments)
        {
            this.segments = new Dictionary<int, Segment>(segments);
            this.position = new Vector2(0, 0);
            this.id = 0;
        }

        public int Draw(PrimitiveBatch primitiveBatch)
        {
            foreach (Segment segment in segments.Values)
            {
                if(segment.Active && segments[segment.ParentID].Active)
                    segment.Draw(primitiveBatch);
            }
            

            //for (int i = segments.Count; i > 0; i--)
            //{
            //    if (segment.Active && segments[segment.ParentID].Active)
            //        segment.Draw(primitiveBatch);
            //}

            return 1;
        }

    }
}

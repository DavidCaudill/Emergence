using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Emergence
{
    class Symet
    {
        // Attributes

        Dictionary<int, Arm> arms;
        Shape bodyShape;
        float bodySize;
        List<Vector2> vertices;
        PrimitiveShape primitive;
        Vector2 position;
        double rotation;
        bool active;

        // Properties

        public Shape BodyShape
        {
            get
            {
                return this.bodyShape;
            }
            set
            {
                this.bodyShape = value;
            }
        }
        public float BodySize
        {
            get
            {
                return this.bodySize;
            }
            set
            {
                this.bodySize = value;
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
                primitive.Position = value;

                foreach (Arm arm in arms.Values)
                    arm.Position = value;
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
                primitive.Rotation = Convert.ToSingle(value);

                foreach (Arm arm in arms.Values)
                    arm.Rotation = value;
            }
        }
        public bool Active
        {
            get
            {
                return this.active;
            }
            set
            {
                this.active = value;
            }
        }

        // Functions

        public Symet()
        {
        }

        public Symet(List<Vector2> vertices, Dictionary<int, Arm> arms)
        {
            this.vertices = vertices;
            this.active = false;
            this.arms = arms;

            // Build the primative shape
            primitive = new PrimitiveShape(vertices.ToArray(), DrawType.LineLoop);
            primitive.ShapeColor = Color.Green; 
        }

        public int Draw(PrimitiveBatch primitiveBatch)
        {
            primitive.Draw(primitiveBatch);
            foreach (Arm arm in arms.Values)
            {
                arm.Draw(primitiveBatch);
            }
            return 1;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Emergence
{
    // Segment type
    public enum SegmentType
    {
        None,
        Attack,
        Defend,
        Photo
    }

    // A polar coordinate
    public struct VectorP
    {
        // Attributes
        public double angle;
        public float magnitude;

        // Functions
        public VectorP(double angle, float magnitude)
        {
            this.angle = angle;
            this.magnitude = magnitude;
        }
        public VectorP Add(VectorP polarVec)
        {
            VectorP temp = new VectorP(angle, magnitude);

            temp.angle += polarVec.angle;
            temp.magnitude += polarVec.magnitude;

            return temp;
        }
        public VectorP Subtract(VectorP polarVec)
        {
            VectorP temp = new VectorP(angle, magnitude);

            temp.angle -= polarVec.angle;
            temp.magnitude -= polarVec.magnitude;

            return temp;
        }
        public Vector2 GetCartesian(double offsetAngle)
        {
            Vector2 temp = new Vector2();

            temp.X = magnitude * Convert.ToSingle(Math.Cos(-angle + offsetAngle));
            temp.Y = magnitude * Convert.ToSingle(Math.Sin(-angle + offsetAngle));
            return temp;
        }
    }

    class Chromosome
    {
        // Attributes

        List<VectorP> instructions;
        bool active;
        int id;
        int parentID;
        int parentFace;
        Dictionary<int, int> faces;
        SegmentType type;

        # region Properties

        public List<VectorP> Instructions
        {
            get
            {
                return this.instructions;
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
        public int ParentID
        {
            get
            {
                return this.parentID;
            }
            set
            {
                this.parentID = value;
            }
        }
        public int ParentFace
        {
            get
            {
                return this.parentFace;
            }
            set
            {
                this.parentFace = value;
            }
        }
        public SegmentType Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }

        #endregion

        // Functions

        public Chromosome(List<VectorP> instructions, int id, int parentID, int parentFace, SegmentType type)
        {
            this.instructions = new List<VectorP>(instructions);
            this.id = id;
            this.parentID = parentID;
            this.parentFace = parentFace;
            this.faces = new Dictionary<int, int>();
            this.type = type;

            // Set up the faces
            faces.Add(0, parentID);
            for (int i = 1; i < this.instructions.Count + 1; i++)
            {
                faces.Add(i, -1);
            }
            
            this.active = true;
        }

        public bool IsFaceFree(int face)
        {
            if (face < faces.Count + 1)
                if (faces[face] == -1)
                    return true;
            
            return false;
        }

        public int FaceCount()
        {
            return faces.Count;
        }
    }
}

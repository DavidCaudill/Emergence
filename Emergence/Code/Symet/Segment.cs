using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Emergence
{
    class Segment
    {
        // Attributes

        List<Vector2> vertices;
        bool alive;
        int id;
        int parentID;
        int parentFace;
        Dictionary<int, int> faces; // <face id, segment id attached>
        Vector2 position;
        double rotation;
        SegmentType type;
        int hitPoints;
        int maxHitPoints;

        # region Properties

        public List<Vector2> Vertices
        {
            get
            {
                return this.vertices;
            }
        }
        public bool Alive
        {
            get
            {
                return this.alive;
            }
            set
            {
                this.alive = value;
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
        public Vector2 Position
        {
            get
            {
                return this.position;
            }
            set
            {
                this.position = value;               
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
            }
        }
        public Dictionary<int, int> Faces
        {
            get
            {
                return this.faces;
            }
            set
            {
                this.faces = value;
            }
        }
        public int HitPoints
        {
            get
            {
                return this.hitPoints;
            }
            set
            {
                this.hitPoints = value;
            }
        }
        public int MaxHitPoints
        {
            get
            {
                return this.maxHitPoints;
            }
            set
            {
                this.maxHitPoints = value;
            }
        }

        #endregion

        // Functions

        public Segment()
        {
        }

        public Segment(List<Vector2> vertices, int id, int parentID, int parentFace, SegmentType type)
        {
            this.vertices = new List<Vector2>(vertices);
            this.id = id;
            this.parentID = parentID;
            this.parentFace = parentFace;
            this.faces = new Dictionary<int, int>();
            this.type = type;

            // Set up the faces
            for (int i = 1; i < this.vertices.Count; i++)
            {
                faces.Add(i, -1);
            }

            hitPoints = 100;
            maxHitPoints = 100;
            this.alive = true;
        }

        public int Update(GameTime gameTime)
        {

            return 1;
        }

        public static Color GetColor(SegmentType type)
        {
            switch (type)
            {
                case SegmentType.None:
                    break;
                case SegmentType.Attack:
                    return Color.Red;
                    break;
                case SegmentType.Defend:
                    return Color.Blue;
                    break;
                case SegmentType.Photo:
                    return Color.Green;
                    break;
                default:
                    break;
            }

            return Color.Black;
        }
    }
}

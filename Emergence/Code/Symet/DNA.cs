using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Emergence
{
    public enum Shape
    {
        None,
        Triangle = 3,
        Square,
        Pentagon,
        Hexagon,
        Heptagon,
        Octogon,
        Nonagon,
        Decagon
    }

    class DNA
    {
        // Attributes

        Dictionary<int, Chromosome> chromosomes;
        Shape bodyShape;
        float bodySize;
        SegmentType bodyType;

        # region Properties

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
        public SegmentType BodyType
        {
            get
            {
                return this.bodyType;
            }
            set
            {
                this.bodyType = value;
            }
        }
        public Dictionary<int, Chromosome> Chromosomes
        {
            get
            {
                return this.chromosomes;
            }
        }

        #endregion

        // Functions
        public DNA()
        {
            this.bodyShape = Shape.None;
            this.bodySize = 0;

            chromosomes = new Dictionary<int, Chromosome>();
        }

        public DNA(Shape bodyShape, float bodySize, SegmentType bodyType)
        {
            this.bodyShape = bodyShape;
            this.bodySize = bodySize;
            this.bodyType = bodyType;

            chromosomes = new Dictionary<int, Chromosome>();
        }

        // Create and attach a new chromosome to an existing one
        public int CreateChromosome(List<VectorP> instructions, int parentID, int faceID, SegmentType type)
        {
            int id;

            if (parentID != 0)
            {
                // See if this parent and its face exists
                if (!chromosomes.ContainsKey(parentID))
                    return -1;
                if (!chromosomes[parentID].IsFaceFree(faceID))
                    return -1;
            }
            else
                chromosomes.Clear();
            
            // Find an available id
            for (id = 1; true; id++)
            {
                if (!chromosomes.ContainsKey(id))
                    break;
            }

            // Attach the chromosome
            chromosomes.Add(id, new Chromosome(instructions, id, parentID, faceID, type));

            return id;
        }

        public Symet BuildDNA()
        {
            Symet symet;
            Dictionary<int, Arm> arms = new Dictionary<int, Arm>();
            Dictionary<int, Segment> segments;
            List<Vector2> bodyVertices = new List<Vector2>();
            List<Vector2> segmentVertices;
            double parentFaceAngle;
            Vector2 vertice;

            // Build the body
            vertice = new Vector2(0, -1 * bodySize);
            bodyVertices.Add(vertice);
            for (int i = 1; i < Convert.ToInt32(bodyShape); i++)
            {
                vertice = new Vector2(0, -1 * bodySize);
                vertice = Vector2.Transform(vertice,
                    Matrix.CreateRotationZ(Convert.ToSingle(((2 * Math.PI) / Convert.ToInt32(bodyShape) * i))));

                bodyVertices.Add(vertice);
            }

            // Build the arms
            for (int i = 0; i < Convert.ToInt32(bodyShape); i++)
            {
                Arm tempArm;

                // Build the segments
                segments = new Dictionary<int, Segment>();
                foreach (Chromosome chromosome in chromosomes.Values)
                {
                    if (!chromosome.Active)
                        continue;
                    // Build the vertices and add segment
                    segmentVertices = new List<Vector2>();

                    // Build the vertices. Ones attached to the main body are a special case
                    if (chromosome.ParentID == 0)
                    {
                        parentFaceAngle = GetAngle(bodyVertices[i],
                            bodyVertices[i < Convert.ToInt32(bodyShape) - 1 ? i + 1 : 0]);

                        segmentVertices.Add(bodyVertices[i]);
                        foreach (VectorP instruction in chromosome.Instructions)
                        {
                            // Get the cartesian coordinates of the instructions using the parent face as a base
                            segmentVertices.Add(instruction.GetCartesian(parentFaceAngle)
                                + bodyVertices[i]);
                        }
                        segmentVertices.Add(bodyVertices[i < Convert.ToInt32(bodyShape) - 1 ? i + 1 : 0]);
                    }
                    else
                    {
                        parentFaceAngle = GetAngle(segments[chromosome.ParentID].Vertices[chromosome.ParentFace - 1],
                            segments[chromosome.ParentID].Vertices[chromosome.ParentFace]);

                        segmentVertices.Add(segments[chromosome.ParentID].Vertices[chromosome.ParentFace - 1]);
                        foreach (VectorP instruction in chromosome.Instructions)
                        {
                            // Get the cartesian coordinates of the instructions using the parent face as a base
                            segmentVertices.Add(instruction.GetCartesian(parentFaceAngle)
                                + segments[chromosome.ParentID].Vertices[chromosome.ParentFace - 1]);
                        }
                        segmentVertices.Add(segments[chromosome.ParentID].Vertices[chromosome.ParentFace]);
                    }

                    if (chromosome.ParentID != 0)
                        segments[chromosome.ParentID].Faces[chromosome.ParentFace] = chromosome.ID;

                    segments.Add(chromosome.ID, new Segment(
                        segmentVertices, 
                        chromosome.ID, 
                        chromosome.ParentID, 
                        chromosome.ParentFace, 
                        chromosome.Type));
                }
                tempArm = new Arm(segments);
                arms.Add(i, tempArm);
            } 
            
            symet = new Symet(bodyVertices, arms, this);

            return symet;
        }

        private double GetAngle(Vector2 verticeOne, Vector2 verticeTwo)
        {
            return Math.Atan2(verticeTwo.Y - verticeOne.Y, verticeTwo.X - verticeOne.X);
        }
    }
}

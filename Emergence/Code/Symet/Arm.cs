using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Emergence
{
    public struct ShapeBuilderVertice
    {
        public Vector2 vertice;
        public Color color;

        public ShapeBuilderVertice(Vector2 vertice, Color color)
        {
            this.vertice = vertice;
            this.color = color;
        }
    }

    class Arm
    {
        // Attributes

        int id;
        Dictionary<int, Segment> segments;
        Vector2 position;
        double rotation;
        bool rebuildSkeleton;

        # region Properties

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
        public bool Alive
        {
            get
            {
                return segments[1].Alive;
            }
        }
        public float Volume
        {
            get
            {
                float volume = 0;
                foreach (Segment segment in segments.Values)
                    volume += segment.Volume;

                return volume;
            }
        }
        public bool RebuildSkeleton
        {
            get
            {
                return this.rebuildSkeleton;
            }
            set
            {
                this.rebuildSkeleton = value;
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

        #endregion

        // Functions

        public Arm(Dictionary<int, Segment> segments)
        {
            this.segments = new Dictionary<int, Segment>(segments);
            this.position = new Vector2(0, 0);
            this.id = 0;
        }

        public int Update(GameTime gameTime)
        {

            foreach (Segment segment in segments.Values)
            {
                segment.Update(gameTime);
            }

            return 1;
        }

        // Regenerate appropreate segments
        // Works with layers of segments depending on how far removed they are from main body
        // Layers closest to main body get priority
        // Segments on the same layer evenly split energy if there is not enough
        public int Regenerate(int energy)
        {
            if (energy == 0)
                return 0;

            int totalEnergy = energy;
            List<int> parentSegments = new List<int>();
            parentSegments.Add(1);
            int totalEnergyNeed = 0;
            Dictionary<int, int> energyInvoice;

            while (parentSegments.Count > 0)
            {
                energyInvoice = new Dictionary<int, int>();

                // Find out how much energy total this layer needs
                foreach (int parentID in parentSegments)
                {
                    // The first 2 conditions in this need to stay in this order
                        if (parentID != 0 && 
                            segments[parentID].ParentID != 0 && 
                            !segments[parentID].Alive && 
                            !segments[segments[parentID].ParentID].Alive)
                            continue;

                    int tempEnergyNeed = segments[parentID].MaxHitPoints - segments[parentID].HitPoints;

                    if (tempEnergyNeed > 10)
                        tempEnergyNeed = 10;

                    energyInvoice.Add(parentID, tempEnergyNeed);
                    totalEnergyNeed += energyInvoice.Last().Value;
                }

                // Allocate appropriate amount of energy
                if(totalEnergyNeed != 0)
                if (totalEnergyNeed < energy)
                {
                    foreach (int parentID in energyInvoice.Keys)
                    {
                        AdjustSegmentHealth(parentID, energyInvoice[parentID]);
                        energy -= energyInvoice[parentID];
                    }
                }
                else
                {
                    // Sort through the list starting with lowest value or energy need
                    foreach (KeyValuePair<int, int> item in energyInvoice.OrderBy(key => key.Value))
                    {
                        if (item.Value < totalEnergyNeed / 3)
                        {
                            AdjustSegmentHealth(item.Key, item.Value);
                            energy -= item.Value;
                        }
                        else
                        {
                            AdjustSegmentHealth(item.Key, totalEnergyNeed / 3);
                            energy -= totalEnergyNeed / 3;
                        }
                    }
                }

                parentSegments = GetChildrenList(parentSegments);
            }

            // Return how much energy was used
            return totalEnergy - energy;
        }

        // Takes in a list of segments and returns a list of all their immediate children
        private List<int> GetChildrenList(List<int> parentSegments)
        {
            List<int> childSegments = new List<int>();

            foreach (int parentID in parentSegments)
                foreach (int childID in segments[parentID].Faces.Values)
                    if (childID != -1)
                        childSegments.Add(childID);

            return childSegments;
        }


        // Recursive function that adds vertices along the outisde of the alive parts of the arm into a list
        public List<ShapeBuilderVertice> RecursiveSkeletonBuilder(int segmentID, Color parentColor)
        {
            List<ShapeBuilderVertice> tempVertices = new List<ShapeBuilderVertice>();

            if (!segments[segmentID].Alive)
            {
                tempVertices.Add(new ShapeBuilderVertice(segments[segmentID].Vertices.First(),
                    parentColor));
                tempVertices.Add(new ShapeBuilderVertice(segments[segmentID].Vertices.Last(), 
                    parentColor));
                return tempVertices;
            }

            foreach (int faceID in segments[segmentID].Faces.Keys)
            {
                if (segments[segmentID].Faces[faceID] == -1)
                    tempVertices.Add(new ShapeBuilderVertice(segments[segmentID].Vertices[faceID - 1], Symet.GetColor(segments[segmentID].Type)));
                else
                    tempVertices.AddRange(RecursiveSkeletonBuilder(segments[segmentID].Faces[faceID], Symet.GetColor(segments[segmentID].Type)));
            }

            return tempVertices;
        }

        // Build a list of vertices to draw lines inbetween each segment
        public List<ShapeBuilderVertice> SegmentDividerBuilder()
        {
            List<ShapeBuilderVertice> tempVertices = new List<ShapeBuilderVertice>();

            foreach (Segment segment in segments.Values)
                if (segment.Alive)
                    foreach (int faceID in segment.Faces.Keys)
                        if (segment.Faces[faceID] != -1 && segments[segment.Faces[faceID]].Alive)
                        {
                            tempVertices.Add(new ShapeBuilderVertice(segment.Vertices[faceID - 1], Symet.GetColor(segment.Type)));
                            tempVertices.Add(new ShapeBuilderVertice(segment.Vertices[faceID], Symet.GetColor(segment.Type)));
                        }

            return tempVertices;
        }

        // Sets the segment to true or false and all its children if false
        public int SetSegmentAlive(int segmentID, bool alive)
        {
            rebuildSkeleton = true;

            segments[segmentID].Alive = alive;

            // If its set to false then we need to apply to all of its children too
            if (!alive)
            {
                segments[segmentID].HitPoints = 0;

                foreach (int faceID in segments[segmentID].Faces.Keys)
                {
                    if (faceID == 0)
                        continue;

                    if (segments[segmentID].Faces[faceID] != -1)
                        SetSegmentAlive(segments[segmentID].Faces[faceID], alive);
                }
            }

            return 1;
        }

        // Adjusts the segments health reletive to healthAdjustment
        // Takes care of activating and deactivating it and its child segments if needed
        // Returns
        public int AdjustSegmentHealth(int segmentID, int healthAdjustment)
        {
            segments[segmentID].HitPoints += healthAdjustment;

            if (segments[segmentID].HitPoints > segments[segmentID].MaxHitPoints)
                segments[segmentID].HitPoints = segments[segmentID].MaxHitPoints;

            if (segments[segmentID].HitPoints < 1)
                SetSegmentAlive(segmentID, false);
            if (segments[segmentID].HitPoints > segments[segmentID].MaxHitPoints / 2 && !segments[segmentID].Alive)
                SetSegmentAlive(segmentID, true);

            return 1;
        }

        public Vector2 GetSegmentCenter(int segmentID)
        {
            Vector2 center = new Vector2();

            foreach (Vector2 vector in segments[segmentID].Vertices)
            {
                center += vector;
            }

            center /= segments[segmentID].Vertices.Count;

            return center;
        }
    }
}

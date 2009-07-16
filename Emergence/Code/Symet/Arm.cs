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

        public int Draw(PrimitiveBatch primitiveBatch)
        {
            foreach (Segment segment in segments.Values)
            {
                if(segment.Active)
                    segment.Draw(primitiveBatch);
            }

            return 1;
        }

        // Regenerate appropreate segments
        // Works with layers of segments depending on how far removed they are from main body
        // Layers closest to main body get priority
        // Segments on the same layer evenly split energy if there is not enough
        public int Regenerate(int energy)
        {
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
                            !segments[parentID].Active && 
                            !segments[segments[parentID].ParentID].Active)
                            continue;

                    int tempEnergyNeed = segments[parentID].MaxHitPoints - segments[parentID].HitPoints;

                    if (tempEnergyNeed > 10)
                        tempEnergyNeed = 10;

                    energyInvoice.Add(parentID, tempEnergyNeed);
                    totalEnergyNeed += energyInvoice.Last().Value;
                }

                // Allocate appropriate amount of energy
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
            {
                foreach (int childID in segments[parentID].Faces.Values)
                {
                    if (childID != -1)
                        childSegments.Add(childID);
                }
            }

            return childSegments;
        }

        // Recursive function that adds vertices along the outisde of the active parts of the arm into a list
        public List<Vector2> RecursiveSkeletonBuilder(int segmentID)
        {
            List<Vector2> tempVertices = new List<Vector2>();

            if (!segments[segmentID].Active)
            {
                tempVertices.Add(segments[segmentID].Vertices.First());
                tempVertices.Add(segments[segmentID].Vertices.Last());
                return tempVertices;
            }

            foreach (int faceID in segments[segmentID].Faces.Keys)
            {
                if (segments[segmentID].Faces[faceID] == -1)
                {
                    tempVertices.Add(segments[segmentID].Vertices[faceID]);
                }
                else
                {
                    tempVertices.AddRange(RecursiveSkeletonBuilder(segments[segmentID].Faces[faceID]));
                }
            }

            return tempVertices;
        }

        // Sets the segment to true or false and all its children if false
        public int SetSegmentActive(int segmentID, bool active)
        {
            segments[segmentID].Active = active;

            // If its set to false then we need to apply to all of its children too
            if (!active)
            {
                segments[segmentID].HitPoints = 0;

                foreach (int faceID in segments[segmentID].Faces.Keys)
                {
                    if (faceID == 0)
                        continue;

                    if (segments[segmentID].Faces[faceID] != -1)
                    {
                        SetSegmentActive(segments[segmentID].Faces[faceID], active);
                    }
                }
            }

            return 1;
        }

        // Adjusts the segments health reletive to healthAdjustment
        // Takes care of activating and deactivating it and its child segments if needed
        public int AdjustSegmentHealth(int segmentID, int healthAdjustment)
        {
            segments[segmentID].HitPoints += healthAdjustment;

            if (segments[segmentID].HitPoints == 0)
                    SetSegmentActive(segmentID, false);
            if (segments[segmentID].HitPoints > 49 && !segments[segmentID].Active)
                segments[segmentID].Active = true;

            return 1;
        }
    }
}

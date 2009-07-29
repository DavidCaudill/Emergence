using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Emergence
{
    public struct SymetUpdate
    {
        public Vector2 position;
        public double rotation;
        public Vector2 velocity;
        public float angularVelocity;
        public int worldID;
        SegmentUpdate segmentUpdate;

        public SymetUpdate(int worldID, Vector2 position, double rotation, Vector2 velocity, float angularVelocity, SegmentUpdate segmentUpdate)
        {
            this.worldID = worldID;
            this.position = position;
            this.rotation = rotation;
            this.velocity = velocity;
            this.angularVelocity = angularVelocity;
            this.segmentUpdate = segmentUpdate;
        }
    }

    public struct SegmentUpdate
    {
        public int arm;
        public int segment;
        public int healthChange;


        public SegmentUpdate(int arm, int segment, int healthChange)
        {
            this.arm = arm;
            this.segment = segment;
            this.healthChange = healthChange;
        }       
    }

    public struct Collisions
    {
        public SegmentShape shape1;
        public SegmentShape shape2;

        public Collisions(SegmentShape shape1, SegmentShape shape2)
        {
            this.shape1 = shape1;
            this.shape2 = shape2;
        }
    }

    class Physics
    {
        public Physics()
        {
        }

        public List<SymetUpdate> DoCollision(List<Symet> symets)
        {
            List<SymetUpdate> updates = new List<SymetUpdate>();
            List<Collisions> collisions = new List<Collisions>();

            List<int> skipList = new List<int>();

            for (int i = 0; i < symets.Count; i++)
            {
                for (int j = i + 1; j < symets.Count; j++)
                {
                    if (PrimitiveShape.TestCollisionSimple(symets[i].Skeleton, symets[j].Skeleton))
                    {
                        collisions = GetCollisions(symets[i], symets[j]);

                        // We now have 2 symets that have collieded, here are the segments that hit and their info of what hit
                        // Info has arm id, segment id, and segment type
                        // Just make changes to first shape in collisions, use the second shape just as info to use on first
                        // Store all the changes you want into updates, this will be applied AFTER all collision has been done
                        // Code to apply it not yet in
                    }

                }
            }

            return updates;
        }

        private List<Collisions> GetCollisions(Symet symet1, Symet symet2)
        {
            List<Collisions> collisions = new List<Collisions>();

            List<SegmentShape> shapes1 = symet1.CollidableShapes;
            List<SegmentShape> shapes2 = symet2.CollidableShapes;

            for (int i = 0; i < shapes1.Count; i++)
            {
                for (int j = 0; j < shapes2.Count; j++)
                {
                    if (PrimitiveShape.TestCollision(shapes1[i].shape, shapes2[j].shape))
                    {
                        collisions.Add(new Collisions(shapes1[i], shapes2[j]));
                    }
                }
            }

            return collisions;
        }
    }
}

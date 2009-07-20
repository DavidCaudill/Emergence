using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Emergence
{
    public struct SymetMovmentUpdate
    {
        public Vector2 position;
        public double rotation;
        public Vector2 velocity;
        public float angularVelocity;
        public int worldID;
        public List<HitUpdate> hits;

        public SymetMovmentUpdate(int worldID, Vector2 position, double rotation, Vector2 velocity, float angularVelocity, List<HitUpdate> hits)
        {
            this.worldID = worldID;
            this.position = position;
            this.rotation = rotation;
            this.velocity = velocity;
            this.angularVelocity = angularVelocity;
            this.hits = hits;
        }
    }

    public struct HitUpdate
    {
        public int arm;
        public int segment;
        public SegmentType attacker;

        public HitUpdate(int arm, int segment, SegmentType attacker)
        {
            this.arm = arm;
            this.segment = segment;
            this.attacker = attacker;
        }
    }

    class Physics
    {
        public Physics()
        {
        }

        public List<SymetMovmentUpdate> DoCollision(Dictionary<int, Symet> symets)
        {
            List<SymetMovmentUpdate> updates = new List<SymetMovmentUpdate>();

            List<int> skipList = new List<int>();
            foreach (Symet symet1 in symets.Values)
            {
                skipList.Add(symet1.WorldID);

                foreach (Symet symet2 in symets.Values)
                {
                    if(skipList.Contains(symet2.WorldID))
                        continue;

                    List<LineHits> hits = PrimitiveShape.TestCollisionFull(symet1.Skeleton, symet1.WorldID, symet2.Skeleton, symet2.WorldID);
                }
            }

            return updates;
        }
    }
}

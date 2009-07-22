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

        public List<SymetMovmentUpdate> DoCollision(List<Symet> symets)
        {
            List<SymetMovmentUpdate> updates = new List<SymetMovmentUpdate>();

            List<int> skipList = new List<int>();

            for (int i = 0; i < symets.Count; i++)
            {
                //if (symets[i].Velocity.Length() == new Vector2(0).Length() && symets[i].AngularVelocity == 0)
                //    continue;

                for (int j = i+1; j < symets.Count; j++)
                {
                    PrimitiveShape.TestCollisionPoint(symets[i].Skeleton, symets[i].WorldID, symets[j].Skeleton, symets[j].WorldID);
                }
            }

            return updates;
        }
    }
}

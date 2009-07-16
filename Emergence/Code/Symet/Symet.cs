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
        List<Vector2> vertices;
        List<Vector2> skeletalVertices;
        PrimitiveShape primitive;
        Vector2 position;
        double rotation;
        bool active;
        DNA dna;
        PrimitiveShape skeleton;
        int hitPoints;

        int lastRegen;
        int energy;

        # region Properties

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
                skeleton.Position = value;

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
                skeleton.Rotation = Convert.ToSingle(value);

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

        #endregion

        // Functions

        public Symet()
        {
        }

        public Symet(List<Vector2> vertices, Dictionary<int, Arm> arms, DNA dna)
        {
            this.vertices = vertices;
            this.active = false;
            this.arms = arms;
            this.dna = dna;

            this.energy = 1000;
            this.hitPoints = 100;

            // Build the primative shape
            primitive = new PrimitiveShape(vertices.ToArray(), DrawType.LineLoop);
            switch (dna.BodyType)
            {
                case SegmentType.None:
                    break;
                case SegmentType.Attack:
                    primitive.ShapeColor = Color.Red;
                    break;
                case SegmentType.Defend:
                    primitive.ShapeColor = Color.Blue;
                    break;
                case SegmentType.Photo:
                    primitive.ShapeColor = Color.Green;
                    break;
                default:
                    break;
            }

            arms[0].SetSegmentActive(1, false);
            arms[1].SetSegmentActive(3, false);
            arms[2].SetSegmentActive(2, false);
            arms[2].SetSegmentActive(5, false);
            BuildSkeleton();
        }

        public int Update(GameTime gameTime)
        {
            // Regenerate any lost hitpoints
            if (lastRegen > 500)
            {
                lastRegen = 0;

                // Regen main body first
                hitPoints += 100 - hitPoints > 30 ? 30 : 100 - hitPoints;

                // Regen all the segments
                int tempEnergyDrain = 0;
                foreach (Arm arm in arms.Values)
                {
                    tempEnergyDrain += arm.Regenerate(energy / 3 > 100 ? 100 : energy / 3);
                }

                // Possible need for rebuilding skeleton so do it
                if(tempEnergyDrain > 0)
                    BuildSkeleton();

                energy -= tempEnergyDrain;
            }

            lastRegen += gameTime.ElapsedGameTime.Milliseconds;

            foreach (Arm arm in arms.Values)
            {
                arm.Update(gameTime);
            }
            return 1;
        }

        public int Draw(PrimitiveBatch primitiveBatch)
        {
            primitive.Draw(primitiveBatch);
            foreach (Arm arm in arms.Values)
            {
                arm.Draw(primitiveBatch);
            }
            skeleton.Draw(primitiveBatch);

            return 1;
        }

        public int BuildSkeleton()
        {
            skeletalVertices = new List<Vector2>();

            int j = 0;
            foreach (Arm arm in arms.Values)
            {
                skeletalVertices.Add(vertices[j]);
                skeletalVertices.AddRange(arm.RecursiveSkeletonBuilder(1));
                j++;
            }

            // Check for double vertices and remove
            for (int i = 0; i < skeletalVertices.Count; i++)
            {
                if (skeletalVertices[i] == skeletalVertices[(i + 1) % skeletalVertices.Count])
                    skeletalVertices.RemoveAt(i);

                // Wanted to use this to create an outline, just scaling isnt going to work.
                // Need to move a vertex outwards inbetween its 2 adjacent faces, probably.
                //skeletalVertices[i] = Vector2.Transform(skeletalVertices[i], Matrix.CreateScale(1.1f));
            }

            skeleton = new PrimitiveShape(skeletalVertices.ToArray(), DrawType.LineLoop);
            skeleton.Position = position;
            skeleton.Rotation = Convert.ToSingle(rotation);

            return 1;
        }
    }
}

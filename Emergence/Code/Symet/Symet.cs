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
        List<ShapeBuilderVertice> skeletalVertices;
        List<ShapeBuilderVertice> dividerVertices;
        Vector2 position;
        double rotation;
        float scale;
        bool alive;
        DNA dna;
        PrimitiveShape skeleton;
        PrimitiveShape segmentDividers;
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
                skeleton.Position = value;
                segmentDividers.Position = value;
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
                skeleton.Rotation = Convert.ToSingle(value);
                segmentDividers.Rotation = Convert.ToSingle(value);
            }
        }
        public float Scale
        {
            get
            {
                return this.scale;
            }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                this.scale = value;
                skeleton.Scale = value;
                segmentDividers.Scale = value;
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

        #endregion

        // Functions

        public Symet()
        {
        }

        public Symet(List<Vector2> vertices, Dictionary<int, Arm> arms, DNA dna)
        {
            this.vertices = vertices;
            this.alive = false;
            this.arms = arms;
            this.dna = dna;

            this.energy = 1900;
            this.hitPoints = 100;
            this.scale = 1;

            // Start off with some battle damage for show
            //arms[0].SetSegmentAlive(1, false);
            //arms[1].SetSegmentAlive(3, false);
            //arms[2].SetSegmentAlive(2, false);
            //arms[2].SetSegmentAlive(5, false);

            BuildSkeleton();
        }

        public int Update(GameTime gameTime)
        {
            // Regenerate any lost hitpoints
            if (lastRegen > 500 && energy > 50)
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

                energy -= tempEnergyDrain;
            }

            lastRegen += gameTime.ElapsedGameTime.Milliseconds;

            // Find out if we need to rebuild the skeleton
            bool tempBool = false;
            foreach (Arm arm in arms.Values)
            {
                arm.Update(gameTime);

                if (arm.RebuildSkeleton)
                {
                    tempBool = true;
                    arm.RebuildSkeleton = false;
                }
            }

            if (tempBool)
                BuildSkeleton();

            return 1;
        }

        public int Draw(PrimitiveBatch primitiveBatch)
        {
            skeleton.Draw(primitiveBatch);
            segmentDividers.Draw(primitiveBatch);

            return 1;
        }

        public int BuildSkeleton()
        {
            skeletalVertices = new List<ShapeBuilderVertice>();
            dividerVertices = new List<ShapeBuilderVertice>();

            List<Vector2> tempVertices = new List<Vector2>();
            List<Color> tempColors = new List<Color>();

            int j = 0;
            foreach (Arm arm in arms.Values)
            {
                skeletalVertices.Add(new ShapeBuilderVertice(vertices[j], Segment.GetColor(dna.BodyType)));

                if (arm.Alive)
                {
                    dividerVertices.Add(new ShapeBuilderVertice(vertices[j], Segment.GetColor(dna.BodyType)));
                    dividerVertices.Add(new ShapeBuilderVertice(vertices[(j + 1) % vertices.Count], Segment.GetColor(dna.BodyType)));
                }

                skeletalVertices.AddRange(arm.RecursiveSkeletonBuilder(1, Segment.GetColor(dna.BodyType)));
                dividerVertices.AddRange(arm.SegmentDividerBuilder());
                j++;
            }

            // Split the skeletalVertices' ShapeBuilderVertice's into seperate lists
            foreach (ShapeBuilderVertice shapeBuilderVertice in skeletalVertices)
            {
                tempVertices.Add(shapeBuilderVertice.vertice);
                tempColors.Add(shapeBuilderVertice.color);
            }

            // Remove any duplicates
            for (int i = 0; i < tempVertices.Count; i++)
            {
                if (tempVertices[i] == tempVertices[(i + 1) % tempVertices.Count])
                {
                    tempVertices.RemoveAt(i);
                    tempColors.RemoveAt(i);
                }
            }
            skeleton = new PrimitiveShape(tempVertices.ToArray(), tempColors.ToArray(), DrawType.LineLoop);

            tempVertices = new List<Vector2>();
            tempColors = new List<Color>();

            // Split the dividerVertices' ShapeBuilderVertice's into seperate lists
            foreach (ShapeBuilderVertice shapeBuilderVertice in dividerVertices)
            {
                tempVertices.Add(shapeBuilderVertice.vertice);
                tempColors.Add(shapeBuilderVertice.color);
            }
            segmentDividers = new PrimitiveShape(tempVertices.ToArray(), tempColors.ToArray(), DrawType.LineList);

            // Match them to current transformation
            skeleton.Position = position;
            skeleton.Rotation = Convert.ToSingle(rotation);
            skeleton.Scale = scale;

            segmentDividers.Position = position;
            segmentDividers.Rotation = Convert.ToSingle(rotation);
            segmentDividers.Scale = scale;

            return 1;
        }
    }
}

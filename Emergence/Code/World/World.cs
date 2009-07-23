using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Emergence
{
    class World
    {
        List<Symet> symets;
        int worldIDCounter;

        PrimitiveShape shape1HitOverlay;
        PrimitiveShape shape2HitOverlay;
        bool drawOverlays;

        Editor editor;
        Physics physics;

        public World()
        {
        }

        public int TestInitialize()
        {
            editor = new Editor();
            physics = new Physics();

            worldIDCounter = 0;
            DNA dna1;
            DNA dna2;

            // Test symet creation code
            dna1 = new DNA(Shape.Triangle, 5.0f, SegmentType.Defend, .6f);
            dna2 = new DNA(Shape.Square, 7.0f, SegmentType.Photo, 2.34f);

            List<VectorP> instructions1 = new List<VectorP>();
            List<VectorP> instructions2 = new List<VectorP>();
            List<VectorP> instructions3 = new List<VectorP>();
            symets = new List<Symet>();

            instructions1.Add(new VectorP(1.2, 5));
            instructions1.Add(new VectorP(.7, 6));
            instructions1.Add(new VectorP(.3, 8));

            instructions2.Add(new VectorP(.9, 5));
            instructions2.Add(new VectorP(.6, 15));

            instructions3.Add(new VectorP(1.1, 10));
            instructions3.Add(new VectorP(1.7, 10));
            instructions3.Add(new VectorP(1.1, 14));
            instructions3.Add(new VectorP(.8, 14));
            instructions3.Add(new VectorP(1.0, 10));

            dna1.CreateChromosome(instructions1, 0, 1, SegmentType.Movement, new Vector2(3, -5));
            dna1.CreateChromosome(instructions1, 1, 1, SegmentType.Attack, new Vector2(5, 4));
            dna1.CreateChromosome(instructions1, 1, 3, SegmentType.Movement, new Vector2(0, 0));
            dna1.CreateChromosome(instructions2, 2, 2, SegmentType.Attack, new Vector2());
            dna1.CreateChromosome(instructions2, 3, 2, SegmentType.Attack, new Vector2());
            //dna1.CreateChromosome(instructions3, 1, 2, SegmentType.Defend, new Vector2());

            dna2.CreateChromosome(instructions1, 0, 1, SegmentType.Movement, new Vector2(0, 0));
            dna2.CreateChromosome(instructions1, 1, 1, SegmentType.Photo, new Vector2(2, -5));
            dna2.CreateChromosome(instructions1, 1, 3, SegmentType.Movement, new Vector2(6, 2));
            dna2.CreateChromosome(instructions2, 2, 2, SegmentType.Defend, new Vector2(-5, 3));
            dna2.CreateChromosome(instructions2, 3, 2, SegmentType.Defend, new Vector2(6, 6));
            dna2.CreateChromosome(instructions3, 1, 2, SegmentType.Defend, new Vector2());

            Symet symet = new Symet();
            for (int i = 0; i < 6; i++)
            {
                symet = dna1.BuildDNA();
                symet.Position = new Vector2(50 + i * 100, 150);
                AddSymet(symet);

            }
            for (int i = 0; i < 6; i++)
            {
                symet = dna2.BuildDNA();
                symet.Position = new Vector2(50 + i * 100, 300);
                AddSymet(symet);
            }

            

            return 1;
        }

        public int Update(GameTime gameTime)
        {

            for (int i = 0; i < symets.Count; i++)
            {
                // Group all the symets at the mouse position
                //if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                //    symets[i].Position = mousePosition;

                // Scale all the symets
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                    symets[i].Scale *= 1.01f;
                if (Keyboard.GetState().IsKeyDown(Keys.Z))
                    symets[i].Scale /= 1.01f;

                // Update all the symets
                if (!Game1.GetGlobals().Editing)
                { 
                    symets[i].Update(gameTime); 
                }
                else
                {
                    editor.Update(gameTime, symets, Mouse.GetState()); 
                }

            }

            physics.DoCollision(symets);

            // TEST CODE FOR 2 SYMETS
            //List<LineHits> hits = PrimitiveShape.TestCollisionFull(symets[0].Skeleton, symets[0].WorldID, symets[1].Skeleton, symets[1].WorldID);

            //drawOverlays = false;
            //if (hits.Count > 1)
            //{
            //    drawOverlays = true;

            //    List<Vector2> vertices = new List<Vector2>();
            //    List<Color> colors = new List<Color>();

            //    foreach (LineHits hit in hits)
            //    {
            //        vertices.Add(hit.line1a);
            //        colors.Add(Color.White);
            //        vertices.Add(hit.line1b);
            //        colors.Add(Color.White);
            //    }

            //    shape1HitOverlay = new PrimitiveShape(vertices.ToArray(), colors.ToArray(), DrawType.LineList);

            //    vertices = new List<Vector2>();
            //    colors = new List<Color>();

            //    foreach (LineHits hit in hits)
            //    {
            //        vertices.Add(hit.line2a);
            //        colors.Add(Color.White);
            //        vertices.Add(hit.line2b);
            //        colors.Add(Color.White);
            //    }
            //    shape2HitOverlay = new PrimitiveShape(vertices.ToArray(), colors.ToArray(), DrawType.LineList);
            //}
 
            return 1;
        }

        public int Draw(GameTime gameTime, PrimitiveBatch primitiveBatch)
        {
            for (int i = 0; i < symets.Count; i++)
            {
                symets[i].Draw(primitiveBatch);
            }
            
            //if (drawOverlays)
            //{
            //    shape1HitOverlay.Draw(primitiveBatch);
            //    shape2HitOverlay.Draw(primitiveBatch);
            //}

            return 1;
        }

        private int CheckCollisions()
        {

            return 1;
        }

        private int GetWorldID()
        {
            int tempID = worldIDCounter;
            worldIDCounter++;

            return tempID;
        }

        public int AddSymet(Symet symet)
        {
            int tempID = GetWorldID();

            symet.WorldID = tempID;
            symets.Add(symet);

            return tempID;
        }
    }
}
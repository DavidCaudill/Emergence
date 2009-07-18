using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Emergence
{
    class World
    {
        List<Symet> symets;

        public World()
        {
        }

        public int TestInitialize()
        {
            DNA dna1;
            DNA dna2;

            // Test symet creation code
            dna1 = new DNA(Shape.Triangle, 5.0f, SegmentType.Defend, .13f);
            dna2 = new DNA(Shape.Square, 7.0f, SegmentType.Photo, 2.78f);

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

            dna1.CreateChromosome(instructions1, 0, 1, SegmentType.Movement, new Vector2(6, 2));
            dna1.CreateChromosome(instructions1, 1, 1, SegmentType.Movement, new Vector2(2, -3));
            dna1.CreateChromosome(instructions1, 1, 3, SegmentType.Movement, new Vector2(-2, 1));
            dna1.CreateChromosome(instructions2, 2, 2, SegmentType.Attack, new Vector2());
            dna1.CreateChromosome(instructions2, 3, 2, SegmentType.Attack, new Vector2());
            //dna1.CreateChromosome(instructions3, 1, 2, SegmentType.Defend, new Vector2());

            dna2.CreateChromosome(instructions1, 0, 1, SegmentType.Movement, new Vector2(-5, 3));
            dna2.CreateChromosome(instructions1, 1, 1, SegmentType.Photo, new Vector2(2, -5));
            dna2.CreateChromosome(instructions1, 1, 3, SegmentType.Photo, new Vector2(6, 2));
            dna2.CreateChromosome(instructions2, 2, 2, SegmentType.Defend, new Vector2(-5, 3));
            dna2.CreateChromosome(instructions2, 3, 2, SegmentType.Defend, new Vector2(6, 6));
            dna2.CreateChromosome(instructions3, 1, 2, SegmentType.Defend, new Vector2());

            Symet symet = new Symet();
            for (int i = 0; i < 100; i++)
            {
                symet = dna1.BuildDNA();
                symet.Position = new Vector2(300);
                symets.Add(symet);
            }
            for (int i = 0; i < 100; i++)
            {
                symet = dna2.BuildDNA();
                symet.Position = new Vector2(300);
                symets.Add(symet);
            }

            return 1;
        }

        public int Update(GameTime gameTime)
        {
            // Get mouse position
            Vector2 mousePosition = new Vector2();
            mousePosition.X = Mouse.GetState().X;
            mousePosition.Y = Mouse.GetState().Y;

            for (int i = 0; i < symets.Count; i++)
            {
                // Group all the symets at the mouse position
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    symets[i].Position = mousePosition;

                // Scale all the symets
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                    symets[i].Scale *= 1.01f;
                if (Keyboard.GetState().IsKeyDown(Keys.Z))
                    symets[i].Scale /= 1.01f;

                // Update all the symets
                symets[i].Update(gameTime);
            }

            return 1;
        }

        public int Draw(GameTime gameTime, PrimitiveBatch primitiveBatch)
        {
            for (int i = 0; i < symets.Count; i++)
            {
                symets[i].Draw(primitiveBatch);
            }

            return 1;
        }
    }
}

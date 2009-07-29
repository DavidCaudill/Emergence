using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Emergence
{
    class Editor
    {
        int ActiveID = 9001;

       public void Init()
       {
       }

       public void Update(GameTime gameTime, List<Symet> symet)
       {
            // Get mouse position
            Vector2 mousePosition = new Vector2();
            mousePosition.X = Mouse.GetState().X;
            mousePosition.Y = Mouse.GetState().Y;

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                for (int i = 0; i < symet.Count; i++)
                {
                    if (mousePosition.X >= symet[i].Position.X - 20 &&
                    mousePosition.Y >= symet[i].Position.Y - 20 &&
                    mousePosition.X <= symet[i].Position.X + 20 &&
                    mousePosition.Y <= symet[i].Position.Y + 20)
                    {

                        ActiveID = symet[i].WorldID;
                    }
                }

                for (int i = 0; i < symet.Count; i++)
                {
                    if (ActiveID == symet[i].WorldID)
                    {
                        if (Game1.Globals.Tool == "Grab")
                        {
                            symet[i].Position = mousePosition;
                            symet[i].GrabUpdate();
                        }
                        if (Game1.Globals.Tool == "Stimulant")
                        {
                            symet[i].AngularVelocity *= 1.001f;
                        }
                        if (Game1.Globals.Tool == "Poison")
                        {
                            symet[i].Alive = false;
                        }
                        if (Game1.Globals.Tool == "Mutigen")
                        {
                            symet[i].Scale *= 1.001f;
                            symet[i].GrabUpdate();
                        }
                    }
                }    
            }

            if (Mouse.GetState().LeftButton == ButtonState.Released)
            {
                ActiveID = 9001;
            }
       }
    }
}
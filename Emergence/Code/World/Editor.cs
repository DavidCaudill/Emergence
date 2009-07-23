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

       public void Update(GameTime gameTime, List<Symet> symet, MouseState mouse)
       {
                       // Get mouse position
            Vector2 mousePosition = new Vector2();
            mousePosition.X = mouse.X;
            mousePosition.Y = mouse.Y;

           for (int i = 0; i < symet.Count; i++)
           {
               if (mouse.X >= symet[i].Position.X - 20 &&
                   mouse.Y >= symet[i].Position.Y - 20 &&
                   mouse.X <= symet[i].Position.X + 20 &&
                   mouse.Y <= symet[i].Position.Y + 20)
               {
                   if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                   {
                       ActiveID = symet[i].WorldID;
                   }
               }

           }

           for (int i = 0; i < symet.Count; i++)
           {
               if (ActiveID == symet[i].WorldID)
               {
                   if (Game1.GetGlobals().Tool == "Grab")
                   {
                       symet[i].Position = mousePosition;
                       symet[i].Update(gameTime);
                   }
                   if (Game1.GetGlobals().Tool == "Stimulant")
                   {
                       symet[i].AngularVelocity *= 1.001f;
                   }
                   if (Game1.GetGlobals().Tool == "Poison")
                   {
                       symet[i].Alive = false;
                   }
                   if (Game1.GetGlobals().Tool == "Mutigen")
                   {
                       symet[i].Scale *= 1.001f;
                   }
                    
                   
               }

               if (Mouse.GetState().LeftButton == ButtonState.Released)
               {
                   ActiveID = 9001;
               }
           }
       }

    }
}
using System;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Emergence
{

    public class Settings
    {

        // Establish Default Global Settings
        public int MutationRate = 5;
        public int SolarIntensity = 5;
        public int SymetLifespan = 15;
        public int InitialPopulation = 30;
        public int RegenCost = 5;
        public int RegenRate = 5;
        public int SymetSpeed = 5;

        public bool ParentCanAttack = false; 
        public bool SiblingCanAttack = true;
        public bool SoundsOn = true;


        public void Load()
        {
            IniHandler ini = new IniHandler();
            System.Reflection.Assembly a = System.Reflection.Assembly.GetEntryAssembly();
            string path = Directory.GetCurrentDirectory() + "\\Settings.ini";

            ini.IniFile(path);

            if (!File.Exists(path))
            {
                ini.IniWriteValue("Settings", "MutationRate", "5");
                ini.IniWriteValue("Settings", "SolarIntensity", "5");
                ini.IniWriteValue("Settings", "SymetLifespan", "15");
                ini.IniWriteValue("Settings", "InitialPopulation", "30");
                ini.IniWriteValue("Settings", "RegenCost", "5");
                ini.IniWriteValue("Settings", "RegenRate", "5");
                ini.IniWriteValue("Settings", "SymetSpeed", "5");
                ini.IniWriteValue("Settings", "ParentCanAttack", "false");
                ini.IniWriteValue("Settings", "SiblingCanAttack", "true");
                ini.IniWriteValue("Settings", "SoundsOn", "true");
            }
            else
            {
                MutationRate = Convert.ToInt32(ini.IniReadValue("Settings", "MutationRate"));
                SolarIntensity = Convert.ToInt32(ini.IniReadValue("Settings", "SolarIntensity"));
                SymetLifespan = Convert.ToInt32(ini.IniReadValue("Settings", "SymetLifespan"));
                InitialPopulation = Convert.ToInt32(ini.IniReadValue("Settings", "InitialPopulation"));
                RegenCost = Convert.ToInt32(ini.IniReadValue("Settings", "RegenCost"));
                RegenRate = Convert.ToInt32(ini.IniReadValue("Settings", "RegenRate"));
                SymetSpeed = Convert.ToInt32(ini.IniReadValue("Settings", "SymetSpeed"));
                ParentCanAttack = Convert.ToBoolean(ini.IniReadValue("Settings", "ParentCanAttack"));
                SiblingCanAttack = Convert.ToBoolean(ini.IniReadValue("Settings", "SiblingCanAttack"));
                SoundsOn = Convert.ToBoolean(ini.IniReadValue("Settings", "SoundsOn"));
            }

        }

    }





}
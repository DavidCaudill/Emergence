using System;
using System.IO;
using System.Collections.Generic;
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
using TomShane.Neoforce.Controls;

namespace Emergence
{

    public class EmergenceGui
    {

        // Define manager and controls we use.
        private Window BottomBar;
        private TextBox textbox;
        private RadioButton radStimulant;
        private RadioButton radPoison;
        private RadioButton radMutigen;
        private RadioButton radGrab;
        private Button btnSettings;
        private Button btnSample;
        private Button btnSave;
        private Button btnLoad;

        //Define settings pannel stuff
        private Window wndSettings;
        private SpinBox spnMutRate;
        private Label lblMutRate;
        private SpinBox spnSolar;
        private Label lblSolar;
        private SpinBox spnLifeSpan;
        private Label lblLifeSpan;
        private SpinBox spnSpeed;
        private Label lblSpeed;
        private Button btnSettingsClose;


        public void GuiInitialize(Manager manager, GraphicsDeviceManager graphics)
        {
            // Create and setup Window control.
            BottomBar = new Window(manager);
            BottomBar.Init();
            BottomBar.Width = graphics.PreferredBackBufferWidth;
            BottomBar.Height = 100;
            BottomBar.Top = graphics.PreferredBackBufferHeight - BottomBar.Height;
            BottomBar.Left = 0;
            BottomBar.BorderVisible = false;
            BottomBar.Movable = false;
            BottomBar.Resizable = false;
            BottomBar.Visible = true;

            // Create Alert Text
            textbox = new TextBox(manager);
            textbox.Init();
            textbox.Text = "In philosophy and science, emergence is the way complex systems and patterns arise out of a multiplicity of relatively simple interactions.";
            textbox.Width = 830;
            textbox.Height = 20;
            textbox.ReadOnly = true;
            textbox.Passive = true;
            textbox.Left = (1024 / 2) - (textbox.Width / 2);
            textbox.Top = 0;
            textbox.Parent = BottomBar;

            // Create Tool Select.
            radStimulant = new RadioButton(manager);
            radStimulant.Init();
            radStimulant.Parent = BottomBar;
            radStimulant.Left = (1024 / 2) - (textbox.Width / 2);
            radStimulant.Top = 30;
            radStimulant.Width = 100;
            radStimulant.Height = 16;
            radStimulant.Text = "Stimulant";
            radStimulant.Checked = true;

            radPoison = new RadioButton(manager);
            radPoison.Init();
            radPoison.Parent = BottomBar;
            radPoison.Left = (1024 / 2) - (textbox.Width / 2) + 110;
            radPoison.Top = 30;
            radPoison.Width = 100;
            radPoison.Height = 16;
            radPoison.Text = "Poison";
            radPoison.Checked = false;

            radMutigen = new RadioButton(manager);
            radMutigen.Init();
            radMutigen.Parent = BottomBar;
            radMutigen.Left = (1024 / 2) - (textbox.Width / 2) + 220;
            radMutigen.Top = 30;
            radMutigen.Width = 100;
            radMutigen.Height = 16;
            radMutigen.Text = "Mutigen";
            radMutigen.Checked = false;

            radGrab = new RadioButton(manager);
            radGrab.Init();
            radGrab.Parent = BottomBar;
            radGrab.Left = (1024 / 2) - (textbox.Width / 2) + 330;
            radGrab.Top = 30;
            radGrab.Width = 100;
            radGrab.Height = 16;
            radGrab.Text = "Grab";
            radGrab.Checked = false;

            // Create Settings button.
            btnSettings = new Button(manager);
            btnSettings.Init();
            btnSettings.Text = "Settings";
            btnSettings.Width = 72;
            btnSettings.Height = 24;
            btnSettings.Left = 10;
            btnSettings.Top = 10;
            btnSettings.Anchor = Anchors.Bottom;
            btnSettings.Parent = BottomBar;
            btnSettings.Click += new MouseEventHandler(btnSettings_Click);

            // Create Sample button.
            btnSample = new Button(manager);
            btnSample.Init();
            btnSample.Text = "Sample";
            btnSample.Width = 72;
            btnSample.Height = 24;
            btnSample.Left = 10;
            btnSample.Top = 44;
            btnSample.Anchor = Anchors.Bottom;
            btnSample.Parent = BottomBar;
            btnSample.Click += new MouseEventHandler(btnSample_Click);

            // Create Save Button.
            btnSave = new Button(manager);
            btnSave.Init();
            btnSave.Text = "Save";
            btnSave.Width = 72;
            btnSave.Height = 24;
            btnSave.Left = 1024 - btnSave.Width - 10;
            btnSave.Top = 10;
            btnSave.Anchor = Anchors.Bottom;
            btnSave.Parent = BottomBar;

            // Create Load Button.
            btnLoad = new Button(manager);
            btnLoad.Init();
            btnLoad.Text = "Load";
            btnLoad.Width = 72;
            btnLoad.Height = 24;
            btnLoad.Left = 1024 - btnLoad.Width - 10;
            btnLoad.Top = 44;
            btnLoad.Anchor = Anchors.Bottom;
            btnLoad.Parent = BottomBar;

            //Create Settings Pane
            wndSettings = new Window(manager);
            wndSettings.Init();
            wndSettings.Text = "Settings";
            wndSettings.Width = 400;
            wndSettings.Height = 300;
            wndSettings.Top = 20;
            wndSettings.Left = 20;
            wndSettings.Resizable = false;
            wndSettings.Visible = false;

            //Create Environment settings

            lblMutRate = new Label(manager);
            lblMutRate.Init();
            lblMutRate.Parent = wndSettings;
            lblMutRate.Left = 10;
            lblMutRate.Top = 10;
            lblMutRate.Text = "Mutation rate:";
            lblMutRate.Width = 130;
            lblMutRate.Height = 20;

            spnMutRate = new SpinBox(manager, SpinBoxMode.Range);
            spnMutRate.Init();
            spnMutRate.ReadOnly = true;
            spnMutRate.Width = 60;
            spnMutRate.Height = 20;
            spnMutRate.Top = 30;
            spnMutRate.Left = 10;
            spnMutRate.Maximum = 10;
            spnMutRate.Minimum = 0;
            spnMutRate.Rounding = 0;
            spnMutRate.Value = Game1.GetGlobals().MutationRate;
            spnMutRate.Step = 1;
            spnMutRate.Passive = true;
            spnMutRate.Text = Convert.ToString(spnMutRate.Value);
            spnMutRate.Parent = wndSettings;

            lblSolar = new Label(manager);
            lblSolar.Init();
            lblSolar.Parent = wndSettings;
            lblSolar.Left = 10;
            lblSolar.Top = 60;
            lblSolar.Text = "Solar Radiation:";
            lblSolar.Width = 130;
            lblSolar.Height = 20;

            spnSolar = new SpinBox(manager, SpinBoxMode.Range);
            spnSolar.Init();
            spnSolar.ReadOnly = true;
            spnSolar.Width = 60;
            spnSolar.Height = 20;
            spnSolar.Top = 80;
            spnSolar.Left = 10;
            spnSolar.Maximum = 10;
            spnSolar.Minimum = 0;
            spnSolar.Rounding = 0;
            spnSolar.Step = 1;
            spnSolar.Value = Game1.GetGlobals().SolarIntensity;
            spnSolar.Passive = true;
            spnSolar.Text = Convert.ToString(spnSolar.Value);
            spnSolar.Parent = wndSettings;

            lblLifeSpan = new Label(manager);
            lblLifeSpan.Init();
            lblLifeSpan.Parent = wndSettings;
            lblLifeSpan.Left = 10;
            lblLifeSpan.Top = 110;
            lblLifeSpan.Text = "Life in Mins:";
            lblLifeSpan.Width = 130;
            lblLifeSpan.Height = 20;

            spnLifeSpan = new SpinBox(manager, SpinBoxMode.Range);
            spnLifeSpan.Init();
            spnLifeSpan.ReadOnly = true;
            spnLifeSpan.Width = 60;
            spnLifeSpan.Height = 20;
            spnLifeSpan.Top = 130;
            spnLifeSpan.Left = 10;
            spnLifeSpan.Maximum = 30;
            spnLifeSpan.Minimum = 0;
            spnLifeSpan.Rounding = 0;
            spnLifeSpan.Step = 1;
            spnLifeSpan.Value = Game1.GetGlobals().SymetLifespan;
            spnLifeSpan.Passive = true;
            spnLifeSpan.Text = Convert.ToString(spnLifeSpan.Value);
            spnLifeSpan.Parent = wndSettings;

            lblSpeed = new Label(manager);
            lblSpeed.Init();
            lblSpeed.Parent = wndSettings;
            lblSpeed.Left = 10;
            lblSpeed.Top = 160;
            lblSpeed.Text = "Game Speed:";
            lblSpeed.Width = 130;
            lblSpeed.Height = 20;

            spnSpeed = new SpinBox(manager, SpinBoxMode.Range);
            spnSpeed.Init();
            spnSpeed.ReadOnly = true;
            spnSpeed.Width = 60;
            spnSpeed.Height = 20;
            spnSpeed.Top = 180;
            spnSpeed.Left = 10;
            spnSpeed.Maximum = 10;
            spnSpeed.Minimum = 0;
            spnSpeed.Rounding = 0;
            spnSpeed.Step = 1;
            spnSpeed.Value = Game1.GetGlobals().SymetSpeed;
            spnSpeed.Text = Convert.ToString(spnSpeed.Value);
            spnSpeed.Passive = true;
            spnSpeed.Parent = wndSettings;

            // Create Done Button.
            btnSettingsClose = new Button(manager);
            btnSettingsClose.Init();
            btnSettingsClose.Text = "Done";
            btnSettingsClose.Width = 72;
            btnSettingsClose.Height = 24;
            btnSettingsClose.Left = 400 - 92;
            btnSettingsClose.Top = 300 - 64;
            btnSettingsClose.Anchor = Anchors.Bottom;
            btnSettingsClose.Parent = wndSettings;
            btnSettingsClose.Click += new MouseEventHandler(btnDone_Click);

            // Add the window controls to the manager processing queue.
            manager.Add(BottomBar);
            manager.Add(wndSettings);
        }

        public void btnSample_Click(object sender, MouseEventArgs e)
        {
            
        }

        public void btnSettings_Click(object sender, MouseEventArgs e)
        {
            wndSettings.Visible = true;
        }

        public void btnDone_Click(object sender, MouseEventArgs e)
        {
            //save settings
            IniHandler ini = new IniHandler();
            System.Reflection.Assembly a = System.Reflection.Assembly.GetEntryAssembly();
            string path = Directory.GetCurrentDirectory() + "\\Settings.ini";
            ini.IniFile(path);

            ini.IniWriteValue("Settings", "MutationRate", Convert.ToString(spnMutRate.Value));
            ini.IniWriteValue("Settings", "SolarIntensity", Convert.ToString(spnSolar.Value));
            ini.IniWriteValue("Settings", "SymetLifespan", Convert.ToString(spnLifeSpan.Value));
            ini.IniWriteValue("Settings", "SymetSpeed", Convert.ToString(spnSpeed.Value));

            wndSettings.Visible = false;
        }


    }
}
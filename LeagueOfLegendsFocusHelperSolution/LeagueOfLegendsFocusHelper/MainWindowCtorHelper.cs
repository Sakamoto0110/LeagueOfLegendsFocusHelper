


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Forms;

namespace LeagueOfLegendsFocusHelper
{
    public partial class MainWindow : Window
    {

        public NotifyIcon MakeNotifyIcon()
        {
            NotifyIcon nIcon;
            nIcon = new NotifyIcon();
            nIcon.Text = "League of Legends Focus helper";
            nIcon.Icon = Properties.Resources.lulu;
            nIcon.Visible = true;
            nIcon.DoubleClick += (s, e) => MainWindowEventRouter(s, "ToggleVisibility");



            var volumeButtons = new List<ToolStripButton>();

            EventHandler NotifyIconContextMenuVolumeChange = (sender, e) =>
            {
                foreach (var toolStripButton in volumeButtons)
                {
                    toolStripButton.Checked = false;
                }

                ((ToolStripButton)sender).Checked = true;
                VolumeSlider.Value = (int)(((ToolStripButton)sender).Tag);
            };

            int width = 200;

            StatusLabel = new ToolStripLabel(Options.IsActive ? "Active" : "Not active")
            {
                ForeColor = Options.IsActive ? System.Drawing.Color.Red : System.Drawing.Color.Black,


                Width = width,
                DisplayStyle = ToolStripItemDisplayStyle.Text
            };

            volumeButtons.AddRange(new[]{
                new ToolStripButton("100%", null, NotifyIconContextMenuVolumeChange) { Tag = 100,
                    DisplayStyle = ToolStripItemDisplayStyle.Text,Width = width,
                    Checked = (int)VolumeSlider.Value == 100 },
                new ToolStripButton("75%", null, NotifyIconContextMenuVolumeChange) { Tag = 75,
                    DisplayStyle = ToolStripItemDisplayStyle.Text,Width = width,
                    Checked = (int)VolumeSlider.Value == 75 },
                new ToolStripButton("50%", null, NotifyIconContextMenuVolumeChange) { Tag = 50 ,
                    DisplayStyle = ToolStripItemDisplayStyle.Text,Width = width,
                    Checked = (int)VolumeSlider.Value == 50},
                new ToolStripButton("25%", null, NotifyIconContextMenuVolumeChange) { Tag = 25 ,
                    DisplayStyle = ToolStripItemDisplayStyle.Text,Width = width,
                    Checked = (int)VolumeSlider.Value == 25},
                new ToolStripButton("0%", null, NotifyIconContextMenuVolumeChange) { Tag = 0,
                    DisplayStyle = ToolStripItemDisplayStyle.Text,
                    Width = width,
                    Checked = (int)VolumeSlider.Value == 0
                },
            });




            nIcon.ContextMenuStrip = new ContextMenuStrip()
            {
                MaximumSize = new System.Drawing.Size(width, 500),
                Items =
                {
                    StatusLabel,
                    new ToolStripSeparator(),
                    new ToolStripMenuItem("Toggle menu",null,(s,e)=>MainWindowEventRouter(s,"ToggleVisibility"))
                    {
                        Name = "ToggleMenu",Width = width,
                            DisplayStyle = ToolStripItemDisplayStyle.Text

                    },
                    new ToolStripMenuItem("Activate/Deactivate",null,(e,args)=>Options.IsActive =!Options.IsActive  ){Name = "PauseUnpause",
                        Width = width,DisplayStyle = ToolStripItemDisplayStyle.Text},
                    new ToolStripMenuItem("Volume")
                    {
                        Name = "VolumeBtnList",Width = width,
                        DisplayStyle = ToolStripItemDisplayStyle.Text,
                        DropDownItems =
                        {
                            volumeButtons[0],
                            volumeButtons[1],
                            volumeButtons[2],
                            volumeButtons[3],
                            volumeButtons[4],
                        }
                    },
                    new ToolStripMenuItem("Terminate",null,(s,args)=>MainWindowEventRouter(s,"Terminate")){Name = "Terminate",
                        Width = width,DisplayStyle = ToolStripItemDisplayStyle.Text}
                }
            };
            return nIcon;
        }




    }
}
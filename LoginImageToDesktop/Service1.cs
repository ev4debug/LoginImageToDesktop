using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LoginImageToDesktop
{
    public partial class Service1 : ServiceBase
    {
        private string sourcedir = ConfigurationManager.AppSettings.Get("sourcedir");
        private string destinationdir = ConfigurationManager.AppSettings.Get("destinationdir");

        private System.Timers.Timer timer;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Start();
        }

        protected override void OnStop()
        {
            timer.Stop();
        }

        public void Start()
        {
            CheckDirsDifference();
            timer = new System.Timers.Timer(3600000); 
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.timer_Elapsed);
            timer.Start();            
        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Stop();
            CheckDirsDifference();
            timer.Start();
        }

        private void CheckDirsDifference()
        {
            var sourcefiles = Directory.GetFiles(sourcedir).ToList();
            foreach (var file in sourcefiles)
            {
                MoveSourceToDestination(file);
            }
        }

        private void MoveSourceToDestination(string file)
        {
            try
            {
                Bitmap img = new Bitmap(file);
                var imageHeight = img.Height;
                var imageWidth = img.Width;
                img = null;
                if (imageHeight == 1080 && imageWidth == 1920)
                {
                    if (!File.Exists(destinationdir + Path.GetFileName(file) + ".jpg"))
                        File.Copy(file, destinationdir + Path.GetFileName(file) + ".jpg");
                }
            }
            catch (Exception)
            {

            }
        }
    }
}

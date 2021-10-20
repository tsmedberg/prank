using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using System.Net;

using NAudio.Wave;
using NAudio.Wave.SampleProviders;

using AudioSwitcher.AudioApi.CoreAudio;

using System.Threading;

namespace form_test
{
    public partial class Form1 : Form
    {
        bool allowClose = false;
        private static Bitmap bmpScreenshot;
        private static Graphics gfxScreenshot;

        private WaveOutEvent outputDevice;
        private AudioFileReader audioFile;

        CoreAudioDevice defaultPlaybackDevice;

        public Form1()
        {
            defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice; 
            string audioPath = Path.Combine(Path.GetTempPath(),"rick.mp3");
            WebClient myWebClient = new WebClient();
            myWebClient.DownloadFile("https://memes.t0rre.dev/rick.mp3", audioPath);
            InitializeComponent();
            this.TopMost = true;
            this.Hide();
            // Set the bitmap object to the size of the screen
            bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
            // Create a graphics object from the bitmap
            gfxScreenshot = Graphics.FromImage(bmpScreenshot);
            // Take the screenshot from the upper left corner to the right bottom corner
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
            // Save the screenshot to the specified path that the user has chosen
            this.BackgroundImage = bmpScreenshot;
            // Show the form again
            this.Show();
            outputDevice = new WaveOutEvent();
    
            audioFile = new AudioFileReader(audioPath);
            outputDevice.Init(audioFile);
    
            outputDevice.Play();
            while (outputDevice.PlaybackState == PlaybackState.Playing)
            {
                setVolume();
                Thread.Sleep(50);
            }
            allowClose = true;
            this.Close();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
        }

        public void setVolume()
        {
            defaultPlaybackDevice.Volume = 17;
            Thread.Sleep(500);
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !allowClose;
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'p')
            {
                allowClose = !allowClose;
            }
        }
    }
}

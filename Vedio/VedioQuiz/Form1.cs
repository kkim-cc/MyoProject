using Camera_NET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VedioQuiz
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Rectangle resolution = Screen.PrimaryScreen.Bounds;
            Point ptLocation = new Point(0, 0);
            Size szSize = new Size(resolution.Width / 2, resolution.Height / 2);
            this.Location = ptLocation;
            this.Size = szSize;
            this.StartPosition = FormStartPosition.Manual;
             
        }

        private void cameraControl1_Load(object sender, EventArgs e)
        {
            // Camera choice
            CameraChoice _CameraChoice = new CameraChoice();

            // Get List of devices (cameras)
            _CameraChoice.UpdateDeviceList();

            // To get an example of camera and resolution change look at other code samples 
            if (_CameraChoice.Devices.Count > 0)
            {
                // Device moniker. It's like device id or handle.
                // Run first camera if we have one
                var camera_moniker = _CameraChoice.Devices[0].Mon;

                // Set selected camera to camera control with default resolution
                cameraControl1.SetCamera(camera_moniker, null);
            }
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            cameraControl1.CloseCamera();
        }
    }
}

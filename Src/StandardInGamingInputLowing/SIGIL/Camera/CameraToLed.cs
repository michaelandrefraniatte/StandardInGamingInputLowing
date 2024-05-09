using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Camera;
using AForge;
using AForge.Imaging;
using System.Drawing;
using System;
using System.Diagnostics;

namespace CameraAPI
{
    public class CameraToLed
    {
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        private bool running, formvisible;
        private AForge.Video.DirectShow.FilterInfoCollection CaptureDevice;
        private AForge.Video.DirectShow.VideoCaptureDevice FinalFrame;
        private static Bitmap img, ClonedImg, EditableImg;
        private static AForge.Imaging.BlobCounter blobCounter = new AForge.Imaging.BlobCounter();
        private static AForge.Imaging.Filters.BlobsFiltering blobfilter = new AForge.Imaging.Filters.BlobsFiltering();
        private static AForge.Imaging.Filters.ConnectedComponentsLabeling componentfilter = new AForge.Imaging.Filters.ConnectedComponentsLabeling();
        private static AForge.Imaging.Blob[] blobs;
        private static List<AForge.IntPoint> corners = new List<AForge.IntPoint>();
        private static AForge.Math.Geometry.SimpleShapeChecker shapeChecker = new AForge.Math.Geometry.SimpleShapeChecker();
        private static AForge.Imaging.Filters.BrightnessCorrection brightnessfilter;
        private static AForge.Imaging.Filters.ColorFiltering colorfilter = new AForge.Imaging.Filters.ColorFiltering();
        private static AForge.Imaging.Filters.Grayscale grayscalefilter = new AForge.Imaging.Filters.Grayscale(1, 0, 0);
        private static AForge.Imaging.Filters.EuclideanColorFiltering euclideanfilter = new AForge.Imaging.Filters.EuclideanColorFiltering();
        private int radius = 175, brightness = -50, red = 0, green = 205, blue = 205;
        public double backpointX, posRightX, backpointY, posRightY, camx, camy;
        private int number;
        private Form1 form1 = new Form1();
        private Stopwatch PollingRate;
        private double pollingrateperm = 0, pollingratetemp = 0, pollingratedisplay = 0, pollingrate;
        public CameraToLed()
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
            running = true;
        }
        public void ViewData()
        {
            if (!form1.Visible)
            {
                PollingRate = new Stopwatch();
                PollingRate.Start();
                formvisible = true;
                form1.SetVisible();
            }
        }
        public void Close()
        {
            running = false;
            Thread.Sleep(100);
            FinalFrame.NewFrame -= FinalFrame_NewFrame;
            Thread.Sleep(100);
            if (FinalFrame.IsRunning)
            {
                FinalFrame.Stop();
            }
        }
        public void BeginPolling()
        {
            Task.Run(() => taskD());
        }
        private void taskD()
        {
            for (; ; )
            {
                if (!running)
                    break;
                ProcessStateLogic();
                if (formvisible)
                {
                    try
                    {
                        pollingratedisplay++;
                        pollingratetemp = pollingrateperm;
                        pollingrateperm = PollingRate.ElapsedMilliseconds;
                        if (pollingratedisplay > 300)
                        {
                            pollingrate = pollingrateperm - pollingratetemp;
                            pollingratedisplay = 0;
                        }
                        string str = "camx : " + camx + Environment.NewLine;
                        str += "camy : " + camy + Environment.NewLine;
                        str += "PollingRate : " + pollingrate + " ms" + Environment.NewLine;
                        str += Environment.NewLine;
                        form1.SetLabel1(str);
                    }
                    catch { }
                }
            }
        }
        public void Init()
        {
        }
        public void Scan(int red = 0, int green = 205, int blue = 205, int brightness = -50, int radius = 175, int number = 0)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
            this.brightness = brightness;
            this.radius = radius;
            this.number = number;
            StartWebcamInputs();
        }
        private void StartWebcamInputs()
        {
            CaptureDevice = new AForge.Video.DirectShow.FilterInfoCollection(AForge.Video.DirectShow.FilterCategory.VideoInputDevice);
            FinalFrame = new AForge.Video.DirectShow.VideoCaptureDevice(CaptureDevice[CaptureDevice.Count - 1].MonikerString);
            FinalFrame.NewFrame += FinalFrame_NewFrame;
            FinalFrame.Start();
        }
        private void FinalFrame_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            img = (Bitmap)eventArgs.Frame.Clone();
        }
        private void ProcessStateLogic()
        {
            try
            {
                ClonedImg = img;
                brightnessfilter = new AForge.Imaging.Filters.BrightnessCorrection(brightness);
                brightnessfilter.ApplyInPlace(ClonedImg);
                colorfilter.Red = new IntRange(red, 255);
                colorfilter.Green = new IntRange(green, 255);
                colorfilter.Blue = new IntRange(blue, 255);
                colorfilter.ApplyInPlace(ClonedImg);
                brightnessfilter.ApplyInPlace(ClonedImg);
                euclideanfilter.CenterColor = new RGB(255, 255, 255);
                euclideanfilter.Radius = (short)radius;
                euclideanfilter.ApplyInPlace(ClonedImg);
                blobCounter.ProcessImage(ClonedImg);
                blobs = blobCounter.GetObjectsInformation();
                for (int i = 0; i < blobs.Length; i++)
                {
                    shapeChecker.RelativeDistortionLimit = 100f;
                    shapeChecker.MinAcceptableDistortion = 20f;
                    if (shapeChecker.IsCircle(blobCounter.GetBlobsEdgePoints(blobs[i])))
                    {
                        backpointX = blobs[0].CenterOfGravity.X;
                        backpointY = blobs[0].CenterOfGravity.Y;
                    }
                }
                posRightX = backpointX - ClonedImg.Width / 2f;
                posRightY = backpointY - ClonedImg.Height / 2f;
                camx = posRightX / (ClonedImg.Width / 2f) * 1024f;
                camy = posRightY / (ClonedImg.Height / 2f) * 1024f;
                ClonedImg.Dispose();
            }
            catch { }
            Thread.Sleep(1);
        }
    }
}
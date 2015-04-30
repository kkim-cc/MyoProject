using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using EMGClass;
using DrawSetup;
using System.Diagnostics;

namespace StarterKit
{
    class Game : GameWindow
    {
        EMGData emg = new EMGData();
        int texture;
        float emgMax = 20;
        int emgColorMax = 20;
        NamedPipeServer PServer2;

        public Game()
            : base(800, 600, GraphicsMode.Default, "OpenTK Quick Start Sample")
        {
            VSync = VSyncMode.On;

            Rectangle resolution = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
            Point ptLocation = new Point(resolution.Width / 2, 0);
            Size szSize = new Size(resolution.Width / 2, resolution.Height / 2);
            this.Size = szSize;
            this.Location = ptLocation;

            PServer2 = new NamedPipeServer(@"\\.\pipe\myNamedPipe2", 0, this);
            PServer2.Start();
            //GL.Enable(EnableCap.Texture2D);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //GL.ClearColor(0.1f, 0.2f, 0.5f, 0.0f);
            //GL.Enable(EnableCap.DepthTest);

            texture = DrawThings.LoadTexture("1.jpg");
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);

        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (Keyboard[Key.Escape])
                Exit();
        }

        public override void Exit()
        {
            PServer2.StopServer();
            base.Exit();
        }

        public void NewData(string strNewData)
        {
            string line_data;
            line_data = strNewData.Replace(" ", "");
            string[] data = line_data.Split(',');

            if (data.Count() != 9)
                return;

            emg.GetData(line_data);
            //string strLog = string.Format("{0} {1} {2} {3} {4} {5}, {6}, {7}\n",
            //    emg.emg[0], emg.emg[1], emg.emg[2], emg.emg[3], emg.emg[4], emg.emg[5], emg.emg[6], emg.emg[7]);
            //Debug.WriteLine(strLog);
        }


        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            //Picture instantiated
            GL.Enable(EnableCap.Texture2D);

            //Clearing screen
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.ClearColor(Color.Blue);

            //Printing picture to the screen
            GL.BindTexture(TextureTarget.Texture2D, texture);

            GL.Begin(PrimitiveType.Quads);

            GL.Color3(Color.White);
            GL.TexCoord2(0, 0);
            GL.Vertex2(-1, 1);

            GL.TexCoord2(1, 0);
            GL.Vertex2(1, 1);

            GL.TexCoord2(1, 1);
            GL.Vertex2(1, -1);

            GL.TexCoord2(0, 1);
            GL.Vertex2(-1, -1);

            GL.End();

            GL.Disable(EnableCap.Texture2D);

            //this.SwapBuffers();

            GL.LineWidth(8f);
            /*
            for (int i = 0; i < 8; i++)
            {
                GL.Color3(Math.Abs(emg.emg[i]) / emgMax, 0.0f, 0.0f);
                GL.Vertex2(-0.78f, -0.04f);
                GL.Vertex2(-0.8f, -0.14f);
                
                if(i < 7)
                    GL.Begin(PrimitiveType.Lines);

            }
            */
            if (Math.Abs(emg.emg[0]) > emgColorMax)// || emg.emg[0] < emgColorMin)
            {
                GL.Begin(PrimitiveType.Lines);

                GL.Color3(Math.Abs(emg.emg[0]) / emgMax, 0.0f, 0.0f);
                GL.Vertex2(-0.78f, -0.04f);
                GL.Vertex2(-0.8f, -0.14f);
            }

            if (Math.Abs(emg.emg[1]) > emgColorMax)//|| emg.emg[1] < emgColorMin)
            {
                GL.Begin(PrimitiveType.Lines);

                GL.Color3(Math.Abs(emg.emg[1]) / emgMax, 0.0f, 0.0f);
                GL.Vertex2(-0.71f, -0.07f);
                GL.Vertex2(-0.73f, -0.17f);
            }
            if (Math.Abs(emg.emg[2]) > emgColorMax)//|| emg.emg[2] < emgColorMin)
            {
                GL.Begin(PrimitiveType.Lines);

                GL.Color3(Math.Abs(emg.emg[2]) / emgMax, 0.0f, 0.0f);
                GL.Vertex2(-0.66f, -0.1);
                GL.Vertex2(-0.68f, -0.2f);
            }
            if (Math.Abs(emg.emg[3]) > emgColorMax)// || emg.emg[3] < emgColorMin)
            {
                GL.Begin(PrimitiveType.Lines);

                GL.Color3(Math.Abs(emg.emg[3]) / emgMax, 0.0f, 0.0f);
                GL.Vertex2(-0.84f, 0f);
                GL.Vertex2(-0.86f, -0.1f);
            }
            if (Math.Abs(emg.emg[4]) > emgColorMax)// || emg.emg[4] < emgColorMin)
            {
                GL.Begin(PrimitiveType.Lines);

                GL.Color3(Math.Abs(emg.emg[4]) / emgMax, 0.0f, 0.0f);
                GL.Vertex2(0.78f, 0.1f);
                GL.Vertex2(0.8f, 0.0f);
            }
            if (Math.Abs(emg.emg[5]) > emgColorMax)// || emg.emg[5] < emgColorMin)
            {
                GL.Begin(PrimitiveType.Lines);

                GL.Color3(Math.Abs(emg.emg[5]) / emgMax, 0.0f, 0.0f);
                GL.Vertex2(0.72f, 0.05f);
                GL.Vertex2(0.74f, -0.05f);
            }
            if (Math.Abs(emg.emg[6]) > emgColorMax)// || emg.emg[6] < emgColorMin)
            {
                GL.Begin(PrimitiveType.Lines);

                GL.Color3(Math.Abs(emg.emg[6]) / emgMax, 0.0f, 0.0f);
                GL.Vertex2(0.66f, 0f);
                GL.Vertex2(0.68f, -0.1f);
            }
            if (Math.Abs(emg.emg[7]) > emgColorMax)// || emg.emg[7] < emgColorMin)
            {
                GL.Begin(PrimitiveType.Lines);

                GL.Color3(Math.Abs(emg.emg[7]) / emgMax, 0.0f, 0.0f);
                GL.Vertex2(0.60f, -0.05f);
                GL.Vertex2(0.62f, -0.15f);
            }
            
            GL.End();

            
            //GL.DeleteTexture(texture);

            System.Threading.Thread.Sleep(20);

            //emg.nextLine();                    
            
            this.SwapBuffers();
        }


        [STAThread]
        static void Main()
        {
            // The 'using' idiom guarantees proper resource cleanup.
            // We request 30 UpdateFrame events per second, and unlimited
            // RenderFrame events (as fast as the computer can handle).
            using (Game game = new Game())
            {
                game.Run(30.0);
            }
        }
    }

}
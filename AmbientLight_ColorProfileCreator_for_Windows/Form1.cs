using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using AmbientLight_ColorProfileCreator_for_Windows;

namespace AmbientLight_ColorProfileCreator_for_Windows
{
    public partial class Form1 : Form
    {
        #region variable definitions
        
        ColorCapture colorCapture; //create an ColorCapture object
        public Graphics graphics; //grapichs object for drawing
        public static Rectangle rectangle = new Rectangle(50, 50, 150, 150);
        public static Rectangle rectangle2 = new Rectangle(250, 50, 150, 150);

        static SolidBrush myBrush = new SolidBrush(Color.Red);
        static SolidBrush myBrush2 = new SolidBrush(Color.Red);
        Thread thread_fillingColorBox;

        private int click_counter = 0;
     

        #endregion
        public Form1()
        {
            InitializeComponent();
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            logger.begin();

            graphics = this.CreateGraphics();
            colorCapture = new ColorCapture();
            thread_fillingColorBox = new Thread(fillingColorBox); //initialization of thread
            
            //thread_fillingColorBox.Start();
            //while (!thread_fillingColorBox.IsAlive) ;
            //thread_fillingColorBox.Join();
            //ThreadPool.QueueUserWorkItem(fillingColorBox, 1);

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //close everything from this app
            Console.WriteLine("muhaha");
            
            logger.close();
            Environment.Exit(Environment.ExitCode);
            //thread_fillingColorBox.Abort();
        }

        /// <summary>
        /// Handler of button1 clicking events.
        /// If you click the button, fillingColorBox thread will be started. If you click again, it will be stopped.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            click_counter++;
            if (click_counter % 2 == 1)
            {
                if (thread_fillingColorBox.ThreadState != ThreadState.Running)
                {
                    //start  
                    
                    thread_fillingColorBox.Start();
                }
            }
            else
            {
                //Stop
                if (thread_fillingColorBox.ThreadState == ThreadState.Running)
                {
                    thread_fillingColorBox.Abort();
                }
                
            }
        }
        

        /// <summary>
        /// Main function of thread_fillingColorBox.
        /// Get calculated color from colorCapture and draw a rectangle and fill it with this color.
        /// </summary>
        private void fillingColorBox(object stateInformation)
        {
            while (true)
            {
                myBrush = colorCapture.getColor();
                myBrush2 = colorCapture.getColor2();
                try {
                    graphics.FillRectangle(myBrush, rectangle);
                    graphics.FillRectangle(myBrush2, rectangle2);
                }
                catch (System.Runtime.InteropServices.ExternalException e)
                {
                    Console.WriteLine("[catched exception] exit with excepction while drawing colored rectangle");
                    Environment.Exit(Environment.ExitCode);
                }
                Thread.Sleep(10);
            }

        }
    }
}

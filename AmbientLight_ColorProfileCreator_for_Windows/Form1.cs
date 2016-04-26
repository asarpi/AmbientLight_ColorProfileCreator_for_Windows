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

namespace AmbientLight_ColorProfileCreator_for_Windows
{
    public partial class Form1 : Form
    {
        #region variable definitions
        //import necessary dlls
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDCm, int x, int y, int nWidth, int nHeigh, IntPtr hSrcDC, int xSrc, int ySrc, int swRop);

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point lpPoint);

        Thread t;
        int x, y;
        Graphics graphics;
        Rectangle rectangle = new Rectangle(50, 50, 150, 150);

        Point cursor = new Point();

        #endregion
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /* init */
            t = new Thread(catchPixelColorFromScreen);
            t.Start();
            DrawIt();
            //Environment.Exit(Environment.ExitCode);
        }

        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //close everything from this app
            Environment.Exit(Environment.ExitCode);
        }
        private void DrawIt()
        {
            graphics = this.CreateGraphics();
            Rectangle rectangle = new Rectangle(50, 50, 150, 150);
            graphics.DrawRectangle(Pens.Red, rectangle);

        }

        private void catchPixelColorFromScreen()
        {
            //create a bitmap (which consists of the pixel data for a graphios image and its attrubtes)
            Bitmap screenCopy = new Bitmap(1, 1); //initialize bitmap with specified size (int32, int32) --> this contains one pixel
            using (Graphics gdest = Graphics.FromImage(screenCopy)) //create a Graphics (IDisposable) object with "screenCopy" bitmap
                                                                    //because it is provide a mechanism for releasing unmanaged resources after using, therefore it is very memory-friendly
            {
                while (true)
                {
                    using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero))
                    {
                        GetCursorPos(ref cursor);
                        int x = cursor.X;
                        int y = cursor.Y;
                        IntPtr hSrcDC = gsrc.GetHdc(); //gets the handle to device, which associated with this Graphics object
                        IntPtr hDC = gdest.GetHdc();
                        int retval = BitBlt(hDC, 0, 0, 1, 1, hSrcDC, x, y, (int)CopyPixelOperation.SourceCopy); //fastest way for access screen that Windows show
                        //TODO: this method may won't working on GPU-accekerated content --> we will get a black image from videos, games etc.
                        //but it's working on my PC :-)
                        gdest.ReleaseHdc();
                        gsrc.ReleaseHdc();   
                    }
                    Color c = Color.FromArgb(screenCopy.GetPixel(0, 0).ToArgb()); //convert captured pixel to Color object
                    //if you close the app, kick off the thread, which gives color value, therefore we needs for this try/catch mechanism for catching exception
                    try
                    {
                        graphics.FillRectangle(new SolidBrush(c), rectangle);
                    }
                    catch (System.Runtime.InteropServices.ExternalException e)
                    {
                        //close the app and everything in it
                        Console.WriteLine("exit with exception: System.Runtime.InteropServices.ExternalException during drawing colored rectangle");
                        Environment.Exit(Environment.ExitCode);
                    }
                }
            }
        }
    }
}

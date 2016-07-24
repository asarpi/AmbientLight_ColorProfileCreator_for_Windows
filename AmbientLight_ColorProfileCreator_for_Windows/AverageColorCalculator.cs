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
    public class AverageColorCalculator
    {
        #region definnitions
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDCm, int x, int y, int nWidth, int nHeigh, IntPtr hSrcDC, int xSrc, int ySrc, int swRop);

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point lpPoint);

        Thread thread_catchingColor;
        Graphics graphics;
        Rectangle rectangle = new Rectangle(50, 50, 150, 150);

        Point cursor = new Point();

        Color average_color = new Color();

        #endregion

        public AverageColorCalculator()
        {
            thread_catchingColor = new Thread(catchColorDataFromScreen);
            thread_catchingColor.Start();

        }

        private struct WatchedPixelData
        {
            public int x;
            public int y;
            public Color color;
        }

        private LinkedList<WatchedPixelData> watched_pixels = new LinkedList<WatchedPixelData>();

        #region do_functions

        private void createWatchedPixelList()
        {

            WatchedPixelData px;
            px.x = 0;
            px.y = 0;
            px.color = new Color();
            watched_pixels.AddLast(px);
        }

        private void draw_rectangle()
        {
            graphics = this.CreateGrapics();
        }

        private void catchColorDataFromScreen()
        {
            Bitmap screenCopy = new Bitmap(1, 1);
            using (Graphics gdest = Graphics.FromImage(screenCopy))
            {
                while (true)
                {
                    using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero))
                    {
                        GetCursorPos(ref cursor);
                        IntPtr hSrcDC = gsrc.GetHdc();
                        IntPtr hDC = gdest.GetHdc();
                        //LinkedList<WatchedPixelData> pxs_tmp = watched_pixels;
                        //watched_pixels = new LinkedList<WatchedPixelData>();
                        //WatchedPixelData px = new WatchedPixelData();
                        //foreach (WatchedPixelData px_tmp in pxs_tmp)
                        //{
                        //  px = px_tmp;
                        //int retval = BitBlt(hDC, 0, 0, 1, 1, hSrcDC, px.x, px.y, (int)CopyPixelOperation.SourceCopy);
                        int retval = BitBlt(hDC, 0, 0, 1, 1, hSrcDC, cursor.X, cursor.Y, (int)CopyPixelOperation.SourceCopy);
                        //px.color = Color.FromArgb(screenCopy.GetPixel(0, 0).ToArgb());
                        //    watched_pixels.AddLast(px);
                        //}
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

        void calcAverageColor()
        {
            int color_A = 0;
            int color_R = 0;
            int color_G = 0;
            int color_B = 0;
            foreach (WatchedPixelData px in watched_pixels)
            {
                color_A += px.color.A;
                color_R += px.color.R;
                color_G += px.color.G;
                color_B += px.color.B;
            }
            color_A /= watched_pixels.Count;
            color_R /= watched_pixels.Count;
            color_G /= watched_pixels.Count;
            color_B /= watched_pixels.Count;
            average_color = new Color();
            average_color = Color.FromArgb(color_A, color_R, color_G, color_B);
        }









        #endregion
        #region get_methods
        public Color getAverageColor() { return average_color;  }

        #endregion
    }


 }

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Windows;



namespace AmbientLight_ColorProfileCreator_for_Windows
{
    public class ColorCapture
    {
        #region variable definitions

        /**** Exterbak DLL imports for capture pixels and mouse position ****/ 

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDCm, int x, int y, int nWidth, int nHeigh, IntPtr hSrcDC, int xSrc, int ySrc, int swRop);


        [DllImport("user32.dll")]

        //Variable definitions for cursor's capturing
        static extern bool GetCursorPos(ref Point lpPoint);
        Point cursor = new Point();

        //thread definitions
        Thread thread_colorCapturing_with_mouse;
        Thread thread_colorCapturing_with_grid;
        



        private int[,] captured_colors_rgb; //storage color data
        private Color captured_color2 = Color.Black; //storage color data


        //get screen specific datas
        private int screenHeight = Screen.PrimaryScreen.Bounds.Height;
        private int screenWidth = Screen.PrimaryScreen.Bounds.Width;

        //capturing variables
        private int vertical_resoulution = 0;
        private int horizontal_resolution = 0;
        private int pixelnum_of_slice = 1;

        long[,] rgb_totals;              //array for store summ of RGB components
        long[,] rgb_averages;            //array for store averages of RGB components
        private Color[] captured_colors; //storage of color data

        #endregion


        #region public_functions_and interfaces
        /// <summary>
        /// Init function
        /// </summary>
        public ColorCapture()
        {
            
            start_capturing_with_mouse();
            //start_capturing_with_grid();
            logger.add(LogTypes.ColorCapturing, "screenHeight: " + screenHeight);
            logger.add(LogTypes.ColorCapturing, "screenWidth: " + screenWidth);
            setResolution(10,10);
        }


        public Color[] getColor()
        {
            Color[] colors = new Color[vertical_resoulution * horizontal_resolution];
            for (int i = 0; i < (vertical_resoulution * horizontal_resolution); i ++)
            {
                colors[i] = Color.FromArgb(Convert.ToInt32(rgb_averages[i,0]), 
                                            Convert.ToInt32(rgb_averages[i, 1]), 
                                            Convert.ToInt32(rgb_averages[i, 2]));
            }
            return colors;
            // return new SolidBrush(captured_color);
        }


        public SolidBrush getColor2()
        {
            return new SolidBrush(captured_color2);
        }

        public void setResolution(int vertical, int horizontal)
        {
            vertical_resoulution = vertical;
            horizontal_resolution = horizontal;
            logger.add(LogTypes.ColorCapturing, "set vertical resolution to " + vertical_resoulution.ToString() + " and horizontal resolution to " + horizontal_resolution.ToString());
            pixelnum_of_slice = (screenHeight / vertical_resoulution +2 ) * (screenWidth / horizontal_resolution +2) ;

        }


        public int[] getResolution()
        {
            return new int[] { vertical_resoulution, horizontal_resolution };
        }

        /**** Thread management ****/


        public void start_capturing_with_mouse()
        {
            thread_colorCapturing_with_mouse = new Thread(capture_pixel_color_with_mouse);
            thread_colorCapturing_with_mouse.Start();
            logger.add(LogTypes.ColorCapturing, "colorCapturing with mouse thread started");
            setResolution(1, 1); 
        }


        public void stop_capturing_with_mouse()
        {
            thread_colorCapturing_with_mouse.Abort();
            logger.add(LogTypes.ColorCapturing, "colorCapturing with mouse thread aborted");
        }


        public void start_capturing_with_grid()
        {
            thread_colorCapturing_with_grid = new Thread(capture_pixel_color_with_full_image);
            thread_colorCapturing_with_grid.Start();
            //while (!thread_colorCapturing_with_grid.IsAlive) ;
            logger.add(LogTypes.ColorCapturing, "colorCapturing with total image started");
        }


        public void stop_capturing_with_grid()
        {
            thread_colorCapturing_with_grid.Abort();
            logger.add(LogTypes.ColorCapturing, "colorCapturing with total image thread aborted");
        }
        #endregion

        #region private_capture_functions_wMouse

        /***** Capture methods ****/

        private void capture_pixel_color_with_mouse()
        {
            rgb_averages = new long[1,3]; //conatiner of rgb values

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
                    Color captured_color = Color.FromArgb(screenCopy.GetPixel(0, 0).ToArgb()); //convert captured pixel to Color object
                    rgb_averages[0, 0] = captured_color.R;
                    rgb_averages[0, 1] = captured_color.G;
                    rgb_averages[0, 2] = captured_color.B;
                   

                    Thread.Sleep(100);
                }
            }
        }

        #endregion

        


      
        #region private_capture_functions_wFullImage

        private void capture_pixel_color_with_full_image(object stateInformation)

        {

            IntPtr Scan0; //
            int retval = 0;             //retval of image capturing
            long t_last = 0;            //timestamps for measuring image processing time
            long t_curr = 0;
            int stride;                 //width of a single row of pixels
            int slicenum, slicenum_height, slicenum_width;
            System.Drawing.Imaging.BitmapData srcData; //pointer of address of the bitmap's first line


            //create a bitmap (which consists of the pixel data for a graphios image and its attrubtes)
            Bitmap screenCopy = new Bitmap(screenWidth, screenHeight); //initialize bitmap with specified size (int32 width, int32 height) --> this contains one pixel
            using (Graphics gdest = Graphics.FromImage(screenCopy)) //create a Graphics (IDisposable) object with "screenCopy" bitmap
                                                                    //because it is provide a mechanism for releasing unmanaged resources after using, therefore it is very memory-friendly
            {
                while (true)
                {
                    t_curr = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                    logger.add(LogTypes.ColorCapturing,"\timage processing turnaround time: " + Convert.ToSingle(t_curr - t_last) / 1000);
                    //Console.Write(" b1 {0}  ", Convert.ToSingle(t_curr - t_last) / 1000);
                    using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero))
                    {
                        IntPtr hSrcDC = gsrc.GetHdc(); //gets the handle to source device, which associated with this Graphics object
                        IntPtr hDC = gdest.GetHdc();   //get handler of destination device
                        /*
                        BitBlt: bit-block transfer of the color data between specified source and destination devive
                        inputs:  
                            - hdcDest: handle to the destination device
                            - nXDest:  x coordinate of upper-left corner of the destination rectangle
                            - nYDest:  y coordinate of upper-left corner of the destination rectangle
                            - nWiddth: width of the destination rectangle
                            - nHeight: height of the destination rectangle
                            - hdcSrc:  handler of source
                            - nXSrc:   x coordinate of upper-left corner of the source rectangle
                            - nYSrc:   y coordinate of upper-left corner of the source rectangle
                            - dwRop:   opration code. What we do.
                        */
                        retval = BitBlt(hDC, 0, 0, screenWidth, screenHeight, hSrcDC, 0, 0, (int)CopyPixelOperation.SourceCopy); //fastest way for access screen that Windows show
                                                                                                                                 //TODO: this method may won't working on GPU-accekerated content --> we will get a black image from videos, games etc.
                        t_curr = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                        //logger.add(LogTypes.ColorCapturing, "2. capture time: " + Convert.ToSingle(t_curr - t_last) / 1000);


                        //lock the bitmap's bits and add its first line's pointer to srcData
                        srcData = screenCopy.LockBits(new Rectangle(0, 0, screenWidth, screenHeight),
                            System.Drawing.Imaging.ImageLockMode.ReadOnly,
                            System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                        stride =  srcData.Stride;  //get width of a single row of pixels in the bitmap
                        Scan0 = srcData.Scan0; //get the first address of the firs pixel data (and this means the first scan line too)

                        rgb_totals = new long[vertical_resoulution * horizontal_resolution, 3]; //set total RGB values to zero
                        
                        int limit_height = 0;
                        int limit_width = 0;
                        long asd = 0;

                        unsafe
                        {
                            slicenum = 0;
                            int slicenum_prev = 0;
                            slicenum_height = 0;
                            slicenum_width = 0;
                            int a = 0;

                            byte* p = (byte*)(void*)Scan0; //add the first addres of first pixel data to pointer
                            
                            for (int y = 0; y < screenHeight; y++) //iterate over height coordinates
                            {
                                limit_height = (slicenum_height + 1) * screenHeight / (vertical_resoulution); // counting slices (increase if, pixels are out of specified area of pixels
                                if (y == 0)
                                {
                                    slicenum_height = 0;
                                    asd = 0;
                                }
                                else if (y >= limit_height)
                                {
                                    slicenum_height++;
                                    //slicenum++;
                                    asd = 0;
                                }

                                for (int x = 0; x < screenWidth; x++) //iterate over width coordinates
                                {
                                    limit_width = (slicenum_width + 1) * screenWidth / (horizontal_resolution);
                                    if (x == 0)
                                    {
                                        slicenum_width = 0;
                                        asd = 0;
                                    }
                                    else if (x >= limit_width)
                                    {
                                        slicenum_width++;
                                        //slicenum++;
                                        asd = 0;
                                    }
                                    slicenum = slicenum_height * horizontal_resolution + slicenum_width;
                                //    if ((y >= screenHeight/2) && (slicenum != slicenum_prev))
                               //         Console.WriteLine("muhaha");
                                    slicenum_prev = slicenum;
                                    asd++;
                                    for (int color = 0; color < 3; color++) //iteration over colors
                                    {
                                        int idx = (y * stride) + x * 3 + color;
                                        //Console.WriteLine(slicenum.ToString() + "  " + color.ToString());
                                        rgb_totals[slicenum, color] += p[idx];
                                    }
                                }
                            }
                        }
                        //Console.WriteLine(asd);
                        screenCopy.UnlockBits(srcData);

                        rgb_averages = new long[vertical_resoulution * horizontal_resolution, 3]; //set average RGB values to zero
                        for (int slice = 0; slice < rgb_totals.GetLength(0); slice ++)
                        {
                            rgb_averages[slice, 0] = rgb_totals[slice, 0] / pixelnum_of_slice; //average R
                            rgb_averages[slice, 1] = rgb_totals[slice, 1] / pixelnum_of_slice; //average G
                            rgb_averages[slice, 2] = rgb_totals[slice, 2] / pixelnum_of_slice; //average B
                        }
                        
                        
                        //logger.add(LogTypes.ColorCapturing, avgR1.ToString() + "  " + avgG1.ToString() + "  " + avgB1.ToString() + "  " + avgR2.ToString() + "  " + avgG2.ToString() + "  " + avgB2.ToString());

                        t_curr = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                        //logger.add(LogTypes.ColorCapturing, "3. conversion time: " + Convert.ToSingle(t_curr - t_last) / 1000);
                        //Console.Write(" b3 {0}  ", Convert.ToSingle(t_curr - t_last) / 1000);
                        gdest.ReleaseHdc();
                        gsrc.ReleaseHdc();
                    }
                }
            }
        }
        #endregion

    }
}

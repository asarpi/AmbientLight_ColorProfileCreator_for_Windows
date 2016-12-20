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
        
        //ColorCapture colorCapture; //create an ColorCapture object
        public Graphics graphics; //grapichs object for drawing
        public ColorAvgCalc_1_simple avgCalculator;

        //tmp
        displayedColorValues colorBoxes;


        public int num_of_boxes_vertical = 10;
        public int num_of_boxes_horizontal = 10;


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
            //colorCapture = new ColorCapture();
            avgCalculator = new ColorAvgCalc_1_simple(num_of_boxes_vertical,num_of_boxes_horizontal);
            //thread_fillingColorBox = new Thread(fillingOneColorBox); //initialization of thread
            thread_fillingColorBox = new Thread(fillingMoreColorBox); //initialization of thread
            
            
            //thread_fillingColorBox.Start();
            //while (!thread_fillingColorBox.IsAlive) ;
            //thread_fillingColorBox.Join();
            //ThreadPool.QueueUserWorkItem(fillingColorBox, 1);

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //close everything from this app and save logger to file
            
            logger.close();
            thread_fillingColorBox.Abort();
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

        public static Rectangle rectangle; // = new Rectangle(50, 50, 150, 150);
        static SolidBrush myBrush; // = new SolidBrush(Color.Red);
        private int[] number_of_rectangles;
        Color[] colors_of_rectangles;
        int rect_x = 50;
        int rect_y = 50;
        int rect_width = 50;
        int rect_height = 50;

        /// <summary>
        /// Main function of thread_fillingColorBox.
        /// Get calculated color from colorCapture and draw a rectangle and fill it with this color.
        /// </summary>
        private void fillingMoreColorBox(object stateInformation)
        {
            while (true)
            {


                number_of_rectangles = avgCalculator.getCapturedResolution();

                calculatedAvgColorMatrix = new Color[number_of_rectangles[0], number_of_rectangles[1]];
                colors_of_rectangles = avgCalculator.getRawColors();
                createColorMatrix(colors_of_rectangles);
                colorBoxes = createDisplayableBoxes(calculatedAvgColorMatrix, 50, 50, 50, 50);

                for (int i = 0; i < number_of_rectangles[0]; i++)
                {
                    for (int j = 0; j < number_of_rectangles[1]; j++)
                    {
                        graphics.FillRectangle(colorBoxes.rectBrushes[i, j], colorBoxes.rects[i, j]);
                    }
                }
            }

            /*
            while (true)
            {
                number_of_rectangles = avgCalculator.getCapturedResolution();

                // INTERFACE TO CALCULATED COLORS
                colors_of_rectangles = avgCalculator.getRawColors();

                rectangle = new Rectangle(50, 50, 150, 150);
                foreach (Color c in colors_of_rectangles)
                {
                    for (int id_rect_vertical = 0; id_rect_vertical < number_of_rectangles[0]; id_rect_vertical++)
                    {
                        for (int id_rect_horizontal = 0; id_rect_horizontal < number_of_rectangles[1]; id_rect_horizontal++)
                        {
                            rectangle = new Rectangle(rect_x + (id_rect_horizontal * rect_width)+5,
                                                      rect_y + (id_rect_vertical * rect_height)+5,
                                                      rect_width,
                                                      rect_height);
                            myBrush = new SolidBrush(colors_of_rectangles[id_rect_vertical * number_of_rectangles[1] + id_rect_horizontal ]);
                                graphics.FillRectangle(myBrush, rectangle);
                            
                        }
                    }
                }
            }
            */

        }

        protected Color[,] calculatedAvgColorMatrix;
        

        public void createColorMatrix(Color[] colorArray)
        {
            foreach (Color c in colorArray)
            {
                for (int id_box_vertical = 0; id_box_vertical < number_of_rectangles[0]; id_box_vertical++)
                {
                    for (int id_box_horizontal = 0; id_box_horizontal < number_of_rectangles[1]; id_box_horizontal++)
                    {
                        calculatedAvgColorMatrix[id_box_vertical, id_box_horizontal] = colorArray[id_box_vertical * number_of_rectangles[1] + id_box_horizontal];
                    }
                }
            }
        }

        public displayedColorValues createDisplayableBoxes(Color[,] colorMatrix, int pos_x, int pos_y, int width, int height)
        {
            displayedColorValues boxes = new displayedColorValues();
            boxes.rects = new Rectangle[number_of_rectangles[0], number_of_rectangles[1]];
            boxes.rectBrushes = new SolidBrush[number_of_rectangles[0], number_of_rectangles[1]];

            for (int id_vert = 0; id_vert < number_of_rectangles[0]; id_vert++)
            {
                for (int id_hor = 0; id_hor < number_of_rectangles[1]; id_hor++)
                {
                    boxes.rects[id_vert, id_hor] = new Rectangle(pos_x + (id_hor * width) + 5, pos_y + (id_vert * height) + 5, width, height);
                    boxes.rectBrushes[id_vert, id_hor] = new SolidBrush(colorMatrix[id_vert, id_hor]);
                }
            }

            return boxes;
        }


        private void fillingOneColorBox(object stateInformation)
        {
            while (true)
            {
                myBrush = new SolidBrush(avgCalculator.getAvgColor()[0]);
                rectangle = new Rectangle(50, 50, 150, 150);
                graphics.FillRectangle(myBrush, rectangle);
                
            }

        }
    }
}

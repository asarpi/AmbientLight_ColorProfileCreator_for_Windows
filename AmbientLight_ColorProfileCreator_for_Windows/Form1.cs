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
using System.IO.Ports;

namespace AmbientLight_ColorProfileCreator_for_Windows
{
    public partial class AmbiLight : Form
    {
        #region variable definitions
        
        //ColorCapture colorCapture; //create an ColorCapture object
        public Graphics graphics; //grapichs object for drawing
        public ColorAvgCalc_1_simple avgCalculator;

        //tmp
        displayedColorValues colorBoxes;


        public int num_of_boxes_vertical = 2;
        public int num_of_boxes_horizontal = 5;
        private int number_of_leds = 30;
        CB2LP_converter_border_zoh_interpolation converter;


        Thread thread_fillingColorBox;

        private int click_counter = 0;



        #endregion
        public AmbiLight()
        {
            InitializeComponent();
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            logger.begin();
            ConfigurationManagger configs = new ConfigurationManagger();
            comboBox_comPortList.Items.Add("Default");
            

            graphics = this.CreateGraphics();
            avgCalculator = new ColorAvgCalc_1_simple(num_of_boxes_vertical,num_of_boxes_horizontal);
            //thread_fillingColorBox = new Thread(fillingOneColorBox); //initialization of thread
            thread_fillingColorBox = new Thread(fillingMoreColorBox); //initialization of thread
            AssociatedLED_indices indices = new AssociatedLED_indices();
            indices.left_first = 0;
            indices.left_last = 1;
            indices.bottom_first = 2;
            indices.bottom_last = 14;
            indices.right_first = 15;
            indices.right_last = 16;
            indices.top_first = 17;
            indices.top_last = 29;
            converter = new CB2LP_converter_border_zoh_interpolation(30, indices, new int[2] { num_of_boxes_vertical, num_of_boxes_horizontal });
            setConnectionStatus(converter.getSerialOpenFlag());
            textBox_board.Text = "Aruino Nano w ATmega 328";

            
            //configs.setDefaultValues();
            //configs = new ConfigurationManagger();


        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //close everything from this app and save logger to file
            logger.close();
            if (thread_fillingColorBox.ThreadState != ThreadState.Unstarted)
            {

                if ((thread_fillingColorBox.ThreadState != ThreadState.Aborted) && (thread_fillingColorBox.ThreadState != ThreadState.AbortRequested))
                {
                    thread_fillingColorBox.Abort();
                }
                while (thread_fillingColorBox.ThreadState != ThreadState.Aborted)
                {
                    Console.WriteLine(thread_fillingColorBox.ThreadState);
                    //do nothing. Wait for aborting.
                }
            }

            Color[] colorArray = new Color[30];
            for (byte i = 0; i < 30; i++)
            {
                colorArray[i] = Color.Black;
            }
            converter.setLEDStrip(colorArray);
            Environment.Exit(Environment.ExitCode);
            //thread_fillingColorBox.Abort();
        }

        /// <summary>
        /// Handler of button1 clicking events.
        /// If you click the button, fillingColorBox thread will be started.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {

            if (thread_fillingColorBox.ThreadState != ThreadState.Running)
            {

                thread_fillingColorBox = new Thread(fillingMoreColorBox); //initialization of the thread or re-initialization if it is aborted
                thread_fillingColorBox.Start();
                textBox_messages.AppendText("Streaming is started\n");
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

                colors_of_rectangles = avgCalculator.getRawColors();
                converter.fillLEDarray(colors_of_rectangles);

                /* DEBUG
                colorBoxes = converter.createDisplayableBoxes(50, 50, 25, 25);

                for (int i = 0; i < number_of_rectangles[0]; i++)
                {
                    for (int j = 0; j < number_of_rectangles[1]; j++)
                    {
                        graphics.FillRectangle(colorBoxes.rectBrushes[i, j], colorBoxes.rects[i, j]);
                    }
                }
                
                colorBoxes = converter.displayLEDstripColors(50, 400, 25, 25);
                for (int i = 0; i < converter.getNumOfLeds(); i++)
                {
                    graphics.FillRectangle(colorBoxes.rectBrushes[i, 0], colorBoxes.rects[i, 0]);
                }

                */
                
            }

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

        public void setConnectionStatus(bool serialOpenFlag)
        {
            if (serialOpenFlag)
            {
                textBox_connectionState.Text = "connected";
            }
            else
            {
                textBox_connectionState.Text = "not connected";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (thread_fillingColorBox.ThreadState == ThreadState.Running)
            {
                thread_fillingColorBox.Abort();
                textBox_messages.AppendText("Streaming is aborted\n");
                textBox_messages.ScrollToCaret();
            }
            colorDialog1.ShowDialog();
            Color[] colorArray = new Color[30];
            Color desiredColor = colorDialog1.Color;
            for (byte i = 0; i<30; i++)
            {
                colorArray[i] = desiredColor;
            }
            converter.setLEDStrip(colorArray);

            textBox_manualColor.BackColor = desiredColor;
            textBox_manualColor.ForeColor = Color.FromArgb(255 - desiredColor.R, 255 - desiredColor.G, 255 - desiredColor.B);
            textBox_manualColor.Text = "R: " + desiredColor.R + " G: " + desiredColor.G + " B: " + desiredColor.B;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string[] availablePorts = SerialPort.GetPortNames();
            foreach (string element in availablePorts)
            {
                comboBox_comPortList.Items.Add(element);
            }
            
        }

        private void tabPage_manualControl_Click(object sender, EventArgs e)
        {

        }

        private void button_stopStreaming_Click(object sender, EventArgs e)
        {
            if (thread_fillingColorBox.ThreadState == ThreadState.Running)
            {
                thread_fillingColorBox.Abort();
                textBox_messages.AppendText("Streaming is aborted\n");
                textBox_messages.ScrollToCaret();
            }
            else
            {
                textBox_messages.AppendText("Streaming is not runing\n");
            }
        }

        private void button_resetLeds_Click(object sender, EventArgs e)
        {
            Color[] colorArray = new Color[number_of_leds];

            for (byte i = 0; i < 30; i++)
            {
                colorArray[i] = Color.Black;
            }
            converter.setLEDStrip(colorArray);
            textBox_messages.AppendText("LEDs are reseted\n");
            textBox_messages.ScrollToCaret();

        }

        private void button_reconnectSerial_Click(object sender, EventArgs e)
        {
            if (comboBox_comPortList.SelectedItem != null)
            {
                converter.reconnectSerial(comboBox_comPortList.SelectedItem.ToString());
            }
            else
            {
                converter.reconnectSerial("Default");
            }
            setConnectionStatus(converter.getSerialOpenFlag());
        }

        private void button_getLogs_Click(object sender, EventArgs e)
        {
            string[] log_entries = logger.getLogs();
            textBox_logger.Text = "";
            foreach (string log_entry in log_entries)
            {
                textBox_logger.AppendText(log_entry + Environment.NewLine);
                textBox_logger.ScrollToCaret();
            }
            textBox_logger.ScrollBars = ScrollBars.Vertical;
            textBox_logger.ScrollToCaret();
            
        }

        private void button_disconnectSerial_Click(object sender, EventArgs e)
        {
            converter.disconnectSerial();
            setConnectionStatus(converter.getSerialOpenFlag());
        }

        private void numericUpDown_factorR_ValueChanged(object sender, EventArgs e)
        {
            converter.setFactor_red(Convert.ToDouble(numericUpDown_factorR.Value) / 100);
        }

        private void numericUpDown_factorG_ValueChanged(object sender, EventArgs e)
        {
            converter.setFactor_green(Convert.ToDouble(numericUpDown_factorG.Value) / 100);
        }

        private void numericUpDown_factorB_ValueChanged(object sender, EventArgs e)
        {
            converter.setFactor_blue(Convert.ToDouble(numericUpDown_factorB.Value) / 100);
        }
    }
}

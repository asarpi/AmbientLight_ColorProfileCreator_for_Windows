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
        ConfigurationManagger configuration;

        //ColorCapture colorCapture; //create an ColorCapture object
        public Graphics graphics; //grapichs object for drawing
        public ColorAvgCalc_1_simple avgCalculator;

        //tmp
        displayedColorValues colorBoxes;


        public int num_of_boxes_vertical = 2;
        public int num_of_boxes_horizontal = 5;
        private int number_of_leds;
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
            configuration = new ConfigurationManagger();

            // set displayed default values
            comboBox_comPortList.Items.Add("Default");
            numericUpDown_num_of_leds.Value = configuration.getConfig(ConfigTypes.number_of_leds);
            

            graphics = this.CreateGraphics();
            


            // DISPLAY DISSAPEARING SETUP IN ADVANCED SETUP TAB
            numericUpDown_led_left_first.Visible = false;
            numericUpDown_led_left_last.Visible = false;
            numericUpDown_led_bottom_first.Visible = false;
            numericUpDown_led_bottom_last.Visible = false;
            numericUpDown_led_right_first.Visible = false;
            numericUpDown_led_right_last.Visible = false;
            numericUpDown_led_top_first.Visible = false;
            numericUpDown_led_top_last.Visible = false;
            numericUpDown_num_of_leds.Visible = false;
            numericUpDown_conf_hor_res.Visible = false;
            numericUpDown_conf_vert_res.Visible = false;
            label_config_hor_res.Visible = false;
            label_config_vert_res.Visible = false;
            label_num_of_leds.Visible = false;
            pictureBox_monitor.Visible = false;

            label_first_LED_id.Visible = false;
            label_last_LED_id.Visible = false;
            flag_showLEDConfig_clicked = false;
            button_configApply.Visible = false;
            button_getDefault.Visible = false;
            groupBox_LEDconfig.Visible = false;
            groupBox_screenConfig.Visible = false;


            numericUpDown_led_left_first.Value = configuration.getConfig(ConfigTypes.led_id_left_first);
            numericUpDown_led_left_last.Value = configuration.getConfig(ConfigTypes.led_id_left_last);
            numericUpDown_led_bottom_first.Value = configuration.getConfig(ConfigTypes.led_id_bottom_first);
            numericUpDown_led_bottom_last.Value = configuration.getConfig(ConfigTypes.led_id_bottom_last);
            numericUpDown_led_right_first.Value = configuration.getConfig(ConfigTypes.led_id_right_first);
            numericUpDown_led_right_last.Value = configuration.getConfig(ConfigTypes.led_id_right_last);
            numericUpDown_led_top_first.Value = configuration.getConfig(ConfigTypes.led_id_top_first);
            numericUpDown_led_top_last.Value = configuration.getConfig(ConfigTypes.led_id_top_last);

            numericUpDown_conf_vert_res.Value = configuration.getConfig(ConfigTypes.screen_capt_vert_res);
            numericUpDown_conf_hor_res.Value = configuration.getConfig(ConfigTypes.screen_capt_hor_res);

            initCore();
            



        }
        /// <summary>
        /// everything in this method is initialized with adjustable parameters and with this section the core of the ambilight
        /// can be restarted. IMPORTANT: before you reinit the core, close the serial port (it requires a time, before it can be reopened)
        /// </summary>
        private void initCore()
        {
            number_of_leds = configuration.getConfig(ConfigTypes.number_of_leds);

            avgCalculator = new ColorAvgCalc_1_simple(num_of_boxes_vertical, num_of_boxes_horizontal);
            //thread_fillingColorBox = new Thread(fillingOneColorBox); //initialization of thread
            thread_fillingColorBox = new Thread(fillingMoreColorBox); //initialization of thread


            AssociatedLED_indices indices = new AssociatedLED_indices();
            indices.left_first = Convert.ToByte(configuration.getConfig(ConfigTypes.led_id_left_first));
            indices.left_last = Convert.ToByte(configuration.getConfig(ConfigTypes.led_id_left_last));
            indices.bottom_first = Convert.ToByte(configuration.getConfig(ConfigTypes.led_id_bottom_first));
            indices.bottom_last = Convert.ToByte(configuration.getConfig(ConfigTypes.led_id_bottom_last));
            indices.right_first = Convert.ToByte(configuration.getConfig(ConfigTypes.led_id_right_first));
            indices.right_last = Convert.ToByte(configuration.getConfig(ConfigTypes.led_id_right_last));
            indices.top_first = Convert.ToByte(configuration.getConfig(ConfigTypes.led_id_top_first));
            indices.top_last = Convert.ToByte(configuration.getConfig(ConfigTypes.led_id_top_last));

            converter = new CB2LP_converter_border_zoh_interpolation(Convert.ToByte(number_of_leds), indices, new int[2] { num_of_boxes_vertical, num_of_boxes_horizontal });
            setConnectionStatus(converter.getSerialOpenFlag());
            textBox_board.Text = "Aruino Nano w ATmega 328";

            //set maximum available values to LED config values
            numericUpDown_led_left_first.Maximum = number_of_leds - 1;
            numericUpDown_led_left_last.Maximum = number_of_leds - 1;
            numericUpDown_led_bottom_first.Maximum = number_of_leds - 1;
            numericUpDown_led_bottom_last.Maximum = number_of_leds - 1;
            numericUpDown_led_right_first.Maximum = number_of_leds - 1;
            numericUpDown_led_right_last.Maximum = number_of_leds - 1;
            numericUpDown_led_top_first.Maximum = number_of_leds - 1;
            numericUpDown_led_top_last.Maximum = number_of_leds - 1;
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

            Color[] colorArray = new Color[number_of_leds];
            for (byte i = 0; i < number_of_leds; i++)
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
            Color[] colorArray = new Color[number_of_leds];
            Color desiredColor = colorDialog1.Color;
            for (byte i = 0; i<number_of_leds; i++)
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

            for (byte i = 0; i < number_of_leds; i++)
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

        bool flag_showLEDConfig_clicked = false;
        private void button_showLEDConfig_Click(object sender, EventArgs e)
        {
            if (!flag_showLEDConfig_clicked)
            {
                numericUpDown_led_left_first.Visible = true;
                numericUpDown_led_left_last.Visible = true;
                numericUpDown_led_bottom_first.Visible = true;
                numericUpDown_led_bottom_last.Visible = true;
                numericUpDown_led_right_first.Visible = true;
                numericUpDown_led_right_last.Visible = true; 
                numericUpDown_led_top_first.Visible = true;
                numericUpDown_led_top_last.Visible = true;
                numericUpDown_num_of_leds.Visible = true;
                numericUpDown_conf_hor_res.Visible = true;
                numericUpDown_conf_vert_res.Visible = true;
                label_config_hor_res.Visible = true;
                label_config_vert_res.Visible = true;
                label_num_of_leds.Visible = true;
                pictureBox_monitor.Visible = true;
                flag_showLEDConfig_clicked = true;
                label_first_LED_id.Visible = true;
                label_last_LED_id.Visible = true;
                button_configApply.Visible = true;
                button_getDefault.Visible = true;
                groupBox_LEDconfig.Visible = true;
                groupBox_screenConfig.Visible = true;
            }
            else
            {

                numericUpDown_led_left_first.Visible = false;
                numericUpDown_led_left_last.Visible = false;
                numericUpDown_led_bottom_first.Visible = false;
                numericUpDown_led_bottom_last.Visible = false;
                numericUpDown_led_right_first.Visible = false;
                numericUpDown_led_right_last.Visible = false;
                numericUpDown_led_top_first.Visible = false;
                numericUpDown_led_top_last.Visible = false;
                numericUpDown_num_of_leds.Visible = false;
                numericUpDown_conf_hor_res.Visible = false;
                numericUpDown_conf_vert_res.Visible = false;
                label_config_hor_res.Visible = false;
                label_config_vert_res.Visible = false;
                label_num_of_leds.Visible = false;
                pictureBox_monitor.Visible = false;
                flag_showLEDConfig_clicked = false;
                label_first_LED_id.Visible = false;
                label_last_LED_id.Visible = false;
                button_configApply.Visible = false;
                button_getDefault.Visible = false;
                groupBox_LEDconfig.Visible = false;
                groupBox_screenConfig.Visible = false;
            }
            
        }

        private void button_configApply_Click(object sender, EventArgs e)
        {
            //set maximum available values to LED config values
            number_of_leds = Convert.ToInt32(numericUpDown_num_of_leds.Value);


            numericUpDown_led_left_first.Maximum = number_of_leds-1;
            numericUpDown_led_left_last.Maximum = number_of_leds - 1;
            numericUpDown_led_bottom_first.Maximum = number_of_leds - 1;
            numericUpDown_led_bottom_last.Maximum = number_of_leds - 1;
            numericUpDown_led_right_first.Maximum = number_of_leds - 1;
            numericUpDown_led_right_last.Maximum = number_of_leds - 1;
            numericUpDown_led_top_first.Maximum = number_of_leds - 1;
            numericUpDown_led_top_last.Maximum = number_of_leds - 1;

            numericUpDown_led_left_first.Value = applyThresholdValue(numericUpDown_led_left_first.Value);
            numericUpDown_led_left_last.Value = applyThresholdValue(numericUpDown_led_left_last.Value);
            numericUpDown_led_bottom_first.Value = applyThresholdValue(numericUpDown_led_bottom_first.Value);
            numericUpDown_led_bottom_last.Value = applyThresholdValue(numericUpDown_led_bottom_last.Value);
            numericUpDown_led_right_first.Value = applyThresholdValue(numericUpDown_led_right_first.Value);
            numericUpDown_led_right_last.Value = applyThresholdValue(numericUpDown_led_right_last.Value);
            numericUpDown_led_top_first.Value = applyThresholdValue(numericUpDown_led_top_first.Value);
            numericUpDown_led_top_last.Value = applyThresholdValue(numericUpDown_led_top_last.Value);


            configuration.addConfigValue(ConfigTypes.led_id_left_first, numericUpDown_led_left_first.Value);
            configuration.addConfigValue(ConfigTypes.led_id_left_last, numericUpDown_led_left_last.Value);
            configuration.addConfigValue(ConfigTypes.led_id_bottom_first, numericUpDown_led_bottom_first.Value);
            configuration.addConfigValue(ConfigTypes.led_id_bottom_last, numericUpDown_led_bottom_last.Value);
            configuration.addConfigValue(ConfigTypes.led_id_right_first, numericUpDown_led_right_first.Value);
            configuration.addConfigValue(ConfigTypes.led_id_right_last, numericUpDown_led_right_last.Value);
            configuration.addConfigValue(ConfigTypes.led_id_top_first, numericUpDown_led_top_first.Value);
            configuration.addConfigValue(ConfigTypes.led_id_top_last, numericUpDown_led_top_last.Value);
            configuration.addConfigValue(ConfigTypes.number_of_leds, numericUpDown_num_of_leds.Value);

            //screen config
            configuration.addConfigValue(ConfigTypes.screen_capt_vert_res, numericUpDown_conf_vert_res.Value);
            configuration.addConfigValue(ConfigTypes.screen_capt_hor_res, numericUpDown_conf_hor_res.Value);
            

            converter.disconnectSerial();
            initCore();
        }

        private decimal applyThresholdValue(decimal value)
        {
            if (value > number_of_leds)
            {
                value = number_of_leds-1;
            }
            return value;
        }

        private void button_getDefault_Click(object sender, EventArgs e)
        {
            configuration.setDefaultValues();
            number_of_leds = configuration.getConfig(ConfigTypes.number_of_leds);

            numericUpDown_led_left_first.Maximum = number_of_leds - 1;
            numericUpDown_led_left_last.Maximum = number_of_leds - 1;
            numericUpDown_led_bottom_first.Maximum = number_of_leds - 1;
            numericUpDown_led_bottom_last.Maximum = number_of_leds - 1;
            numericUpDown_led_right_first.Maximum = number_of_leds - 1;
            numericUpDown_led_right_last.Maximum = number_of_leds - 1;
            numericUpDown_led_top_first.Maximum = number_of_leds - 1;
            numericUpDown_led_top_last.Maximum = number_of_leds - 1;


            numericUpDown_led_left_first.Value = configuration.getConfig(ConfigTypes.led_id_left_first);
            numericUpDown_led_left_last.Value = configuration.getConfig(ConfigTypes.led_id_left_last);
            numericUpDown_led_bottom_first.Value = configuration.getConfig(ConfigTypes.led_id_bottom_first);
            numericUpDown_led_bottom_last.Value = configuration.getConfig(ConfigTypes.led_id_bottom_last);
            numericUpDown_led_right_first.Value = configuration.getConfig(ConfigTypes.led_id_right_first);
            numericUpDown_led_right_last.Value = configuration.getConfig(ConfigTypes.led_id_right_last);
            numericUpDown_led_top_first.Value = configuration.getConfig(ConfigTypes.led_id_top_first);
            numericUpDown_led_top_last.Value = configuration.getConfig(ConfigTypes.led_id_top_last);
            numericUpDown_num_of_leds.Value = configuration.getConfig(ConfigTypes.number_of_leds);

            //screen values
            numericUpDown_conf_vert_res.Value = configuration.getConfig(ConfigTypes.screen_capt_vert_res);
            numericUpDown_conf_hor_res.Value = configuration.getConfig(ConfigTypes.screen_capt_hor_res);
        }

        bool flag_button_show_ScreenCapturer_clicked = false;

        private void button_show_ScreenCapturer_conf_Click(object sender, EventArgs e)
        {
            if (!flag_button_show_ScreenCapturer_clicked)
            {
                numericUpDown_conf_hor_res.Visible = true;
                numericUpDown_conf_vert_res.Visible = true;
                label_config_hor_res.Visible = true;
                label_config_vert_res.Visible = true;
                button_configApply.Visible = true;
                button_getDefault.Visible = true;

                flag_button_show_ScreenCapturer_clicked = true;
            }
            else
            {
                numericUpDown_conf_hor_res.Visible = false;
                numericUpDown_conf_vert_res.Visible = false;
                label_config_hor_res.Visible = false;
                label_config_vert_res.Visible = false;
                button_configApply.Visible = false;
                button_getDefault.Visible = false;

                flag_button_show_ScreenCapturer_clicked = false;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Drawing;

namespace AmbientLight_ColorProfileCreator_for_Windows
{
    public class SerialInterface
    {
        private SerialPort serialPort;

        private byte[] data = new byte[8];
        private byte[] msg;
        private bool isDeviceAvailable;

        public SerialInterface()
        {
            logger.add(LogTypes.SerialPort, "initializing started");
            isDeviceAvailable = false;
            serialPort = new SerialPort();
            serialPort.PortName = FindPerfectSerialPort();
            serialPort.BaudRate = 19200;
            serialPort.DataBits = 8;
            serialPort.Parity = Parity.None;
            serialPort.StopBits = StopBits.One;
            if (isDeviceAvailable)
            {
                OpenPort();
                logger.add(LogTypes.SerialPort, "serial port is initialized");
            }
            else
            {
                logger.add(LogTypes.SerialPort, "serial communication cannot be initialized, because of the lack of port");
            }
        }
        private string FindPerfectSerialPort()
        {
            string[] availablePorts = SerialPort.GetPortNames();

            string log_entry = "available ports: ";
            foreach (string name in availablePorts)
                log_entry += " " + name + " ";
            logger.add(LogTypes.SerialPort, log_entry);
            if (availablePorts.GetLength(0) > 1)
            {
                isDeviceAvailable = true;
                log_entry = "selected port: " + availablePorts[1];
                logger.add(LogTypes.SerialPort, log_entry);
                return availablePorts[1];
            }
            else
            {
                isDeviceAvailable = false;
                logger.add(LogTypes.SerialPort, "serial port not found");
                return " ";
            }
            
            
            
        }

        public bool OpenPort()
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
                logger.add(LogTypes.SerialPort, "serial port is reopened");
            }
            serialPort.DataReceived += new SerialDataReceivedEventHandler(HandleIncomingData);
            serialPort.Open();
            serialPort.DtrEnable = true;
            logger.add(LogTypes.SerialPort, "Port is open");
            return serialPort.IsOpen;

        }

        private void HandleIncomingData(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] bytes = new byte[8];
            serialPort.Read(bytes, 0, 8);
        }

        public void ClosePort()
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
            logger.add(LogTypes.SerialPort, "Port is closed");
        }

        public bool WriteMsg(string msg)
        {
            if (serialPort.IsOpen)
            {
                serialPort.WriteLine(msg);
                return true;
            }
            else
            {
                return false;
            }
                

        }


        /// <summary>
        /// Method for sending byte-vector to the serial line. 
        /// </summary>
        private bool WriteBytes()
        {
            string[] str = SerialPort.GetPortNames();
            if (serialPort.IsOpen && str.Contains(serialPort.PortName))
            {
                try {
                    serialPort.Write(data, 0, 8); //8, because this is the lenght of the waited signal in the arduino
                    return true;
                }
                catch(Exception e)
                {
                    logger.add(LogTypes.SerialPort, "<exception handled> : IOExeption (the port is get dead during writing) Description: ");
                    logger.add(LogTypes.SerialPort, e.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// method for linearization of the RGB values. A LED is not a linear device, therefore, there are a nonlinearity between
        /// sent and seen light values. The nonlinearity can be approximated with third root function. Thus, it mostly can be compensated with
        /// third power function. This method normalize the value to [0,1], calculate the third power, and denormalize it to [0,255] interval.
        /// Thus the sent value and th seen value are proportional.
        /// </summary>
        /// <param name="value_b">original value</param>
        /// <returns>linearized value</returns>
        private byte linearizator(byte value_b)
        {
            double value_f = Convert.ToDouble(value_b);
            value_f /= 255;
            value_f = Math.Pow(value_f, 3) * 254;
            return Convert.ToByte(value_f);
        }

        /// <summary>
        /// Method for sending indicator before starting the streaming og RGB codes
        /// </summary>
        public bool sendIndicatorOfRGBstream()
        {
            if (serialPort.IsOpen)
            {
                data[0] = 255;
                WriteBytes();
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Method for encoding and sending RGB codes for two specified LED on the strip
        /// </summary>
        /// <param name="id1">first LED's ID</param>
        /// <param name="id2"></param>
        /// <param name="R1"></param>
        /// <param name="G1"></param>
        /// <param name="B1"></param>
        /// <param name="R2"></param>
        /// <param name="G2"></param>
        /// <param name="B2"></param>
        public bool sendRGBcodes(byte id1, byte id2, byte R1, byte G1, byte B1, byte R2, byte G2, byte B2)
        {
            data[0] = id1;
            data[1] = id2;
            data[2] = R1;
            data[3] = G1;
            data[4] = B1;
            data[5] = R2;
            data[6] = G2;
            data[7] = B2;
            return WriteBytes();
        }
        
        /// <summary>
        /// Method for sending the color array, which associated to each led, to the Arduino
        /// </summary>
        /// <param name="colorArray"></param>
        /// <returns></returns>
        public bool sendColorArray(Color[] colorArray)
        {
            bool retVal = false;
            sendIndicatorOfRGBstream();
            for (byte led_id = 0; led_id < colorArray.GetLength(0); led_id += 2)
            {

                data[0] = led_id;
                
                data[2] = linearizator(colorArray[led_id].R);
                data[3] = linearizator(colorArray[led_id].G);
                data[4] = linearizator(colorArray[led_id].B);
                if ((led_id + 1) < colorArray.GetLength(0))
                {
                    data[1] = Convert.ToByte(led_id + 1);
                    data[5] = linearizator(colorArray[led_id + 1].R);
                    data[6] = linearizator(colorArray[led_id + 1].G);
                    data[7] = linearizator(colorArray[led_id + 1].B);
                }
                else
                {
                    data[1] = Convert.ToByte(led_id + 1);
                    data[5] = 0;
                    data[6] = 0;
                    data[7] = 0;
                }
                retVal = WriteBytes();

            }
            return retVal;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace AmbientLight_ColorProfileCreator_for_Windows
{

    public enum ConfigTypes
    {
        led_id_left_first = 0,
        led_id_left_last = 1,
        led_id_bottom_first = 2,
        led_id_bottom_last = 3,
        led_id_right_first = 4,
        led_id_right_last = 5,
        led_id_top_first = 6,
        led_id_top_last = 7,
        number_of_leds = 8,
    }
    public class ConfigurationManagger
    {
        Dictionary<ConfigTypes, int> configs = new Dictionary<ConfigTypes, int>();


        public ConfigurationManagger()
        {
            configs.Add(ConfigTypes.led_id_left_first, 0);
            configs.Add(ConfigTypes.led_id_left_last, 0);
            configs.Add(ConfigTypes.led_id_bottom_first, 0);
            configs.Add(ConfigTypes.led_id_bottom_last, 0);
            configs.Add(ConfigTypes.led_id_right_first, 0);
            configs.Add(ConfigTypes.led_id_right_last, 0);
            configs.Add(ConfigTypes.led_id_top_first, 0);
            configs.Add(ConfigTypes.led_id_top_last, 0);
            configs.Add(ConfigTypes.number_of_leds, 0);
            
            //saveConfig();
            loadConfig();
        }

        public void addConfigValue(ConfigTypes configtype, int value)
        {
            configs.Add(configtype, value);
            saveConfig();
        }

        public void setDefaultValues()
        {
            configs[ConfigTypes.led_id_left_first] = 0;
            configs[ConfigTypes.led_id_left_last] = 1;
            configs[ConfigTypes.led_id_bottom_first] = 2;
            configs[ConfigTypes.led_id_bottom_last] = 14;
            configs[ConfigTypes.led_id_right_first] = 15;
            configs[ConfigTypes.led_id_right_last] = 16;
            configs[ConfigTypes.led_id_top_first] = 17;
            configs[ConfigTypes.led_id_top_last] = 29;


            configs[ConfigTypes.number_of_leds] = 30;
            saveConfig();
        }

        public void saveConfig()
        {
            string[] configFileLines = new string[configs.Count];
            int line_id = 0;
            foreach(KeyValuePair<ConfigTypes,int> element in configs)
            {
                configFileLines[line_id] = element.ToString();
                line_id++;
            }

            File.WriteAllLines("config.txt", configFileLines);


        }

        private void loadConfig()
        {
            Dictionary<ConfigTypes, int> new_configs = new Dictionary<ConfigTypes, int>();
            string[] configFileLines = File.ReadAllLines("config.txt");
            string configName;
            string configParam_str;
            int configParam;
            int char_id = 0;
            foreach(string line in configFileLines)
            {

                configName = "";
                char_id = 0;
                while ((char_id < line.Length) && ((line[char_id] != Convert.ToChar(",")) && (line[char_id] != Convert.ToChar("]"))))
                {
                    if (line[char_id] != Convert.ToChar("["))
                    {
                        configName += line[char_id];
                    }
                    char_id++;
                }

                configParam_str = "";
                configParam = 0;
                while ((char_id < line.Length) && (line[char_id] != Convert.ToChar("]")))
                {
                    if ((line[char_id] != Convert.ToChar(",")) && (line[char_id] != Convert.ToChar(" ")))
                    {
                        configParam_str += line[char_id];
                    }
                    char_id++;
                }
                configParam = Int32.Parse(configParam_str);

                foreach(KeyValuePair<ConfigTypes, int> configPair in configs)
                {
                    if (configName == configPair.Key.ToString())
                    {
                        new_configs[configPair.Key] = configParam;
                    }
                }                

            }
            configs = new_configs;

            logger.add(LogTypes.Config, "config file parsered. Found the next parameters:");
            foreach (KeyValuePair<ConfigTypes, int> configPair in configs)
            {
                logger.add(LogTypes.Config, configPair.ToString());
            }
        }

    }
}

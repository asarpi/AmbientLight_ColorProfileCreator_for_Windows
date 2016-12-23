using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace AmbientLight_ColorProfileCreator_for_Windows
{
    public abstract class ScreenInterface
    {
        private int resolution_vertical;
        private int resolution_horizontal;
        protected ColorCapture colorCapture;
        private AvgColorCalculator colorCalculator;
        private Color[,] colorMatrix;
        private Color[] colorArray;

        public ScreenInterface (int res_vertical, int res_horizontal)
        {
            resolution_vertical = res_vertical;
            resolution_horizontal = res_horizontal;
            colorCapture = new ColorCapture(resolution_vertical, resolution_horizontal);
            logger.add(LogTypes.ColorAvgCalculator, "colorCapture initialized");

        }



        public Color[] getRawColors()
        {
            return colorCapture.getColor();
        }
        public abstract Color[] getAvgColor();

        public Color[,] getColorMatrix()
        {
            colorArray = getRawColors();
            foreach (Color c in colorArray)
            {
                for (int id_box_vertical = 0; id_box_vertical < resolution_vertical; id_box_vertical++)
                {
                    for (int id_box_horizontal = 0; id_box_horizontal < resolution_horizontal; id_box_horizontal++)
                    {
                        colorMatrix[id_box_vertical, id_box_horizontal] = colorArray[id_box_vertical * resolution_horizontal + id_box_horizontal];
                    }
                }
            }

            return colorMatrix;
        }
        
    }
}

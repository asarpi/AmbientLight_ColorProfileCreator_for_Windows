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
        protected ColorCapture colorCapture;
        private AvgColorCalculator colorCalculator;

        public ScreenInterface (int res_vertical, int res_horizontal)
        {
            colorCapture = new ColorCapture(res_vertical, res_horizontal);
            logger.add(LogTypes.ColorAvgCalculator, "colorCapture initialized");

        }



        public Color[] getRawColors()
        {
            return colorCapture.getColor();
        }
        public abstract Color[] getAvgColor();
        
    }
}

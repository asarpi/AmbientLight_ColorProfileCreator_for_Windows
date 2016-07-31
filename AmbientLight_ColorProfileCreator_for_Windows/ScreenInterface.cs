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

        public ScreenInterface ()
        {
            colorCapture = new ColorCapture();
            logger.add(LogTypes.ColorAvgCalculator, "colorCapture initialized");

        }



        public Color[] getRawColors()
        {
            return colorCapture.getColor();
        }
        public abstract Color[] getAvgColor();
        
    }
}

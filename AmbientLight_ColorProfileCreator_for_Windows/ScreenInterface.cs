using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace AmbientLight_ColorProfileCreator_for_Windows
{
    public class ScreenInterface
    {
        private ColorCapture colorCapture;
        private AvgColorCalculator colorCalculator;

        public ScreenInterface ()
        {

        }

        public Color getAvgColor()
        {
            return Color.Black;
        }

        public int getAvgColor()
        {
            return 5;
            //return new Color[] { Color.Black, Color.Black};

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace AmbientLight_ColorProfileCreator_for_Windows
{
    public abstract class ColorAvgCalc_one_out: AvgColorCalculator
    {
        protected Color avgColor1 = Color.Black;

        public override Color[] getAvgColor()
        {
            return new Color[] { avgColor1 };
        }

    
    }
}

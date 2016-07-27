using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace AmbientLight_ColorProfileCreator_for_Windows
{
    public class ColorAvgCalc_1_simple: ColorAvgCalc_one_out
    {

        protected override void createAvgBaseMatrix()
        {
            avgBaseMatrix = new int[1, 1];

        }

        protected override void calcAvgColor()
        {
            avgColor1 = Color.Purple;
        }

    }
}

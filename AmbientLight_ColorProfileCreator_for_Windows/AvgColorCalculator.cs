using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace AmbientLight_ColorProfileCreator_for_Windows
{
    public abstract class AvgColorCalculator: ScreenInterface
    {


        protected int[,] avgBaseMatrix;
        protected int[,] avgBaseMatrix_weights;
        protected Color[] rawColors;


        protected abstract void createAvgBaseMatrix();
        
        protected abstract void calcAvgColor();
        protected void getScreenMatrix()
        {
            rawColors = getRawColors();
        }

        public override abstract Color[] getAvgColor();
        



    }
}

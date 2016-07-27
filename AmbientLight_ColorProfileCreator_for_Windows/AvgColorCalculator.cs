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
        protected Color[] avgColor = new Color[] { Color.Black };


        protected abstract void createAvgBaseMatrix();
        protected abstract void getScreenMatrix();
        protected abstract void calcAvgColor();


        public override Color[] getAvgColor()
        {
            return avgColor;
        }




    }
}

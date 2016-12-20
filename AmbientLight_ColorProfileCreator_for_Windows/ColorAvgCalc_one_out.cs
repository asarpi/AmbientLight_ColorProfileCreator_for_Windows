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
        protected long color_R, color_G, color_B;

        /// <summary>
        /// initialization of base class' init method with given parameters
        /// </summary>
        /// <param name="res_vert"></param>
        /// <param name="res_hor"></param>
        public ColorAvgCalc_one_out(int res_vert, int res_hor) : base(res_vert, res_hor) { }
        
        /// <summary>
        /// Interface method for getting average color. 
        /// Calculate derived class dependent calc module and return calculated Color
        /// </summary>
        /// <returns></returns>
        public override Color[] getAvgColor()
        {
            calcAvgColor(); 
            return new Color[] { avgColor1 };
        }

        
    }
}

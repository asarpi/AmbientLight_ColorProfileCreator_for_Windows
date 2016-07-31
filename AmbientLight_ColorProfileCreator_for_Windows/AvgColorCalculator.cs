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
        protected int captured_vertical_res;
        protected int captured_horizontal_res;
        protected int[,] avgBaseMatrix_weights;
        protected Color[] rawColors;

        /// <summary>
        /// Abstract method for creatind base matrix for average calculations. Each element of matrix associated to an element of captured matrix.
        /// The matrix contains the weights of elements of captured matrix. 
        /// This method must be in init section, becaouse the matrix must be completed before first calculation runs.
        /// </summary>
        protected abstract void createAvgBaseMatrix();
        
        /// <summary>
        /// Abstract method for average color's calculation.
        /// </summary>
        protected abstract void calcAvgColor();

        /// <summary>
        /// Interface method for get captured resolution from colorCapture
        /// </summary>
        /// <returns></returns>
        public int[] getCapturedResolution()
        {
            captured_vertical_res = colorCapture.getResolution()[0];
            captured_horizontal_res = colorCapture.getResolution()[1];
            return new int[] { captured_vertical_res, captured_horizontal_res };
        }

        /// <summary>
        /// Abstract method for public interface of getting average colors
        /// </summary>
        /// <returns></returns>
        public override abstract Color[] getAvgColor();

        /// <summary>
        /// init function
        /// </summary>
        public AvgColorCalculator()
        {
            getCapturedResolution();
            createAvgBaseMatrix();
            logger.add(LogTypes.ColorAvgCalculator, "AvgColorCalculator initialized");
        }
        



    }
}

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
        public ColorAvgCalc_1_simple(int resolution_vertical, int resolution_horizontal) : base(resolution_vertical, resolution_horizontal) { }
        //simple average, every elements of matrix are 1
        protected override void createAvgBaseMatrix()
        {
            
            avgBaseMatrix = new int[captured_vertical_res, captured_horizontal_res];
            for (int i = 0; i < captured_horizontal_res; i++)
            {
                for (int j = 0; j < captured_vertical_res; j++)
                {
                    avgBaseMatrix[i, j] = 1;
                }
            }
            logger.add(LogTypes.ColorAvgCalculator, "base matrix created for simple average calculation");

        }

        //simple average of captured colors
        protected override void calcAvgColor()
        {
            color_R = color_G = color_B = 0;
            for (int rect_id_vert = 0; rect_id_vert< captured_horizontal_res; rect_id_vert++)
            {
                for (int rect_id_hor = 0; rect_id_hor < captured_vertical_res; rect_id_hor++)
                {
                    rawColors = colorCapture.getColor();

                    color_R += rawColors[rect_id_vert * captured_vertical_res + rect_id_hor].R;
                    color_G += rawColors[rect_id_vert * captured_vertical_res + rect_id_hor].G;
                    color_B += rawColors[rect_id_vert * captured_vertical_res + rect_id_hor].B;

                }
            }
            color_R /= (captured_vertical_res * captured_horizontal_res);
            color_G /= (captured_vertical_res * captured_horizontal_res);
            color_B /= (captured_vertical_res * captured_horizontal_res);
            avgColor1 = Color.FromArgb(Convert.ToInt32(color_R ),
                                       Convert.ToInt32(color_G ),
                                       Convert.ToInt32(color_B ));

        }
    }
}

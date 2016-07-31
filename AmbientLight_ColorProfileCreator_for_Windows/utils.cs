using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientLight_ColorProfileCreator_for_Windows
{
    public class utils
    {
        public static int threshold_int(object variable, int min, int max)
        {
            int result = Convert.ToInt32(variable);
            if (result >= max)
                result = max;
            if (result <= min)
                result = min;
            return result;
        }
    }


}

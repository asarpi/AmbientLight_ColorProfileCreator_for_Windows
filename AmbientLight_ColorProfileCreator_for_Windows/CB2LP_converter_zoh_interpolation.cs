using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientLight_ColorProfileCreator_for_Windows
{
    public class CB2LP_converter_border_zoh_interpolation : ColorBoxes2LEDPixelsConverter
    {
        private struct associatedLEDstoBoxes
        {
            // TODO
        }
        public CB2LP_converter_border_zoh_interpolation(byte num_of_leds, AssociatedLED_indices indices, int[] captured_box_num) : base(num_of_leds, indices, captured_box_num) { }

        public override void fillAssociatedLEDs()
        {
            
        }

    }
}

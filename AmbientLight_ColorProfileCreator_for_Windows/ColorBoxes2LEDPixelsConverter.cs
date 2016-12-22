using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace AmbientLight_ColorProfileCreator_for_Windows
{
    public struct AssociatedLED_indices
    {
        public byte left_first;
        public byte left_last;
        public byte bottom_first;
        public byte bottom_last;
        public byte right_first;
        public byte right_last;
        public byte top_first;
        public byte top_last;
    }

    public struct displayedColorValues
    {
        public Rectangle[,] rects;
        public SolidBrush[,] rectBrushes;
    }



    public abstract class ColorBoxes2LEDPixelsConverter
    {
        /***** VARIABLE DEFINITIONS *****/
        protected byte num_of_LEDs = 30;
        protected AssociatedLED_indices LED_indices;
        protected Color[,] colorMatrix;
        protected int[] num_of_captured_boxes;
        protected Color[] ledColors;
        private int real_led_id;


        protected struct AssociatedLEDs
        {
            public Color[] left;
            public Color[] bottom;
            public Color[] right;
            public Color[] top;
        }
        protected AssociatedLEDs associatedLEDs;


        // Struct for store which boxes are associated to wich LEDs. Elements are divided to left, bottom, right and top sides.
        // Each element is an array of LinkedLists, where the elements (LinkedLists) are associated to the physical LEDs. A LinkedList conatins
        // each associated boxes with their coordinates in the captured grid.
        // The filling method is case specific, thus its implementation must be in the derived class.
        protected struct AssociatedColorBoxesLists
        {
            public LinkedList<utils.ColRowRepresentation>[] left;
            public LinkedList<utils.ColRowRepresentation>[] bottom;
            public LinkedList<utils.ColRowRepresentation>[] right;
            public LinkedList<utils.ColRowRepresentation>[] top;
        }
        protected AssociatedColorBoxesLists associatedColorBoxesLits;

        /***** PUBLIC FUNCTIONS *****/

        public ColorBoxes2LEDPixelsConverter(byte num_of_leds, AssociatedLED_indices indices, int[] captured_box_num)
        {
            num_of_LEDs = num_of_leds;
            LED_indices = indices;
            num_of_captured_boxes = captured_box_num;
            colorMatrix = new Color[num_of_captured_boxes[0], num_of_captured_boxes[1]];

            associatedLEDs = new AssociatedLEDs();
            // LED array : new Color[number of contained LEDs]
            associatedLEDs.left =   new Color[LED_indices.left_last   - LED_indices.left_first + 1];
            associatedLEDs.bottom = new Color[LED_indices.bottom_last - LED_indices.bottom_first + 1];
            associatedLEDs.right =  new Color[LED_indices.right_last  - LED_indices.right_first + 1];
            associatedLEDs.top =    new Color[LED_indices.top_last    - LED_indices.top_first + 1];

            createAssociatedColorBoxesList();

        }

        /***** ABSTRACT FUNCTIONS ****/

        public abstract void fillAssociatedLEDs(Color[] colorArray);

        protected abstract Color calcAvgColorFromColorList(LinkedList<utils.ColRowRepresentation> colorBoxCoordinates);

        protected abstract void createAssociatedColorBoxesList();
        /***** PRIVATE FUNCTIONS ****/
        protected void createColorMatrix(Color[] colorArray)
        {
            foreach (Color c in colorArray)
            {
                for (int id_box_vertical = 0; id_box_vertical < num_of_captured_boxes[0]; id_box_vertical++)
                {
                    for (int id_box_horizontal = 0; id_box_horizontal < num_of_captured_boxes[1]; id_box_horizontal++)
                    {
                        colorMatrix[id_box_vertical, id_box_horizontal] = colorArray[id_box_vertical * num_of_captured_boxes[1] + id_box_horizontal];
                    }
                }
            }
        }

        /**** PUBLIC FUNCTIONS ****/
        public Color[] fillLEDarray(Color[] capturedColorArray)
        {
            fillAssociatedLEDs(capturedColorArray);
            ledColors = new Color[num_of_LEDs];
            real_led_id = 0;
            //left
            for (int led_id = 0; led_id < associatedLEDs.left.GetLength(0); led_id++)
            {
                ledColors[real_led_id] = associatedLEDs.left[led_id];
                real_led_id++;
            }
            //bottom
            for (int led_id = 0; led_id < associatedLEDs.bottom.GetLength(0); led_id++)
            {
                ledColors[real_led_id] = associatedLEDs.bottom[led_id];
                real_led_id++;
            }
            //right (reversed direction, because the grid indices are increasing in the generated list)
            for (int led_id = associatedLEDs.right.GetLength(0) -1; led_id >= 0; led_id--)
            {
                ledColors[real_led_id] = associatedLEDs.right[led_id];
                real_led_id++;
            }
            //top (reversed direction, because the grid indices are increasing in the generated list)
            for (int led_id = associatedLEDs.top.GetLength(0) - 1; led_id >= 0; led_id--)
            {
                ledColors[real_led_id] = associatedLEDs.top[led_id];
                real_led_id++;
            }

            return ledColors;
        }


        /*** PUBLIC GET AND DISPLAY METHODS ***/
        
        public int getNumOfLeds()
        {
            return Convert.ToInt32(num_of_LEDs);
        }

        public Color[,] getColorMatrix()
        {
            return colorMatrix;
        }
            
        public displayedColorValues createDisplayableBoxes(int pos_x, int pos_y, int width, int height)
        {
            displayedColorValues boxes = new displayedColorValues();
            boxes.rects = new Rectangle[num_of_captured_boxes[0], num_of_captured_boxes[1]];
            boxes.rectBrushes = new SolidBrush[num_of_captured_boxes[0], num_of_captured_boxes[1]];

            for (int id_vert = 0; id_vert < num_of_captured_boxes[0]; id_vert++)
            {
                for (int id_hor = 0; id_hor < num_of_captured_boxes[1]; id_hor++)
                {
                    boxes.rects[id_vert, id_hor] = new Rectangle(pos_x + (id_hor * width) + 5 , pos_y + (id_vert * height) + 5, width, height);
                    boxes.rectBrushes[id_vert, id_hor] = new SolidBrush(colorMatrix[id_vert, id_hor]);
                }
            }

            return boxes;
        }

        public displayedColorValues displayLEDstripColors(int pos_x, int pos_y, int width, int height)
        {
            displayedColorValues boxes = new displayedColorValues();
            boxes.rects = new Rectangle[num_of_LEDs, 1];
            boxes.rectBrushes = new SolidBrush[num_of_LEDs, 1];

            for (int led_id = 0; led_id < num_of_LEDs; led_id++)
            {
                boxes.rects[led_id, 0] = new Rectangle(pos_x + (led_id * width) + 10, pos_y + 5, width, height);
                boxes.rectBrushes[led_id, 0] = new SolidBrush(ledColors[led_id]);
            }

            return boxes;
        }

    }
}

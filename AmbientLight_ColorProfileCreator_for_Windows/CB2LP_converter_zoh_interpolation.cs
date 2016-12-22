using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace AmbientLight_ColorProfileCreator_for_Windows
{
    public class CB2LP_converter_border_zoh_interpolation : ColorBoxes2LEDPixelsConverter
    {
        
        
        public CB2LP_converter_border_zoh_interpolation(byte num_of_leds, AssociatedLED_indices indices, int[] captured_box_num) : base(num_of_leds, indices, captured_box_num)
        {
            
        }

        protected override void createAssociatedColorBoxesList()
        {
            int number_of_leds; 
            int number_of_boxes;
            

            associatedColorBoxesLits = new AssociatedColorBoxesLists();
            associatedColorBoxesLits.left = new LinkedList<utils.ColRowRepresentation>[associatedLEDs.left.GetLength(0)];
            associatedColorBoxesLits.bottom= new LinkedList<utils.ColRowRepresentation>[associatedLEDs.bottom.GetLength(0)];
            associatedColorBoxesLits.right = new LinkedList<utils.ColRowRepresentation>[associatedLEDs.right.GetLength(0)];
            associatedColorBoxesLits.top = new LinkedList<utils.ColRowRepresentation>[associatedLEDs.top.GetLength(0)];

            /*** left side association ***/
            number_of_leds = associatedLEDs.left.GetLength(0);
            number_of_boxes = num_of_captured_boxes[1]; //rows
            // create the list of coordinates of associated boxes 
            //      (N x 2 matrix, where for example A[0] = {1,2}  means the 0-th associated box is in the 1-st column and 2-nd row in the captured grid)
            int[,] associated_box_array_to_left_side = new int[number_of_boxes, 2];
            for (int i = 0; i < number_of_boxes; i++)
            {
                associated_box_array_to_left_side[i, 0] = i; // all rows
                associated_box_array_to_left_side[i, 1] = 0; //first column, because it is the left side
            }
            // TODO
            if (number_of_leds < number_of_boxes)
                associatedColorBoxesLits.left = associateBoxesToLEDs(number_of_leds, number_of_boxes, associated_box_array_to_left_side);
            else
                associatedColorBoxesLits.left = associateLEDstoBoxes(number_of_leds, number_of_boxes, associated_box_array_to_left_side);

            /*** bottom side association ***/
            number_of_leds = associatedLEDs.bottom.GetLength(0);
            number_of_boxes = num_of_captured_boxes[0]; //columns
            // create the list of coordinates of associated boxes 
            //      (N x 2 matrix, where for example A[0] = {1,2}  means the 0-th associated box is in the 1-st column and 2-nd row in the captured grid)
            int[,] associated_box_array_to_bottom_side = new int[number_of_boxes, 2];
            for (int i = 0; i < number_of_boxes; i++)
            {
                associated_box_array_to_bottom_side[i, 0] = num_of_captured_boxes[1] - 1; // last row (number of rows - 1), because it is the bottom side
                associated_box_array_to_bottom_side[i, 1] = i; // all columns
            }

            if (number_of_leds < number_of_boxes)
                associatedColorBoxesLits.bottom = associateBoxesToLEDs(number_of_leds, number_of_boxes, associated_box_array_to_bottom_side);
            else
                associatedColorBoxesLits.bottom = associateLEDstoBoxes(number_of_leds, number_of_boxes, associated_box_array_to_bottom_side);

            /*** right side association ***/
            number_of_leds = associatedLEDs.right.GetLength(0);
            number_of_boxes = num_of_captured_boxes[1]; //rows
            // create the list of coordinates of associated boxes 
            //      (N x 2 matrix, where for example A[0] = {1,2}  means the 0-th associated box is in the 1-st column and 2-nd row in the captured grid)
            int[,] associated_box_array_to_right_side = new int[number_of_boxes, 2];
            for (int i = 0; i < number_of_boxes; i++)
            {
                associated_box_array_to_right_side[i, 0] = i; // all rows
                associated_box_array_to_right_side[i, 1] = num_of_captured_boxes[0] - 1; // last column (numOfColumns - 1) , because it is the right side of the grid
            }

            if (number_of_leds < number_of_boxes)
                associatedColorBoxesLits.right = associateBoxesToLEDs(number_of_leds, number_of_boxes, associated_box_array_to_right_side);
            else
                associatedColorBoxesLits.right = associateLEDstoBoxes(number_of_leds, number_of_boxes, associated_box_array_to_right_side);


            /*** top side association ***/
            number_of_leds = associatedLEDs.top.GetLength(0);
            number_of_boxes = num_of_captured_boxes[0]; //columns
            // create the list of coordinates of associated boxes 
            //      (N x 2 matrix, where for example A[0] = {1,2}  means the 0-th associated box is in the 1-st column and 2-nd row in the captured grid)
            int[,] associated_box_array_to_top_side = new int[number_of_boxes, 2];
            for (int i = 0; i < number_of_boxes; i++)
            {
                associated_box_array_to_top_side[i, 0] = 0; // first row, because it is the top of the grid
                associated_box_array_to_top_side[i, 1] = i; // all columns
            }


            if (number_of_leds < number_of_boxes)
                associatedColorBoxesLits.top = associateBoxesToLEDs(number_of_leds, number_of_boxes, associated_box_array_to_top_side);
            else
                associatedColorBoxesLits.top = associateLEDstoBoxes(number_of_leds, number_of_boxes, associated_box_array_to_top_side);


            createLog(associated_box_array_to_left_side, associated_box_array_to_bottom_side, associated_box_array_to_right_side, associated_box_array_to_top_side);
        }

        /// <summary>
        /// Method for associating the captured boxes to LEDs on the LEDstrip if the number of boxes are greater than available LEDs
        /// </summary>
        /// <param name="num_of_leds"></param>
        /// <param name="num_of_boxes"></param>
        /// <returns></returns>
        private LinkedList<utils.ColRowRepresentation>[] associateBoxesToLEDs(int num_of_leds, int num_of_boxes, int[,] selected_box_array)
        {
            //int num_of_possible_associated_boxes = Convert.ToInt32(num_of_leds / num_of_boxes);
            int num_of_possible_associated_boxes = Convert.ToInt32(num_of_boxes / num_of_leds);
            utils.ColRowRepresentation boxCoordinates = new utils.ColRowRepresentation();
                        
            LinkedList<utils.ColRowRepresentation>[] associatedBoxes = new LinkedList<utils.ColRowRepresentation>[num_of_leds];


            int associatedNum = 0; //number of already associated boxes to a LED 
            int slotNum = 0; //index of the LED's "slot", where will be the associated boxes

            for (int box_i = 0; box_i < num_of_boxes; box_i++)
            {
                // if associated number of boxes are less than the possible number of association to a LED
                if (associatedNum < num_of_possible_associated_boxes)
                {
                    // is this is the first associated box --> initialize the new LED's slot
                    if (associatedNum == 0)
                        associatedBoxes[slotNum] = new LinkedList<utils.ColRowRepresentation>();
                    // store box's coordinates
                    boxCoordinates.row = selected_box_array[box_i, 0];
                    boxCoordinates.column = selected_box_array[box_i, 1]; // in this case, the border columns are associated to the LEDs. Therefore, the first column is corresponding to the left side LEDs
                    associatedBoxes[slotNum].AddLast(boxCoordinates);
                    associatedNum++;
                }
                // if associated number of boxes are greater than the enabled number, there are two possibilities
                else
                {
                    // if the slotNum is the maximum, the remained boxes will be associated to the last LED
                    if (slotNum == num_of_leds - 1)
                    {
                        //store box's coordinates
                        boxCoordinates.row = selected_box_array[box_i, 0];
                        boxCoordinates.column = selected_box_array[box_i, 1];
                        associatedBoxes[slotNum].AddLast(boxCoordinates);
                    }
                    // if slotNum is less than the maximum, step to the next one
                    else
                    {
                        // reset number of associated boxes and increase the slot's index
                        slotNum++;
                        if (slotNum < num_of_leds)
                        {
                            //store box's coordinates to the new slot
                            associatedBoxes[slotNum] = new LinkedList<utils.ColRowRepresentation>();
                            boxCoordinates.row = selected_box_array[box_i, 0];
                            boxCoordinates.column = selected_box_array[box_i, 1];
                            associatedBoxes[slotNum].AddLast(boxCoordinates);

                            associatedNum = 1;
                        }
                    }

                }


            }

            return associatedBoxes;
        }

        /// <summary>
        /// Method for associating the captured Boxes to LEDs, if the number of LEDs are greater than the number of boxes.
        /// This method is the inversion solution of the associateBoxesToLEDs method.
        /// </summary>
        /// <param name="num_of_leds"></param>
        /// <param name="num_of_boxes"></param>
        /// <param name="selected_box_array"></param>
        /// <returns></returns>
        private LinkedList<utils.ColRowRepresentation>[] associateLEDstoBoxes(int num_of_leds, int num_of_boxes, int[,] selected_box_array)
        {
            // get, how many LEDs can be associated to a captured box
            int num_of_possible_associated_LEDs = Convert.ToInt32(num_of_boxes / num_of_leds);

            // init conatainers of associated boxes
            utils.ColRowRepresentation boxCoordinates = new utils.ColRowRepresentation();
            LinkedList<utils.ColRowRepresentation>[] associatedBoxes = new LinkedList<utils.ColRowRepresentation>[num_of_leds];

            int associatedNum = 0; //number of how many LEDs are associated to a box
            int boxNum = 0; //index of associated box
            

            for (int led_id = 0; led_id < num_of_leds; led_id++)
            {
                // initializate the new slot
                associatedBoxes[led_id] = new LinkedList<utils.ColRowRepresentation>();
                // if the number of associated LEDs is less than the possible value, store the examined box to a LED's "slot"
                if (associatedNum < num_of_possible_associated_LEDs)
                {
                    boxCoordinates.row = selected_box_array[boxNum, 0];
                    boxCoordinates.column = selected_box_array[boxNum, 1];
                    associatedBoxes[led_id].AddLast(boxCoordinates);
                    associatedNum++;
                }
                // if number of associated LEDs is greater than the possible value, there are two possibilities
                else
                {
                    // is we haven't more boxes, the last one will be associated to all remained LEDs
                    if (boxNum == num_of_boxes - 1)
                    {
                        boxCoordinates.row = selected_box_array[boxNum, 0];
                        boxCoordinates.column = selected_box_array[boxNum, 1];
                        associatedBoxes[led_id].AddLast(boxCoordinates);
                    }

                    // is we have moore boxes, we step to the next one
                    else
                    {
                        //increase the index of boxes, put the new box and set the number of associated LEDs to 1
                        
                        boxNum++;
                        if (boxNum < num_of_boxes)
                        {
                            // store next box to the slot
                            boxCoordinates.row = selected_box_array[boxNum, 0];
                            boxCoordinates.column = selected_box_array[boxNum, 1];
                            associatedBoxes[led_id].AddLast(boxCoordinates);
                            associatedNum = 1;
                        }
                    }

                }
            }

            return associatedBoxes;
        }

        private void createLog(int[,] associated_box_array_to_left_side, int[,] associated_box_array_to_bottom_side, int[,] associated_box_array_to_right_side, int[,] associated_box_array_to_top_side)
        {
            /*** logger ***/
            logger.add(LogTypes.ColorBox2LEDPixelConverter, "CB2LP converter initialized");
            //associated grid elements to sides
            // left
            string log_entry = "associated grid elements to left side:";
            for (int i = 0; i < num_of_captured_boxes[1]; i++)
            {

                log_entry = log_entry + " [" + Convert.ToString(associated_box_array_to_left_side[i, 0]);
                log_entry = log_entry + " , " + Convert.ToString(associated_box_array_to_left_side[i, 1]) + " ] ;";
            }

            logger.add(LogTypes.ColorBox2LEDPixelConverter, log_entry);

            // bottom
            log_entry = "associated grid elements to bottom side:";
            for (int i = 0; i < num_of_captured_boxes[0]; i++)
            {

                log_entry = log_entry + " [" + Convert.ToString(associated_box_array_to_bottom_side[i, 0]);
                log_entry = log_entry + " , " + Convert.ToString(associated_box_array_to_bottom_side[i, 1]) + " ] ;";
            }

            logger.add(LogTypes.ColorBox2LEDPixelConverter, log_entry);

            // right
            log_entry = "associated grid elements to right side:";
            for (int i = 0; i < num_of_captured_boxes[1]; i++)
            {

                log_entry = log_entry + " [" + Convert.ToString(associated_box_array_to_right_side[i, 0]);
                log_entry = log_entry + " , " + Convert.ToString(associated_box_array_to_right_side[i, 1]) + " ] ;";
            }

            logger.add(LogTypes.ColorBox2LEDPixelConverter, log_entry);

            // top
            log_entry = "associated grid elements to top side:";
            for (int i = 0; i < num_of_captured_boxes[0]; i++)
            {

                log_entry = log_entry + " [" + Convert.ToString(associated_box_array_to_top_side[i, 0]);
                log_entry = log_entry + " , " + Convert.ToString(associated_box_array_to_top_side[i, 1]) + " ] ;";
            }

            logger.add(LogTypes.ColorBox2LEDPixelConverter, log_entry);

            //associated box elements to LEDs

            // left
            log_entry = "associated box elements to left side LEDs: ";
            for(int led_id = 0; led_id < associatedColorBoxesLits.left.GetLength(0); led_id++)
            {
                log_entry += " | LED " + Convert.ToString(led_id);
                foreach(utils.ColRowRepresentation element in associatedColorBoxesLits.left[led_id])
                {
                    log_entry = log_entry + " [ " + element.column + " , " + element.row + " ]";
                }
            }
            logger.add(LogTypes.ColorBox2LEDPixelConverter, log_entry);

            // bottom
            log_entry = "associated box elements to bottom side LEDs";
            for (int led_id = 0; led_id < associatedColorBoxesLits.bottom.GetLength(0); led_id++)
            {
                log_entry += " | LED " + Convert.ToString(led_id);
                foreach (utils.ColRowRepresentation element in associatedColorBoxesLits.bottom[led_id])
                {
                    log_entry = log_entry + " [ " + element.column + " , " + element.row + " ]";
                }
            }
            logger.add(LogTypes.ColorBox2LEDPixelConverter, log_entry);

            // right
            log_entry = "associated box elements to right side LEDs";
            for (int led_id = 0; led_id < associatedColorBoxesLits.right.GetLength(0); led_id++)
            {
                log_entry += " | LED " + Convert.ToString(led_id);
                foreach (utils.ColRowRepresentation element in associatedColorBoxesLits.right[led_id])
                {
                    log_entry = log_entry + " [ " + element.column + " , " + element.row + " ]";
                }
            }
            logger.add(LogTypes.ColorBox2LEDPixelConverter, log_entry);

            // top
            log_entry = "associated box elements to top side LEDs";
            for (int led_id = 0; led_id < associatedColorBoxesLits.top.GetLength(0); led_id++)
            { 
                log_entry += " | LED " + Convert.ToString(led_id);
                foreach (utils.ColRowRepresentation element in associatedColorBoxesLits.top[led_id])
                {
                    log_entry = log_entry + " [ " + element.column + " , " + element.row + " ]";
                }
            }
            logger.add(LogTypes.ColorBox2LEDPixelConverter, log_entry);

        }

        public override void fillAssociatedLEDs(Color[] colorArray)
        {

            associatedLEDs.left = new Color[LED_indices.left_last - LED_indices.left_first + 1];
            associatedLEDs.bottom = new Color[LED_indices.bottom_last - LED_indices.bottom_first + 1];
            associatedLEDs.right = new Color[LED_indices.right_last - LED_indices.right_first + 1];
            associatedLEDs.top = new Color[LED_indices.top_last - LED_indices.top_first + 1];

            createColorMatrix(colorArray);
            
            //left
            for (int led_id = 0; led_id < associatedLEDs.left.GetLength(0); led_id ++)
            {
                associatedLEDs.left[led_id] = calcAvgColorFromColorList(associatedColorBoxesLits.left[led_id]);
            }
            //bottom
            for (int led_id = 0; led_id < associatedLEDs.bottom.GetLength(0); led_id++)
            {
                associatedLEDs.bottom[led_id] = calcAvgColorFromColorList(associatedColorBoxesLits.bottom[led_id]);
            }
            //right
            for (int led_id = 0; led_id < associatedLEDs.right.GetLength(0); led_id++)
            {
                associatedLEDs.right[led_id] = calcAvgColorFromColorList(associatedColorBoxesLits.right[led_id]);
            }
            //top
            for (int led_id = 0; led_id < associatedLEDs.top.GetLength(0); led_id++)
            {
                associatedLEDs.top[led_id] = calcAvgColorFromColorList(associatedColorBoxesLits.top[led_id]);
            }


        }


        private long c_R, c_G, c_B;
        private Color t_color;
        protected override Color calcAvgColorFromColorList(LinkedList<utils.ColRowRepresentation> colorBoxCoordinates)
        {
            c_R = c_G = c_B = 0;
            t_color = new Color();
            foreach (utils.ColRowRepresentation box in colorBoxCoordinates)
            {
                t_color = colorMatrix[box.row, box.column];
                c_R += t_color.R;
                c_G += t_color.G;
                c_B += t_color.B;
            }
            c_R /= colorBoxCoordinates.Count();
            c_G /= colorBoxCoordinates.Count();
            c_B /= colorBoxCoordinates.Count();


            return Color.FromArgb(Convert.ToInt32(c_R), Convert.ToInt32(c_G), Convert.ToInt32(c_B));
        }

    }
}

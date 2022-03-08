using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HelpThisColor = ColorHelper;

namespace WPF_FirstProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        List<string> lengthAndWidth = new List<string>();
        //boven constructor declareren
        //private ervoor zetten
        //underscore voor
        int countColored;
        int myLength;
        int myWidth;
        List<string> myCoordinates = new List<string>();
        List<string> coloredCircle = new List<string>();
        List<string> notColoredCircle = new List<string>();
        bool squareChecked;
        int totalCircles;

        public MainWindow()
        {
            InitializeComponent();

             
            //list maken ipv string array
            //alfabetisch maken
            //
            string[] colors = { "Blue", "Black", "Red", "Green", "Brown", "Gray", "Pink", "Yellow", "Beige", "Aqua" };
            foreach (var color in colors)
            {
                if (cbChooseColor.Items.Contains(color) == false)
                {
                    cbChooseColor.Items.Add(color);
                }

                if (cbChooseEmptyColor.Items.Contains(color) == false)
                {
                    cbChooseEmptyColor.Items.Add(color);
                }

                if (cbChooseStroke.Items.Contains(color) == false)
                {
                    cbChooseStroke.Items.Add(color);
                }
            }

            cbChooseEmptyColor.SelectedItem = cbChooseEmptyColor.Items[1];
            cbChooseColor.SelectedItem = cbChooseColor.Items[2];
            cbChooseStroke.SelectedItem = cbChooseStroke.Items[1];

            //Generates array of strings, sorts them and aadds them to the 'empty wells' combobox
            //string[] emptyWellColors = { "Blue", "Red", "Green", "Brown", "Gray", "Pink", "Yellow", "Beige", "Aqua" };
            //Array.Sort(emptyWellColors, StringComparer.InvariantCulture);

            //foreach (var emptyColor in emptyWellColors)
            //{
            //    cbChooseEmptyColor.Items.Source(emptyWellColors);
            //    cbChooseStroke.Items.Add(emptyColor);
            //}

            ////Generates array of strings, sorts them and adds them to the 'chosen wells' combobox
            //string[] chosenWellColors = { "Blue", "Black", "Green", "Brown", "Gray", "Pink", "Yellow", "Beige", "Aqua" };
            //Array.Sort(chosenWellColors, StringComparer.InvariantCulture);

            //foreach (var myColor in chosenWellColors)
            //{
            //    cbChooseColor.Items.Add(myColor);
                
            //}

            

            txbBlockTester.Text = "";

        }

        
        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

            string formatColorCircle = "";
            string formatNotColorCircle = "";
            //this takes care of the colors of the circles.
            //magic numbers in variabeles
            string chosenColor = cbChooseColor.SelectedItem as string;
            string emptyWellsColor = cbChooseEmptyColor.SelectedItem as string;
            string chosenStrokeColor = cbChooseStroke.SelectedItem as string;
            //
            var convertColor = (Color)ColorConverter.ConvertFromString(chosenColor);
            var convertEmptyColor = (Color)ColorConverter.ConvertFromString(emptyWellsColor);
            var convertStrokeColor = (Color)ColorConverter.ConvertFromString(chosenStrokeColor);

            
            foreach (object child in gMyGrid.Children)
            {
                Rectangle ellipse = child as Rectangle;

               
                if (((Rectangle)child).IsMouseOver)
                {
           
                    SolidColorBrush brush = ellipse.Fill as SolidColorBrush;
                    if (brush != null)
                    {
                        //duidelijkere naam geven
                        string removeNFromCoordinate = ellipse.Name.Replace("n", "");
                        string[] sepCoorFromNumber = removeNFromCoordinate.Split("_");
                        
                        
                        txbBlockTester.Text = $"{sepCoorFromNumber[0]}:{sepCoorFromNumber[1]}\t nr. {sepCoorFromNumber[2]}"; 
                        
                        if (brush.Color == convertEmptyColor)
                        {
                            ellipse.Fill = new SolidColorBrush(convertColor);
                            ellipse.Stroke = new SolidColorBrush(convertStrokeColor);
                            countColored += 1;
                            coloredCircle.Add(ellipse.Name);
                            notColoredCircle.Remove(ellipse.Name);
                            
                        }
                        else if(brush.Color == convertColor)
                        {
                            ellipse.Fill = new SolidColorBrush(convertEmptyColor);
                            ellipse.Stroke = new SolidColorBrush(convertStrokeColor);
                            countColored -= 1;
                            coloredCircle.Remove(ellipse.Name);

                            if (notColoredCircle.Contains(ellipse.Name) == false)
                            {
                                notColoredCircle.Add(ellipse.Name);
                            }

                        }
                        
                    }


                }

                
            }

            lblUncolored.Content = "";

            //lblUncoloredList.Content = "Colored circles\r\n";
            //environment.newline;
            lblUncolored.Content = $"Gekleurde hokjes: {countColored}\t";
            
            foreach (string myColoredCircle in coloredCircle)
            {
                formatColorCircle = $"{myColoredCircle.Replace("n", "").Split("_")[0]}:{myColoredCircle.Replace("n", "").Split("_")[1]}";
                lblUncolored.Content += $"[{formatColorCircle}], ";
                //Debug.WriteLine(myColoredCircle);
            }


            notColoredCircle.Sort();
            lblUncoloredList.Content = "";
            lblUncoloredList.Content = $"\r\nNiet gekleurde hokjes: {(myLength * myWidth) - countColored}\t";

            foreach (string circleWithoutColor in notColoredCircle)
            {
                formatNotColorCircle = $"{circleWithoutColor.Replace("n", "").Split("_")[0]}:{circleWithoutColor.Replace("n", "").Split("_")[1]}";
                lblUncoloredList.Content += $"[{formatNotColorCircle}], ";
                //Debug.WriteLine(myColoredCircle);
            }
            //notColoredCircle.Clear();

            //lblUncolored.Content = $"Gekleurde hokjes: " + countColored + $"\r\nNiet gekleurde hokjes: {(myLength * myWidth) - countColored}";
            //string[] lblFilter = lblUncolored.Content.ToString().Remove(0, 18).Split("\r\n");


            //Debug.WriteLine(lblFilter);
        }


        //this is the function to reset the colors in the circles
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //as string
            string emptyWellsColor = cbChooseEmptyColor.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "").Trim();
            string chosenStroke = cbChooseStroke.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "").Trim();
            var resetColor = (Color)ColorConverter.ConvertFromString(emptyWellsColor);
            var resetStrokeColor = (Color)ColorConverter.ConvertFromString(chosenStroke);


            coloredCircle.Clear();
            notColoredCircle.Clear();
            lblUncoloredList.Content = "";
            lblUncolored.Content = "";

            
            foreach (object child in gMyGrid.Children)
            {
                Rectangle ellipse = child as Rectangle;

                notColoredCircle.Add(ellipse.Name);

                ellipse.Fill = new SolidColorBrush(resetColor);
                ellipse.Stroke = new SolidColorBrush(resetStrokeColor);

                //ellipse.Fill = new SolidColorBrush(Colors.AliceBlue);
            }

            txbBlockTester.Text = "";

            countColored = 0;
            


            
        }

        private void btnApply(object sender, RoutedEventArgs e)
        {
            string emptyWellsColor = cbChooseEmptyColor.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "").Trim();
            string chosenStrokeColor = cbChooseStroke.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "").Trim();
            var convertEmptyWells = (Color)ColorConverter.ConvertFromString(emptyWellsColor);
            var convertStrokeColor = (Color)ColorConverter.ConvertFromString(chosenStrokeColor);
            double sliderValue = slCircleSize.Value;
            float alphabeticLabelDistance = 0;

            //reset the coordinates of the plate
            coloredCircle.Clear();
            notColoredCircle.Clear();
            lblUncoloredList.Content = "";
            lblUncolored.Content = "";
            txbBlockTester.Text = "";
            countColored = 0;

            //create more variables
            float spacePerCircle = 0; //replace met double
            string selectItem = ""; //
            string selectItemFormat = ""; //later used to replace the text in a string, so that the formatting is correct.
            
            
            int ellipseCounter = 0;

            gMyGrid.Children.Clear();
            gLabelTest.Children.Clear();

            //string myString = cbBox.Text;
            //cbBox.Items.Add("3x3");
            if (txbSize.Text.Contains("x"))
            {
                selectItem = txbSize.Text;
                selectItemFormat = selectItem;
            }
            else
            {
                selectItem = cbSize.SelectedItem.ToString();

                selectItemFormat = selectItem.Replace("System.Windows.Controls.ComboBoxItem: ", "");//selectItem.Replace("System.Windows.Controls.ComboBoxItem: ", "");
            }

            //gMyGrid.RenderTransform = 90;

            lblDebugLabel.Content = $"Grootte: {selectItemFormat}";

           
            lengthAndWidth = selectItemFormat.Split("x").ToList();
            myLength = int.Parse(lengthAndWidth[0].Trim()); //replace met replace
            myWidth = int.Parse(lengthAndWidth[1].Trim());

            //switch

            totalCircles = myWidth * myLength;

            switch (totalCircles)
            {
                case var expression when totalCircles >= 300:
                    alphabeticLabelDistance = -300;
                    break;
                default:
                    break;
            }

            if (totalCircles >= 300)
            {
                alphabeticLabelDistance = -300;
            }
            else if (totalCircles >= 200 && totalCircles < 300)
            {
                alphabeticLabelDistance = -383;
            }
            else if(totalCircles >= 100 && totalCircles < 200)
            {
                alphabeticLabelDistance = -466;
            }
            else if(totalCircles < 100 && totalCircles >= 50)
            {
                alphabeticLabelDistance = -500;
            }
            else if (totalCircles < 50 && totalCircles >= 1)
            {
                alphabeticLabelDistance = -550;
            }


            //calculates the size
            if (myLength < 27 && myWidth < 27)
            {
                float totalSizeField = 600;
                float EllipseDistance = 2F; //(float)(60.0 / EllipseSize)

                if (myWidth > 1)
                {
                    spacePerCircle = totalSizeField / (myLength - 1);
                }
                else if (myWidth == 1)
                {
                    spacePerCircle = (float)(totalSizeField / myLength);
                }

                float EllipseSize = spacePerCircle / EllipseDistance * (float)(sliderValue / 100 * 4 + 1);
               

                //int EllipseSize = 140;

                int countAlphabet = -1;
                int countAlphabet2 = 0;

                string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

                //generates rows and colums of circles

                for (int h = 0; h < myLength; h++) //height
                {

                    for (int w = 0; w < myWidth; w++) //width
                    {
                        

                        Rectangle ellipse = new Rectangle();

                        ellipse.Fill = new SolidColorBrush(convertEmptyWells);
                        ellipse.Stroke = new SolidColorBrush(convertStrokeColor);
                        ellipse.StrokeThickness = EllipseSize * 0.08F;
                        ellipse.Height = EllipseSize;
                        ellipse.Width = EllipseSize;
                        if ((bool)checkSquare.IsChecked)
                        {
                            ellipse.RadiusX = 0;
                            ellipse.RadiusY = 0;
                        }
                        else if(!(bool)checkSquare.IsChecked)
                        {
                            ellipse.RadiusX = 150;
                            ellipse.RadiusY = 150;
                        }

                        ellipse.Name = $"n{alphabet[h]}_{myWidth - w - 1}_{ellipseCounter}";
                        
                        myCoordinates.Add($"n{alphabet[h]}_{myWidth - w - 1}_{ellipseCounter}");
                        
                        if (notColoredCircle.Contains(ellipse.Name) == false)
                        {
                            notColoredCircle.Add(ellipse.Name);
                        }

                        ellipse.Margin = new Thickness(200, -300 + h * (EllipseSize * EllipseDistance), w * (EllipseSize * EllipseDistance), 200);

                        gMyGrid.Children.Add(ellipse);

                        

                        //numeric labels
                        if (h == 0)
                        {
                            Label lblCoordinateNum = new Label();
                            lblCoordinateNum.FontSize = EllipseSize / EllipseDistance;
                            lblCoordinateNum.Height = EllipseSize * 1.1;
                            lblCoordinateNum.Width = EllipseSize;
                            //lblCoordinateNum.Background = Brushes.Red;
                            lblCoordinateNum.Content = myWidth - w - 1;

                            lblCoordinateNum.Margin = new Thickness(0, -550, -205 + (w * EllipseSize * EllipseDistance) + 3, 0);
                            lblCoordinateNum.Foreground = Brushes.Black;

                            

                            gLabelTest.Children.Add(lblCoordinateNum);

                        }

                        //alphabetic labels
                        if (w == 0)
                        {
                            Label lblCoordinateAlphabet = new Label();
                            lblCoordinateAlphabet.FontSize = EllipseSize * 0.66; //magic numbers
                            lblCoordinateAlphabet.Height = EllipseSize * 1.33;
                            lblCoordinateAlphabet.Width = EllipseSize * 1.5;

                            if (countAlphabet > 25)
                            {
                                countAlphabet = 0;
                                countAlphabet2 += 1;
                            }
                            else
                            {
                                countAlphabet += 1;
                            }
                            //lblCoordinateAlphabet.Content = alphabet[countAlphabet2].ToString() + alphabet[countAlphabet].ToString();
                            lblCoordinateAlphabet.Content = alphabet[h]; //tostring overbodig
                            //lblCoordinateAlphabet.Background = Brushes.Aquamarine;
                            lblCoordinateAlphabet.Margin = new Thickness(0, -500 + (h * EllipseSize * EllipseDistance), alphabeticLabelDistance, 0);
                            lblCoordinateAlphabet.Foreground = Brushes.Black;

                            gLabelTest.Children.Add(lblCoordinateAlphabet);
                        }

                        ellipseCounter++;

                    }

                    //gMyGrid.RenderTransform
                }


                txbSize.Text = "";
                //Debug.WriteLine($"Ellipse height: {EllipseSize}");


            }
            else
            {
                
                MessageBox.Show("Maximum alphabet count");
            }



        }

        private void addValue(object sender, RoutedEventArgs e)
        {
            if (txbSize.Text != "")
            {
                if (txbSize.Text.Contains("x"))
                {
                    cbSize.Items.Add(txbSize.Text);
                    txbSize.Text = "";
                }

            }
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ViewCoordinate(object sender, RoutedEventArgs e)
        {
            //Debug.WriteLine($"Test:{txbCoordinatePicker.Text}||");
            if (txbCoordinatePicker.Text != "")
            {
                if (int.TryParse(txbCoordinatePicker.Text, out int parsedNumber))
                {
                    int numberInTxb = int.Parse(txbCoordinatePicker.Text.Trim());

                    foreach (string coordinate in myCoordinates)
                    {
                        if (coordinate.Contains(txbCoordinatePicker.Text))
                        {

                            string[] splitCoordinate = coordinate.Replace("n", "").Split("_");
                            if (splitCoordinate[2] == txbCoordinatePicker.Text.Trim())
                            {
                                string convertedCoordinate = $"{splitCoordinate[0]}{splitCoordinate[1]}";
                                MessageBox.Show(convertedCoordinate);
                            }

                        }
                    }
                }
                else
                {
                    MessageBox.Show("Input needs to be a number", MessageBoxButton.OK.ToString());
                    
                }

                

                

            }
            else
            {
                MessageBox.Show("Box can't be empty");
            }
        }

        private void CoordinateToColor(object sender, RoutedEventArgs e)
        {
            int checkNumber;
            string formatCoordinate;
            //string formatNumber;
            string emptyWellsColor = cbChooseColor.SelectedItem as string;
            var coordinateColor = (Color)ColorConverter.ConvertFromString(emptyWellsColor);

            foreach (object child in gMyGrid.Children)
            {
                Rectangle ellipse = child as Rectangle;

                if (int.TryParse(txbColorCreator.Text.Trim(), out checkNumber))
                {
                    //Number
                    if (ellipse.Name.Split("_")[2] == txbColorCreator.Text.Trim())
                    {
                        Debug.WriteLine(ellipse.Name.Split("_")[2]);
                        ellipse.Fill = new SolidColorBrush(coordinateColor);
                    }
                }
                else
                {
                    //Alphabetic
                    formatCoordinate = $"n{txbColorCreator.Text[0].ToString().ToUpper()}_{txbColorCreator.Text.Split(txbColorCreator.Text[0])[1]}";
                   
                    if (ellipse.Name.Contains(formatCoordinate))
                    {
                        ellipse.Fill = new SolidColorBrush(coordinateColor);
                    }
                }

            }

            

            
        }

        private void AddColor(object sender, RoutedEventArgs e)
        {
            string chosenColor = cbChooseColor.SelectedItem as string;
            int red;
            int green;
            int blue;
            string newColor = txbAddColor.Text.Trim();
            
            if (newColor.Contains(","))
            {
                string[] splitNewColor = newColor.Split(",");
                red = int.Parse(splitNewColor[0].Trim());
                green = int.Parse(splitNewColor[1].Trim());
                blue = int.Parse(splitNewColor[2].Trim());

                HelpThisColor.RGB rgb = new HelpThisColor.RGB(Convert.ToByte(red), Convert.ToByte(green), Convert.ToByte(blue));
                HelpThisColor.HEX hex = HelpThisColor.ColorConverter.RgbToHex(rgb);

                Debug.WriteLine($"Hex: {hex}");

                newColor = "#" + hex.ToString();

                ////Debug.WriteLine($"Testformat: {testFormat}");
                //Debug.WriteLine($"{red}\r\n{green}\r\n{blue}");
                //MessageBox.Show("RGB is not allowed in this version");
            }

            if (!cbChooseEmptyColor.Items.Contains(newColor) && newColor.Contains("#") || char.IsLetter(newColor.FirstOrDefault()))
            {
                try
                {
                    var convertColor = (Color)ColorConverter.ConvertFromString(newColor);
                    cbChooseColor.Items.Add(newColor);
                    cbChooseEmptyColor.Items.Add(newColor);
                    cbChooseStroke.Items.Add(newColor);
                    txbAddColor.Text = "";
                }
                catch (Exception E)
                {
                    MessageBox.Show("Something went wrong:\r\n" + E.ToString());
                }


                
                
            }
            else
            {
                MessageBox.Show("This format is not supported");
            }
            
        }

        private void checkSquare_Click(object sender, RoutedEventArgs e)
        {
            string theShape;
            string colour = "red";
            var convertColor = (Color)ColorConverter.ConvertFromString(colour);
            if ((bool)checkSquare.IsChecked)
            {
                theShape = "rectangle";
                
                
            }
            else
            {
                squareChecked = (bool)checkSquare.IsChecked;
                theShape = "circle";
            }
            KleineTest(theShape, colour);
        }

        public void KleineTest(string myShape, string color )
        {
            var convertColor = (Color)ColorConverter.ConvertFromString(color);
            if (myShape == "rectangle")
            {
                gTestGrid.Children.Clear();
                Rectangle rectangle = new Rectangle();
                rectangle.Fill = new SolidColorBrush(convertColor);
                rectangle.Stroke = new SolidColorBrush(convertColor);
                rectangle.StrokeThickness = 30 * 0.08F;
                rectangle.Height = 30;
                rectangle.Width = 30;
                //rectangle.RadiusX = 15;
                //rectangle.RadiusY = 15;

                gTestGrid.Children.Add(rectangle);
            }
            else if (myShape == "circle")
            {
                gTestGrid.Children.Clear();
                Rectangle ellipse = new Rectangle();
                ellipse.Fill = new SolidColorBrush(convertColor);
                ellipse.Stroke = new SolidColorBrush(convertColor);
                ellipse.StrokeThickness = 30 * 0.08F;
                ellipse.Height = 30;
                ellipse.Width = 30;
                

                gTestGrid.Children.Add(ellipse);
            }

            

        }
    }
}

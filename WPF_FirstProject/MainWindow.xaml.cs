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
        int myHeight;
        int myWidth;
        List<string> myCoordinates = new List<string>();
        List<string> coloredCircle = new List<string>();
        List<string> notColoredCircle = new List<string>();
        List<string> listColors = new List<string>();
        //bool squareChecked;
        int totalCircles;

        public MainWindow()
        {
            InitializeComponent();

             
            //list maken ipv string array
            //alfabetisch maken
            //
            string[] colors = { "Aqua", "Beige", "Black", "Blue", "Brown", "Gray", "Green", "Pink", "Red", "Yellow" };

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

            cbChooseEmptyColor.SelectedItem = cbChooseEmptyColor.Items[2];
            cbChooseColor.SelectedItem = cbChooseColor.Items[8];
            cbChooseStroke.SelectedItem = cbChooseStroke.Items[2];

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

            lblUncolored.Content = $"Gekleurde hokjes: {countColored}\t";
            
            foreach (string myColoredCircle in coloredCircle)
            {
                formatColorCircle = $"{myColoredCircle.Replace("n", "").Split("_")[0]}:{myColoredCircle.Replace("n", "").Split("_")[1]}";
                lblUncolored.Content += $"[{formatColorCircle}], ";
            }


            notColoredCircle.Sort();
            lblUncoloredList.Content = "";
            lblUncoloredList.Content = $"\r\nNiet gekleurde hokjes: {(myHeight * myWidth) - countColored}\t";

            foreach (string circleWithoutColor in notColoredCircle)
            {
                formatNotColorCircle = $"{circleWithoutColor.Replace("n", "").Split("_")[0]}:{circleWithoutColor.Replace("n", "").Split("_")[1]}";
                lblUncoloredList.Content += $"[{formatNotColorCircle}], ";
            }
        
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
            myCoordinates.Clear();

            //create more variables
            float spacePerCircle = 0; //replace met double
            string selectItem = ""; //
            string selectItemFormat = ""; //later used to replace the text in a string, so that the formatting is correct.
            
            int ellipseCounter = 0;

            //clear the grids that contain the shapes and labels
            gMyGrid.Children.Clear();
            gLabelTest.Children.Clear();

            

            
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

            lblDebugLabel.Content = $"Grootte: {selectItemFormat}";

            lengthAndWidth = selectItemFormat.Replace(" ", "").Split("x").ToList();
            myHeight = int.Parse(lengthAndWidth[0]); //replace met replace
            myWidth = int.Parse(lengthAndWidth[1]);

            //default 4x6
            //verticaal: 4 (0) myHeight
            //horizontaal: 6 (1) myWidth
            totalCircles = myWidth * myHeight;

            //switch (totalCircles)
            //{
            //    case var expression when totalCircles >= 300:
            //        alphabeticLabelDistance = -300;
            //        break;
            //    default:
            //        break;
            //}

            if (totalCircles >= 300)
            {
                alphabeticLabelDistance = -300;
            }
            else if (totalCircles >= 200 && totalCircles < 300)
            {
                alphabeticLabelDistance = -383;
            }
            else if (totalCircles >= 100 && totalCircles < 200)
            {
                alphabeticLabelDistance = -466;
            }
            else if (totalCircles < 100 && totalCircles >= 50)
            {
                alphabeticLabelDistance = -500;
            }
            else if (totalCircles < 50 && totalCircles >= 1)
            {
                alphabeticLabelDistance = -550;
            }
            



            //calculates the size
            if (myHeight < 27 && myWidth < 27)
            {
                float totalSizeField = 600;
                float EllipseDistance = 2F; //(float)(60.0 / EllipseSize)

                if (myHeight < myWidth && myHeight < 11)
                {
                    totalSizeField = 840;
                    if (myWidth > 1)
                    {
                        spacePerCircle = totalSizeField / (myWidth - 1);
                    }
                    else if (myWidth == 1)
                    {
                        spacePerCircle = (float)(totalSizeField / myWidth);
                    }
                }
                else
                {
                    if (myHeight > 1)
                    {
                        spacePerCircle = totalSizeField / (myHeight - 1);
                    }
                    else if (myHeight == 1)
                    {
                        spacePerCircle = (float)(totalSizeField / myHeight);
                    }
                }

                

                float EllipseSize = spacePerCircle / EllipseDistance * (float)(sliderValue / 100 * 4 + 1);
               
                int countAlphabet = -1;
                int countAlphabet2 = 0;

                string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

                //generates rows and colums of circles

                for (int h = 0; h < myHeight; h++) //height
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
                            
                            lblCoordinateAlphabet.Content = alphabet[h]; //tostring overbodig
                            
                            lblCoordinateAlphabet.Margin = new Thickness(0, -500 + (h * EllipseSize * EllipseDistance), alphabeticLabelDistance, 0);
                            lblCoordinateAlphabet.Foreground = Brushes.Black;

                            gLabelTest.Children.Add(lblCoordinateAlphabet);
                        }

                        ellipseCounter++;

                    }

                }


                txbSize.Text = "";
            
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
            
            if (newColor.Contains(",")) //Check for rgb
            {
                string[] splitNewColor = newColor.Split(",");
                red = int.Parse(splitNewColor[0].Trim());
                green = int.Parse(splitNewColor[1].Trim());
                blue = int.Parse(splitNewColor[2].Trim());

                HelpThisColor.RGB rgb = new HelpThisColor.RGB(Convert.ToByte(red), Convert.ToByte(green), Convert.ToByte(blue));
                HelpThisColor.HEX hex = HelpThisColor.ColorConverter.RgbToHex(rgb);

                Debug.WriteLine($"Hex: {hex}");

                newColor = "#" + hex.ToString();

            }

            if (!cbChooseEmptyColor.Items.Contains(newColor) && newColor.Contains("#") || char.IsLetter(newColor.FirstOrDefault())) //check for a # (hex) or the first character to be a letter
            {
                if (char.IsLetter(newColor.FirstOrDefault()))
                {
                    newColor = char.ToUpper(newColor.First()) + newColor.Substring(1).ToLower();
                    
                }
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
                    MessageBox.Show("Something went wrong:" +  Environment.NewLine + E.ToString());
                }

            }
            else
            {
                MessageBox.Show("This format is not supported");
            }
            
        }

        private void checkSquare_Click(object sender, RoutedEventArgs e)
        {
            //emptyness
        }

    }
}

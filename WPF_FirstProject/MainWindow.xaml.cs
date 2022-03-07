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


        public MainWindow()
        {
            InitializeComponent();

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
            //    cbChooseEmptyColor.Items.Add(emptyColor);
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

        int countColored;
        int myLength;
        int myWidth;
        List<string> myCoordinates = new List<string>();
        List<string> coloredCircle = new List<string>();
        List<string> notColoredCircle = new List<string>(); 

        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

            string formatColorCircle = "";
            string formatNotColorCircle = "";
            //this takes care of the colors of the circles.

            string chosenColor = cbChooseColor.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "").Trim();
            string emptyWellsColor = cbChooseEmptyColor.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "").Trim();
            string chosenStrokeColor = cbChooseStroke.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "").Trim();

            var convertColor = (Color)ColorConverter.ConvertFromString(chosenColor);
            var convertEmptyColor = (Color)ColorConverter.ConvertFromString(emptyWellsColor);
            var convertStrokeColor = (Color)ColorConverter.ConvertFromString(chosenStrokeColor);

            int spamCounter = 0;

            //notColoredCircle.Clear();

            foreach (object child in gMyGrid.Children)
            {
                Ellipse ellipse = child as Ellipse;

                spamCounter += 1;
                
                
                if (((Ellipse)child).IsMouseOver)
                {
           
                    SolidColorBrush brush = ellipse.Fill as SolidColorBrush;
                    if (brush != null)
                    {
                        string cleanCoordinate = ellipse.Name.Replace("n", "");
                        string[] sepCoorFromNumber = cleanCoordinate.Split("_");
                        
                        
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

            //Debug.WriteLine(spamCounter);
            //lblUncoloredList.Content = "Colored circles\r\n";
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
                Ellipse ellipse = child as Ellipse;

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

            //reset the coordinates of the plate
            coloredCircle.Clear();
            notColoredCircle.Clear();
            lblUncoloredList.Content = "";
            lblUncolored.Content = "";
            txbBlockTester.Text = "";
            countColored = 0;

            //create more variables
            float spacePerCircle = 0;
            string selectItem = ""; //
            string selectItemFormat = ""; //later used to replace the text in a string, so that the formatting is correct.
            string[] myArray = new string[0];
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

                selectItemFormat = selectItem.Replace("System.Windows.Controls.ComboBoxItem: ", "");
            }

            //gMyGrid.RenderTransform = 90;

            lblDebugLabel.Content = $"Grootte: {selectItemFormat}";

           
            myArray = selectItemFormat.Split("x");
            myLength = int.Parse(myArray[0].Trim());
            myWidth = int.Parse(myArray[1].Trim());

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

                float EllipseSize = spacePerCircle / EllipseDistance;
               

                //int EllipseSize = 140;

                int countAlphabet = -1;
                int countAlphabet2 = 0;

                string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

                //generates rows and colums of circles

                for (int h = 0; h < myLength; h++)
                {

                    for (int w = 0; w < myWidth; w++)
                    {
                        

                        Ellipse ellipse = new Ellipse();

                        ellipse.Fill = new SolidColorBrush(convertEmptyWells);
                        ellipse.Stroke = new SolidColorBrush(convertStrokeColor);
                        ellipse.StrokeThickness = EllipseSize * 0.08F;
                        ellipse.Height = EllipseSize;
                        ellipse.Width = EllipseSize;
                        
                        ellipse.Name = $"n{alphabet[h]}_{myWidth - w - 1}_{ellipseCounter}";
                        
                        myCoordinates.Add($"n{alphabet[h]}_{myWidth - w - 1}_{ellipseCounter}");
                        
                        if (notColoredCircle.Contains(ellipse.Name) == false)
                        {
                            notColoredCircle.Add(ellipse.Name);
                        }

                        ellipse.Margin = new Thickness(200, h * (EllipseSize * EllipseDistance), w * (EllipseSize * EllipseDistance), 200);

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

                            lblCoordinateNum.Margin = new Thickness(0, -250, -205 + (w * EllipseSize * EllipseDistance) + 3, 0);
                            lblCoordinateNum.Foreground = Brushes.Black;

                            

                            gLabelTest.Children.Add(lblCoordinateNum);

                        }

                        //alphabetic labels
                        if (w == 0)
                        {
                            Label lblCoordinateAlphabet = new Label();
                            lblCoordinateAlphabet.FontSize = EllipseSize * 0.66;
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
                            lblCoordinateAlphabet.Content = alphabet[h].ToString();
                            //lblCoordinateAlphabet.Background = Brushes.Aquamarine;
                            lblCoordinateAlphabet.Margin = new Thickness(0, -202 + (h * EllipseSize * EllipseDistance), -400, 0);
                            lblCoordinateAlphabet.Foreground = Brushes.Black;

                            gLabelTest.Children.Add(lblCoordinateAlphabet);
                        }

                        ellipseCounter += 1;

                    }

                    //gMyGrid.RenderTransform
                }


                txbSize.Text = "";
                Debug.WriteLine($"Ellipse height: {EllipseSize}");


            }
            else
            {
                
                MessageBox.Show("Getal kan niet boven de 26 zijn");
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
            Debug.WriteLine($"Test:{txbCoordinatePicker.Text}||");
            if (txbCoordinatePicker.Text != "")
            {
                int numberInTxb = int.Parse(txbCoordinatePicker.Text);

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
                MessageBox.Show("Test");
            }
        }

        private void CoordinateToColor(object sender, RoutedEventArgs e)
        {
            int checkNumber;
            string formatCoordinate;
            //string formatNumber;
            string emptyWellsColor = cbChooseColor.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "").Trim();
            var coordinateColor = (Color)ColorConverter.ConvertFromString(emptyWellsColor);

            foreach (object child in gMyGrid.Children)
            {
                Ellipse ellipse = child as Ellipse;

                if (int.TryParse(txbColorCreator.Text.Trim(), out checkNumber))
                {
                    //Debug.WriteLine($"{txbColorCreator.Text} is a number");
                    if (ellipse.Name.Split("_")[2].Contains(txbColorCreator.Text.Trim()))
                    {
                        ellipse.Fill = new SolidColorBrush(coordinateColor);
                    }
                }
                else
                {
                    //Debug.WriteLine($"{txbColorCreator.Text} is alphabetic");
                    //formatCoordinate = txbColorCreator.Text.ToUpper();
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
            string chosenColor = cbChooseColor.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "").Trim();
            //int red;
            //int green;
            //int blue;
            //string hexColor;
            //string testFormat;
            string newColor = txbAddColor.Text.Trim();
            
            if (newColor.Contains(","))
            {
                //string[] splitNewColor = newColor.Split(",");
                //red = int.Parse(splitNewColor[0].Trim());
                //green = int.Parse(splitNewColor[1].Trim());
                //blue = int.Parse(splitNewColor[2].Trim());

                //HelpThisColor.RGB rgb = new HelpThisColor.RGB(Convert.ToByte(red), Convert.ToByte(green), Convert.ToByte(blue));
                //HelpThisColor.HEX hex = HelpThisColor.ColorConverter.RgbToHex(rgb);

                //Debug.WriteLine(hex);

                //newColor = hex.ToString();

                ////Debug.WriteLine($"Testformat: {testFormat}");
                //Debug.WriteLine($"{red}\r\n{green}\r\n{blue}");
                MessageBox.Show("RGB is not allowed in this version");
            }

            if (cbChooseEmptyColor.Items.Contains(newColor) == false)
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
            
        }
    }
}

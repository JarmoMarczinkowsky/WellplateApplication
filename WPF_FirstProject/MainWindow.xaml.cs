﻿using System;
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

            txbBlockTester.Text = "";

        }

        
        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            
            foreach(object child in gMyGrid.Children)
            {
                Ellipse ellipse = child as Ellipse;
                
                if (((Ellipse)child).IsMouseOver)
                {
           
                    SolidColorBrush brush = ellipse.Fill as SolidColorBrush;
                    if (brush != null)
                    {
                        string cleanCoordinate = ellipse.Name.Replace("n", "").Replace("_", ":");
                        txbBlockTester.Text = $"{txbBlockTester.Text}\r\n{cleanCoordinate}"; 
                        if (brush.Color == Colors.Black)
                        {
                            ellipse.Fill = new SolidColorBrush(Colors.Red);
                            
                        }
                        else if(brush.Color == Colors.Red)
                        {
                            ellipse.Fill = new SolidColorBrush(Colors.Black);
                            
                        }
                    }


                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (object child in gMyGrid.Children)
            {
                Ellipse ellipse = child as Ellipse;

                ellipse.Fill = new SolidColorBrush(Colors.Black);

                //ellipse.Fill = new SolidColorBrush(Colors.AliceBlue);
            }

            txbBlockTester.Text = "";

            
        }

        private void btnApply(object sender, RoutedEventArgs e)
        {
            float spacePerCircle = 0;
            string selectItem = "";
            string selectItemFormat = "";
            string[] myArray = new string[0];
            
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
                selectItem = cbBox.SelectedItem.ToString();

                selectItemFormat = selectItem.Replace("System.Windows.Controls.ComboBoxItem: ", "");
            }

            lblDebugLabel.Content = $"Grootte: {selectItemFormat}";

            Debug.WriteLine($"-_{selectItemFormat}_-");
            myArray = selectItemFormat.Split("x");

            int myLength = int.Parse(myArray[0].Trim());
            int myWidth = int.Parse(myArray[1].Trim());

            

            float totalSizeField = 840;
            float EllipseDistance = 2F; //(float)(60.0 / EllipseSize)

            if (myWidth > 1)
            {
                spacePerCircle = totalSizeField / (myWidth - 1);
            }
            else if(myWidth == 1)
            {
                spacePerCircle = (float)(totalSizeField / myWidth);
            }

            float EllipseSize = (spacePerCircle) / EllipseDistance;


            //int EllipseSize = 140;

            int countAlphabet = -1;
            int countAlphabet2 = 0;

            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            for (int h = 0; h < myLength; h++)
            {
                
                for (int w = 0; w < myWidth; w++)
                {
                    
                    Ellipse ellipse = new Ellipse();
                    
                    ellipse.Fill = new SolidColorBrush(Colors.Black);
                    ellipse.Height = EllipseSize;
                    ellipse.Width = EllipseSize;
                    ellipse.Name = $"n{alphabet[h]}_{myWidth - w - 1}";
                    
                    ellipse.Margin = new Thickness(200, (h * (EllipseSize * EllipseDistance)), w * (EllipseSize * EllipseDistance), 200);
                    
                    gMyGrid.Children.Add(ellipse);


                    //numeric labels
                    if (h == 0)
                    {
                        Label lblCoordinateNum = new Label();
                        lblCoordinateNum.FontSize = EllipseSize / EllipseDistance;
                        lblCoordinateNum.Height = EllipseSize * 1.1;
                        lblCoordinateNum.Width = EllipseSize;
                        lblCoordinateNum.Background = Brushes.Red;
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
                        lblCoordinateAlphabet.Content = alphabet[countAlphabet2].ToString() + alphabet[countAlphabet].ToString();
                        lblCoordinateAlphabet.Background = Brushes.Aquamarine;
                        lblCoordinateAlphabet.Margin = new Thickness(0, -202 + (h * EllipseSize * EllipseDistance), -400, 0);
                        lblCoordinateAlphabet.Foreground = Brushes.Black;

                        gLabelTest.Children.Add(lblCoordinateAlphabet);
                    }

                }
            }

            //Debug.WriteLine($"TC: {totalCircles}");
            txbSize.Text = "";

        }

        private void addValue(object sender, RoutedEventArgs e)
        {
            if (txbSize.Text != "")
            {
                if (txbSize.Text.Contains("x"))
                {
                    cbBox.Items.Add(txbSize.Text);
                    txbSize.Text = "";
                }

            }
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
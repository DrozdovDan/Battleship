using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using static Battleship.FirstTurnClass;
using static Battleship.SecondTurnClass;

namespace Battleship
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const int maxSizeOfField = 10;
        private bool startOfGame = true;
        private bool IsSomeoneWon = false;
        public static int[,] firstFleet = new int[maxSizeOfField, maxSizeOfField];
        public static int[,] secondFleet = new int[maxSizeOfField, maxSizeOfField];
        public static int[,] firstFleetAtStart = new int[maxSizeOfField, maxSizeOfField];
        public static int[,] secondFleetAtStart = new int[maxSizeOfField, maxSizeOfField];
        public static Button[,] firstField = new Button[maxSizeOfField, maxSizeOfField];
        public static Button[,] secondField = new Button[maxSizeOfField, maxSizeOfField];
        public static bool firstTurn = true;
        public static List<int> firstBattleships = new List<int>();
        public static List<int> secondBattleships = new List<int>();
        public static string str = "";

        public MainWindow()
        {
            InitializeComponent();

            //Create a field for first player
            for (int i = 1; i < maxSizeOfField + 1; i++)
            {
                for (int j = 1; j < maxSizeOfField + 1; j++)
                {
                    Button button = new Button();
                    button.Click += FirstClick;
                    Grid.SetColumn(button, j);
                    Grid.SetRow(button, i);
                    button.Width = 50;
                    button.Height = 50;
                    button.Background = Brushes.Aqua;
                    firstField[i - 1, j - 1] = button;
                    MyGrid.Children.Add(button);
                }
            }

            //Create a field for second player
            for (int i = 1; i < maxSizeOfField + 1; i++)
            {
                for (int j = maxSizeOfField + 3; j < maxSizeOfField * 2 + 3; j++)
                {
                    Button button = new Button();
                    button.Click += SecondClick;
                    Grid.SetColumn(button, j);
                    Grid.SetRow(button, i);
                    button.Width = 50;
                    button.Height = 50;
                    button.Background = Brushes.Aqua;
                    secondField[i - 1, j - 3 - maxSizeOfField] = button;
                    MyGrid.Children.Add(button);
                }
            }

            //Create empty fleets
            for (int i = 0; i < maxSizeOfField; i++)
            {
                for (int j = 0; j < maxSizeOfField; j++)
                {
                    firstFleetAtStart[i, j] = 0;
                    secondFleetAtStart[i, j] = 0;
                }
            }

            int sign = 1;
            //Create signs
            for (int i = 1; i < maxSizeOfField + 1; i++)
            {
                TextBlock textBlock1 = new TextBlock();
                textBlock1.Text = ((char)(sign + 96)).ToString();
                textBlock1.TextAlignment = TextAlignment.Center;
                Grid.SetColumn(textBlock1, i);
                Grid.SetRow(textBlock1, 0);
                MyGrid.Children.Add(textBlock1);
                TextBlock textBlock2 = new TextBlock();
                textBlock2.Text = ((char)(sign + 96)).ToString();
                textBlock2.TextAlignment = TextAlignment.Center;
                Grid.SetColumn(textBlock2, i + maxSizeOfField + 2);
                Grid.SetRow(textBlock2, 0);
                MyGrid.Children.Add(textBlock2);
                TextBlock textBlock3 = new TextBlock();
                textBlock3.Text = sign.ToString();
                textBlock3.TextAlignment = TextAlignment.Center;
                Grid.SetColumn(textBlock3, 0);
                Grid.SetRow(textBlock3, i);
                MyGrid.Children.Add(textBlock3);
                TextBlock textBlock4 = new TextBlock();
                textBlock4.Text = sign.ToString();
                textBlock4.TextAlignment = TextAlignment.Center;
                Grid.SetColumn(textBlock4, maxSizeOfField + 2);
                Grid.SetRow(textBlock4, i);
                MyGrid.Children.Add(textBlock4);
                sign += 1;
            }
        }

        private void FirstClick(object? sender, RoutedEventArgs e)
        {
            if (firstTurn && !IsSomeoneWon)
            {
                var thatButton = (Button)sender!;

                if (startOfGame)
                {
                    SetFirstBattleships(thatButton);
                }
                else
                {
                    str = "";
                    BeatFirst(thatButton);
                    MessageBox.Text = str;
                    if (secondBattleships.Count == 0)
                    {
                        IsSomeoneWon = true;
                        MessageBox.Text = "FIRST PLAYER\nWON!";
                        ColorForLooser(firstFleet, secondField);
                    }
                }
            }
        }

        private void SecondClick(object? sender, RoutedEventArgs e)
        {
            if (!firstTurn && !IsSomeoneWon)
            {
                var thatButton = (Button)sender!;

                if (startOfGame)
                {
                    SetSecondBattleships(thatButton);
                }
                else
                {
                    str = "";
                    BeatSecond(thatButton);
                    MessageBox.Text = str;
                    if (firstBattleships.Count == 0)
                    {
                        IsSomeoneWon = true;
                        MessageBox.Text = "SECOND PLAYER\nWON!";
                        ColorForLooser(secondFleet, firstField);
                    }
                }
            }
        }

        private void TurnClick(object? sender, RoutedEventArgs e)
        {
            if (startOfGame)
            {
                StartCheck();
            }
        }

        private void ResetClick(object? sender, RoutedEventArgs e)
        {
            startOfGame = true;
            IsSomeoneWon = false;
            firstFleet = new int[maxSizeOfField, maxSizeOfField];
            secondFleet = new int[maxSizeOfField, maxSizeOfField];
            firstFleetAtStart = new int[maxSizeOfField, maxSizeOfField];
            secondFleetAtStart = new int[maxSizeOfField, maxSizeOfField];
            firstField = new Button[maxSizeOfField, maxSizeOfField];
            secondField = new Button[maxSizeOfField, maxSizeOfField];
            firstTurn = true;
            firstBattleships = new List<int>();
            secondBattleships = new List<int>();
            str = "";
            
            InitializeComponent();

            //Create a field for first player
            for (int i = 1; i < maxSizeOfField + 1; i++)
            {
                for (int j = 1; j < maxSizeOfField + 1; j++)
                {
                    Button button = new Button();
                    button.Click += FirstClick;
                    Grid.SetColumn(button, j);
                    Grid.SetRow(button, i);
                    button.Width = 50;
                    button.Height = 50;
                    button.Background = Brushes.Aqua;
                    firstField[i - 1, j - 1] = button;
                    MyGrid.Children.Add(button);
                }
            }

            //Create a field for second player
            for (int i = 1; i < maxSizeOfField + 1; i++)
            {
                for (int j = maxSizeOfField + 3; j < maxSizeOfField * 2 + 3; j++)
                {
                    Button button = new Button();
                    button.Click += SecondClick;
                    Grid.SetColumn(button, j);
                    Grid.SetRow(button, i);
                    button.Width = 50;
                    button.Height = 50;
                    button.Background = Brushes.Aqua;
                    secondField[i - 1, j - 3 - maxSizeOfField] = button;
                    MyGrid.Children.Add(button);
                }
            }

            //Create empty fleets
            for (int i = 0; i < maxSizeOfField; i++)
            {
                for (int j = 0; j < maxSizeOfField; j++)
                {
                    firstFleetAtStart[i, j] = 0;
                    secondFleetAtStart[i, j] = 0;
                }
            }

            int sign = 1;
            //Create signs
            for (int i = 1; i < maxSizeOfField + 1; i++)
            {
                TextBlock textBlock1 = new TextBlock();
                textBlock1.Text = ((char)(sign + 96)).ToString();
                textBlock1.TextAlignment = TextAlignment.Center;
                Grid.SetColumn(textBlock1, i);
                Grid.SetRow(textBlock1, 0);
                MyGrid.Children.Add(textBlock1);
                TextBlock textBlock2 = new TextBlock();
                textBlock2.Text = ((char)(sign + 96)).ToString();
                textBlock2.TextAlignment = TextAlignment.Center;
                Grid.SetColumn(textBlock2, i + maxSizeOfField + 2);
                Grid.SetRow(textBlock2, 0);
                MyGrid.Children.Add(textBlock2);
                TextBlock textBlock3 = new TextBlock();
                textBlock3.Text = sign.ToString();
                textBlock3.TextAlignment = TextAlignment.Center;
                Grid.SetColumn(textBlock3, 0);
                Grid.SetRow(textBlock3, i);
                MyGrid.Children.Add(textBlock3);
                TextBlock textBlock4 = new TextBlock();
                textBlock4.Text = sign.ToString();
                textBlock4.TextAlignment = TextAlignment.Center;
                Grid.SetColumn(textBlock4, maxSizeOfField + 2);
                Grid.SetRow(textBlock4, i);
                MyGrid.Children.Add(textBlock4);
                sign += 1;
            }
        }

        private void StartCheck()
        {
            if (firstTurn)
            {
                firstBattleships = new List<int>();
                Array.Copy(firstFleetAtStart, firstFleet, maxSizeOfField * maxSizeOfField);
                if (CheckFirstFleet())
                {
                    for (int i = 0; i < maxSizeOfField; i++)
                    {
                        for (int j = 0; j < maxSizeOfField; j++)
                        {
                            firstField[i, j].Background = Brushes.Aqua;
                        }
                    }

                    MessageBox.Text = "";
                    firstTurn = false;
                }

                else
                {
                    MessageBox.Text = "WRONG\nPLACEMENT,\nTRY AGAIN";
                }
            }
            else
            {
                secondBattleships = new List<int>();
                Array.Copy(secondFleetAtStart, secondFleet, maxSizeOfField * maxSizeOfField);
                if (CheckSecondFleet())
                {
                    for (int i = 0; i < maxSizeOfField; i++)
                    {
                        for (int j = 0; j < maxSizeOfField; j++)
                        {
                            secondField[i, j].Background = Brushes.Aqua;
                        }
                    }

                    MessageBox.Text = "";
                    firstTurn = true;
                    startOfGame = false;
                }

                else
                {
                    MessageBox.Text = "WRONG\nPLACEMENT,\nTRY AGAIN";
                }
            }
        }

        private static void ColorForLooser(int[,] fleet, Button[,] field)
        {
            for (int i = 0; i < maxSizeOfField; i++)
            {
                for (int j = 0; j < maxSizeOfField; j++)
                {
                    if (fleet[i, j] > 0)
                    {
                        field[i, j].Background = Brushes.Green;
                    }
                }
            }
        }
    }
}
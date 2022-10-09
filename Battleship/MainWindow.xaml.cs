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
        public static int[,] firstFleet = new int[maxSizeOfField, maxSizeOfField];
        public static int[,] secondFleet = new int[maxSizeOfField, maxSizeOfField];
        private Button[,] firstField = new Button[maxSizeOfField, maxSizeOfField];
        private Button[,] secondField = new Button[maxSizeOfField, maxSizeOfField];
        private bool firstTurn = true;
        public static List<int> firstBattleships = new List<int>();
        public static List<int> secondBattleships = new List<int>();

        public MainWindow()
        {
            InitializeComponent();

            //Create a field for first player
            for (int i = 0; i < maxSizeOfField; i++)
            {
                for (int j = 0; j < maxSizeOfField; j++)
                {
                    Button button = new Button();
                    button.Click += FirstClick;
                    Grid.SetColumn(button, j);
                    Grid.SetRow(button, i);
                    button.Width = 50;
                    button.Height = 50;
                    button.Background = Brushes.Aqua;
                    firstField[i, j] = button;
                    MyGrid.Children.Add(button);
                }
            }

            //Create a field for second player
            for (int i = 0; i < maxSizeOfField; i++)
            {
                for (int j = maxSizeOfField + 1; j < maxSizeOfField * 2 + 1; j++)
                {
                    Button button = new Button();
                    button.Click += SecondClick;
                    Grid.SetColumn(button, j);
                    Grid.SetRow(button, i);
                    button.Width = 50;
                    button.Height = 50;
                    button.Background = Brushes.Aqua;
                    secondField[i, j - 1 - maxSizeOfField] = button;
                    MyGrid.Children.Add(button);
                }
            }

            //Create empty fleets
            for (int i = 0; i < maxSizeOfField; i++)
            {
                for (int j = 0; j < maxSizeOfField; j++)
                {
                    firstFleet[i, j] = 0;
                    secondFleet[i, j] = 0;
                }
            }
        }

        private void FirstClick(object? sender, RoutedEventArgs e)
        {
            if (firstTurn)
            {
                var thatButton = (Button)sender!;

                if (startOfGame)
                {
                    SetFirstBattleships(thatButton);
                }
            }
        }

        private void SecondClick(object? sender, RoutedEventArgs e)
        {
            if (!firstTurn)
            {
                var thatButton = (Button)sender!;

                if (startOfGame)
                {
                    SetSecondBattleships(secondFleet, thatButton);
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

        private void StartCheck()
        {
            if (firstTurn)
            {
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
    }
}
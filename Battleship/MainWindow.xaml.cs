using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
using static Battleship.Bot;

namespace Battleship
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const int maxSizeOfField = 10;
        private bool startOfGame = true;
        private bool isSomeoneWon = false;
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
        private static bool bot = false;

        public MainWindow()
        {
            InitializeComponent();
            
            SetField();

            CreateFleets();
            
            CreateSigns();
        }

        private void FirstClick(object? sender, RoutedEventArgs e)
        {
            if (firstTurn && !isSomeoneWon)
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
                        isSomeoneWon = true;
                        MessageBox.Text = "FIRST PLAYER WON!";
                        ColorForLooser(firstFleet, secondField);
                        TurnBox.Text = "";
                    }
                    else if (!firstTurn)
                    {
                        TurnBox.Text = "Second turn";
                    }

                    if (bot)
                    {
                        BotClick(sender, e);
                    }
                }
            }
        }

        private void SecondClick(object? sender, RoutedEventArgs e)
        {
            if (!firstTurn && !isSomeoneWon)
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
                        isSomeoneWon = true;
                        MessageBox.Text = "SECOND PLAYER WON!";
                        ColorForLooser(secondFleet, firstField);
                        TurnBox.Text = "";
                    }
                    else if (firstTurn)
                    {
                        TurnBox.Text = "First turn";
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
            isSomeoneWon = false;
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
            MessageBox.Text = "";
            TurnBox.Text = "First turn";
            bot = false;
            BotButton.Visibility = Visibility.Visible;
            
            InitializeComponent();

            SetField();

            CreateFleets();
            
            CreateSigns();
        }

        private void StartCheck()
        {
            if (firstTurn)
            {
                FirstTurn();
                
                if (!firstTurn && bot)
                {
                    SecondTurn();
                }
            }
            else
            {
                SecondTurn();
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

        private void SetField()
        {
            for (int i = 0; i < maxSizeOfField; i++)
            {
                for (int j = 0; j < maxSizeOfField; j++)
                {
                    //Create a field for first player
                    Button button1 = new Button();
                    button1.Click += FirstClick;
                    Grid.SetColumn(button1, j);
                    Grid.SetRow(button1, i);
                    button1.Width = 50;
                    button1.Height = 50;
                    button1.Background = Brushes.Aqua;
                    firstField[i, j] = button1;
                    MyGrid1.Children.Add(button1);
                    
                    //Create a field for second player
                    Button button2 = new Button();
                    button2.Click += SecondClick;
                    Grid.SetColumn(button2, j);
                    Grid.SetRow(button2, i);
                    button2.Width = 50;
                    button2.Height = 50;
                    button2.Background = Brushes.Aqua;
                    secondField[i, j] = button2;
                    MyGrid2.Children.Add(button2);
                }
            }
        }

        private void CreateFleets()
        {
            for (int i = 0; i < maxSizeOfField; i++)
            {
                for (int j = 0; j < maxSizeOfField; j++)
                {
                    firstFleetAtStart[i, j] = 0;
                    secondFleetAtStart[i, j] = 0;
                }
            }
        }

        private void CreateSigns()
        {
            //Create horizontal signs
            for (int i = 0; i < maxSizeOfField; i++)
            {
                TextBlock textBlock1 = new TextBlock();
                textBlock1.Text = ((char)(i + 65)).ToString();
                textBlock1.HorizontalAlignment = HorizontalAlignment.Center;
                textBlock1.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetRow(textBlock1, 0);
                Grid.SetColumn(textBlock1, i);
                TextBlock textBlock2 = new TextBlock();
                textBlock2.Text = ((char)(i + 65)).ToString();
                textBlock2.HorizontalAlignment = HorizontalAlignment.Center;
                textBlock2.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetRow(textBlock2, 0);
                Grid.SetColumn(textBlock2, i + maxSizeOfField + 1);
                MyGrid3.Children.Add(textBlock1);
                MyGrid3.Children.Add(textBlock2);
            }
            
            //Create vertical signs
            for (int i = 0; i < maxSizeOfField; i++)
            {
                TextBlock textBlock1 = new TextBlock();
                textBlock1.Text = (maxSizeOfField - i).ToString();
                textBlock1.HorizontalAlignment = HorizontalAlignment.Center;
                textBlock1.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetRow(textBlock1, i);
                Grid.SetColumn(textBlock1, 0);
                MyGrid4.Children.Add(textBlock1);
            }
        }

        private void FirstTurn()
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

                BotButton.Visibility = Visibility.Collapsed;
                MessageBox.Text = "";
                firstTurn = false;
                TurnBox.Text = "Second turn";
            }

            else
            {
                MessageBox.Text = "WRONG PLACEMENT, TRY AGAIN";
            }
        }

        private void SecondTurn()
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
                TurnBox.Text = "First turn";
                startOfGame = false;
            }

            else
            {
                MessageBox.Text = "WRONG PLACEMENT, TRY AGAIN";
            }
        }

        [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH", MessageId = "type: System.Random")]
        [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH", MessageId = "type: XoshiroImpl; size: 3733MB")]
        public void BotClick(object? sender, RoutedEventArgs e)
        {
            bot = true;
            BotButton.Visibility = Visibility.Collapsed;
            
            if (startOfGame)
            {
                CreateBotFleet();
            }
            else
            {
                while (!firstTurn)
                {
                    var randomRow = new Random().Next(maxSizeOfField);
                    var randomColumn = new Random().Next(maxSizeOfField);
                    
                    SecondClick(secondField[randomRow, randomColumn], e);
                }
            }
        }

        public static void NullClick(object? sender, RoutedEventArgs e)
        {
        }
    }
}
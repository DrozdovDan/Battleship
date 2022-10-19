namespace Battleship;

using static MainWindow;
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
using static Battleship.SecondTurnClass;

public class Bot
{
    public static List<int> ships = new List<int> { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 };

    public static void CreateBotFleet()
    {
        foreach (var ship in ships)
        {
            TrySet(ship);
        }
    }

    private static void TrySet(int ship)
    {
        int isVertical = new Random().Next(2);

        if (isVertical == 1)
        {
            TryVerticalSet(ship);
        }
        else
        {
            TryHorizontalSet(ship);
        }
    }

    private static void TryVerticalSet(int ship)
    {
        int randomRow = new Random().Next(maxSizeOfField - ship + 1);
        int randomColumn = new Random().Next(maxSizeOfField);

        while (!CheckForVertical(ship, randomColumn, randomRow))
        {
            randomRow = new Random().Next(maxSizeOfField - ship + 1);
            randomColumn = new Random().Next(maxSizeOfField);
        }

        SetVertical(ship, randomColumn, randomRow);
    }

    private static bool CheckForVertical(int ship, int randomColumn, int randomRow)
    {
        for (int i = 0; i < maxSizeOfField; i++)
        {
            for (int j = 0; j < maxSizeOfField; j++)
            {
                if (i >= randomRow - 1 && i <= randomRow + ship && j >= randomColumn - 1 && j <= randomColumn + 1 &&
                    secondFleetAtStart[i, j] > 0)
                    return false;
            }
        }

        return true;
    }

    private static void SetVertical(int ship, int randomColumn, int randomRow)
    {
        for (int i = randomRow; i < randomRow + ship; i++)
        {
            secondFleetAtStart[i, randomColumn] = 1;
        }
    }

    private static void TryHorizontalSet(int ship)
    {
        int randomRow = new Random().Next(maxSizeOfField);
        int randomColumn = new Random().Next(maxSizeOfField - ship + 1);

        while (!CheckForHorizontal(ship, randomColumn, randomRow))
        {
            randomRow = new Random().Next(maxSizeOfField);
            randomColumn = new Random().Next(maxSizeOfField - ship + 1);
        }

        SetHorizontal(ship, randomColumn, randomRow);
    }

    private static bool CheckForHorizontal(int ship, int randomColumn, int randomRow)
    {
        for (int i = 0; i < maxSizeOfField; i++)
        {
            for (int j = 0; j < maxSizeOfField; j++)
            {
                if (i >= randomRow - 1 && i <= randomRow + 1 && j >= randomColumn - 1 && j <= randomColumn + ship &&
                    secondFleetAtStart[i, j] > 0)
                    return false;
            }
        }

        return true;
    }

    private static void SetHorizontal(int ship, int randomColumn, int randomRow)
    {
        for (int i = randomColumn; i < randomColumn + ship; i++)
        {
            secondFleetAtStart[randomRow, i] = 1;
        }
    }
}
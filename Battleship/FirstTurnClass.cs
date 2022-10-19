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

public static class FirstTurnClass
{
    public static void SetFirstBattleships(Button thatButton)
    {
        if (thatButton.Background == Brushes.Aqua)
        {
            thatButton.Background = Brushes.Red;
            firstFleetAtStart[Grid.GetRow(thatButton), Grid.GetColumn(thatButton)] = 1;
        }
        else
        {
            thatButton.Background = Brushes.Aqua;
            firstFleetAtStart[Grid.GetRow(thatButton), Grid.GetColumn(thatButton)] = 0;
        }
    }

    public static bool CheckFirstFleet()
    {
        for (int i = 0; i < maxSizeOfField; i++)
        {
            for (int j = 0; j < maxSizeOfField; j++)
            {
                if (firstFleet[i, j] == 1)
                {
                    //Check wrong placement
                    if (i > 0 && j > 0 &&
                        (firstFleet[i - 1, j - 1] > 0 || firstFleet[i - 1, j] > 0 && firstFleet[i, j - 1] > 0))
                        return false;
                    if (i > 0 && j < maxSizeOfField - 1 && (firstFleet[i - 1, j + 1] > 0 ||
                                                            firstFleet[i - 1, j] > 0 && firstFleet[i, j + 1] > 0))
                        return false;
                    if (i < maxSizeOfField - 1 && j > 0 && (firstFleet[i + 1, j - 1] > 0 ||
                                                            firstFleet[i + 1, j] > 0 && firstFleet[i, j - 1] > 0))
                        return false;
                    if (i < maxSizeOfField - 1 && j < maxSizeOfField - 1 && (firstFleet[i + 1, j + 1] > 0 ||
                                                                             firstFleet[i + 1, j] > 0 &&
                                                                             firstFleet[i, j + 1] > 0)) return false;
                    CreateShip(i, j);
                }
            }
        }

        if (firstBattleships.Count(x => x == 4) != 1) return false;
        if (firstBattleships.Count(x => x == 3) != 2) return false;
        if (firstBattleships.Count(x => x == 2) != 3) return false;
        if (firstBattleships.Count(x => x == 1) != 4) return false;

        return true;
    }

    private static void CreateShip(int i, int j)
    {
        firstBattleships.Add(0);
        for (int x = i; x < maxSizeOfField; x++)
        {
            if (firstFleet[x, j] == 0) break;
            firstBattleships[^1] += 1;
        }

        for (int x = i; x < maxSizeOfField; x++)
        {
            if (firstFleet[x, j] == 0) break;
            firstFleet[x, j] = firstBattleships[^1];
        }

        for (int x = j + 1; x < maxSizeOfField; x++)
        {
            if (firstFleet[i, x] == 0) break;
            firstBattleships[^1] += 1;
        }

        for (int x = j + 1; x < maxSizeOfField; x++)
        {
            if (firstFleet[i, x] == 0) break;
            firstFleet[i, x] = firstBattleships[^1];
        }
        
        firstFleet[i, j] = firstBattleships[^1];
    }

    public static void BeatFirst(Button thatButton)
    {
        if (thatButton.Background == Brushes.Aqua)
        {
            if (secondFleet[Grid.GetRow(thatButton), Grid.GetColumn(thatButton)] > 0)
            {
                thatButton.Background = Brushes.Red;
                thatButton.IsHitTestVisible = false;

                str = "You beat a ship";

                if (CheckKilledShip(thatButton))
                {
                    ColorKilledShip(thatButton);
                }
            }
            else
            {
                firstTurn = false;
                
                thatButton.Background = Brushes.Blue;
                thatButton.IsHitTestVisible = false;
                
                str = "You missed";
            }
        }
    }

    private static bool CheckKilledShip(Button thatButton)
    {
        var countOfBlocks = 1;

        countOfBlocks += CheckForDown(thatButton);

        countOfBlocks += CheckForUp(thatButton);

        countOfBlocks += CheckForRight(thatButton);

        countOfBlocks += CheckForLeft(thatButton);

        var returnValue = CheckIfShipFullDamaged(thatButton, countOfBlocks);

        return returnValue;
    }

    private static int CheckForDown(Button thatButton)
    {
        var countOfBlocks = 0;
        
        for (int i = Grid.GetRow(thatButton) + 1; i < maxSizeOfField; i++)
        {
            if (secondFleet[i, Grid.GetColumn(thatButton)] > 0 &&
                firstField[i, Grid.GetColumn(thatButton)].Background == Brushes.Red)
            {
                countOfBlocks += 1;
            }
            else
            {
                break;
            }
        }

        return countOfBlocks;
    }

    private static int CheckForUp(Button thatButton)
    {
        var countOfBlocks = 0;

        for (int i = Grid.GetRow(thatButton) - 1; i >= 0; i--)
        {
            if (secondFleet[i, Grid.GetColumn(thatButton)] > 0 &&
                firstField[i, Grid.GetColumn(thatButton)].Background == Brushes.Red)
            {
                countOfBlocks += 1;
            }
            else
            {
                break;
            }
        }

        return countOfBlocks;
    }

    private static int CheckForRight(Button thatButton)
    {
        var countOfBlocks = 0;

        for (int i = Grid.GetColumn(thatButton) + 1; i < maxSizeOfField; i++)
        {
            if (secondFleet[Grid.GetRow(thatButton), i] > 0 &&
                firstField[Grid.GetRow(thatButton), i].Background == Brushes.Red)
            {
                countOfBlocks += 1;
            }
            else
            {
                break;
            }
        }

        return countOfBlocks;
    }

    private static int CheckForLeft(Button thatButton)
    {
        var countOfBlocks = 0;
        
        for (int i = Grid.GetColumn(thatButton) - 1; i >= 0; i--)
        {
            if (secondFleet[Grid.GetRow(thatButton), i] > 0 &&
                firstField[Grid.GetRow(thatButton), i].Background == Brushes.Red)
            {
                countOfBlocks += 1;
            }
            else
            {
                break;
            }
        }

        return countOfBlocks;
    }

    private static bool CheckIfShipFullDamaged(Button thatButton, int countOfBlocks)
    {
        if (countOfBlocks == secondFleet[Grid.GetRow(thatButton), Grid.GetColumn(thatButton)])
        {
            secondBattleships.Remove(countOfBlocks);
            switch (countOfBlocks)
            {
                case 4:
                    str = "You killed a battleship";
                    break;
                case 3:
                    str = "You killed a cruiser";
                    break;
                case 2:
                    str = "You killed a destroyer";
                    break;
                case 1:
                    str = "You killed a torpedo boat";
                    break;
            }

            return true;
        }

        return false;
    }

    private static void ColorKilledShip(Button thatButton)
    {
        thatButton.Background = Brushes.Gold;

        ColorForDown(thatButton);

        ColorForUp(thatButton);

        ColorForRight(thatButton);

        ColorForLeft(thatButton);
    }

    private static void ColorForDown(Button thatButton)
    {
        for (int i = Grid.GetRow(thatButton) + 1; i < maxSizeOfField; i++)
        {
            if (secondFleet[i, Grid.GetColumn(thatButton)] > 0)
            {
                firstField[i, Grid.GetColumn(thatButton)].Background = Brushes.Gold;
            }
            else
            {
                break;
            }
        }
    }

    private static void ColorForUp(Button thatButton)
    {
        for (int i = Grid.GetRow(thatButton) - 1; i >= 0; i--)
        {
            if (secondFleet[i, Grid.GetColumn(thatButton)] > 0)
            {
                firstField[i, Grid.GetColumn(thatButton)].Background = Brushes.Gold;
            }
            else
            {
                break;
            }
        }
    }

    private static void ColorForRight(Button thatButton)
    {
        for (int i = Grid.GetColumn(thatButton) + 1; i < maxSizeOfField; i++)
        {
            if (secondFleet[Grid.GetRow(thatButton), i] > 0)
            {
                firstField[Grid.GetRow(thatButton), i].Background = Brushes.Gold;
            }
            else
            {
                break;
            }
        }
    }

    private static void ColorForLeft(Button thatButton)
    {
        for (int i = Grid.GetColumn(thatButton) - 1; i >= 0; i--)
        {
            if (secondFleet[Grid.GetRow(thatButton), i] > 0)
            {
                firstField[Grid.GetRow(thatButton), i].Background = Brushes.Gold;
            }
            else
            {
                break;
            }
        }
    }
}
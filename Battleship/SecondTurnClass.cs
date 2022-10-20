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

public static class SecondTurnClass
{
    public static void SetSecondBattleships(Button thatButton)
    {
        if (thatButton.Background == Brushes.Aqua)
        {
            thatButton.Background = Brushes.Red;
            secondFleetAtStart[Grid.GetRow(thatButton), Grid.GetColumn(thatButton)] = 1;
        }
        else
        {
            thatButton.Background = Brushes.Aqua;
            secondFleetAtStart[Grid.GetRow(thatButton), Grid.GetColumn(thatButton)] = 0;
        }
    }

    public static bool CheckSecondFleet()
    {
        for (int i = 0; i < maxSizeOfField; i++)
        {
            for (int j = 0; j < maxSizeOfField; j++)
            {
                if (secondFleet[i, j] == 1)
                {
                    //Check wrong placement
                    if (i > 0 && j > 0 &&
                        (secondFleet[i - 1, j - 1] > 0 || secondFleet[i - 1, j] == 1 && secondFleet[i, j - 1] > 0))
                        return false;
                    if (i > 0 && j < maxSizeOfField - 1 && (secondFleet[i - 1, j + 1] > 0 ||
                                                            secondFleet[i - 1, j] > 0 && secondFleet[i, j + 1] > 0))
                        return false;
                    if (i < maxSizeOfField - 1 && j > 0 && (secondFleet[i + 1, j - 1] > 0 ||
                                                            secondFleet[i + 1, j] > 0 && secondFleet[i, j - 1] > 0))
                        return false;
                    if (i < maxSizeOfField - 1 && j < maxSizeOfField - 1 && (secondFleet[i + 1, j + 1] > 0 ||
                                                                             secondFleet[i + 1, j] > 0 &&
                                                                             secondFleet[i, j + 1] > 0)) return false;
                    CreateShip(i, j);
                }
            }
        }

        if (secondBattleships.Count(x => x == 4) != 1) return false;
        if (secondBattleships.Count(x => x == 3) != 2) return false;
        if (secondBattleships.Count(x => x == 2) != 3) return false;
        if (secondBattleships.Count(x => x == 1) != 4) return false;

        return true;
    }

    private static void CreateShip(int i, int j)
    {
        secondBattleships.Add(0);
        for (int x = i; x < maxSizeOfField; x++)
        {
            if (secondFleet[x, j] == 0) break;
            secondBattleships[^1] += 1;
        }

        for (int x = i; x < maxSizeOfField; x++)
        {
            if (secondFleet[x, j] == 0) break;
            secondFleet[x, j] = secondBattleships[^1];
        }

        for (int x = j + 1; x < maxSizeOfField; x++)
        {
            if (secondFleet[i, x] == 0) break;
            secondBattleships[^1] += 1;
        }

        for (int x = j + 1; x < maxSizeOfField; x++)
        {
            if (secondFleet[i, x] == 0) break;
            secondFleet[i, x] = secondBattleships[^1];
        }

        secondFleet[i, j] = secondBattleships[^1];
    }

    public static void BeatSecond(Button thatButton)
    {
        if (thatButton.Background == Brushes.Aqua)
        {
            if (firstFleet[Grid.GetRow(thatButton), Grid.GetColumn(thatButton)] > 0)
            {
                thatButton.Background = Brushes.Red;
                thatButton.IsHitTestVisible = false;
                thatButton.Click += NullClick;

                str = "You beat a ship";

                if (CheckKilledShip(thatButton))
                {
                    ColorKilledShip(thatButton);
                }
            }
            else
            {
                firstTurn = true;

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
            if (firstFleet[i, Grid.GetColumn(thatButton)] > 0 &&
                secondField[i, Grid.GetColumn(thatButton)].Background == Brushes.Red)
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
            if (firstFleet[i, Grid.GetColumn(thatButton)] > 0 &&
                secondField[i, Grid.GetColumn(thatButton)].Background == Brushes.Red)
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
            if (firstFleet[Grid.GetRow(thatButton), i] > 0 &&
                secondField[Grid.GetRow(thatButton), i].Background == Brushes.Red)
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
            if (firstFleet[Grid.GetRow(thatButton), i] > 0 &&
                secondField[Grid.GetRow(thatButton), i].Background == Brushes.Red)
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
        if (countOfBlocks == firstFleet[Grid.GetRow(thatButton), Grid.GetColumn(thatButton)])
        {
            firstBattleships.Remove(countOfBlocks);
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
            if (firstFleet[i, Grid.GetColumn(thatButton)] > 0)
            {
                secondField[i, Grid.GetColumn(thatButton)].Background = Brushes.Gold;
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
            if (firstFleet[i, Grid.GetColumn(thatButton)] > 0)
            {
                secondField[i, Grid.GetColumn(thatButton)].Background = Brushes.Gold;
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
            if (firstFleet[Grid.GetRow(thatButton), i] > 0)
            {
                secondField[Grid.GetRow(thatButton), i].Background = Brushes.Gold;
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
            if (firstFleet[Grid.GetRow(thatButton), i] > 0)
            {
                secondField[Grid.GetRow(thatButton), i].Background = Brushes.Gold;
            }
            else
            {
                break;
            }
        }
    }
}
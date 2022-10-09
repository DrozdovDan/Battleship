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
    public static void SetSecondBattleships(int[,] fleet, Button thatButton)
    {
        if (thatButton.Background == Brushes.Aqua)
        {
            thatButton.Background = Brushes.Red;
            fleet[Grid.GetRow(thatButton), Grid.GetColumn(thatButton) - 1 - maxSizeOfField] = 1;
        }
        else
        {
            thatButton.Background = Brushes.Aqua;
            fleet[Grid.GetRow(thatButton), Grid.GetColumn(thatButton) - 1 - maxSizeOfField] = 0;
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
                        (secondFleet[i - 1, j - 1] == 1 || secondFleet[i - 1, j] == 1 && secondFleet[i, j - 1] == 1))
                        return false;
                    if (i > 0 && j < maxSizeOfField - 1 && (secondFleet[i - 1, j + 1] == 1 ||
                                                            secondFleet[i - 1, j] == 1 && secondFleet[i, j + 1] == 1))
                        return false;
                    if (i < maxSizeOfField - 1 && j > 0 && (secondFleet[i + 1, j - 1] == 1 ||
                                                            secondFleet[i + 1, j] == 1 && secondFleet[i, j - 1] == 1))
                        return false;
                    if (i < maxSizeOfField - 1 && j < maxSizeOfField - 1 && (secondFleet[i + 1, j + 1] == 1 ||
                                                                             secondFleet[i + 1, j] == 1 &&
                                                                             secondFleet[i, j + 1] == 1)) return false;
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
            secondFleet[x, j] = secondBattleships[^1];
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
            secondFleet[i, x] = secondBattleships[^1];
        }

        for (int x = j + 1; x < maxSizeOfField; x++)
        {
            if (secondFleet[i, x] == 0) break;
            secondFleet[i, x] = secondBattleships[^1];
        }
    }
}
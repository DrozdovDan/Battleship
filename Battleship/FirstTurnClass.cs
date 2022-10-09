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
            firstFleet[Grid.GetRow(thatButton), Grid.GetColumn(thatButton)] = 1;
        }
        else
        {
            thatButton.Background = Brushes.Aqua;
            firstFleet[Grid.GetRow(thatButton), Grid.GetColumn(thatButton)] = 0;
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
                        (firstFleet[i - 1, j - 1] == 1 || firstFleet[i - 1, j] == 1 && firstFleet[i, j - 1] == 1))
                        return false;
                    if (i > 0 && j < maxSizeOfField - 1 && (firstFleet[i - 1, j + 1] == 1 ||
                                                            firstFleet[i - 1, j] == 1 && firstFleet[i, j + 1] == 1))
                        return false;
                    if (i < maxSizeOfField - 1 && j > 0 && (firstFleet[i + 1, j - 1] == 1 ||
                                                            firstFleet[i + 1, j] == 1 && firstFleet[i, j - 1] == 1))
                        return false;
                    if (i < maxSizeOfField - 1 && j < maxSizeOfField - 1 && (firstFleet[i + 1, j + 1] == 1 ||
                                                                             firstFleet[i + 1, j] == 1 && firstFleet[i, j + 1] == 1)) return false;
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
            firstFleet[x, j] = firstBattleships[^1];
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
            firstFleet[i, x] = firstBattleships[^1];
        }

        for (int x = j + 1; x < maxSizeOfField; x++)
        {
            if (firstFleet[i, x] == 0) break;
            firstFleet[i, x] = firstBattleships[^1];
        }
    }

    public static void BeatFirst(Button thatButton)
    {
        
    }
}
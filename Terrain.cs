using System;
using System.Collections.Generic;
using System.Text;

namespace LifeProjectAvalonia;

public class Terrain
{
    public event Action<CellField>? TurnFinished;

    public CellField Field { get; init; }

    //private List<Colony> colonies = new();

    public Terrain(int width, int height)
    {
        Field = new(width, height);
    }

    public void Iterate()
    {
        foreach (Cell cell in Field)
            cell.CalculateNextState();

        foreach (Cell cell in Field)
            cell.ToNextState();

        //UniteAll();

        /*foreach (Colony colony in colonies)
            colony.TryMove();*/

        TurnFinished?.Invoke(Field);
    }

    public void Randomize()
    {
        foreach (Cell cell in Field)
            if (Random.Shared.Next(4) == 0)
                cell.ToWhite();

        TurnFinished?.Invoke(Field);
    }

    /*public void StablePatternEncountered(List<Cell> stablePattern)
    {
        Colony born = new Colony(stablePattern, Field);

        colonies.Add(born);
    }
    private void UniteAll()
    {
        bool uniting = true;
        while (uniting)
        {
            uniting = false;
            foreach (Colony colony in colonies)
            {
                Cell? common = colony.NeighbourColonyCell();
                if (common != null)
                {
                    Colony neighbour = GetColony(common);
                    //if (neighbour == null) continue;
                    Unite(colony, neighbour);
                    uniting = true;
                    break;
                }
            }
        }
    }
    private void Unite(Colony A, Colony B)
    {
        colonies.Add(A + B);

        colonies.Remove(A);
        colonies.Remove(B);
    }
    private Colony GetColony(Cell member)
    {
        Colony? found = colonies.FirstOrDefault(colony => colony.Members.Contains(member));

        if (found == null) throw new NullReferenceException($"No Colony with {member} in");

        return found;
    }*/
}


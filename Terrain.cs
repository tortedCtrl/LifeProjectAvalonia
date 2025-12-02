using System;
using System.Collections.Generic;
using System.Text;

namespace LifeProjectAvalonia;

public class Terrain
{
    public event Action<CellField>? TurnFinished;

    public CellField Field { get; init; }

    private List<CellColony> colonies = new();

    public Terrain(int width, int height)
    {
        Field = new(width, height);
    }

    public void Iterate()
    {
        UniteAllColonies();

        foreach (Cell cell in Field)
            cell.CalculateNextState();

        foreach (Cell cell in Field)
            cell.ToNextState();

        colonies.ForEach(colony => colony.TryMove());

        TurnFinished?.Invoke(Field);
    }

    public void Randomize()
    {
        foreach (Cell cell in Field)
            if (Random.Shared.Next(4) == 0)
                cell.ToWhite();

        TurnFinished?.Invoke(Field);
    }

    public void StablePatternEncountered(List<Cell> stablePattern)
    {
        CellColony born = new CellColony(stablePattern, Field);

        colonies.Add(born);
    }

    private void UniteAllColonies()
    {
        bool uniting = true;
        while (uniting)
        {
            uniting = false;
            foreach (CellColony colony in colonies)
            {
                (bool found, Cell? common) = colony.CellInNeighbourColony();
                if (found)
                {
                    Unite(colony, common!.Colony);
                    uniting = true;
                    break;
                }
            }
        }
    }
    private void Unite(CellColony? A, CellColony? B)
    {
        if (A == null || B == null) return;

        colonies.Add(A + B);

        colonies.Remove(A);
        colonies.Remove(B);
    }
}


using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LifeProjectAvalonia;

public class Terrain : ITerrain
{
    public CellField Field { get; init; }

    private List<CellColony> colonies = new();

    private Action<CellField> _fieldPainter;

    public Terrain(int width, int height, Action<CellField> fieldPainter)
    {
        Field = new(width, height);

        _fieldPainter = fieldPainter != null ? fieldPainter : throw new NullReferenceException(nameof(fieldPainter));


    }

    public void MakeTurn()
    {
        UniteAllColonies();

        foreach (Cell cell in Field)
            cell.CalculateNextState();

        foreach (Cell cell in Field)
            cell.ToNextState();

        colonies.ForEach(colony => colony.TryMove());

        _fieldPainter?.Invoke(Field);
    }

    public void Randomize()
    {
        foreach (Cell cell in Field)
            if (Random.Shared.Next(4) == 0)
                cell.ToWhite();

        _fieldPainter(Field);
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


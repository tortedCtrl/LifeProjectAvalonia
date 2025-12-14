using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;

namespace LifeProjectAvalonia;

public class Terrain : ITerrain
{
    public CellField Field { get; init; }

    private List<CellColony> colonies = new();

    private Action<Cell> _cellPainter;
    private Action<Cell> _cellClearer;

    public Terrain(int width, int height, Action<Cell> fieldPainter, Action<Cell> fieldClearer)
    {
        Field = new(width, height);

        _cellPainter = fieldPainter != null ? fieldPainter : throw new NullReferenceException(nameof(fieldPainter));
        _cellClearer = fieldClearer != null ? fieldClearer : throw new NullReferenceException(nameof(fieldClearer));
    }

    public ITerrain SetWrappedTerrain(ITerrain newWrappedTerrain) //Создать отдельный интерфейс или абстракцию для декоратора, вместо реализации декораторами ITerrain
    {
        throw new NotImplementedException();
    }

    public void MakeTurn()
    {
        UniteAllColonies();

        foreach (Cell cell in Field)
            cell.CalculateNextState();

        foreach (Cell cell in Field)
            cell.ToNextState();

        colonies.ForEach(colony => colony.TryMove());

        foreach (Cell cell in Field.Where(cell => cell.State is Dead))
            _cellClearer(cell);
        foreach (Cell cell in Field.Where(cell => cell.State is not Dead))
            DrawCell(cell);
    }

    public void Randomize()
    {
        foreach (Cell cell in Field)
            if (Random.Shared.Next(7) == 0)
                cell.ToWhite();

        foreach (Cell cell in Field.Where(cell => cell.State is Dead))
            _cellClearer(cell);
        foreach (Cell cell in Field.Where(cell => cell.State is not Dead))
            DrawCell(cell);
    }

    public void StablePatternEncountered(List<Cell> stablePattern)
    {
        CellColony born = new CellColony(stablePattern, Field);

        colonies.Add(born);
    }
    public void DrawCell(Cell cell) =>
        _cellPainter?.Invoke(cell);


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
using System;
using System.Collections.Generic;
using System.Linq;

namespace LifeProjectAvalonia;

public class Terrain : ITerrain
{
    public CellField Field { get; init; }

    private Action<Cell> _cellPainter;
    private Action<Cell> _cellClearer;

    public Terrain(int width, int height, Action<Cell> fieldPainter, Action<Cell> fieldClearer)
    {
        Field = new(width, height);

        _cellPainter = fieldPainter != null ? fieldPainter : throw new NullReferenceException(nameof(fieldPainter));
        _cellClearer = fieldClearer != null ? fieldClearer : throw new NullReferenceException(nameof(fieldClearer));
    }

    public void MakeTurn()
    {
        foreach (Cell cell in Field)
            cell.CalculateNextState();

        foreach (Cell cell in Field)
            cell.ToNextState();

        foreach (Cell cell in Field)
            cell.State.OnStateChanged();

        Draw();
    }

    public void Randomize()
    {
        foreach (Cell cell in Field)
            if (Random.Shared.Next(7) == 0)
                cell.State = new White(cell);

        foreach (Cell cell in Field.Where(cell => cell.State is Dead))
            _cellClearer(cell);
        foreach (Cell cell in Field.Where(cell => cell.State is not Dead))
            DrawCell(cell);
    }

    public void StablePatternEncountered(List<Cell> stablePattern) { }
    public void Draw()
    {
        foreach (Cell cell in Field.Where(cell => cell.State is Dead))
            _cellClearer(cell);
        foreach (Cell cell in Field.Where(cell => cell.State is not Dead))
            DrawCell(cell);
    }
    public void DrawCell(Cell cell) =>
        _cellPainter?.Invoke(cell);

}
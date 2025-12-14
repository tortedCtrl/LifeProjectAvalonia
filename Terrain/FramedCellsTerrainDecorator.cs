using System;
using System.Collections.Generic;
using System.Linq;

namespace LifeProjectAvalonia;

public class FramedCellsTerrainDecorator : ITerrain
{
    private ITerrain _wrappedTerrain;

    public FramedCellsTerrainDecorator(ITerrain terrain)
    {
        _wrappedTerrain = terrain != null ? terrain : throw new NullReferenceException(nameof(terrain));
    }

    public CellField Field => _wrappedTerrain.Field;


    public void MakeTurn()
    {
        _wrappedTerrain.MakeTurn();
        foreach (Cell cell in Field.Where(cell => cell.State is Dead))
            DrawCell(cell);
    }


    public void Randomize()
    {
        _wrappedTerrain.Randomize();
        foreach (Cell cell in Field)
            DrawCell(cell);
    }

    public void StablePatternEncountered(List<Cell> pattern) =>
        _wrappedTerrain.StablePatternEncountered(pattern);

    public void DrawCell(Cell cell) =>
        _wrappedTerrain.DrawCell(cell);

    public ITerrain SetWrappedTerrain(ITerrain newWrappedTerrain)
    {
        var prev = _wrappedTerrain;
        _wrappedTerrain = newWrappedTerrain;
        return prev;
    }

    public void Draw() => _wrappedTerrain.Draw();
}

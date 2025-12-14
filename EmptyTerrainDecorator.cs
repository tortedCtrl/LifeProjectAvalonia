using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeProjectAvalonia;

internal class EmptyTerrainDecorator : ITerrain
{
    private ITerrain _wrappedTerrain;

    public EmptyTerrainDecorator(ITerrain wrappingTerrain)
    {
        _wrappedTerrain = wrappingTerrain;
    }

    public CellField Field => _wrappedTerrain.Field;

    public void Draw() => _wrappedTerrain.Draw();

    public void DrawCell(Cell cell) => _wrappedTerrain.DrawCell(cell);

    public void MakeTurn() => _wrappedTerrain.MakeTurn();

    public void Randomize() => _wrappedTerrain.Randomize();

    public ITerrain? SetWrappedTerrain(ITerrain newWrappedTerrain)
    {
        var prev = _wrappedTerrain;
        _wrappedTerrain = newWrappedTerrain;
        return prev;
    }

    public void StablePatternEncountered(List<Cell> pattern) => _wrappedTerrain.StablePatternEncountered(pattern);
}


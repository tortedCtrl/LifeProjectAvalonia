using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeProjectAvalonia;

internal class FramedCellsTerrainDecorator : ITerrain
{
    private ITerrain _wrappedTerrain;
    public FramedCellsTerrainDecorator(ITerrain terrain)
    {
        _wrappedTerrain = terrain != null ? terrain : throw new NullReferenceException(nameof(terrain));

        //_fieldPainter = favortites.PaintField;
    }
    public CellField Field => _wrappedTerrain.Field;


    public void MakeTurn()
    {
        throw new NotImplementedException();
    }

    public void Randomize()
    {
        throw new NotImplementedException();
    }

    public void StablePatternEncountered(List<Cell> pattern)
    {
        throw new NotImplementedException();
    }

    public void Draw()
    {
        throw new NotImplementedException();
    }
}

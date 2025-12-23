using System;
using System.Collections.Generic;

namespace LifeProjectAvalonia;

public abstract class TerrainDecorator : ITerrain
{
    private ITerrain _wrappedTerrain;

    public TerrainDecorator(ITerrain wrappingEntity)
    {
        if (wrappingEntity == null) 
            throw new NullReferenceException(nameof(wrappingEntity)); // ??=
        _wrappedTerrain = wrappingEntity;
    }

    public virtual CellField Field { get => _wrappedTerrain.Field; } 
    public virtual void MakeTurn()  => _wrappedTerrain.MakeTurn();
    public virtual void Draw() => _wrappedTerrain.Draw();
    public virtual void Randomize() => _wrappedTerrain.Randomize();
    public virtual void DrawCell(Cell cell) => _wrappedTerrain.DrawCell(cell);
    public virtual void StablePatternEncountered(List<Cell> pattern) => _wrappedTerrain.StablePatternEncountered(pattern);

    public ITerrain SetWrappedDecorator(ITerrain newWrappedTerrain)
    {
        ITerrain previousTerrain = _wrappedTerrain;
        _wrappedTerrain = newWrappedTerrain;
        return previousTerrain;
    }    
}
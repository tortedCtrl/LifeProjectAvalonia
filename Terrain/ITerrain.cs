using System.Collections.Generic;

namespace LifeProjectAvalonia;

public interface ITerrain
{
    public CellField Field {get; }
    public void MakeTurn();
    public void Randomize();
    public void StablePatternEncountered(List<Cell> pattern);
    public void Draw();
    public void DrawCell(Cell cell);
}

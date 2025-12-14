using System.Collections.Generic;

namespace LifeProjectAvalonia;

public class ColoniesTerrainDecorator : ITerrain
{
    private ITerrain _wrappedTerrain;

    private List<CellColony> colonies = new();

    public ColoniesTerrainDecorator(ITerrain wrappedTerrain)
    {
        _wrappedTerrain = wrappedTerrain;
    }

    public CellField Field => _wrappedTerrain.Field;

    private void UniteAllColonies()
    {
        bool uniting = true;
        while (uniting)
        {
            uniting = false;
            foreach (CellColony colony in colonies)
            {
                (bool found, Cell? common, CellColony? neighbourColony) = colony.CellInNeighbourColony();

                if (found && colonies.Contains(neighbourColony) == false) continue; // НА ИСПРАВЛЕНИЕ, ДОЛЖНО РАБОТАТЬ БЕЗ ПРОВЕРКИ
                
                if (found)
                {
                    Unite(colony, neighbourColony);
                    uniting = true;
                    break;
                }
            }
        }
    }
    private void Unite(CellColony? A, CellColony? B)
    {
        if (A == null || B == null)
            return;

        _ = colonies.Count;

        colonies.Add(A + B);

        _ = colonies.Count;
        colonies.Remove(A);
        colonies.Remove(B);
        _ = colonies.Count;
    }

    public ITerrain SetWrappedTerrain(ITerrain newWrappedTerrain)
    {
        var prev = _wrappedTerrain;
        _wrappedTerrain = newWrappedTerrain;
        return prev;
    }

    public void MakeTurn()
    {
        UniteAllColonies();

        foreach (Cell cell in Field)
            cell.CalculateNextState();

        foreach (Cell cell in Field)
            cell.ToNextState();

        foreach (Cell cell in Field)
            cell.State.OnStateChanged();

        colonies.ForEach(colony => colony.TryMove());

        _wrappedTerrain.Draw();
    }

    public void Randomize() => _wrappedTerrain.Randomize();

    public void StablePatternEncountered(List<Cell> stablePattern)
    {
        CellColony born = new CellColony(stablePattern, Field);

        colonies.Add(born);
    }
    public void Draw() => _wrappedTerrain.Draw();

    public void DrawCell(Cell cell) => _wrappedTerrain.DrawCell(cell);
}

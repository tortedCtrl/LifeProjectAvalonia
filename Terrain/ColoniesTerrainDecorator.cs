using System.Collections.Generic;

namespace LifeProjectAvalonia;

public class ColoniesTerrainDecorator : TerrainDecorator
{
    private List<CellColony> colonies = new();

    public ColoniesTerrainDecorator(ITerrain wrappedTerrain) : base(wrappedTerrain) { }

    public override void MakeTurn()
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

    public override void StablePatternEncountered(List<Cell> stablePattern)
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
                (bool found, Cell? common, CellColony? neighbourColony) = colony.CellInNeighbourColony();

                if (neighbourColony == null || found && colonies.Contains(neighbourColony) == false) continue; // НА ИСПРАВЛЕНИЕ, ДОЛЖНО РАБОТАТЬ БЕЗ ПРОВЕРКИ

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

        colonies.Add(A + B);

        colonies.Remove(A);
        colonies.Remove(B);
    }
}

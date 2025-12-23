using System.Linq;

namespace LifeProjectAvalonia;

public class FramedCellsTerrainDecorator : TerrainDecorator
{
    public FramedCellsTerrainDecorator(ITerrain wrappingTerrain) : base(wrappingTerrain) {}

    public override void MakeTurn()
    {
        base.MakeTurn();
        foreach (Cell cell in Field.Where(cell => cell.State is Dead))
            DrawCell(cell);
    }

    public override void Draw()
    {
        base.Draw();
        foreach (Cell cell in Field.Where(cell => cell.State is Dead))
            DrawCell(cell);
    }
    public override void Randomize()
    {
        base.Randomize();
        foreach (Cell cell in Field)
            DrawCell(cell);
    }
}

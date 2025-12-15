using System.Linq;

namespace LifeProjectAvalonia;

public class FramedCellsTerrainDecorator : TerrainDecorator
{
    public FramedCellsTerrainDecorator(ITerrain wrappingTerrain) : base(wrappingTerrain) {}

    public override void MakeTurn()
    {
        _wrappedTerrain.MakeTurn();
        foreach (Cell cell in Field.Where(cell => cell.State is Dead))
            DrawCell(cell);
    }

    public override void Draw()
    {
        _wrappedTerrain.Draw();
        foreach (Cell cell in Field.Where(cell => cell.State is Dead))
            DrawCell(cell);
    }
    public override void Randomize()
    {
        _wrappedTerrain.Randomize();
        foreach (Cell cell in Field)
            DrawCell(cell);
    }
}

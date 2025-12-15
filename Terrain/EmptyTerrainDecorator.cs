namespace LifeProjectAvalonia;

public class EmptyTerrainDecorator : TerrainDecorator
{
    public EmptyTerrainDecorator(ITerrain wrappingEntity) : base(wrappingEntity) { }
}


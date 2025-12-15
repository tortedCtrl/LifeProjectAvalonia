using System;
using System.Linq;

namespace LifeProjectAvalonia;

public class ClassicGameFactory : GameFactory
{
    private TerrainDecorator? _statistics1;
    private TerrainDecorator? _scanner;
    private TerrainDecorator? _statistics2;

    public ClassicGameFactory(StartData windowData, LifePagePresenter presenter) :
        base(windowData, presenter)
    { }

    protected override ITerrain CreateTerrain()
    {
        _terrain = new Terrain(_fieldWidth, _fieldHeight, _presenter.PaintBox, _presenter.ClearBox);
        (_statistics1, _scanner, _statistics2) = WrapTerrain();

        Dead.InjectLogic(DeadLogic);
        White.InjectLogic(WhiteLogic);

        return _terrain;

        (TerrainDecorator, TerrainDecorator, TerrainDecorator) WrapTerrain()
        {
            _statistics1 = new StatisticsTerrainDecorator(_terrain, _presenter, _presenter.UpdateTurnTime);
            _terrain = _statistics1;
            _terrain = new FramedCellsTerrainDecorator(_terrain);

            _scanner = new ScannerTerrainDecorator(_terrain, _windowData, ShowEmptyCells_FavoritesPresenter);
            _terrain = _scanner;
            _statistics2 = new StatisticsTerrainDecorator(_terrain, _presenter, _presenter.UpdateScantime, statAll: false);
            _terrain = _statistics2;
            _terrain = new FramedCellsTerrainDecorator(_terrain);
            _terrain = new EmptyTerrainDecorator(_terrain);

            return (_statistics1, _scanner, _statistics2);
        }
    }

    public CellState DeadLogic(Cell related)
    {
        int whiteNear = related.Neighbours.Count(cell => cell.State is White);
        if (whiteNear == 3)
            return new White(related);

        return new Dead(related);
    }
    public CellState WhiteLogic(Cell related)
    {
        int whiteNear = related.Neighbours.Count(cell => cell.State is White);
        if (whiteNear == 2 || whiteNear == 3)
            return new White(related);

        return new Dead(related);
    }

    public override void ShowEmptyCells_LifePresenter(bool show)
    {
        if (_scanner == null) throw new NullReferenceException(nameof(_scanner));
        if (_statistics1 == null) throw new NullReferenceException(nameof(_statistics1));

        if (show == false)
            _scanner!.SetWrappedTerrain(_statistics1!);
        else
            _scanner!.SetWrappedTerrain(new FramedCellsTerrainDecorator(_statistics1!));
    }

    public override void ShowEmptyCells_FavoritesPresenter(bool show)
    {
        if (_statistics2 == null) throw new NullReferenceException(nameof(_statistics2));

        if (_terrain is TerrainDecorator decorator)
        {
            if (show == false)
                Console.WriteLine(decorator.SetWrappedTerrain(_statistics2));
            else
                decorator.SetWrappedTerrain(new FramedCellsTerrainDecorator(_statistics2));
        }
        else
            throw new ArgumentException();
    }
}

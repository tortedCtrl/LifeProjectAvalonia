using System;
using System.Linq;

namespace LifeProjectAvalonia;

public class ColoniesGameFactory : GameFactory
{
    private ITerrain? _statistics1;
    private ITerrain? _scanner;
    private ITerrain? _statistics2;

    public ColoniesGameFactory(StartData windowData, LifePagePresenter presenter) 
        : base(windowData, presenter)
    {
    }
    protected override ITerrain CreateTerrain()
    {
        _terrain = new Terrain(_fieldWidth, _fieldHeight, _presenter.PaintBox, _presenter.ClearBox);
        (_statistics1, _scanner, _statistics2) = WrapTerrain();

        Dead.InjectLogic(DeadLogic);
        White.InjectLogic(WhiteLogic);
        Black.InjectLogic(BlackLogic);

        return _terrain;

        (ITerrain, ITerrain, ITerrain) WrapTerrain()
        {
            _terrain = new ColoniesTerrainDecorator(_terrain); //Дополнительно оборачивает декоратором с колониями
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
        int whiteNear = related.Neighbours.Where(cell => cell.State is White).Count();
        if (whiteNear == 3)
            return new White(related);

        return new Dead(related);
    }
    public CellState WhiteLogic(Cell related)
    {
        int whiteNear = related.Neighbours.Where(cell => cell.State is White).Count();
        int blackNear = related.Neighbours.Where(cell => cell.State is Black).Count();
        if (blackNear > whiteNear + 1)
        {
            Cell? cell = related.Neighbours.FirstOrDefault(cell => cell.State is Black);
            return new Black(related, CellColony.GetColony(cell).colony);
        }

        if (whiteNear == 2 || whiteNear == 3)
            return new White(related);

        return new Dead(related);
    }
    public CellState BlackLogic(Cell related)
    {
        int whiteNear = related.Neighbours.Where(cell => cell.State is White).Count();
        int blackNear = related.Neighbours.Where(cell => cell.State is Black).Count();
        if (blackNear + 0 < whiteNear)
            return new White(related);

        if (blackNear == 2 || blackNear == 3 || blackNear == 4)
            return new Black(related, CellColony.GetColony(related).colony);

        return new Dead(related);
    }


    public override void ShowEmptyCells_FavoritesPresenter(bool show)
    {
        if (_statistics2 == null) throw new NullReferenceException(nameof(_statistics2));

        if (show == false)
            _terrain!.SetWrappedTerrain(_statistics2!);
        else
            _terrain!.SetWrappedTerrain(new FramedCellsTerrainDecorator(_statistics2!));
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
}
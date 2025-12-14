using System;

namespace LifeProjectAvalonia;

public class ClassicGameFactory : GameFactory
{
    private ITerrain? _statistics1;
    private ITerrain? _scanner;
    private ITerrain? _statistics2;

    public ClassicGameFactory(StartData windowData, LifePagePresenter presenter)
        : base(windowData, presenter)
    {
    }
    protected override ITerrain CreateTerrain()
    {
        _terrain = new Terrain(_fieldWidth, _fieldHeight, _presenter.PaintBox, _presenter.ClearBox);
        (_statistics1, _scanner, _statistics2) = WrapTerrain();

        return _terrain;

        (ITerrain, ITerrain, ITerrain) WrapTerrain()
        {
            _statistics1 = new StatisticsTerrainDecorator(_terrain, _presenter, _presenter.UpdateTurnTime);
            _terrain = _statistics1;
            _terrain = new FramedCellsTerrainDecorator(_terrain);

            _scanner = new ScannerTerrainDecorator(_terrain, _windowData, ShowEmptyCells_FavoritesPresenter);
            _terrain = _scanner;
            _statistics2 = new StatisticsTerrainDecorator(_terrain, _presenter, _presenter.UpdateScantime, statAll: false);
            _terrain = _statistics2;
            _terrain = new FramedCellsTerrainDecorator(_terrain);

            return (_statistics1, _scanner, _statistics2);
        }
    }
    public override void ShowEmptyCells_FavoritesPresenter(bool show)
    {
        if (_statistics2 == null) throw new NullReferenceException(nameof(_statistics2));

        if (show == false)
            _terrain = _statistics2;
        else
            _terrain = new FramedCellsTerrainDecorator(_statistics2!);
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

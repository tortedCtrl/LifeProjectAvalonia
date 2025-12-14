using System;

namespace LifeProjectAvalonia;

public abstract class GameFactory
{
    protected ITerrain? _terrain;

    protected int _fieldWidth;
    protected int _fieldHeight;
    protected LifePagePresenter _presenter;
    protected StartData _windowData;

    protected GameFactory(StartData windowData, LifePagePresenter presenter)
    {
        _fieldWidth = windowData.width;
        _fieldHeight = windowData.height;
        _presenter = presenter;
        _windowData = windowData;
    }

    public ITerrain GetTerrain()
    {
        if (_terrain == null)
            _terrain = CreateTerrain();
        return _terrain;
    }
    protected abstract ITerrain CreateTerrain();

    public (Action<bool>, Action<bool>) GetFramingCellsActions() =>
        (ShowEmptyCells_LifePresenter, ShowEmptyCells_FavoritesPresenter);

    public abstract void ShowEmptyCells_LifePresenter(bool show);
    public abstract void ShowEmptyCells_FavoritesPresenter(bool show);
}

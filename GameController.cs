using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeProjectAvalonia;

public class GameController
{
    private ITerrain _terrain;

    private Action<bool> onFramingCells_LifePresenter;
    private Action<bool> onFramingCells_FavoritesPresenter;


    private bool _started = false;
    private bool _pause = false;

    private int _timeDelay = 600;

    public GameController(GameFactory factory, int timeDelay)
    {
        _terrain = factory.GetTerrain();
        (onFramingCells_LifePresenter, onFramingCells_FavoritesPresenter) = factory.GetFramingCellsActions();

        _timeDelay = timeDelay;
    }

    private int TimeDelay
    {
        get => _timeDelay;
        set => _timeDelay = Math.Clamp(value, 35, 5000);
    }


    public void ShowEmptyCells_LifePresenter(bool show) => onFramingCells_LifePresenter(show);
    public void ShowEmptyCells_FavoritesPresenter(bool show) => onFramingCells_FavoritesPresenter(show);

    public void Randomize() => _terrain.Randomize();

    public void ToggleGame()
    {
        if (_started)
        {
            _pause = !_pause;
            return;
        }

        _started = true;
        Game();
    }
    public async void Game()
    {
        while (true)
        {
            if (_pause == false)
                _terrain.MakeTurn();

            await Task.Delay(TimeDelay);
        }
    }
}


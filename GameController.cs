using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeProjectAvalonia;

public class GameController
{
    private ITerrain terrain;

    private bool _started = false;
    private int _timeDelay = 600;

    private bool _pause = false;


    private ITerrain statistics1;
    private ITerrain scanner;
    private ITerrain statistics2;

    public GameController(int _width, int _height, int timeDelay, LifePagePresenter presenter, StartData windowData)
    {
        if (_width < 1 || _height < 1) throw new ArgumentException("Width or Height <= 0");

        TimeDelay = timeDelay;

        terrain = new Terrain(_width, _height, presenter.PaintBox, presenter.ClearBox);
        (statistics1, scanner, statistics2) = WrapTerrain();

        return;

        (ITerrain, ITerrain, ITerrain) WrapTerrain()
        {
            statistics1 = new StatisticsTerrainDecorator(terrain, presenter, presenter.UpdateTurnTime);
            terrain = statistics1;
            terrain = new FramedCellsTerrainDecorator(terrain);

            scanner = new ScannerTerrainDecorator(terrain, windowData, ShowEmptyCells_FavoritesPresenter);
            terrain = scanner;
            statistics2 = new StatisticsTerrainDecorator(terrain, presenter, presenter.UpdateScantime, statAll: false);
            terrain = statistics2;
            terrain = new FramedCellsTerrainDecorator(terrain);
            return (statistics1, scanner, statistics2);
        }
    }

    private int TimeDelay
    {
        get => _timeDelay;
        set => _timeDelay = Math.Clamp(value, 35, 5000);
    }


    public void ShowEmptyCells_LifePresenter(bool show)
    {
        if (show == false)
            scanner.SetWrappedTerrain(statistics1);
        else
            scanner.SetWrappedTerrain(new FramedCellsTerrainDecorator(statistics1));
    }
    public void ShowEmptyCells_FavoritesPresenter(bool show)
    {
        if (show == false)
            terrain = statistics2;
        else
            terrain = new FramedCellsTerrainDecorator(statistics2);
    }
    public void Randomize() => terrain.Randomize();

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
                terrain.MakeTurn();

            await Task.Delay(TimeDelay);
        }
    }
}


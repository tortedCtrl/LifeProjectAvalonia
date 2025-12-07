using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeProjectAvalonia;

public class GameController
{
    public ITerrain terrain;

    private bool _started = false;
    private int _timeDelay = 600;

    private bool _pause = false;

    public GameController(int _width, int _height, int timeDelay, Action<Cell> lifeFormPainter, StartData windowData)
    {
        if (_width < 1 || _height < 1) throw new ArgumentException("Width or Height <= 0");

        TimeDelay = timeDelay;
        terrain = new Terrain(_width, _height, lifeFormPainter);

        terrain = new ScannerTerrainDecorator(terrain, windowData);
    }

    public void ToggleGame()
    {
        if (_started == false)
        {
            _started = true;
            Game();
            return;
        }

        _pause = !_pause;
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

    private int TimeDelay
    {
        get => _timeDelay;
        set
        {
            _timeDelay = Math.Clamp(value, 35, 5000);
        }
    }
}


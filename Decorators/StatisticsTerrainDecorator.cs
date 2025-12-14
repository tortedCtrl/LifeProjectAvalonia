using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeProjectAvalonia;

public class StatisticsTerrainDecorator : ITerrain
{
    private ITerrain _wrappedTerrain;
    private LifePagePresenter _presenter;
    private Action<TimeSpan>? _timePresenter;

    private int _generation = 0;
    private bool _statAll;

    public StatisticsTerrainDecorator(ITerrain wrappingTerrain, LifePagePresenter presenter,
                                      Action<TimeSpan>? timePresenter, bool statAll = true)
    {
        _wrappedTerrain = wrappingTerrain != null ? wrappingTerrain :
            throw new NullReferenceException(nameof(wrappingTerrain));

        _presenter = presenter != null ? presenter :
            throw new NullReferenceException(nameof(presenter));

        _timePresenter = timePresenter;

        _statAll = statAll;
    }

    public CellField Field => _wrappedTerrain.Field;

    public void DrawCell(Cell cell) => _wrappedTerrain.DrawCell(cell);

    public void MakeTurn()
    {
        TimeSpan turnTime = DateTime.Now.TimeOfDay;

        _wrappedTerrain.MakeTurn();

        _generation++;
        if (_statAll)
        {
            int deadCells = Field.Where(cell => cell.State is Dead).Count();
            int whiteCells = Field.Where(cell => cell.State is White).Count();
            int blackCells = Field.Where(cell => cell.State is Black).Count();
            _presenter.UpdateStatistics(_generation, whiteCells, blackCells, deadCells);
        }

        turnTime = DateTime.Now.TimeOfDay - turnTime;
        _timePresenter?.Invoke(turnTime);
    }


    public void Randomize() => _wrappedTerrain.Randomize();

    public ITerrain SetWrappedTerrain(ITerrain newWrappedTerrain)
    {
        var prev = _wrappedTerrain;
        _wrappedTerrain = newWrappedTerrain;
        return prev;
    }

    public void StablePatternEncountered(List<Cell> pattern) => _wrappedTerrain.StablePatternEncountered(pattern);
}
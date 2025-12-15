using System;
using System.Linq;

namespace LifeProjectAvalonia;

public class StatisticsTerrainDecorator : TerrainDecorator
{
    private LifePagePresenter _presenter;
    private Action<TimeSpan>? _timePresenter;

    private int _generation = 0;
    private bool _statAll;

    public StatisticsTerrainDecorator(ITerrain wrappingTerrain, LifePagePresenter presenter,
                                      Action<TimeSpan>? timePresenter, bool statAll = true) : base(wrappingTerrain)
    {
        _presenter = presenter != null ? presenter :
            throw new NullReferenceException(nameof(presenter));

        _timePresenter = timePresenter;

        _statAll = statAll;
    }

    public override void MakeTurn()
    {
        TimeSpan turnTime = DateTime.Now.TimeOfDay;

        _wrappedTerrain.MakeTurn();

        _generation++;
        if (_statAll)
        {
            int deadCells = Field.Count(cell => cell.State is Dead);
            int whiteCells = Field.Count(cell => cell.State is White);
            int blackCells = Field.Count(cell => cell.State is Black);
            _presenter.UpdateStatistics(_generation, whiteCells, blackCells, deadCells);
        }

        turnTime = DateTime.Now.TimeOfDay - turnTime;
        _timePresenter?.Invoke(turnTime);
    }
}
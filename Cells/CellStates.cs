using Avalonia.Media;
using System.Linq;
using System.Collections.Generic;
using System;

namespace LifeProjectAvalonia;

public abstract class CellState
{
    protected Cell _related;
    public CellState(Cell related) => _related = related;
    public abstract Func<Cell, CellState>? NextState { get; protected set; }
    public virtual void OnStateChanged() { }
    public abstract IImmutableSolidColorBrush BrushToPaintCell();

}
public class Dead : CellState
{
    public Dead(Cell related) : base(related) { }

    private static Func<Cell, CellState>? _nextState;

    public override Func<Cell, CellState>? NextState { get => _nextState; protected set => _nextState = value; }

    public static void InjectLogic(Func<Cell, CellState> logic) => _nextState = logic;

    public override IImmutableSolidColorBrush BrushToPaintCell() => Brushes.LightBlue;

}
public class Border : CellState
{
    private static Func<Cell, CellState>? _nextState;

    public Border(Cell related) : base(related)
    {
        _related = related;
    }
    public override Func<Cell, CellState>? NextState
    {
        get
        {
            if (_nextState == null)
                return (Cell related) => new Border(related);
            else return _nextState;
        }
        protected set => _nextState = value;
    }

    public override IImmutableSolidColorBrush BrushToPaintCell() => Brushes.Brown;

}

public class White : CellState
{
    public White(Cell related) : base(related) { }

    private static Func<Cell, CellState>? _nextState;

    public override Func<Cell, CellState>? NextState { get => _nextState; protected set => _nextState = value; }

    public static void InjectLogic(Func<Cell, CellState> logic) => _nextState = logic;


    public override IImmutableSolidColorBrush BrushToPaintCell() => Brushes.White;

}
public class Black : CellState
{
    private CellColony? _colony;
    public Black(Cell related, CellColony? colony = null) : base(related)
    {
        _colony = colony;
    }
    private static Func<Cell, CellState>? _nextState;

    public override Func<Cell, CellState>? NextState { get => _nextState; protected set => _nextState = value; }

    public static void InjectLogic(Func<Cell, CellState> logic) => _nextState = logic;


    public override void OnStateChanged()
    {
        _colony?.Join(_related);
    }

    public override IImmutableSolidColorBrush BrushToPaintCell() => Brushes.Black;
}


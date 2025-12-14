using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Drawing;

using Avalonia.Media;

namespace LifeProjectAvalonia;

public class Cell
{
    private CellState _nextState;

    private List<Cell> _neighbours = new(8);

    public Cell(int x, int y)
    {
        X = x;
        Y = y;
        State = new Dead(this);
        _nextState = new Dead(this);
    }

    // Equals to col, row indexes in CellField
    public int X { get; init; }
    public int Y { get; init; }

    public IReadOnlyList<Cell> Neighbours { get => _neighbours; }

    public CellState State { get; set; }


    public void CalculateNextState()
    {
        if (State.NextState == null) throw new NullReferenceException($"{State.NextState?.GetType()}'s logic is not implemented.");
        _nextState = State.NextState(this);
    }
    

    public void ToNextState()
    {
        if (State.GetType() == _nextState.GetType()) return;

        State = _nextState;
    }

    public void AddNeighbour(Cell neighbour) => _neighbours.Add(neighbour);

    public IImmutableSolidColorBrush BrushToPaintCell() => State.BrushToPaintCell();

    public bool CanMove((int x, int y) dir)
    {
        Cell? goingInto = _neighbours.FirstOrDefault(
            cell => cell.X == X + dir.x && cell.Y == Y + dir.y);

        return goingInto?.State is not Border;
    }

    public override string ToString() =>
        $"{X}, {Y}, State {State.GetType().Name}";
}

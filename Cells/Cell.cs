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
        State = new Dead();
        _nextState = new Dead();
    }

    // Equals to col, row indexes in CellField
    public int X { get; init; }
    public int Y { get; init; }

    public IReadOnlyList<Cell> Neighbours { get => _neighbours; }

    public CellState State { get; set; }

    private int WhitesNearby => _neighbours.Where(cell => cell.State is White).Count();
    private int BlackNearby => _neighbours.Where(cell => cell.State is Black).Count();

    public void CalculateNextState() =>
        _nextState = State.NextState(_neighbours, BlackNearby, WhitesNearby);
    

    public void ToNextState()
    {
        if (State.GetType() == _nextState.GetType()) return;

        if (State is White && _nextState is Black)        
            State.Colony?.Join(this);
        

        if (State is Black && _nextState is not Black)
            CellColony.GetColony(this).Item2?.Leave(this);

       State = _nextState;
    }

    public void AddNeighbour(Cell neighbour) => _neighbours.Add(neighbour);

    public void ToDead() => State = new Dead();
    public void ToWhite() => State = new White();
    public void ToBlack(CellColony? colony = null) => State = new Black(colony);

    public IImmutableSolidColorBrush BrushToPaintCell() => State.BrushToPaintCell();

    public bool CanMove((int x, int y) dir)
    {
        Cell? goingInto = _neighbours.FirstOrDefault(
            cell => cell.X == X + dir.x && cell.Y == Y + dir.y);

        return goingInto?.State is not Border;
    }

    public override string ToString()
    {
        return $"{X}, {Y}, State {State.GetType().Name}";
    }
}

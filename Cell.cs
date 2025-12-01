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
        
        State = _nextState;
        //if (State is Black) State.ItsColony!.Members.Add(this);
            
    }

    public void AddNeighbour(Cell neighbour) => _neighbours.Add(neighbour);

    public void ToDead() => State = new Dead();
    public void ToWhite() => State = new White();
    public void ToBlack() => State = new Black();
    //public void ToBlack(Colony colony) => State = new Black(colony);

    public SolidColorBrush Brush() => State.Brush();

    public bool CanMove((int x, int y) dir)
    {
        Cell? goingInto = _neighbours.FirstOrDefault(
            cell => cell.X == X + dir.x && cell.Y == Y + dir.y);

        return goingInto?.State is not Border;
    }

    public override string ToString()
    {
        return $"{X}, {Y}, State {State}";
    }
}

#region States
public interface CellState
{
    public CellState NextState(List<Cell> nbors, int blackNear, int whiteNear);
    public SolidColorBrush Brush();
    //public Colony? ItsColony { get => null; }

}
public struct Dead : CellState
{
    public CellState NextState(List<Cell> nbors, int blackNear, int whiteNear)
    {
        if (whiteNear == 3)
            return new White();

        return new Dead();
    }

    public SolidColorBrush Brush() => new SolidColorBrush(Colors.Gray);
}
public struct White : CellState
{
    public CellState NextState(List<Cell> nbors, int blackNear, int whiteNear)
    {
        if (blackNear > whiteNear + 1)
        {
            /*var colony = nbors.FirstOrDefault(cell => cell.State is Black)?.State.ItsColony;
            return new Black(colony!);*/
        }

        if (whiteNear == 2 || whiteNear == 3)
            return new White();

        return new Dead();
    }

    public SolidColorBrush Brush() => new SolidColorBrush(Colors.White);
}
public struct Black : CellState
{
    //public Colony? ItsColony { get; init; } 

    public Black()
    {

    }
    /*public Black(Colony colony)
    {
        ItsColony = colony;
    }*/

    public CellState NextState(List<Cell> nbors, int blackNear, int whiteNear)
    {
        /*if (ItsColony != null)
            ItsColony.PassesTurn = whiteNear > 0;*/

        if (blackNear + 1 < whiteNear)
            return new White();

        if (blackNear == 2 || blackNear == 3)
            return new Black();

        return new Dead();
    }

    public SolidColorBrush Brush() => new SolidColorBrush(Colors.Black);
}
public struct Border : CellState
{
    public CellState NextState(List<Cell> nbors, int blackNear, int whiteNear)
    {
        return new Border();
    }

    public SolidColorBrush Brush() => new SolidColorBrush(Colors.Brown);
}

#endregion

using Avalonia.Media;
using System.Linq;
using System.Collections.Generic;

namespace LifeProjectAvalonia;
public interface CellState
{
    public CellColony? Colony { get; }
    public CellState NextState(List<Cell> nbors, int blackNear, int whiteNear);
    public IImmutableSolidColorBrush BrushToPaintCell();

}
public struct Dead : CellState
{
    public CellColony? Colony { get => null; }

    public CellState NextState(List<Cell> nbors, int blackNear, int whiteNear)
    {
        if (whiteNear == 3)
            return new White();

        return new Dead();
    }

    public IImmutableSolidColorBrush BrushToPaintCell() => Brushes.LightBlue;
}
public struct White : CellState
{
    public CellColony? Colony { get; private set; }

    public CellState NextState(List<Cell> nbors, int blackNear, int whiteNear)
    {
        if (blackNear > whiteNear + 1)
        {
            Colony = nbors.FirstOrDefault(cell => cell.State is Black)?.Colony;
            return new Black(Colony);
        }

        if (whiteNear == 2 || whiteNear == 3)
            return new White();

        return new Dead();
    }

    public IImmutableSolidColorBrush BrushToPaintCell() => Brushes.White;
}
public struct Black : CellState
{
    public CellColony? Colony { get; private set; }

    public Black(CellColony? colony = null)
    {
        Colony = colony;
    }

    public CellState NextState(List<Cell> nbors, int blackNear, int whiteNear)
    {
        if (blackNear + 0 < whiteNear)
            return new White();

        if (blackNear == 2 || blackNear == 3 || blackNear == 4)
            return new Black();

        return new Dead();
    }

    public IImmutableSolidColorBrush BrushToPaintCell() => Brushes.Black;
}
public struct Border : CellState
{
    public CellColony? Colony => null;

    public CellState NextState(List<Cell> nbors, int blackNear, int whiteNear)
    {
        return new Border();
    }

    public IImmutableSolidColorBrush BrushToPaintCell() => Brushes.Brown;
}

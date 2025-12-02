using System;
using System.Collections.Generic;
using System.Linq;

namespace LifeProjectAvalonia;

public class CellColony
{
    private List<Cell> _members;
    private (int x, int y) _direction;

    private CellField _field;

    private bool _passesTurn = false;


    public CellColony(List<Cell> members, CellField field)
    {
        _members = members;
        _field = field;

        foreach (Cell cell in _members)
        {
            cell.ToBlack(this);
        }

        SetNewDirection();
    }

    public static CellColony operator +(CellColony left, CellColony right)
    {
        var unique = right._members.Where(memb => left._members.Contains(memb) == false);
        left._members.AddRange(unique);
        var colony = new CellColony(left._members, left._field);
        colony.PassNextTurn();
        return colony;
    }

    private bool CanMove => _members.All(cell => cell.CanMove(Direction));

    private (int x, int y) Direction
    {
        get => _direction;
        set
        {
            bool moovingX = Math.Abs(value.x) == 1 && value.y == 0;
            bool moovingY = value.x == 0 && Math.Abs(value.y) == 1;
            if (moovingX || moovingY)
                _direction = value;
            else
                throw new ArgumentException($"{nameof(Direction)} supports only (+-1, 0), (0, +-1) vectors");
        }
    }


    public (bool, Cell?) CellInNeighbourColony()
    {
        foreach (Cell cell in _members)
        {
            Cell? neighbourNotFromHere = cell.Neighbours.FirstOrDefault(cell => cell.Colony != null && cell.Colony != this);
            if (neighbourNotFromHere != null)
                return (true, neighbourNotFromHere);
        }

        return (false, null);
    }

    public void TryMove()
    {
        if (CanMove)
            Move();
        else
            SetNewDirection();
    }


    public void Join(Cell newCell)
    {
        PassNextTurn();
        SetNewDirection();
        _members.Add(newCell);
    }
    public void Leave(Cell oldCell) =>
        _members.Remove(oldCell);


    private void Move()
    {
        if (_passesTurn)
        {
            _passesTurn = false;
            return;
        }

        var newMembers = _members.Select(cell =>
            _field[cell.X + Direction.x, cell.Y + Direction.y]).ToList();

        _members.ForEach(cell => cell.ToDead());

        newMembers.ForEach(cell => cell.ToBlack(this));
        _members = newMembers;
    }

    private void SetNewDirection()
    {
        int n = Random.Shared.Next(4);

        (int, int) dir = n switch
        {
            0 => (0, -1),
            1 => (0, 1),
            2 => (1, 0),
            3 => (-1, 0),
            _ => (0, 0)
        };

        Direction = dir;
    }

    private void PassNextTurn() => _passesTurn = true;
}


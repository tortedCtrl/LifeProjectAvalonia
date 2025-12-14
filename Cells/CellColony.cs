using System;
using System.Collections.Generic;
using System.Linq;

namespace LifeProjectAvalonia;

public class CellColony
{
    private static Dictionary<Cell, CellColony> colonyOf = new();

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
            colonyOf[cell] = this;
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

    public static (bool, CellColony?) GetColony(Cell? cell)
    {
        if (cell == null) return (false, null);
        bool hasColony = colonyOf.TryGetValue(cell, out CellColony? colony);
        return (hasColony, colony);
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


    public (bool, Cell?, CellColony?) CellInNeighbourColony()
    {
        CellColony? neighbourColony = null;
        foreach (Cell cell in _members)
        {
            
            Cell? neighbourNotFromHere = cell.Neighbours.FirstOrDefault(cell =>
                colonyOf.TryGetValue(cell, out neighbourColony) && neighbourColony != this);

            if (neighbourNotFromHere != null)
                return (true, neighbourNotFromHere, neighbourColony);
        }

        return (false, null, null);
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

        colonyOf[newCell] = this;
    }
    public void Leave(Cell oldCell)
    {
        colonyOf.Remove(oldCell);
        _members.Remove(oldCell);
    }


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


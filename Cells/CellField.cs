using System;
using System.Collections;
using System.Collections.Generic;

namespace LifeProjectAvalonia;

public class CellField : IEnumerable<Cell>
{
    private Cell[,] _field;

    public int Width { get; init; }
    public int Height { get; init; }

    public CellField(int width, int height)
    {
        Width = width > 1 ? width : throw new ArgumentException("Width ,= 0");
        Height = height > 1 ? height : throw new ArgumentException("Heigth <= 0");

        _field = InitCells();

        foreach (Cell cell in this)
            LinkWithNeighbours(cell);

        return;


        Cell[,] InitCells()
        {
            var cells = new Cell[Width + 2, Height + 2];
            for (int row = 0; row < Height + 2; row++)
                for (int col = 0; col < Width + 2; col++)
                {
                    cells[col, row] = new Cell(col - 1, row - 1);
                    if (col == 0 || col == Width + 1 || row == 0 || row == Height + 1)
                        cells[col, row].State = new Border(cells[col, row]);
                }
            return cells;
        }
    }

    public Cell this[int x, int y] => _field[x + 1, y + 1];   

    /// <summary>
    /// Iterate all cells which not borders
    /// </summary>
    /// <returns></returns>
    public IEnumerator<Cell> GetEnumerator()
    {
        for (int row = 0; row < Height; row++)
            for (int col = 0; col < Width; col++)
                yield return this[col, row];
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
    
        return GetEnumerator();
    }

    private void LinkWithNeighbours(Cell cell)
    {
        (int x, int y) = (cell.X, cell.Y);
        for (int row = y - 1; row < y + 2; row++)
            for (int col = x - 1; col < x + 2; col++)
            {
                if (row == y && col == x)
                    continue;
                this[x, y].AddNeighbour(this[col, row]);
            }
    }
}


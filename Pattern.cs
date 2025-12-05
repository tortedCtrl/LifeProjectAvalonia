using System;
using System.Collections.Generic;
using System.Text;

namespace LifeProjectAvalonia;

public abstract class Pattern
{
    protected abstract List<int[,]> PatternForms { get; }
    public virtual bool Stable => true;

    public (bool, List<Cell>?) ScanAllForms(CellField cells, int left, int up)
    {
        foreach (int[,] form in PatternForms)
        {
            (bool found, List<Cell>?) scanned = ScanForm(form);
            if (scanned.found)
                return scanned;
        }

        return (false, null);


        (bool, List<Cell>?) ScanForm(int[,] patternForm)
        {
            List<Cell> cellsInPatter = new();

            int formW = patternForm.GetLength(0);
            int formH = patternForm.GetLength(1);

            if (left + formW - 1 > cells.Width) return (false, null);
            if (up + formH - 1 > cells.Height) return (false, null);

            return CheckPiece();


            (bool, List<Cell>?) CheckPiece()
            {
                for (int col = 0; col < formW; col++)
                    for (int row = 0; row < formH; row++)
                    {
                        Cell currCell = cells[col + left, row + up];
                        bool noInfluence = patternForm[col, row] == 2;
                        bool aliveInPattern = patternForm[col, row] == 1;
                        bool aliveInFact = currCell.State is White;

                        if (aliveInFact != aliveInPattern && noInfluence == false)
                            return (false, null);

                        if (aliveInPattern)
                            cellsInPatter.Add(currCell);
                    }

                return (true, cellsInPatter);
            }

        }
    }
}

public class SquarePattern : Pattern
{
    protected override List<int[,]> PatternForms => [new int[,] {
        { 0, 0, 0, 0 },
        { 0, 1, 1, 0 },
        { 0, 1, 1, 0 },
        { 0, 0, 0, 0 } }];
}

public class HivePattern : Pattern
{
    protected override List<int[,]> PatternForms => [
    new int[,] {
        { 2, 0, 0, 0, 0, 2 },
        { 0, 0, 1, 1, 0, 0 },
        { 0, 1, 0, 0, 1, 0 },
        { 0, 0, 1, 1, 0, 0 },
        { 2, 0, 0, 0, 0, 2 } },
    new int[,] {
        { 2, 0, 0, 0, 2 },
        { 0, 0, 1, 0, 0 },
        { 0, 1, 0, 1, 0 },
        { 0, 1, 0, 1, 0 },
        { 0, 0, 1, 0, 0 },
        { 2, 0, 0, 0, 2 } }];
}

public class LightPattern : Pattern
{
    public override bool Stable => false;
    protected override List<int[,]> PatternForms => [
    new int[,] {
        { 0, 0, 0 },
        { 0, 1, 0 },
        { 0, 1, 0 },
        { 0, 1, 0 },
        { 0, 0, 0 } },
    new int[,] {
        { 0, 0, 0, 0, 0 },
        { 0, 1, 1, 1, 0 },
        { 0, 0, 0, 0, 0 }
    }];
}

public class GliderPattern : Pattern
{
    public override bool Stable => false;
    protected override List<int[,]> PatternForms => [orig];
        

    private int[,] orig = new int[,] {
        { 2, 0, 0, 0, 0 },
        { 2, 0, 1, 0, 0 },
        { 0, 0, 0, 1, 0 },
        { 0, 1, 1, 1, 0 },
        { 0, 0, 0, 0, 0 } };



    public static int[,] CreateMirroredMatrix(int[,] original, bool horizontally, bool vertically)
    {
        int rows = original.GetLength(0);
        int cols = original.GetLength(1);
        int[,] mirrored = new int[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                int newRow = vertically ? rows - 1 - i : i;
                int newCol = horizontally ? cols - 1 - j : j;
                mirrored[i, j] = original[newRow, newCol];
            }
        }

        return mirrored;
    }
}


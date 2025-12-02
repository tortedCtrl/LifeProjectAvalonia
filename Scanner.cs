using System;
using System.Collections.Generic;
using System.Text;

namespace LifeProjectAvalonia;

public class Scanner : IDisposable
{
    public event Action<CellField>? PatternDetected;

    private readonly Pattern[] patterns = { new SquarePattern(), new LightPattern(), new GliderPattern(), new HivePattern() };

    private CellField _scanningField;

    private Terrain _terrain;


    public Scanner(Terrain terrain)
    {
        _terrain = terrain;

        _scanningField = new(terrain.Field.Width, terrain.Field.Height);

        _terrain.TurnFinished += ScanPatterns;
    }
    public void Dispose()
    {
        _terrain.TurnFinished -= ScanPatterns;
    }

    private void ScanPatterns(CellField cells)
    {
        foreach (Cell cell in _scanningField) cell.ToDead();

        for (int row = -1; row < _scanningField.Height + 1; row++)
            for (int col = -1; col < _scanningField.Width + 1; col++)
                ScanPatternsFromCorner(col, row);

        AppendAllBlack();

        PatternDetected?.Invoke(_scanningField);

        return;


        void ScanPatternsFromCorner(int x, int y)
        {
            foreach (Pattern pattern in patterns)
            {
                (bool found, var aliveCells) = pattern.ScanAllForms(cells, x, y);

                if (found == false) continue;

                Revive(aliveCells!);

                if (pattern.Stable) 
                    _terrain.StablePatternEncountered(aliveCells!);

                return;
            }
        }

        void Revive(List<Cell> patternCells)
        {
            foreach (Cell cell in patternCells)
                _scanningField[cell.X, cell.Y].State = cell.State;
        }

        void AppendAllBlack()
        {
            foreach (Cell cell in cells)
            {
                if (cell.State is not Black) continue;
                _scanningField[cell.X, cell.Y].ToBlack();
            }
        }
    }
}


using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LifeProjectAvalonia;

internal class ScannerTerrainDecorator : ITerrain
{
    private Action<Cell> _cellClearer;
    private Action<Cell> _cellPainter;
    private ITerrain _wrappedTerrain;


    public ScannerTerrainDecorator(ITerrain terrain, StartData data, Action<bool>? showEmptyCells)
    {
        _wrappedTerrain = terrain != null ? terrain : throw new NullReferenceException(nameof(terrain));

        Field = new(terrain.Field.Width, terrain.Field.Height);

        var favortites = CreateWindow();

        _cellPainter = favortites.PaintBox;
        _cellClearer = favortites.Clear;

        FavoritesWindow CreateWindow()
        {
            var favoritesPresenter = new FavoritesWindow(data.width, data.height, data.cellSize, showEmptyCells);
            Window favorites = favoritesPresenter;
            favorites.Show();

            return favoritesPresenter;
        }
    }

    public CellField Field { get; init; }

    private readonly Pattern[] patterns = { new SquarePattern(), new LightPattern(), new GliderPattern(), new HivePattern() };

    public void MakeTurn()
    {
        _wrappedTerrain.MakeTurn();

        ScanPatterns(_wrappedTerrain.Field);
    }

    public void Randomize()
    {
        _wrappedTerrain.Randomize();

        ScanPatterns(_wrappedTerrain.Field);
    }

    public void StablePatternEncountered(List<Cell> pattern) =>
        _wrappedTerrain.StablePatternEncountered(pattern);

    public void DrawCell(Cell cell) => //parent draw called in parent make turn
        _cellPainter(cell);

    public ITerrain SetWrappedTerrain(ITerrain newWrappedTerrain)
    {
        var prev = _wrappedTerrain;
        _wrappedTerrain = newWrappedTerrain;
        return prev;
    }


    private void ScanPatterns(CellField cells)
    {
        foreach (Cell cell in Field) cell.ToDead();

        for (int row = -1; row < Field.Height + 1; row++)
            for (int col = -1; col < Field.Width + 1; col++)
                ScanPatternsFromCorner(col, row);

        AppendAllBlack();

        foreach (Cell cell in Field.Where(cell => cell.State is Dead))
            _cellClearer(cell);
        foreach (Cell cell in Field.Where(cell => cell.State is not Dead))
            DrawCell(cell);

        return;


        void ScanPatternsFromCorner(int x, int y)
        {
            foreach (Pattern pattern in patterns)
            {
                (bool found, var aliveCells) = pattern.ScanAllForms(cells, x, y);

                if (found == false) continue;

                Revive(aliveCells!);

                if (pattern.Stable)
                    _wrappedTerrain.StablePatternEncountered(aliveCells!);

                return;
            }
        }

        void Revive(List<Cell> patternCells)
        {
            foreach (Cell cell in patternCells)
                Field[cell.X, cell.Y].State = cell.State;
        }

        void AppendAllBlack()
        {
            foreach (Cell cell in cells)
            {
                if (cell.State is not Black) continue;
                Field[cell.X, cell.Y].ToBlack();
            }
        }
    }
}
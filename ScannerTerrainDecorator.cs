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
    private Action<Cell> _cellPainter;
    private ITerrain _wrappedTerrain;


    public ScannerTerrainDecorator(ITerrain terrain, StartData data)
    {
        _wrappedTerrain = terrain != null ? terrain : throw new NullReferenceException(nameof(terrain));

        Field = new(terrain.Field.Width, terrain.Field.Height);

        var favortites = CreateWindow();

        _cellPainter = favortites.PaintBox;
        // + Closing => (_, _)

        FavoritesWindow CreateWindow()
        {
            var favoritesPresenter = new FavoritesWindow(data.width, data.height, data.cellSize);
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

    public void Draw()
    {
        foreach (Cell cell in Field)
            _cellPainter(cell);
    }

    private void ScanPatterns(CellField cells)
    {
        foreach (Cell cell in Field) cell.ToDead();

        for (int row = -1; row < Field.Height + 1; row++)
            for (int col = -1; col < Field.Width + 1; col++)
                ScanPatternsFromCorner(col, row);

        AppendAllBlack();

        Draw();

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
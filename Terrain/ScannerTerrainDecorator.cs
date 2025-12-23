using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
namespace LifeProjectAvalonia;

internal class ScannerTerrainDecorator : TerrainDecorator
{
    private readonly CellField _scanningField;
    private readonly Pattern[] patterns = { new SquarePattern(), new LightPattern(), new GliderPattern(), new HivePattern() };

    private Action<Cell> _cellClearer;
    private Action<Cell> _cellPainter;

    public ScannerTerrainDecorator(ITerrain terrain, StartData data, Action<bool>? showEmptyCells) : base(terrain)
    {
        _scanningField = new(terrain.Field.Width, terrain.Field.Height);

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

    public override CellField Field { get => _scanningField; }

    public override void MakeTurn()
    {
        base.MakeTurn(); //calling its Draw

        ScanPatterns(base.Field);

        Draw();
    }

    public override void Randomize()
    {
        base.Randomize();

        ScanPatterns(base.Field);

        Draw();
    }

    public override void Draw()
    {
        foreach (Cell cell in Field.Where(cell => cell.State is Dead))
            _cellClearer(cell);
        foreach (Cell cell in Field.Where(cell => cell.State is not Dead))
            DrawCell(cell);
    }
    
    public override void DrawCell(Cell cell) =>
        _cellPainter(cell);

    private void ScanPatterns(CellField cells)
    {
        foreach (Cell cell in Field) cell.State = new Dead(cell);

        for (int row = -1; row < Field.Height + 1; row++)
            for (int col = -1; col < Field.Width + 1; col++)
                ScanPatternsFromCorner(col, row);

        AppendAllBlack();


        return;

        void ScanPatternsFromCorner(int x, int y)
        {
            foreach (Pattern pattern in patterns)
            {
                (bool found, var aliveCells) = pattern.ScanAllForms(cells, x, y);

                if (found == false) continue;

                Revive(aliveCells!);

                if (pattern.Stable) base.StablePatternEncountered(aliveCells!);

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
                var fcell = Field[cell.X, cell.Y];
                fcell.State = new Black(fcell);
            }
        }
    }
}
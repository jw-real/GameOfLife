using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class PatternNormalizer
{
    public static string ComputeCanonicalHash(HashSet<Vector2Int> liveCells)
    {
        if (liveCells == null || liveCells.Count == 0)
            return string.Empty;

        // Step 1: normalize translation
        bool[,] baseGrid = BuildCroppedGrid(liveCells);

        // Step 2: store rotations as variables
        var r0 = baseGrid;
        var r90 = Rotate90(r0);
        var r180 = Rotate180(r0);
        var r270 = Rotate270(r0);

        // Step 3: generate rotations & reflections
        List<bool[,]> variants = new List<bool[,]>
        {
            r0,
            r90,
            r180,
            r270,
            ReflectHorizontal(r0),
            ReflectHorizontal(r90),
            ReflectHorizontal(r180),
            ReflectHorizontal(r270)
        };

        // Step 4: canonical string
        string canonical = variants
            .Select(v => SerializeGrid(CropGrid(v)))
            .OrderBy(s => s, StringComparer.Ordinal)
            .First();

        // Step 4: hash
        return HashString(canonical);
    }

    // -------------------------
    // Grid helpers
    // -------------------------

    private static bool[,] BuildCroppedGrid(HashSet<Vector2Int> cells)
    {
        int minX = cells.Min(c => c.x);
        int maxX = cells.Max(c => c.x);
        int minY = cells.Min(c => c.y);
        int maxY = cells.Max(c => c.y);

        int width = maxX - minX + 1;
        int height = maxY - minY + 1;

        bool[,] grid = new bool[width, height];

        foreach (var c in cells)
        {
            grid[c.x - minX, c.y - minY] = true;
        }

        return grid;
    }

    private static bool[,] CropGrid(bool[,] grid)
    {
        int w = grid.GetLength(0);
        int h = grid.GetLength(1);

        int minX = w, maxX = -1, minY = h, maxY = -1;

        for (int x = 0; x < w; x++)
        for (int y = 0; y < h; y++)
        {
            if (!grid[x, y]) continue;

            minX = Math.Min(minX, x);
            maxX = Math.Max(maxX, x);
            minY = Math.Min(minY, y);
            maxY = Math.Max(maxY, y);
        }

        if (maxX < minX || maxY < minY)
            return new bool[1, 1];

        bool[,] cropped = new bool[maxX - minX + 1, maxY - minY + 1];

        for (int x = minX; x <= maxX; x++)
        for (int y = minY; y <= maxY; y++)
            cropped[x - minX, y - minY] = grid[x, y];

        return cropped;
    }

    private static bool[,] Rotate90(bool[,] grid)
    {
        int w = grid.GetLength(0);
        int h = grid.GetLength(1);
        bool[,] rotated = new bool[h, w];

        for (int x = 0; x < w; x++)
        for (int y = 0; y < h; y++)
            rotated[y, w - 1 - x] = grid[x, y];

        return rotated;
    }

    private static bool[,] Rotate180(bool[,] grid) =>
        Rotate90(Rotate90(grid));

    private static bool[,] Rotate270(bool[,] grid) =>
        Rotate90(Rotate180(grid));

    private static bool[,] ReflectHorizontal(bool[,] grid)
    {
        int w = grid.GetLength(0);
        int h = grid.GetLength(1);
        bool[,] reflected = new bool[w, h];

        for (int x = 0; x < w; x++)
        for (int y = 0; y < h; y++)
            reflected[w - 1 - x, y] = grid[x, y];

        return reflected;
    }

    // -------------------------
    // Serialization & hashing
    // -------------------------

    private static string SerializeGrid(bool[,] grid)
    {
        int w = grid.GetLength(0);
        int h = grid.GetLength(1);

        StringBuilder sb = new StringBuilder();

        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
                sb.Append(grid[x, y] ? '1' : '0');

            sb.Append('|');
        }

        return sb.ToString();
    }

    private static string HashString(string input)
    {
        using (SHA256 sha = SHA256.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            byte[] hash = sha.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }
}
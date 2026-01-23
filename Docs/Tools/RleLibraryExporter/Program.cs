using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;

class RleLibraryExporter
{
    static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("  RleLibraryExporter <inputFolder> <outputJsonPath>");
            return;
        }

        string inputFolder = args[0];
        string outputPath = args[1];

        // Ensure the directory exists
        Directory.CreateDirectory(Path.GetDirectoryName(outputPath));

        using var writer = new StreamWriter(outputPath, false, Encoding.UTF8);
        writer.WriteLine("[");  // start JSON array
        bool firstEntry = true;

        foreach (string file in Directory.EnumerateFiles(inputFolder, "*.rle", SearchOption.AllDirectories))
        {
            try
            {
                LibraryEntry entry = ParseRleFile(file);
                if (entry == null) continue;

                if (!firstEntry)
                    writer.WriteLine(",");
                firstEntry = false;

                string json = JsonSerializer.Serialize(entry);
                writer.Write(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to parse {file}: {ex.Message}");
            }
        }

        writer.WriteLine("\n]");
        Console.WriteLine($"RLE export completed: {outputPath}");
    }

    // ----------------------------
    // Parsing
    // ----------------------------

    static LibraryEntry ParseRleFile(string path)
    {
        string[] lines = File.ReadAllLines(path);

        string name = Path.GetFileNameWithoutExtension(path);
        var rleData = new StringBuilder();

        foreach (string line in lines)
        {
            if (line.StartsWith("#N"))
            {
                name = line.Substring(2).Trim();
            }
            else if (!line.StartsWith("#") && !line.StartsWith("x"))
            {
                rleData.Append(line.Trim());
            }
        }

        if (rleData.Length == 0) return null;

        var cells = DecodeRle(rleData.ToString());
        if (cells.Count == 0) return null;

        string canonical = EncodeTo01Pipe(cells);

        return new LibraryEntry
        {
            name = name,
            patternCanonical = canonical
        };
    }

    // ----------------------------
    // RLE decoding
    // ----------------------------

    static HashSet<(int x, int y)> DecodeRle(string rle)
    {
        var cells = new HashSet<(int, int)>();
        int x = 0, y = 0;
        int count = 0;

        foreach (char c in rle)
        {
            if (char.IsDigit(c))
            {
                count = count * 10 + (c - '0');
                continue;
            }

            int n = count == 0 ? 1 : count;
            count = 0;

            if (c == 'b')
            {
                x += n;  // dead cells
            }
            else if (c == 'o' || c == 'A') // 'A' alive in generation 0
            {
                for (int i = 0; i < n; i++)
                    cells.Add((x + i, y));
                x += n;
            }
            else if (c == '$')
            {
                y += n;
                x = 0;
            }
            else if (c == '!')
            {
                break;
            }
            // ignore other characters
        }

        return cells;
    }

    // ----------------------------
    // Encode HashSet into pipe-separated 0/1 string
    // ----------------------------
    static string EncodeTo01Pipe(HashSet<(int x, int y)> cells)
    {
        int minX = int.MaxValue, maxX = int.MinValue;
        int minY = int.MaxValue, maxY = int.MinValue;

        foreach (var c in cells)
        {
            if (c.x < minX) minX = c.x;
            if (c.x > maxX) maxX = c.x;
            if (c.y < minY) minY = c.y;
            if (c.y > maxY) maxY = c.y;
        }

        int width = maxX - minX + 1;
        int height = maxY - minY + 1;
        var sb = new StringBuilder();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                sb.Append(cells.Contains((x + minX, y + minY)) ? '1' : '0');
            }
            sb.Append('|');
        }

        return sb.ToString();
    }

    // ----------------------------
    // Data model
    // ----------------------------
    class LibraryEntry
    {
        public string name { get; set; }
        public string patternCanonical { get; set; }
    }
}
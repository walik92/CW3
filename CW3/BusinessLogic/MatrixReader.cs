using System;
using System.Collections.Generic;
using System.IO;

namespace CW3.BusinessLogic
{
    public class MatrixReader
    {
        private static readonly char _separator = ',';

        public static int[][] Read(string path)
        {
            string[] lines;
            try
            {
                lines = File.ReadAllLines(path);
            }
            catch (Exception ex)
            {
                throw new Exception("Błąd podczas wczytywaniu pliku", ex);
            }
            return ConvertToMatrix(lines);
        }

        private static int[][] ConvertToMatrix(string[] lines)
        {
            var result = new int[lines.Length][];

            if (lines.Length == 0)
            {
                throw new Exception("Wybrany plik jest pusty");
            }

            for (var i = 0; i < lines.Length; i++)
            {
                var row = ConvertToRow(lines[i]);
                if (i > 0)
                    if (result[0].Length != row.Length)
                    {
                        throw new Exception("Różna liczba kolumn w wierszach");
                    }
                result[i] = row;
            }
            return result;
        }

        private static int[] ConvertToRow(string line)
        {
            var elements = line.Split(_separator);
            if (elements.Length == 0)
                throw new Exception("Nieprawidłowy format pliku, wiersz ma 0 elementów");
            var result = new List<int>();
            foreach (var element in elements)
            {
                int value;
                if (int.TryParse(element, out value))
                {
                    result.Add(value);
                }
                else
                {
                    throw new Exception("Nieprawidłowy format pliku, element nie jest liczbą");
                }
            }
            return result.ToArray();
        }
    }
}
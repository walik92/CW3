using System;
using System.IO;
using Microsoft.Win32;

namespace CW3.BusinessLogic
{
    public class MatrixSaver
    {
        private static readonly string _separator = ",";

        public static void Save(int[][] matrix, string path)
        {
            if (matrix == null || matrix.Length == 0)
            {
                throw new Exception("Macierz wynikowa jest pusta");
            }

            using (var writer = new StreamWriter(path))
            {
                foreach (var row in matrix)
                {
                    writer.WriteLine(ConvertRowToString(row));
                }
            }
        }

        private static string ConvertRowToString(int[] row)
        {
            return string.Join(_separator, row);
        }
    }
}
using System;
using System.Threading;

namespace CW3.BusinessLogic
{
    public class MatrixCalculator
    {
        public delegate void ProgressUpdate(double value);

        public event ProgressUpdate OnProgressUpdate;

        public int[][] Addition(int[][] matrix1, int[][] matrix2)
        {
            if (matrix1 == null || matrix1.Length == 0)
                throw new Exception("Macierz pierwsza jest pusta");

            if (matrix2 == null || matrix2.Length == 0)
                throw new Exception("Macierz druga jest pusta");

            if (matrix1.Length != matrix2.Length)
            {
                throw new Exception(
                    "Nie można wykonać operacji sumowania na wczytanych macierzach, macierze mają różne wymiary");
            }

            var result = new int[matrix1.Length][];
            double countAllElements = matrix1.Length * matrix1[0].Length;

            for (var i = 0; i < matrix1.Length; i++)
            {
                var m = matrix1[i];
                var n = matrix2[i];
                if (m.Length != n.Length)
                    throw new Exception(
                        "Nie można wykonać operacji sumowania na wczytanych macierzach,, macierze mają różne wymiary");

                var row = new int[m.Length];

                for (var j = 0; j < m.Length; j++)
                {
                    row[j] = m[j] + n[j];
                    if (OnProgressUpdate != null)
                    {
                        var valueProgress = Math.Ceiling((i * m.Length + j) / countAllElements * 100);
                        OnProgressUpdate(valueProgress);
                        Thread.Sleep(10);
                    }
                }
                result[i] = row;
            }
            OnProgressUpdate(100);
            return result;
        }

        public int[][] Substract(int[][] matrix1, int[][] matrix2)
        {
            if (matrix1 == null || matrix1.Length == 0)
                throw new Exception("Macierz pierwsza jest pusta");

            if (matrix2 == null || matrix2.Length == 0)
                throw new Exception("Macierz druga jest pusta");

            if (matrix1.Length != matrix2.Length)
            {
                throw new Exception(
                    "Nie można wykonać operacji odejmowania na wczytanych macierzach, macierze mają różne wymiary");
            }

            var result = new int[matrix1.Length][];
            double countAllElements = matrix1.Length * matrix1[0].Length;

            for (var i = 0; i < matrix1.Length; i++)
            {
                var m = matrix1[i];
                var n = matrix2[i];
                if (m.Length != n.Length)
                    throw new Exception(
                        "Nie można wykonać operacji odejmowania na wczytanych macierzach,, macierze mają różne wymiary");

                var row = new int[m.Length];

                for (var j = 0; j < m.Length; j++)
                {
                    row[j] = m[j] - n[j];
                    if (OnProgressUpdate != null)
                    {
                        var valueProgress = Math.Ceiling((i * m.Length + j) / countAllElements * 100);
                        OnProgressUpdate(valueProgress);
                        Thread.Sleep(10);
                    }
                }
                result[i] = row;
            }
            OnProgressUpdate(100);
            return result;
        }
    }
}
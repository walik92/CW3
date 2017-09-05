using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CW3.BusinessLogic;

namespace CW3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int[][] _matrix1;
        private int[][] _matrix2;
        private int[][] _matrixResult;

        private readonly BackgroundWorker _backgroundWorkerReader1;
        private readonly BackgroundWorker _backgroundWorkerReader2;
        private readonly BackgroundWorker _backgroundWorkerOperation;
        private readonly BackgroundWorker _backgroundWorkerSaver;

        private readonly MatrixCalculator _matrixCalculator;
        public MainWindow()
        {
            InitializeComponent();

            _backgroundWorkerReader1 = new BackgroundWorker();
            _backgroundWorkerReader1.DoWork +=
                BackgroundWorkerReader_DoWork;
            _backgroundWorkerReader1.RunWorkerCompleted +=
                BackgroundWorkerReader1_RunWorkerCompleted;

            _backgroundWorkerReader2 = new BackgroundWorker();
            _backgroundWorkerReader2.DoWork +=
                BackgroundWorkerReader_DoWork;
            _backgroundWorkerReader2.RunWorkerCompleted +=
                BackgroundWorkerReader2_RunWorkerCompleted;

            _backgroundWorkerSaver = new BackgroundWorker();
            _backgroundWorkerSaver.DoWork +=
                BackgroundWorkerSaver_DoWork;
            _backgroundWorkerSaver.RunWorkerCompleted +=
                BackgroundWorkerSaver_RunWorkerCompleted;

            _backgroundWorkerOperation = new BackgroundWorker();
            _backgroundWorkerOperation.DoWork +=
                BackgroundWorkerOperation_DoWork;
            _backgroundWorkerOperation.RunWorkerCompleted +=
                BackgroundWorkerOperation_RunWorkerCompleted;

            _matrixCalculator = new MatrixCalculator();
            _matrixCalculator.OnProgressUpdate += OperationProgressUpdate;
        }

        private void ShowMatrixInListView(ListView listView, int[][] matrix)
        {
            listView.Items.Clear();
            for (var i = 0; i < matrix.Length; i++)
            {
                var row = matrix[i];
                listView.Items.Add(row);
                if (i > 1000)
                {
                    MessageBox.Show("Wyświetlono tylko 1000 pierwszych wierszy macierzy");
                    break;
                }
            }
        }

        private void MenuItemReadMatrix1_Click(object sender, RoutedEventArgs e)
        {
            var path = FileSelector.SelectReadFile();
            if (!string.IsNullOrEmpty(path))
            {
                if (!_backgroundWorkerReader1.IsBusy)
                {
                    ProgressBar.IsIndeterminate = true;
                    Menu.IsEnabled = false;
                    _backgroundWorkerReader1.RunWorkerAsync(path);
                }
            }
        }

        private void MenuItemReadMatrix2_Click(object sender, RoutedEventArgs e)
        {
            var path = FileSelector.SelectReadFile();
            if (!string.IsNullOrEmpty(path))
            {
                if (!_backgroundWorkerReader2.IsBusy)
                {
                    ProgressBar.IsIndeterminate = true;
                    Menu.IsEnabled = false;
                    _backgroundWorkerReader2.RunWorkerAsync(path);
                }
            }
        }

        private void MenuItemSaveResult_Click(object sender, RoutedEventArgs e)
        {
            var path = FileSelector.SelectSaveFile();
            if (!string.IsNullOrEmpty(path))
            {
                if (!_backgroundWorkerSaver.IsBusy)
                {
                    ProgressBar.IsIndeterminate = true;
                    Menu.IsEnabled = false;
                    _backgroundWorkerSaver.RunWorkerAsync(path);
                }
            }
        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuItemSum_Click(object sender, RoutedEventArgs e)
        {
            if (!_backgroundWorkerOperation.IsBusy)
            {
                Menu.IsEnabled = false;
                _backgroundWorkerOperation.RunWorkerAsync("Add");
            }
        }

        private void MenuItemSubstract_Click(object sender, RoutedEventArgs e)
        {
            if (!_backgroundWorkerOperation.IsBusy)
            {
                Menu.IsEnabled = false;
                _backgroundWorkerOperation.RunWorkerAsync("Substract");
            }
        }

        private void BackgroundWorkerReader_DoWork(object sender, DoWorkEventArgs e)
        {
            var path = e.Argument as string;
            e.Result = MatrixReader.Read(path);
        }

        private void BackgroundWorkerReader1_RunWorkerCompleted(object sender,
            RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else
            {
                _matrix1 = e.Result as int[][];
                ShowMatrixInListView(ListView1, _matrix1);
            }
            ProgressBar.IsIndeterminate = false;
            Menu.IsEnabled = true;
        }

        private void BackgroundWorkerReader2_RunWorkerCompleted(object sender,
            RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else
            {
                _matrix2 = e.Result as int[][];
                ShowMatrixInListView(ListView2, _matrix2);
            }
            ProgressBar.IsIndeterminate = false;
            Menu.IsEnabled = true;
        }

        private void BackgroundWorkerSaver_DoWork(object sender, DoWorkEventArgs e)
        {
            var path = e.Argument as string;
            MatrixSaver.Save(_matrixResult, path);
        }

        private void BackgroundWorkerSaver_RunWorkerCompleted(object sender,
            RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("Wystąpił błąd podczas zapisu: " + e.Error.Message);
            }
            ProgressBar.IsIndeterminate = false;
            Menu.IsEnabled = true;
        }

        private void BackgroundWorkerOperation_DoWork(object sender, DoWorkEventArgs e)
        {
            if (e.Argument as string == "Add")
            {
                _matrixResult = _matrixCalculator.Addition(_matrix1, _matrix2);
            }
            else
                _matrixResult = _matrixCalculator.Substract(_matrix1, _matrix2);
        }

        private void BackgroundWorkerOperation_RunWorkerCompleted(object sender,
            RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("Wystąpił błąd podczas zapisu: " + e.Error.Message);
            }
            else
            {
                ShowMatrixInListView(ListViewResult, _matrixResult);
            }
            Menu.IsEnabled = true;
            ProgressBar.Value = 0;
        }

        private void OperationProgressUpdate(double value)
        {
            Dispatcher.Invoke(() =>
            {
                ProgressBar.Value = value;
            });
        }

    }
}

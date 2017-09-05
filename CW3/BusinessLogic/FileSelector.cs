using System;
using Microsoft.Win32;

namespace CW3.BusinessLogic
{
    public class FileSelector
    {
        public static string SelectReadFile()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }
            return null;
        }

        public static string SelectSaveFile()
        {
            var saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Matrix|*.txt";
            saveFileDialog1.Title = "Save Matrix";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                return saveFileDialog1.FileName;
            }
            return null;
        }
    }
}
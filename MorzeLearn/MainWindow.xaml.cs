using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

namespace MorzeLearn
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Путь к файлу для воспроизведения
        public string pathToFile = "";
        public MainWindow()
        {
            //Загрузка настроек
            this.pathToFile = Properties.Settings.Default.pathToFile;
            InitializeComponent();
        }

        private void mnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            //Открытие файла
            var ofd = new OpenFileDialog();
            ofd.Filter = "TXT|*.txt";
            ofd.Multiselect = true;
            ofd.ShowDialog();
            this.pathToFile = ofd.FileName;
        }

       

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this.pathToFile);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Сохранение настроек при закрытии
            Properties.Settings.Default.pathToFile = this.pathToFile;
            Properties.Settings.Default.Save();
        }
    }
}

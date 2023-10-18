using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
    enum Znak
    {
        dot,
        dash,
        next
    }
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Путь к файлу для воспроизведения
        public string pathToFile = "";
        //Текст для печати
        public string textFor = "";
        //Начало нажатия кнопки
        public DateTime startPush;
        //Конец  нажатия кнопки
        public DateTime endPush;
        System.Windows.Threading.DispatcherTimer timer;

        private void InitializeTimers()
        {
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
        }
        public MainWindow()
        {


            InitializeComponent();
            
            
            timer.Start();
            
            //Загрузка настроек
            this.pathToFile = Properties.Settings.Default.pathToFile;
            this.sldrSpeed.Value = Properties.Settings.Default.speed;
        }
        private void timerTick(object sender, EventArgs e)
        {
            Znak z = isZnak(DateTime.Now - startPush);
            switch (z)
            {
                case Znak.dot:
                    this.lblAddSymbol.Content = ".";
                    break;
                case Znak.dash:
                    this.lblAddSymbol.Content = "-";
                    break;
                case Znak.next:
                    this.lblAddSymbol.Content = "";
                    break;
            }
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
            Properties.Settings.Default.speed = sldrSpeed.Value;
            Properties.Settings.Default.Save();
        }
        private void loadText()
        {
            this.textFor = File.ReadAllText(this.pathToFile);

        }
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            loadText();
        }
    
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (!e.IsRepeat)
            {
                timer.Start();
                this.startPush = DateTime.Now;
            }
        }
        //Определяет, получилась точка, тире или следующая буква
        private Znak isZnak(TimeSpan ts)
        {
            //lblMorzeCode.Content = ts.Seconds.ToString();
            if (ts.TotalMilliseconds > (2000 / sldrSpeed.Value))
            {
                return Znak.next;
            }
            else
            {
                if (ts.TotalMilliseconds > (1000 / sldrSpeed.Value))
                {
                    return Znak.dash;
                }
                else
                {
                    return Znak.dot;
                }
            }
        }
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            //MessageBox.Show(
            //    startPush.ToString() + "\n" +
            //    DateTime.Now.ToString() + "\n" +
            //    (DateTime.Now - startPush).ToString()
            //    );
            Znak z = isZnak(DateTime.Now - startPush);
            switch (z)
            {
                case Znak.dot:
                    this.lblEnteredMorzeCode.Content += ".";
                    break;
                case Znak.dash:
                    this.lblEnteredMorzeCode.Content += "-";
                    break;
                case Znak.next:
                    this.lblEnteredMorzeCode.Content = "";
                    break;
            }
            timer.Stop();
            lblAddSymbol.Content = "";
        }
    }
}
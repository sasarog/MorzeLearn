using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        remove
    }
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //    Dictionary<string,char> alphabet = new Dictionary<string, char>() {
        //        {".-",'А' },
        //        {"-...",'Б' },
        //        {".--",'В' },
        //        {"--.",'Г' },
        //        {"-..",'Д' },
        //        {".",'Е' },
        //        {"...-",'Ж' },
        //        {"--..",'З' },
        //        {"..",'И' },
        //        {"-.-",'К' },
        //        {".-..",'Л' },
        //        {"--",'М' },
        //        {"-.",'Н' },
        //        {"---",'О' },
        //        {".--.",'П' },
        //        {".-.",'Р' },
        //        {"...",'С' },
        //        {"-",'Т' },
        //        {"..-",'У' },
        //        {"..-.",'Ф' },
        //        {"....",'Х' },
        //        {"-.-.",'Ц' },
        //        {"---.",'Ч' },
        //        {"----",'Ш' },
        //        {"--.-",'Щ' },
        //        {"-..-",'Ь' },
        //        {"-.--",'Ы' },
        //        {"..-..",'Э' },
        //        {"..--",'Ю' },
        //        {".-.-",'Я' },
        //        {"-..-",'Ъ' },
        //        {"-----",'0' },
        //        {".----",'1' },
        //        {"..---",'2' },
        //        {"...--",'3' },
        //        {"....-",'4' },
        //        {".....",'5' },
        //        {"-....",'6' },
        //        {"--...",'7' },
        //        {"---..",'8' },
        //        {"----.",'9' },
        //        {"......",'.' },
        //        {".-.-.-",',' },
        //        {"..--..",'?' },
        //        {"-....-",'-' },
        //        {".-..-.",'"' },
        //        {"-.-.-.",';' },
        //        {"--..--",'!' },
        //        {"-.--.-",'(' },
        //        {"---...",':' },
        //        {".-.-..",'+' }
        //    };

        Dictionary<char, string> alphabet = new Dictionary<char, string>() {
    {'А', ".-" },
    {'Б', "-..." },
    {'В', ".--" },
    {'Г', "--." },
    {'Д', "-.." },
    {'Е', "." },
    {'Ж', "...-" },
    {'З', "--.." },
    {'И', ".." },
    {'К', "-.-" },
    {'Л', ".-.." },
    {'М', "--" },
    {'Н', "-." },
    {'О', "---" },
    {'П', ".--." },
    {'Р', ".-." },
    {'С', "..." },
    {'Т', "-" },
    {'У', "..-" },
    {'Ф', "..-." },
    {'Х', "...." },
    {'Ц', "-.-." },
    {'Ч', "---." },
    {'Ш', "----" },
    {'Щ', "--.-" },
    {'Ь', "-..-" },
    {'Ы', "-.--" },
    {'Э', "..-.." },
    {'Ю', "..--" },
    {'Я', ".-.-" },
    {'Ъ', "-..-" },
    {'0', "-----" },
    {'1', ".----" },
    {'2', "..---" },
    {'3', "...--" },
    {'4', "....-" },
    {'5', "....." },
    {'6', "-...." },
    {'7', "--..." },
    {'8', "---.." },
    {'9', "----." },
    {'.', "......" },
    {',', ".-.-.-" },
    {'?', "..--.." },
    {'-', "-....-" },
    {'"', ".-..-." },
    {';', "-.-.-." },
    {'!', "--..--" },
    {'(', "-.--.-" },
    {')', "-.--.-" },
    {':', "---..." },
    {'+', ".-.-.." }
};

        //Путь к файлу для воспроизведения
        public string pathToFile = "";
        //Текст для печати
        public string textFor = "";
        //Начало нажатия кнопки
        public DateTime startPush;
        //Конец  нажатия кнопки
        public DateTime endPush;
        //Таймер для отображения текущей буквы
        private System.Windows.Threading.DispatcherTimer timer;
        //Таймер для преобразования кода в букву
        private System.Windows.Threading.DispatcherTimer timerInput;
        //выключена ли прога
        bool isStopped = true;

        //на каком символе находимся
        int symbolNumber = 0;
        private void InitializeTimers()
        {
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timerInput = new System.Windows.Threading.DispatcherTimer();
            timerInput.Tick += new EventHandler(TimerInputTick);

        }
        public MainWindow()
        {


            InitializeComponent();
            InitializeTimers();

            //Загрузка настроек
            this.pathToFile = Properties.Settings.Default.pathToFile;
            this.sldrSpeed.Value = Properties.Settings.Default.speed;
            this.chckbxHelp.IsChecked = Properties.Settings.Default.checkedHelp;
            SaveMode();
        }
        /// <summary>
        /// Задаёт режим работы программы
        /// </summary>
        void SetMode()
        {
            switch (Properties.Settings.Default.mode)
            {
                //sandbox
                case 0:
                    this.rbtnSandbox.IsChecked = true;
                    break;
                //rewriting
                case 1:
                    this.rbtnRewriting.IsChecked = true;
                    break;

            }
        }
        void SaveMode()
        {
            if (this.rbtnSandbox.IsChecked == true)
            {
                Properties.Settings.Default.mode = 0;
            }
            else
            {
                Properties.Settings.Default.mode = 1;
            }
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
                case Znak.remove:
                    this.lblAddSymbol.Content = "";
                    break;
            }
        }
        private void TimerInputTick(object sender, EventArgs e)
        {
            if ((DateTime.Now - endPush).TotalMilliseconds > (1500 / sldrSpeed.Value))
            {
                AddCharToString();
                this.timerInput.Stop();
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
            Properties.Settings.Default.checkedHelp = this.chckbxHelp.IsChecked.Value;
            SetMode();
            Properties.Settings.Default.Save();
        }
        private void loadText()
        {
            this.tbTextForEnter.Text = File.ReadAllText(this.pathToFile);

        }
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (isStopped)
            {
                this.isStopped = false;
                this.btnStart.Content = "Stop";
                NextSymbolNumber();
            }
            else
            {
                this.isStopped = true;
                this.btnStart.Content = "Start";
            }
            this.Focus();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            timerInput.Stop();
            if (isStopped) return;
            //если нажат backspace
            if (e.Key == Key.Back)
            {
                //Пытаемся удалить одну букву из написанного слова
                try
                {
                    string s = this.lblEnteredText.Content.ToString();
                    s = s.Remove(s.Length - 1);
                    this.lblEnteredText.Content = s;
                    return;
                }
                catch (Exception ex)
                {
                    return;
                }
            }
            if (!e.IsRepeat)
            {
                timer.Start();
                this.startPush = DateTime.Now;
            }
        }
        //Определяет, получилась точка, тире или стирание буквы
        private Znak isZnak(TimeSpan ts)
        {
            //lblMorzeCode.Content = ts.Seconds.ToString();
            if (ts.TotalMilliseconds > (4000 / sldrSpeed.Value))
            {
                return Znak.remove;
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
            if (isStopped) return;
            Znak z = isZnak(DateTime.Now - startPush);
            switch (z)
            {
                case Znak.dot:
                    this.lblEnteredMorzeCode.Content += ".";
                    break;
                case Znak.dash:
                    this.lblEnteredMorzeCode.Content += "-";
                    break;
                case Znak.remove:
                    this.lblEnteredMorzeCode.Content = "";
                    break;
            }
            timer.Stop();
            lblAddSymbol.Content = "";
            this.endPush = DateTime.Now;
            timerInput.Start();
        }

        private void chckbxHelp_Checked(object sender, RoutedEventArgs e)
        {
            this.lblMorzeCode.Visibility = Visibility.Visible;
        }

        private void chckbxHelp_Unchecked(object sender, RoutedEventArgs e)
        {
            this.lblMorzeCode.Visibility = Visibility.Collapsed;
        }
        char GetCharByMorze(string code)
        {
            foreach (var pair in alphabet)
            {
                if (pair.Value == code)
                {
                    return pair.Key;
                }
            }
            return '\0';
        }
        string GetMorzeByChar(char ch)
        {
            foreach (var pair in alphabet)
            {
                if (pair.Key == ch)
                {
                    return pair.Value;
                }
            }
            return null;
        }
        void AddCharToString()
        {
            if (rbtnRewriting.IsChecked == true)
            {
                if (RightNumberCheck())
                {
                    lblEnteredText.Content += GetCharByMorze(lblEnteredMorzeCode.Content.ToString()).ToString();
                    
                    NextSymbolNumber();
                }
            }
            if (rbtnSandbox.IsChecked == true)
            {
                lblEnteredText.Content += GetCharByMorze(lblEnteredMorzeCode.Content.ToString()).ToString();

                NextSymbolNumber();
            }
            this.lblEnteredMorzeCode.Content = "";
           
        }
        /// <summary>
        /// Переход к следующему символу
        /// </summary>
        void NextSymbolNumber()
        {
            try
            {
                char symbol = tbTextForEnter.Text[symbolNumber];
                symbol = char.ToUpper(symbol);
                lblCharacterToEnter.Content = symbol;
                symbolNumber++;
                if (chckbxHelp.IsChecked == true)
                {
                    lblMorzeCode.Content = GetMorzeByChar(symbol);
                }
            }catch (System.IndexOutOfRangeException ex) { }
        }
        bool RightNumberCheck()
        {
            char enteredChar = GetCharByMorze(lblEnteredMorzeCode.Content.ToString());
            if (enteredChar == Convert.ToChar(lblCharacterToEnter.Content))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void tbTextForEnter_TextChanged(object sender, TextChangedEventArgs e)
        {
            symbolNumber = 0;
        }
    }
}
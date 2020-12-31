using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
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
using ColorPickerWPF;
using UtilityClassDLL;

namespace DaSuKeTeMoChi
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        Type type;
        Object _settings;
        PapagoEngine engine = new PapagoEngine();
        UiConfig.CheckConfigInfo check =new UiConfig.CheckConfigInfo();

        private double WindowX;
        private double WindowY;

        string Region = "Kor";
        #region propertyChange Event Handler

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string Name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Name));
        }

        #endregion
        #region Binder Method
        public double _Opactiys = 0.5;
        public double Opactiys
        {
            get { return _Opactiys; }
            set
            {

                _Opactiys = value;

                OnPropertyChanged("Opactiys");

            }
        }
        private int _sliderValue = 5;
        public int SliderValue
        {
            get { return _sliderValue; }
            set
            {
                _sliderValue = value;
                OnPropertyChanged("SliderValue");
            }
        }


        private SolidColorBrush _TextColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#000000");
        public SolidColorBrush TextColor
        {
            get { return _TextColor; }
            set
            {
                _TextColor = value;
                OnPropertyChanged("TextColor");
            }
        }

        private Color _fieldColor = (Color)ColorConverter.ConvertFromString("#000000");
        public Color FieldColor
        {
            get { return _fieldColor; }
            set
            {
                _fieldColor = value;
                OnPropertyChanged("FieldColor");
            }
        }
        #endregion

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;
            type = typeof(UiConfig.OverLaySettings);
            _settings = Activator.CreateInstance(type);


            MoveLocationConfig();
            MainWindows.Left = WindowX;
            MainWindows.Top = WindowY;

            if (check.CheckSettingsConfig())
            {

                CallSettings();
                SetUpSettings();
            }
            else
            {
                MessageBox.Show("세팅파일 불러오기/생성 실패! 다시켜주세요");
            }

        }

        protected void MoveLocationConfig()
        {
            if (check.FileCheckConfig(@"Config\LocationConfig.txt"))
            {
                string[] line = File.ReadAllLines(@"Config\LocationConfig.txt");
                WindowX = Convert.ToDouble(StrCut.StrChange(line[0], "=", null, true));
                WindowY = Convert.ToDouble(StrCut.StrChange(line[1], "=", null, true));


            }
        }





        protected void CallSettings()
        {
            string[] SettingValues = File.ReadAllLines(@"Config\Settings.txt");
            for (int i = 0; i < SettingValues.Length; i++)
            {
                string[] values = StrCut.ArrSplit(SettingValues[i], "=");
                Type ts = _settings.GetType().GetProperty(values[0]).PropertyType;
                _settings.GetType().GetProperty(values[0]).SetValue(_settings, Convert.ChangeType(values[1], ts), null);
            }
        }
        protected void SetUpSettings()
        {
            System.Diagnostics.Debug.WriteLine(Convert.ToInt32(_settings.GetType().GetProperty("SliderValue").GetValue(_settings, null)));
            TextColor = (SolidColorBrush)new BrushConverter().ConvertFrom((string)_settings.GetType().GetProperty("TextColor").GetValue(_settings, null));
            FieldColor = (Color)ColorConverter.ConvertFromString((string)_settings.GetType().GetProperty("FieldColor").GetValue(_settings, null));
            SliderValue = Convert.ToInt32(_settings.GetType().GetProperty("SliderValue").GetValue(_settings, null));
            double temp = (double)(Convert.ToDouble(SliderValue) / 10);
             Opactiys = temp;
        }


        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

           
            this.DragMove();

        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            var location = MainWindows.PointToScreen(new Point(0, 0));
            System.Diagnostics.Debug.WriteLine(location.ToString());
            WindowX = location.X;
            WindowY = location.Y;

            using (System.IO.StreamWriter SaveFile = new System.IO.StreamWriter(@"Config\LocationConfig.txt"))
            {
                    StreamHandler.Writer writer = new StreamHandler.Writer();
                    writer.WriteTextToFile(SaveFile, "X", location.X.ToString());
                    writer.WriteTextToFile(SaveFile, "Y", location.Y.ToString());
            }

            base.OnMouseLeftButtonUp(e);
        }

      
        private async Task<bool> Call_Papago_Transrator()
        {

            try
            {
                string InSertText = InsertTextBox.Text;
                Tuple<string, string> CallbackTuple = await engine.PostAsync(InSertText, Region);

                if (String.IsNullOrEmpty(CallbackTuple.Item1))
                {
                    return false;
                }


                string ResultText = CallbackTuple.Item1;

                string ttsText = CallbackTuple.Item2;

                if (!string.IsNullOrEmpty(ResultText))
                {
                    ResultTextBox.Text = ResultText;
                    SpeakText.Content = ttsText;


                    string CheckText = (await engine.TrueEmote(ResultText, Region));
                    CheckText = CheckText.Replace(" ", "");
                    System.Diagnostics.Debug.WriteLine(CheckText);
                    CheckTextBox.Text = CheckText;
                    return true;
                }
                return false;
            }
            catch
            { return false; }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            /* if (await Call_Papago_Transrator())
             {
                 Clipboard.SetText(ResultTextBox.Text);   
             }*/


            Color color;

            bool ok = ColorPickerWindow.ShowDialog(out color);

            if (ok)
            {
                System.Diagnostics.Debug.WriteLine(color.ToString());

                TextColor = (SolidColorBrush)new BrushConverter().ConvertFrom($"{color.ToString()}");

            }

        }

        private async void fieldColorChange_Button_Click(object sender, RoutedEventArgs e)
        {



            Color color;

            bool ok = ColorPickerWindow.ShowDialog(out color);

            if (ok)
            {
                System.Diagnostics.Debug.WriteLine(color.ToString());

                FieldColor = color;

            }

        }
        private void JpCon_Checked(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("isCheck - Jpcon"); 
            Region = "Kor";
        }
        private void KorCon_Checked(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("isCheck - Korcon"); 
            Region = "Jp";
        }

        private async void KeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                System.Diagnostics.Debug.WriteLine("You Put Enter");
                if (await Call_Papago_Transrator())
                {
                    try
                    {
                        Clipboard.SetText(ResultTextBox.Text);
                    }
                    catch
                    {

                    }
                }
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            double temp = e.NewValue / 10;
            Opactiys = temp;
            //System.Diagnostics.Debug.WriteLine(temp);
        }



        private async void Settings_Config_Click(object sender, RoutedEventArgs e)
        {
            NavigationWindow navigation = new NavigationWindow();

            UiConfig.SettingsWindows settingsWindows = new UiConfig.SettingsWindows();
            
            settingsWindows.Show();
            this.Close();

        }

        private async void Exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
            System.Diagnostics.Process.GetCurrentProcess().Kill();
            this.Close();
        }


    }
}


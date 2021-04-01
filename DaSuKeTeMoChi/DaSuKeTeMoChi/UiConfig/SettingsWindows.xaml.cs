using ColorPickerWPF;
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
using System.Windows.Shapes;
using System.Reflection;
using System.ComponentModel;
using UtilityClassDLL;
using System.IO;

namespace DaSuKeTeMoChi.UiConfig
{
    /// <summary>
    /// SettingsWindows.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SettingsWindows : Window,INotifyPropertyChanged
    {


        Type type;
        Object _settings;

        #region propertyChange Event Handler

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string Name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Name));
        }
        #endregion

        #region Binder Method

        private Color _FieldColorBinder;
        public Color FieldColorBinder
        {
            get => _FieldColorBinder;
            set {
                _FieldColorBinder = value;
                OnPropertyChanged("FieldColorBinder");
            }
        }
        
        private Color _TextColorBinder;
        public Color TextColorBinder
        {
            get => _TextColorBinder;
            set {
                _TextColorBinder = value;
                OnPropertyChanged("TextColorBinder");
            }
        }


        private int _sliderValue;
        public int SliderValue
        {
            get { return _sliderValue; }
            set
            {
                _sliderValue = value;
                OnPropertyChanged("SliderValue");
            }
        }


        #endregion



        public SettingsWindows()
        {
            InitializeComponent();

            this.DataContext = this;

            SetUpSettingsReflection();
            CallSettings();
            SetUpSettings();
        }


        protected void SetUpSettings()
        {

            TextColorBinder = (Color)ColorConverter.ConvertFromString((string)_settings.GetType().GetProperty("TextColor").GetValue(_settings, null));
            FieldColorBinder = (Color)ColorConverter.ConvertFromString((string)_settings.GetType().GetProperty("FieldColor").GetValue(_settings, null));
            SliderValue = Convert.ToInt32(_settings.GetType().GetProperty("SliderValue").GetValue(_settings, null));
            

        }
        protected void CallSettings()
        {
            string[] SettingValues = File.ReadAllLines(@"Config\Settings.txt");
            ushort Length = (ushort)SettingValues.Length;

            for (ushort i = 0; i < Length; i++)
            {
                string[] values = StrCut.ArrSplit(SettingValues[i], "=");
                Type ts = _settings.GetType().GetProperty(values[0]).PropertyType;
                _settings.GetType().GetProperty(values[0]).SetValue(_settings, Convert.ChangeType(values[1], ts), null);
            }
        }
        protected void SetUpSettingsReflection()
        {
            type = typeof(UiConfig.OverLaySettings);
            _settings = Activator.CreateInstance(type);
        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            this.DragMove(); 
        }
        private async void FieldColorChange_Button_Click(object sender, RoutedEventArgs e)
        {

            Color color;
            bool ok = ColorPickerWindow.ShowDialog(out color);
            if (ok)
            {
                FieldColorBinder = color;
                _settings.GetType().GetProperty("FieldColor").SetValue(_settings, color.ToString());
            }
        }
        private async void TextColorChange_Button_Click(object sender, RoutedEventArgs e)
        {
            Color color;
            bool ok = ColorPickerWindow.ShowDialog(out color);
            if (ok)
            {
                TextColorBinder = color;
                _settings.GetType().GetProperty("TextColor").SetValue(_settings, color.ToString());
            }
        }
        private async void Settings_Save_Button_Click(object sender, RoutedEventArgs e)
        {

            SaveDataToTextFile();
            MainWindow window = new MainWindow();

            window.Show();
            this.Close();
        }
       

        protected async void SaveDataToTextFile()
        { 
            string[] Lines;
            
            Lines = File.ReadAllLines(@"Config\Settings.txt");

            using (System.IO.StreamWriter SaveFile = new System.IO.StreamWriter(@"Config\Settings.txt"))
            {
                foreach (string lineText in Lines)
                {
                    string TempText = null;
                    int PixNum = Array.IndexOf(Lines, lineText);
                    TempText = StrCut.StrChange(lineText, null, "=", false);
                    string WriteValue =null;

                    switch (TempText)
                    {
                        case "TextColor":
                            WriteValue = $"{TextColorBinder}";
                            break;
                        case "FieldColor":
                            WriteValue = $"{FieldColorBinder}";
                            break;
                        case "SliderValue":
                            WriteValue = $"{SliderValue}";
                            break;
                        default:
                            break;
                    }
                    StreamHandler.Writer writer = new StreamHandler.Writer();

                    writer.WriteTextToFile(SaveFile, TempText,WriteValue); 
                }
            } 
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UiConfig.HotKey.HotKeyWindows hotKeyWindows = new UiConfig.HotKey.HotKeyWindows();
            hotKeyWindows.Show();
            this.Close(); 
        }
    }
}

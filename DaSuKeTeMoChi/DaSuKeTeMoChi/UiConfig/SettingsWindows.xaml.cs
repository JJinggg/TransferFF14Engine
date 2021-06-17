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
using Newtonsoft.Json;

namespace DaSuKeTeMoChi.UiConfig
{
    /// <summary>
    /// SettingsWindows.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SettingsWindows : Window,INotifyPropertyChanged
    {

        UiConfig.CheckConfigInfo check = new UiConfig.CheckConfigInfo();
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

        JsonProperty.Write_Panel_Setting_JsonProperty PanelSetting_Instance;

        public SettingsWindows()
        {
            InitializeComponent();

            this.DataContext = this;
            WritePannelConfig(); 
        }

        protected void WritePannelConfig()
        {
            string jsonPath = ResourceDex.PathResource.WritePannel;
            if (!check.CheckController(jsonPath)) return;

            using (StreamReader r = new StreamReader(jsonPath))
            {
                string jsontext = r.ReadToEnd();
                PanelSetting_Instance = JsonConvert.DeserializeObject<JsonProperty.Write_Panel_Setting_JsonProperty>(jsontext); 

                TextColorBinder = (Color)ColorConverter.ConvertFromString(PanelSetting_Instance.TextColor);
                FieldColorBinder = (Color)ColorConverter.ConvertFromString(PanelSetting_Instance.FieldColor);
                SliderValue = Convert.ToInt32(PanelSetting_Instance.SliderValue); 
            } 
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
                PanelSetting_Instance.FieldColor = color.ToString(); 
            }
        }
        private async void TextColorChange_Button_Click(object sender, RoutedEventArgs e)
        {
            Color color;
            bool ok = ColorPickerWindow.ShowDialog(out color);
            if (ok)
            {
                TextColorBinder = color;
                PanelSetting_Instance.TextColor = color.ToString(); 
            }
        }
        private async void Settings_Save_Button_Click(object sender, RoutedEventArgs e)
        {

            SaveJsonFile();
            MainWindow window = new MainWindow();

            window.Show();
            this.Close();
        }

        protected async void SaveJsonFile()
        {
            if (!check.CheckController(ResourceDex.PathResource.WritePannel)) return;

            string JsonToText = JsonConvert.SerializeObject(PanelSetting_Instance);

            File.WriteAllText(ResourceDex.PathResource.WritePannel, JsonToText);
        } 

        private void InHotKey(object sender, RoutedEventArgs e)
        {
            UiConfig.HotKey.HotKeyWindows hotKeyWindows = new UiConfig.HotKey.HotKeyWindows();
            hotKeyWindows.Show();
            this.Close(); 
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        { 
            PanelSetting_Instance.SliderValue = e.NewValue.ToString();
        }
    }
}

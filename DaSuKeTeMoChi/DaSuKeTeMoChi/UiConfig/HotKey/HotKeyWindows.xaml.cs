using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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

namespace DaSuKeTeMoChi.UiConfig.HotKey
{
    /// <summary>
    /// HotKeyWindows.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class HotKeyWindows : Window, INotifyPropertyChanged
    {
        public HotKeyWindows()
        {
            InitializeComponent();

            this.DataContext = this;
            LoadConfig();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string Name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Name));
        }

        /*
         * 1. 입력창으로 자동이동
         * 2. 출력값 복사 
         * 
         */
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            this.DragMove();

        }
        private string _MoveInputText = null;
        public string MoveInputText
        {
            get => _MoveInputText;
            set
            {
                _MoveInputText = value;
                OnPropertyChanged("MoveInputText");
            }
        }


        private string _CopyPutText = null;
        public string CopyPutText
        {
            get => _CopyPutText;
            set { _CopyPutText = value;
                OnPropertyChanged("CopyPutText");
            } 
        }   
        private async void InputSetClick(object sender, RoutedEventArgs e) // 처음 hotkey
        {
            UiConfig.HotKey.GetKey GetKeyWindow= new UiConfig.HotKey.GetKey();
            GetKeyWindow.Show();

            MoveInputText = await GetKeyWindow.ReturnValue();
             
        }

        private async void OutputSetClick(object sender, RoutedEventArgs e) // 뒤 hotkey
        {
            UiConfig.HotKey.GetKey GetKeyWindow = new UiConfig.HotKey.GetKey();
            GetKeyWindow.Show();

            CopyPutText = await GetKeyWindow.ReturnValue();
        }

        private void Close(object sender, RoutedEventArgs e)
        {

            SaveConfig();
            UiConfig.SettingsWindows settingsWindows = new UiConfig.SettingsWindows();
            settingsWindows.Show();
            this.Close();
        }

        protected void SaveConfig()
        {
            string selectKey = null;
            string outcopyKey = null;

            if (!string.IsNullOrEmpty(MoveInputText)) selectKey = MoveInputText;
            if (!string.IsNullOrEmpty(CopyPutText)) outcopyKey = CopyPutText;

            JObject job = new JObject();
            job.Add("inputSelectKey", selectKey);
            job.Add("outputCopyKey", outcopyKey);

            File.WriteAllText(@"Config\HotKeys.json", job.ToString());

        }


        protected void LoadConfig()
        {
            string jsonPath = @"Config\HotKeys.json";
            using (StreamReader r = new StreamReader(jsonPath))
            {
                string jsontext = r.ReadToEnd();
                var json = JsonConvert.DeserializeObject<UiConfig.HotKey.HotKeyJsonProperty.HotKeys>(jsontext);

                System.Diagnostics.Debug.WriteLine(json.InputSelectKey + "/" + json.OutputCopyKey);


                if (!String.IsNullOrEmpty(json.InputSelectKey))
                {
                    MoveInputText = json.InputSelectKey; 
                }

                if (!String.IsNullOrEmpty(json.OutputCopyKey))
                {
                    CopyPutText = json.OutputCopyKey;
                }
            }

        }
    }
}

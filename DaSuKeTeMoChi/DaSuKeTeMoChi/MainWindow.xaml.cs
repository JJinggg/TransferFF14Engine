using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
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
using DaSuKeTeMoChi.UiConfig.HotKey;
using Newtonsoft.Json;
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



        HotsKey ActivateHotkey;
        HotsKey ResultCopyHotkey;



/*        [DllImport("user32.dll")]
        public static extern int FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
        private const int SW_SHOWNORMAL = 1;
        private const int SW_SHOWMINIMIZED = 2;
        private const int SW_SHOWMAXIMIZED = 3;

        void Find()
        {
            // 윈도우 타이틀명으로 핸들을 찾는다
            IntPtr hWnd = (IntPtr)FindWindow(null, "Test");
            if (!hWnd.Equals(IntPtr.Zero))
            {
                // 윈도우가 최소화 되어 있다면 활성화 시킨다
                ShowWindowAsync(hWnd, SW_SHOWNORMAL);
                // 윈도우에 포커스를 줘서 최상위로 만든다
                SetForegroundWindow(hWnd);
            }
        } */

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
            BindKey();
            try
            {
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
            catch
            {
                MessageBox.Show("오류입니다. 다시켜주세요");
            }

        }
         
        public void BindKey()
        {
            string jsonPath = @"Config\HotKeys.json";

           if (!check.CheckHotKeyConfig()) return;
            
            using (StreamReader r = new StreamReader(jsonPath))
            {
                string jsontext = r.ReadToEnd();
                var json = JsonConvert.DeserializeObject<UiConfig.HotKey.HotKeyJsonProperty.HotKeys>(jsontext);

                System.Diagnostics.Debug.WriteLine(json.InputSelectKey + "/" + json.OutputCopyKey);


                if (!String.IsNullOrEmpty(json.InputSelectKey))
                {
                    Tuple<Key, KeyModifier> InputTuple = GetHotKey(json.InputSelectKey);
                    ActivateHotkey= new HotsKey(InputTuple.Item1, InputTuple.Item2, ActiveHandler);

                    
                }

                if (!String.IsNullOrEmpty(json.OutputCopyKey))
                {
                    Tuple<Key, KeyModifier> OutputTuple = GetHotKey(json.OutputCopyKey);
                    ResultCopyHotkey= new HotsKey(OutputTuple.Item1, OutputTuple.Item2, resultCopyHandler);
                } 
            }
        }

        private Tuple<Key,KeyModifier> GetHotKey(string jsontext)
        {
             
            string[] Inputkey = StrCut.ArrSplit(jsontext, "+");

            Key Insk = Key.None;
            KeyModifier modi = KeyModifier.None;


            foreach (var v in Inputkey)
            {

                var enums = (Key)Enum.Parse(typeof(Key), v, true);

                if (enums != Key.RightAlt &&
                    enums != Key.LeftAlt &&
                    enums != Key.RightCtrl &&
                    enums != Key.LeftCtrl &&
                    enums != Key.RightShift &&
                    enums != Key.LeftShift)
                {
                    Insk = enums;
                }
                else
                {
                    if (enums == Key.RightAlt || enums == Key.LeftAlt) modi |= KeyModifier.Alt;
                    if (enums == Key.RightCtrl || enums == Key.LeftCtrl) modi |= KeyModifier.Ctrl;
                    if (enums == Key.RightShift || enums == Key.LeftShift) modi |= KeyModifier.Shift;
                }
            }
            return Tuple.Create(Insk, modi);
        }
        
        private void ActiveHandler(HotsKey hotKey)
        {
            this.Activate();
            InsertTextBox.Focus();
        }

        private void resultCopyHandler(HotsKey hotKey)
        {

            try
            {
                Clipboard.SetText(ResultTextBox.Text);
            }
            catch
            {

            }
        }
         
        protected void MoveLocationConfig()
        {
            if (check.FileCheckConfig(@"Config\LocationConfig.txt"))
            {
                try
                {


                    string[] line = File.ReadAllLines(@"Config\LocationConfig.txt");
                    WindowX = Convert.ToDouble(StrCut.StrChange(line[0], "=", null, true));
                    WindowY = Convert.ToDouble(StrCut.StrChange(line[1], "=", null, true));
                }
                catch
                {
                    return;
                }
            }
        } 
        protected void CallSettings()
        {
            try
            {
                string[] SettingValues = File.ReadAllLines(@"Config\Settings.txt");
                for (int i = 0; i < SettingValues.Length; i++)
                {
                    string[] values = StrCut.ArrSplit(SettingValues[i], "=");
                    Type ts = _settings.GetType().GetProperty(values[0]).PropertyType;
                    _settings.GetType().GetProperty(values[0]).SetValue(_settings, Convert.ChangeType(values[1], ts), null);
                }
            }
            catch
            {
                return;
            }
        }
        protected void SetUpSettings()
        {
            try
            {
                TextColor = (SolidColorBrush)new BrushConverter().ConvertFrom((string)_settings.GetType().GetProperty("TextColor").GetValue(_settings, null));
                FieldColor = (Color)ColorConverter.ConvertFromString((string)_settings.GetType().GetProperty("FieldColor").GetValue(_settings, null));
                SliderValue = Convert.ToInt32(_settings.GetType().GetProperty("SliderValue").GetValue(_settings, null));
                double temp = (double)(Convert.ToDouble(SliderValue) / 10);
                Opactiys = temp;
            }
            catch
            {
                return;
            }
        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            try
            {
                base.OnMouseLeftButtonDown(e);
                this.DragMove();
            }
            catch
            {
                return;
            }
        }
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            try
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
            }
            catch
            {
                return;
            }
            base.OnMouseLeftButtonUp(e);
        }
        private async Task<bool> Call_Papago_Transrator()
        {

            try
            {
                string InSertText = InsertTextBox.Text;
                Tuple<string, string> CallbackTuple = await engine.PostAsync(InSertText, Region);

                if (String.IsNullOrEmpty(CallbackTuple.Item1)||string.IsNullOrEmpty(CallbackTuple.Item2))
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
                    if (String.IsNullOrEmpty(CheckText)) return false;

                    CheckText = CheckText.Replace(" ", "");
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
            try
            {
                Color color;
                bool ok = ColorPickerWindow.ShowDialog(out color);
                if (ok)
                {
                    TextColor = (SolidColorBrush)new BrushConverter().ConvertFrom($"{color.ToString()}");
                }
            }
            catch
            {
                
            }
        }
        private async void fieldColorChange_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Color color;

                bool ok = ColorPickerWindow.ShowDialog(out color);

                if (ok)
                {
                    FieldColor = color; 
                }
            }
            catch
            { 

            } 
        }
        private void JpCon_Checked(object sender, RoutedEventArgs e)
        {
            Region = "Kor";
        }
        private void KorCon_Checked(object sender, RoutedEventArgs e)
        {
            Region = "Jp";
        }
        private void EKCon_Checked(object sender, RoutedEventArgs e)
        {
            Region = "EK";
        }
        private void KECon_Checked(object sender, RoutedEventArgs e)
        {
            Region = "KE";
        }

        private async void KeyDownHandler(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return)
                {
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
            catch
            {
                return;
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            try
            {
                double temp = e.NewValue / 10;
                Opactiys = temp; 
            }
            catch
            {
                
            }
        } 

        private async void Settings_Config_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationWindow navigation = new NavigationWindow();

                UiConfig.SettingsWindows settingsWindows = new UiConfig.SettingsWindows();


/*
                ActivateHotkey.Unregister();
                ResultCopyHotkey.Unregister();*/


                settingsWindows.Show();
                this.Close();
            }
            catch
            { 
            }

        }

        private async void Exit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                this.Close();
            }
            catch
            {
                Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                this.Close();
            }
        }


    }
}


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
    public partial class MainWindow : Window , INotifyPropertyChanged
    {
        PropertyInfo Property_Opactiys;
        PropertyInfo Property_TextColor;
        PropertyInfo Property_FieldColor;
        PropertyInfo Property_SliderValue;
            


        PapagoEngine engine = new PapagoEngine();
        string Region = "Kor";
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;

            CallSettings();
        }


        protected void CallSettings()
        {
            string[] SettingValues = File.ReadAllLines(@"Settings.txt");


            Type type = typeof(UiConfig.OverLaySettings);
            Object _settings = Activator.CreateInstance(type);
            Property_Opactiys = type.GetProperty("Opactiys");
            Property_TextColor = type.GetProperty("TextColor");
            Property_FieldColor = type.GetProperty("FieldColor");
            Property_SliderValue = type.GetProperty("SliderValue");


            Property_TextColor.SetValue(_settings, "3030",null);

            Console.WriteLine(Property_TextColor.GetValue(_settings, null));

            for (int i = 0; i < SettingValues.Length; i++)
            {
                string[] values = StrCut.ArrSplit(SettingValues[i], "=");
                Type ts =_settings.GetType().GetProperty(values[0]).PropertyType;
                
                _settings.GetType().GetProperty(values[0]).SetValue(_settings, Convert.ChangeType(values[1],ts), null);


                System.Diagnostics.Debug.WriteLine(">>>>>>>>" + _settings.GetType().GetProperty(values[0]).GetValue(_settings, null));
            }







        }


        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            this.DragMove();

        }

        public double _Opactiys = 0.5;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public double Opactiys
        {
            get {return _Opactiys; }
            set {
                
                _Opactiys  = value;

                OnPropertyChanged("Opactiys");

            }
        }
        private int _sliderValue = 5;
        public int SliderValue
        {
            get { return _sliderValue; }
            set {
                _sliderValue = value;
                OnPropertyChanged("SliderValue");
            }
        }


        private SolidColorBrush _TextColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#000000");
        public SolidColorBrush TextColor
        {
            get { return _TextColor; }
            set {
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
            { return false;  }
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
         
            InsertLabel.Content = "入力 テキスト";
            ResultLabel.Content = "出力 テキスト";
            Region = "Kor";
        }

        private void KorCon_Checked(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("isCheck - Korcon");

           

            InsertLabel.Content = "입력 택스트";
            ResultLabel.Content = "출력 택스트";
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
    }
}

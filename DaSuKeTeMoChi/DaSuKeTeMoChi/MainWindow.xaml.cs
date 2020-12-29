using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

namespace DaSuKeTeMoChi
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window , INotifyPropertyChanged
    {
  
        PapagoEngine engine = new PapagoEngine();
        string Region = "Kor";
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;
            
          
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

        private SolidColorBrush _TextColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#000000");
        public SolidColorBrush TextColor
        {
            get { return _TextColor; }
            set {
                  _TextColor = value;
                  OnPropertyChanged("TextColor");
                }
        }


        private async Task<bool> Call_Papago_Transrator()
        {
            string InSertText = InsertTextBox.Text;
            Tuple<string,string> CallbackTuple = await engine.PostAsync(InSertText, Region);

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
                    Clipboard.SetText(ResultTextBox.Text);
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

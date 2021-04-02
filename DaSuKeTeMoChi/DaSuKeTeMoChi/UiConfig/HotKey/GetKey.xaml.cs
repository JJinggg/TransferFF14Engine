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

namespace DaSuKeTeMoChi.UiConfig.HotKey
{
    /// <summary>
    /// GetKey.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class GetKey : Window
    {
        TaskCompletionSource<string> _tcs = null;
        public GetKey()
        {
            InitializeComponent();

            this.KeyDown += GetKey_KeyDown;


        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            this.DragMove();
        }
        private void GetKey_KeyDown(object sender, KeyEventArgs e)
        {
            bool Ctrl = false;
            bool Alt = false;
            bool Shift = false;
            string BuildText = String.Empty;
            if (Keyboard.IsKeyDown(Key.LeftCtrl)) BuildText += Key.LeftCtrl.ToString()+"+";
            if (Keyboard.IsKeyDown(Key.RightCtrl)) BuildText +=Key.RightCtrl.ToString() + "+";
            if (Keyboard.IsKeyDown(Key.LeftAlt )) BuildText += Key.LeftAlt.ToString() + "+";
            if (Keyboard.IsKeyDown(Key.RightAlt)) BuildText += Key.RightAlt.ToString() + "+";
            if (Keyboard.IsKeyDown(Key.LeftShift )) BuildText += Key.LeftShift.ToString() + "+";
            if (Keyboard.IsKeyDown(Key.RightShift)) BuildText +=Key.RightShift.ToString() + "+";


            if (BuildText.LastIndexOf("+") == BuildText.Length) BuildText = BuildText.Substring(0, BuildText.Length - 1);
            if(!(e.Key ==Key.LeftShift|| 
                e.Key == Key.RightShift|| 
                e.Key == Key.LeftCtrl ||
                e.Key == Key.RightCtrl ||
                e.Key == Key.LeftAlt ||
                e.Key == Key.RightAlt )) BuildText += e.Key.ToString();

            InputText.Content = BuildText;


        }
         

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _tcs?.SetResult(InputText.Content.ToString());
                this.Close();
            }
            catch
            {

            }
        }

        public async Task<string> ReturnValue()
        {
            _tcs = new TaskCompletionSource<string>();
            return await _tcs.Task;
        }
    }
}

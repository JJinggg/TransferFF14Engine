﻿using System;
using System.Collections.Generic;
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

namespace DaSuKeTeMoChi
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
  
        PapagoEngine engine = new PapagoEngine();

        public MainWindow()
        {
            InitializeComponent();
        }


        private async void Button_Click(object sender, RoutedEventArgs e)
        {
         /*   string Encdata = "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";
            string aef = EncCode.StringCipher.Decrypt(Encdata, "7790451bdf519bf976aaiyJYl9da8a302");*/
            string InSertText = InsertTextBox.Text;
            
            ResultTextBox.Text= await engine.PostAsync(InSertText);
        }

        private void JpCon_Checked(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("isCheck - Jpcon");

            InsertLabel.Content = "入力 テキスト";
            ResultLabel.Content = "出力 テキスト";
        }

        private void KorCon_Checked(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("isCheck - Korcon");

            InsertLabel.Content = "입력 택스트";
            ResultLabel.Content = "출력 택스트";
        }
    }
}

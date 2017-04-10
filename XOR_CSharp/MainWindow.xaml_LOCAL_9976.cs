using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace XOR_CSharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //qqqqqqqqqqqqqqqqqq
        public SynchronizationContext uiContext;
        CancellationTokenSource cts;
        static int Key2 = 0;
        static string s="";
        string FILE_NAME = "";
        int number;

        public MainWindow()
        {
            //aaaaaaaaaa
            uiContext = SynchronizationContext.Current;
            InitializeComponent();
            Button1.IsEnabled = false;
            Button2.IsEnabled = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                MyTextBox.Text = File.ReadAllText(openFileDialog.FileName, Encoding.Default);
                Button1.IsEnabled = true;
            }
            FILE_NAME = openFileDialog.FileName;
        }
     
        private async void Button1_Click(object sender, RoutedEventArgs e)
        {
            uiContext.Send(d=>s=MyKey.Text,null);
            if (s == "")
            {
                s="0";
            }
            
            try
            {
                bool result = Int32.TryParse(s, out number);
                if (result)
                {
                }
                else
                {
                    MessageBox.Show("Enter the correct int key.");
                    s = "0";
                }
            }
            catch { }

            Key2 = Int32.Parse(s);

            Test();
        }

        private async void Test()
        {
            cts = new CancellationTokenSource();
            MyTextBox.Text = "Please wait";
            Button2.IsEnabled = true;
            await Task.Run(() => EncodingDecoding(cts.Token, FILE_NAME, Key2), cts.Token);
            Button2.IsEnabled = false;
            MyTextBox.Text = File.ReadAllText(FILE_NAME, Encoding.Default);
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            cts.Cancel();
        }

        public void EncodingDecoding(CancellationToken token, string file_name, int key)
        {
            byte[] buf = File.ReadAllBytes(file_name);
            for (int i = 0; i < buf.Length; i++)
            {
                buf[i] = (byte)(buf[i] ^ key);
//                Thread.Sleep(10);
                if (token.IsCancellationRequested)
                {
                    MessageBox.Show("Cancelled");
                    break;
                }
            }
            File.WriteAllBytes(file_name, buf);
        }
    }
}

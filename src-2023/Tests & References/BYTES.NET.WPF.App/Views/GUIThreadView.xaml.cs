using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BYTES.NET.WPF.App.Views
{
    /// <summary>
    /// Interaction logic for GUIThreadView.xaml
    /// </summary>
    public partial class GUIThreadView : Window
    {
        public event EventHandler<string> TextSubmitted;

        public GUIThreadView()
        {
            InitializeComponent();
            GUIInputText.TextChanged += GUIInputText_TextChanged;
            Button.Click += ButtonClick; // Assuming you have a button named Button
        }

        private void GUIInputText_TextChanged(object sender, RoutedEventArgs e)
        {
            string text = GUIInputText.Text;
            TextSubmitted?.Invoke(this, text);
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            string text = GUIInputText.Text;
            TextSubmitted?.Invoke(this, text);
        }
    }
}

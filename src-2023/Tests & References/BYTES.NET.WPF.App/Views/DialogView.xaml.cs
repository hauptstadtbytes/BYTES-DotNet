using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using BYTES.NET.WPF.App.ViewModels;

namespace BYTES.NET.WPF.App.Views
{
    public partial class DialogView : Window
    {
        public event EventHandler<string> TextSubmitted;
        public DialogView()
        {
            InitializeComponent();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            // Get the text from the TextBox
            string text = InputText.Text;

            // Do something with the text (for example, display it in a message box)
            TextSubmitted?.Invoke(this, text);

            Close();
        }
    }
}

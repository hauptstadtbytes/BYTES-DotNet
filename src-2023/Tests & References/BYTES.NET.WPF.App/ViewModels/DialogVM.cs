using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BYTES.NET.WPF.App.Views;
using BYTES.NET.WPF.MVVM;

namespace BYTES.NET.WPF.App.ViewModels
{
    internal class DialogVM : DialogViewModel
    {
        protected override void CloseView()
        {
            throw new NotImplementedException();
        }

        protected override void ShowView()
        {
            DialogView view = new DialogView();
            view.ShowDialog();
        }

        
    }
}

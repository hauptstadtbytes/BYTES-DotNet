//import .net (default) namespace(s)
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using BYTES.NET.Primitives;


//import namespace(s) required from 'BYTES.NET.WPF' framework
using BYTES.NET.WPF.MVVM;

namespace BYTES.NET.WPF.App.ViewModels
{
    public class MainVM : ViewModel
    {
        #region private variable(s)

        private string _title = "WPF Sample Application";

        private AnimalVM[] _animals;

        private DialogVM _dialogVM;

        private StringMatchingVM _matchingVM = new StringMatchingVM(); //contains the entire example for string matching

        #endregion

        #region private variable(s), for the validation example(s)

        private int? _theAnswer = null;

        #endregion

        #region private variable(s), for the dialog example(s)

        private bool _showDialogBlocking = false;
        private string _dialogMessage = "Hello World!";

        #endregion

        #region public properties

        private string _outputText;
        
        public string Title
        {
            get => _title; set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public AnimalVM[] Animals { get => GetAnimals(); set
            {
                _animals = value;
                OnPropertyChanged();
            }
        }

        public StringMatchingVM StringMatching { get => _matchingVM; }

        #endregion

        #region public properties, for the validation example(s)

        public int? TheAnswer
        {
            get => _theAnswer; set
            {
                _theAnswer = value;
                OnPropertyChanged(true); //the 'true' parameter triggers the (re-evaluation)
            }
        }

        #endregion

        #region public properties for the dialog example(s)

        public bool ShowDialogBlocking
        {
            get => _showDialogBlocking; set
            {
                _showDialogBlocking = value;
                OnPropertyChanged();
            }
        }

        public string DialogMessage { 
            get => _dialogMessage; 
            set 
            { 
                _dialogMessage = value;
                OnPropertyChanged();
            } 
        }

        #endregion

        #region public new instance method(s)

        /// <summary>
        /// default new instance method
        /// </summary>
        public MainVM()
        {

            this.Title = "Sample";
            _animals = GetAnimals();

            //add command(s)
            this.Commands.Add("PromptTextCmd", new ViewModelRelayCommand(PromptText));

            //add validation rule(s)
            //this.ValidationRules.Add(new ViewModelValidationRule("TheAnswer",))

            // add DialogueViewModel Command
            this.Commands.Add("ShowDialogCmd", new ViewModelRelayCommand(ShowDialog));

        }

        #endregion

        #region private method(s)

        /// <summary>
        /// returns an example array of animals
        /// </summary>
        /// <returns></returns>
        private AnimalVM[] GetAnimals()
        {
            List<AnimalVM> output = new List<AnimalVM>();

            output.Add(new AnimalVM("Sparky", "Dog"));
            output.Add(new AnimalVM("Birdy", "Bird"));

            return output.ToArray();
        }

        #endregion

        #region private method(s), for the command example(s)

        /// <summary>
        /// prompts the title text/ a relay command example
        /// </summary>
        /// <param name="arg"></param>
        private void PromptText(object arg)
        {
            MessageBox.Show((string)arg);
        }

        #endregion

        #region private method(s), for the validation example(s)

        /// <summary>
        /// prompts the title text/ a relay command example
        /// </summary>
        /// <param name="arg"></param>
        //private ViewModelValidationResult[] ValidateTheAnswer(object arg)
        //{

        //}

        #endregion

        #region private method(s) for dialog example(s)

        /// <summary>
        /// opens up the DialogView (possibly blocking the MainView instance)
        /// </summary>
        /// <param name="arg"></param>
        private void ShowDialog(object arg)
        {
            //create a new instance of the dialog view model
            DialogVM dialog = new DialogVM() { DialogMessage = this.DialogMessage };

            //set the event handlers
            if(!ShowDialogBlocking)
            {
                dialog.PropertyChanged += HandleDialogPropertyChanged;
            }
            
            dialog.DialogClosed += HandleDialogClosed;

            //open the dialog
            dialog.ShowDialog(ShowDialogBlocking);
        }

        /// <summary>
        /// handles the property changed event for the dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleDialogPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "DialogMessage")
            {
                DialogVM dialog = (DialogVM)sender;
                this.DialogMessage = dialog.DialogMessage + " (OnPropertyChanged)";
            }
            
        }

        /// <summary>
        /// handles the dialog closing
        /// </summary>
        /// <param name="sender"></param>
        private void HandleDialogClosed(object? sender)
        {
            DialogVM dialog = (DialogVM)sender;
            this.DialogMessage = dialog.DialogMessage + " (On Closed)";
        }

        #endregion
  
    }
}

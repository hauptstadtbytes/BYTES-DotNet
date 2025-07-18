//import .net (default) namespace(s)
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Microsoft.Win32;

//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.Logging;
using BYTES.NET.Logging.Appenders;
using BYTES.NET.Primitives;

using Ookii.Dialogs.Wpf;

//import namespace(s) required from 'BYTES.NET.WPF' framework
using BYTES.NET.WPF.MVVM;
using BYTES.NET.WPF.MVVM.Menu;
using BYTES.NET.WPF.MVVM.Dialog;

namespace BYTES.NET.WPF.App.ViewModels
{
    public class MainVM : ViewModel
    {
        #region private variable(s)

        private string _title = "WPF Sample Application";

        private AnimalVM[] _animals;

        private DialogVM _dialogVM;

        private StringMatchingVM _matchingVM = new StringMatchingVM(); //contains the entire example for string matching
        private LoggingVM _loggingVM = new LoggingVM(); //contains the entire logging example

        private string _sampleInputString = string.Empty;

        private int _progressTotal = 0;
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

        public string SampleInputString { get => _sampleInputString; set
            {
                _sampleInputString = value;
                OnPropertyChanged();
                OnPropertyChanged("SampleStringList");
                OnPropertyChanged("SampleStringListCount");
            } 
        }

        public string[] SampleStringList
        {
            get
            {
                if(_sampleInputString == null || string.IsNullOrEmpty(_sampleInputString))
                {
                    return [];
                } else
                {

                    List<string> list = new List<string>();

                    foreach (string s in _sampleInputString.Split(','))
                    {
                        if (!string.IsNullOrWhiteSpace(s))
                        {
                            list.Add(s);
                        }
                    }

                    return list.ToArray();
                }
                
            }
        }

        public string SampleStringListCount
        {
            get
            {
                return this.SampleStringList.Length.ToString();

            }
        }

        public StringMatchingVM StringMatching { get => _matchingVM; }
        public LoggingVM Logging { get => _loggingVM; }

        #endregion

        #region public properties for the validation example(s)

        public int? TheAnswer
        {
            get => _theAnswer; set
            {
                _theAnswer = value;
                OnPropertyChanged(true); //the 'true' parameter triggers the (re-evaluation)
            }
        }

        public int ProgressTotal
        {
            get => _progressTotal; set
            {
                _progressTotal = value;
                OnPropertyChanged();
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

        #region public properties for the menu example(s)

        public ObservableCollection<MenuItemViewModel> Menu { get => GetMenu(); }

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

            // add DialogueViewModel Command(s)
            this.Commands.Add("ShowDialogCmd", new ViewModelRelayCommand(ShowDialog));
            this.Commands.Add("ShowProgressDialogCmd", new ViewModelRelayCommand(ShowProgressDialog));

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

        #region private method(s) for the command example(s)

        /// <summary>
        /// prompts the title text/ a relay command example
        /// </summary>
        /// <param name="arg"></param>
        private void PromptText(object arg)
        {
            MessageBox.Show((string)arg);
        }

        #endregion

        #region private method(s) for the validation example(s)

        /// <summary>
        /// prompts the title text/ a relay command example
        /// </summary>
        /// <param name="arg"></param>
        //private ViewModelValidationResult[] ValidateTheAnswer(object arg)
        //{

        //}

        #endregion

        #region private method(s) for the menu example(s)

        private ObservableCollection<MenuItemViewModel> GetMenu()
        {

            ObservableCollection<MenuItemViewModel> output = new ObservableCollection<MenuItemViewModel>();

            output.Add(new MenuItemViewModel() { Caption = "Menu" });

            ObservableCollection<MenuItemViewModel> toolsMenu = new ObservableCollection<MenuItemViewModel>();
            toolsMenu.Add(new MenuItemViewModel() { Caption = "Show Dialog", Command = new ViewModelRelayCommand(PromptText) });

            output.Add(new MenuItemViewModel() { Caption = "Action", Children = toolsMenu });

            return output;
        }

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

        //shows a progress dialog
        private async void ShowProgressDialog()
        {
            var dialog = new ProgressDialogViewModel("Show the Progress")
            {
                Message = "This is a sample progress dialog for demonstration",
                Total = this.ProgressTotal // Pass total seconds from VM property
            };

            dialog.DialogClosed += HandleDialogClosed;

            // Show dialog (blocking or not based on ShowDialogBlocking)
            dialog.ShowDialog(ShowDialogBlocking);

            // If total seconds is > 0, simulate progress
            if (this.ProgressTotal > 0)
            {
                for (int i = 0; i <= this.ProgressTotal; i++)
                {
                    dialog.Current = i;
                    dialog.Message = $"Loading... {i} / {this.ProgressTotal} seconds";

                    await Task.Delay(1000); // wait 1 second

                    if (i == this.ProgressTotal)
                    {
                        dialog.CloseDialog(); // close dialog on finish
                    }
                }
            }
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
            if (sender is DialogVM dialog)
            {
                this.DialogMessage = dialog.DialogMessage + " (On Closed)";
            }
            else if (sender is ProgressDialogViewModel progressDialog)
            {
                this.DialogMessage = progressDialog.Message + " (Progress Closed)";
            }
        }

        #endregion

    }
}

//import .net (default) namespace(s)
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using BYTES.NET.Logging;
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

        private string _sampleInputString = string.Empty;
        #endregion

        #region private variable(s), for the validation example(s)

        private int? _theAnswer = null;

        #endregion

        #region private variable(s), for the dialog example(s)

        private bool _showDialogBlocking = false;
        private string _dialogMessage = "Hello World!";

        #endregion

        #region private variable(s), for the logging example(s)

        private string _logText;

        private Log _log;

        private LogEntry.InformationLevel _selectedInformationLevel;

        private ObservableCollection<LogEntry> _logEntries = new ObservableCollection<LogEntry>();

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

        #region public properties for the logging example(s)

        public string LogText
        {
            get => _logText; set
            {
                _logText = value;
                OnPropertyChanged();
            }
        }

        public Array InformationLevels => Enum.GetValues(typeof(LogEntry.InformationLevel));

        public LogEntry.InformationLevel SelectedInformationLevel
        {
            get => _selectedInformationLevel;
            set
            {
                _selectedInformationLevel = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<LogEntry> LogEntries
        {
            get => _logEntries;
            set
            {
                _logEntries = value;
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

            //add logging Command
            this.Commands.Add("LogCmd", new ViewModelRelayCommand(LogMessage));

            // initialize Logging Components
            SelectedInformationLevel = LogEntry.InformationLevel.Info;
            _log = new Log("MainLog");

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

        #region private method(s) for logging example(s)

        /// <summary>
        /// logs a message
        /// </summary>
        /// <param name="arg"></param>
        private void LogMessage(object arg)
        {
            // Create a new log entry
            LogEntry entry = new LogEntry(LogText, SelectedInformationLevel);


            if (!string.IsNullOrEmpty(LogText))
            {
                // Log the entry
                _log.Write(entry);

                // Add the log entry to the collection
                LogEntries.Add(entry);

                // Clear the log text after logging
                LogText = string.Empty;
            }
        }
        #endregion

    }
}

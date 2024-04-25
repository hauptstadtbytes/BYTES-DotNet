//import .net (default) namespace(s)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using BYTES.NET.WPF.App.Views;

//import namespace(s) required from 'BYTES.NET.WPF' framework
using BYTES.NET.WPF.MVVM;

namespace BYTES.NET.WPF.App.ViewModels
{
    public class MainVM : ViewModel
    {
        #region private variable(s)

        private string _title = "WPF Sample Application";

        private AnimalVM[] _animals;

        #endregion

        #region private variable(s), for the validation example(s)

        private int? _theAnswer = null;

        #endregion

        #region public properties

        private string _outputText;
        public string OutputText
        {
            get => _outputText;
            set
            {
                _outputText = value;
                OnPropertyChanged(nameof(OutputText));
            }
        }
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
            this.Commands.Add("ShowDialogueCmd", new ViewModelRelayCommand(ShowDialog));

            // Add ThreadViewModel Command
            this.Commands.Add("ShowGUIThreadCmd", new ViewModelRelayCommand(ShowThread));

            // Update the output text in the main window from different thread
            //this.Commands.Add("UpdateOutputTextGUIThreadCmd", new ViewModelRelayCommand(UpdateOutputTextGUIThread));
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

        /// <summary>
        /// opens up the DialogView and blocks the MainView 
        /// </summary>
        /// <param name="arg"></param>
        private void ShowDialog(object arg)
        {
            DialogView dialog = new DialogView();
            dialog.TextSubmitted += (sender, text) =>
            {
                // Get the text box from the main window
                TextBlock OutputText = (TextBlock)Application.Current.MainWindow.FindName("OutputText");
                // Update the text box in the main window with the text from the dialog
                OutputText.Text = text;
            };
            dialog.ShowDialog();
        }

        /// <summary>
        /// opena up the GUIThreadView on a different thread and lets the user input text to be updated in the main window
        /// </summary>
        /// <param name="arg"></param>
        private void ShowThread(object arg)
        {
            GUIThreadView threadView = new GUIThreadView();
            threadView.TextSubmitted += (sender, text) => UpdateOutputTextGUIThread(text);
            threadView.Show();
        }

        /// <summary>
        /// Updates the output text in the main window from a different thread
        /// </summary>
        /// <param name="text"></param>
        public void UpdateOutputTextGUIThread(string text)
        {
            OutputText = text;
            OnPropertyChanged(nameof(OutputText)); // Notify the UI of the change
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

    }
}

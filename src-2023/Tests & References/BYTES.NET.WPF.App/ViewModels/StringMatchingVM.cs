//import .net (default) namespace(s)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//import namespace(s) required from 'BYTES.NET.WPF' framework
using BYTES.NET.WPF.MVVM;

//import internal namespace(s) required
using BYTES.NET.WPF.App.Views;
using BYTES.NET.Primitives;
using System.Windows;

namespace BYTES.NET.WPF.App.ViewModels
{
    public class StringMatchingVM : ViewModel
    {
        #region private variable(s)

        private StringMatchingView _view = new StringMatchingView();

        private string _textOne = "Phone";
        private string _textTwo = "Phones";

        private string _trigramSimilarity = "N/A";
        private string _levenshteinSimilarity = "N/A";

        private List<string> _options = new List<string>() {"Phones","CellPhones","Postpone","Mobile Phone"};

        private string? _trigramBestMatch = string.Empty;
        private string? _levenshteinBestMatch = String.Empty;

        #endregion

        #region public properties

        public StringMatchingView View { get => _view; }

        public string TextOne { get => _textOne; set {
                _textOne = value;
                OnPropertyChanged();
                UpdateSimilarities();
                UpdateBestMatches();
                } 
        }

        public string TextTwo
        {
            get => _textTwo; set
            {
                _textTwo = value;
                OnPropertyChanged();
                UpdateSimilarities();
            }
        }

        public string TrigramSimilarity
        {
            get => _trigramSimilarity; set
            {
                _trigramSimilarity = value;
                OnPropertyChanged();
            }
        }

        public string LevenshteinSimilarity
        {
            get => _levenshteinSimilarity; set
            {
                _levenshteinSimilarity = value;
                OnPropertyChanged();
            }
        }

        public string? TrigramBestMatch
        {
            get => _trigramBestMatch; set
            {
                _trigramBestMatch = value;
                OnPropertyChanged();
            }
        }

        public string? LevenshteinBestMatch
        {
            get => _levenshteinBestMatch; set
            {
                _levenshteinBestMatch = value;
                OnPropertyChanged();
            }
        }

        public string[] Options { get => _options.ToArray(); }

        #endregion

        #region public new instance method(s)

        /// <summary>
        /// default new instance method
        /// </summary>
        public StringMatchingVM()
        {
            _view.DataContext = this;

            UpdateSimilarities();
            UpdateBestMatches();

            this.Commands.Add("AddOptionCmd", new ViewModelRelayCommand(AddOption));
        }

        #endregion

        #region private methods

        /// <summary>
        /// updates the similarities calculated
        /// </summary>
        private void UpdateSimilarities()
        {
            if(string.IsNullOrEmpty(this.TextOne) || string.IsNullOrEmpty(this.TextTwo))
            {
                this.TrigramSimilarity = "<must not be empty>";
                this.LevenshteinSimilarity = "<must not be empty>";

                return;
            }

            if(this.TextOne.Length < 3 || this.TextTwo.Length < 3)
            {
                this.TrigramSimilarity = "<must be at least 3 chars long>";
            }
            else
            {
                this.TrigramSimilarity = ((double?)_textOne.SimilarityTo(_textTwo, "Trigram")).ToString();
            }
            
            this.LevenshteinSimilarity = ((double?)_textOne.SimilarityTo(_textTwo, "Levenshtein")).ToString();
        }

        /// <summary>
        /// adds an option to the list
        /// </summary>
        /// <param name="arg"></param>
        private void AddOption(object arg)
        {
            _options.Add((string)arg);
            OnPropertyChanged("Options");
            UpdateBestMatches() ;
        }

        //updates the best matches found
        private void UpdateBestMatches()
        {
            if (string.IsNullOrEmpty(this.TextOne))
            {
                this.TrigramBestMatch = "<must not be empty>";

                return;
            }

            if (this.TextOne.Length < 3)
            {
                this.TrigramBestMatch = "<must be at least 3 chars long>";
            }
            else
            {
                this.TrigramBestMatch = TextOne.BestMatch(this.Options, "Trigram");
            }

            this.LevenshteinBestMatch = TextOne.BestMatch(this.Options, "Levenshtein");
        }

        #endregion

    }
}

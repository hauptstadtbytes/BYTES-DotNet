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

        private double? _trigramSimilarity = 0;
        private double? _levenshteinSimilarity = 0;

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

        public double? TrigramSimilarity
        {
            get => _trigramSimilarity; set
            {
                _trigramSimilarity = value;
                OnPropertyChanged();
            }
        }

        public double? LevenshteinSimilarity
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

            this.TrigramSimilarity = (double?)_textOne.SimilarityTo(_textTwo, "Trigram");
            this.LevenshteinSimilarity = (double?)_textOne.SimilarityTo(_textTwo, "Levenshtein");
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
            this.TrigramBestMatch = TextOne.BestMatch(this.Options, "Trigram");
            this.LevenshteinBestMatch = TextOne.BestMatch(this.Options, "Levenshtein");
        }

        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;

namespace RegexUtility
{
    class RegexTesterViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Properties

        string _inputText;
        public string InputText
        {
            get { return _inputText; }
            set
            {
                if (_inputText != value)
                {
                    _inputText = value;

                    RaisePropertyChanged("InputText");
                }
            }
        }

        string _regexText;
        public string RegexText
        {
            get { return _regexText; }
            set
            {
                if (_regexText != value)
                {
                    _regexText = value;

                    RaisePropertyChanged("RegexText");
                }
            }
        }

        ObservableCollection<Match> _matches;
        public ObservableCollection<Match> Matches
        {
            get
            {
                if (_matches == null)
                    _matches = new ObservableCollection<Match>();
                return _matches;
            }
        }

        Match _selectedMatch;
        public Match SelectedMatch
        {
            get { return _selectedMatch; }
            set
            {
                if (_selectedMatch != value)
                {
                    _selectedMatch = value;

                    RaisePropertyChanged("SelectedMatch");
                }
            }
        }

        #region Commands

        DelegateCommand _findMatchesCommand;
        public DelegateCommand FindMatchesCommand
        {
            get
            {
                if (_findMatchesCommand == null)
                    _findMatchesCommand = new DelegateCommand(
                        (obj) =>
                        {
                            Matches.Clear();
                            MatchCollection matches = Regex.Matches(InputText, RegexText);
                            foreach (Match match in matches)
                                Matches.Add(match);
                        },
                        (obj) =>
                        {
                            return !string.IsNullOrEmpty(InputText) && !string.IsNullOrEmpty(RegexText);
                        });

                return _findMatchesCommand;
            }
        }

        #endregion

        #endregion
    }
}

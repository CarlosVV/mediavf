using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.IO;

using ExternalServicesPOC.Web;
using MediaVF.UI.Core;
using Microsoft.Practices.Unity;

namespace ExternalServicesPOC
{

    public class GoogleImageSearchViewModel : ViewModelBase
    {
        const string SEARCH_URL = "http://www.metalsucks.net/";

        string _keywordText;
        public string KeywordText
        {
            get { return _keywordText; }
            set
            {
                if (_keywordText != value)
                {
                    _keywordText = value;

                    RaisePropertyChanged("KeywordText");
                }
            }
        }

        ObservableCollection<string> _imageURLs;
        public ObservableCollection<string> ImageURLs
        {
            get
            {
                if (_imageURLs == null)
                    _imageURLs = new ObservableCollection<string>();
                return _imageURLs;
            }
        }

        DelegateCommand _searchCommand;
        public DelegateCommand SearchCommand
        {
            get
            {
                if (_searchCommand == null)
                    _searchCommand = new DelegateCommand(obj => GetSearchResults());
                return _searchCommand;
            }
        }

        public GoogleImageSearchViewModel()
            : base((IUnityContainer)null)
        {
        }

        public void GetSearchResults()
        {
            SearchContext searchContext = new SearchContext();
            searchContext.GetSearchResults(KeywordText,
                (invokeOp) =>
                {
                    foreach (string imageURL in invokeOp.Value)
                        ImageURLs.Add(imageURL);
                },
                null);
        }
    }
}

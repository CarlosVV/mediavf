using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using MediaVF.UI.Core;

namespace MediaVF.Web.BandedTogether.UI.Bands.ViewModels
{
    public class BandViewerViewModel : ViewModelBase, IHeadered
    {
        #region BandItem

        public class BandItem : ViewModelBase
        {
            bool _add;
            public bool Add
            {
                get { return _add; }
                set
                {
                    if (_add != value)
                    {
                        _add = value;

                        RaisePropertyChanged("Add");
                    }
                }
            }

            string _bandName;
            public string BandName
            {
                get { return _bandName; }
                set
                {
                    if (_bandName != value)
                    {
                        _bandName = value;

                        RaisePropertyChanged("BandName");
                    }
                }
            }
        }

        #endregion

        #region Properties

        IDisplayService DisplayService { get; set; }

        public string Header { get { return "Bands"; } }

        ObservableCollection<BandItem> _bands;
        public ObservableCollection<BandItem> Bands
        {
            get
            {
                if (_bands == null)
                    _bands = new ObservableCollection<BandItem>();
                return _bands;
            }
        }

        DelegateCommand _browseCommand;
        public DelegateCommand BrowseCommand
        {
            get
            {
                if (_browseCommand == null)
                    _browseCommand = new DelegateCommand((obj) =>
                        DisplayService.ShowPopup(BandsModule.PopupManagerKey,
                            BandsModule.DirectorBrowserPopupKey,
                            BandsModule.DirectoryBrowserBandsFoundCallbackKey,
                            (values) =>
                            {
                                if (values != null && values.Length > 0)
                                    PopulateBands(values[0]);
                            }));
                return _browseCommand;
            }
        }

        #endregion

        #region Constructor

        public BandViewerViewModel(IDisplayService displayService)
        {
            DisplayService = displayService;
        }

        #endregion

        #region Methods

        public void PopulateBands(object bands)
        {
            if (bands != null)
            {
                string bandsText = bands.ToString();

                Bands.Clear();

                bandsText.Split('|').ToList().ForEach(band =>
                {
                    if (!string.IsNullOrEmpty(band))
                        Bands.Add(new BandItem() { Add = true, BandName = band });
                });
            }
        }

        #endregion
    }
}

using System;
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
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.Events;
using MediaVF.Entities.ArtistTrack;

namespace MediaVF.Web.BandedTogether.UI
{
    public class MainViewModel : ContainerViewModel
    {
        IEventAggregator EventAggregator { get; set; }

        bool _loggedIn;
        public bool LoggedIn
        {
            get { return _loggedIn; }
            set
            {
                if (_loggedIn != value)
                {
                    _loggedIn = value;

                    RaisePropertyChanged("LoggedIn");
                }
            }
        }

        public MainViewModel(IUnityContainer container, IEventAggregator eventAggregator)
            : base(container)
        {
            EventAggregator = eventAggregator;
        }

        public void Initialize()
        {
            EventAggregator.GetEvent<CompositePresentationEvent<UIEventArgs<bool>>>().Subscribe(
                OnLoggedIn,
                ThreadOption.UIThread,
                false,
                FilterEvents);
        }

        public bool FilterEvents(UIEventArgs<bool> args)
        {
            return args.EventID == "LoggedIn";
        }

        public void OnLoggedIn(UIEventArgs<bool> args)
        {
            LoggedIn = args.EventData;
        }
    }
}

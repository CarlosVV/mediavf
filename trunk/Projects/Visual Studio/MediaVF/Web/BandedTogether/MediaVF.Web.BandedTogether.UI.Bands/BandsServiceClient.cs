using System;
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

using Microsoft.Practices.Unity;

using MediaVF.Common.Communication;
using MediaVF.Entities.ArtistTrack;
using MediaVF.UI.Core;
using MediaVF.Web.BandedTogether.Server.Bands;
using System.Collections.Generic;

namespace MediaVF.Web.BandedTogether.UI.Bands
{
    public class BandsServiceClient
    {
        IUnityContainer Container { get; set; }

        InvokableServiceClient<IBandsService> Service { get; set; }

        public BandsServiceClient(IUnityContainer container)
        {
            Container = container;
            Service = container.Resolve<InvokableServiceClient<IBandsService>>();
        }

        public void GetBandsForUser(int userID, Action<List<Band>> callback)
        {
            Service.Invoke("GetBandsForUser",
                (response) => callback(response.GetValue<IEnumerable<Band>>().ToList()),
                userID);
        }

        public void AddBandsForUser(int userID, IEnumerable<Band> bands, Action callback)
        {
            Service.Invoke("AddBandsForUser",
                (response) => callback(),
                userID,
                bands);
        }
    }
}

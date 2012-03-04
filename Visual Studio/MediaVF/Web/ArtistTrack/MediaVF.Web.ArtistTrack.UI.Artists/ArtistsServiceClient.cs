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
using MediaVF.Web.ArtistTrack.Server.Artists;
using System.Collections.Generic;

namespace MediaVF.Web.ArtistTrack.UI.Artists
{
    public class ArtistsServiceClient
    {
        IUnityContainer Container { get; set; }

        InvokableServiceClient<IArtistsService> Service { get; set; }

        public ArtistsServiceClient(IUnityContainer container)
        {
            Container = container;
            Service = container.Resolve<InvokableServiceClient<IArtistsService>>();
        }

        public void GetArtistsForUser(int userID, Action<List<Artist>> callback)
        {
            Service.Invoke("GetArtistsForUser",
                (response) => callback(response.GetValue<IEnumerable<Artist>>().ToList()),
                userID);
        }

        public void AddArtistsForUser(int userID, IEnumerable<Artist> artists, Action callback)
        {
            Service.Invoke("AddArtistsForUser",
                (response) => callback(),
                userID,
                artists);
        }
    }
}

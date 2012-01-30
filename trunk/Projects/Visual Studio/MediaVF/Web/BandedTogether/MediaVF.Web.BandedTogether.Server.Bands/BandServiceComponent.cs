using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MediaVF.Entities.ArtistTrack;
using MediaVF.Services.Components;
using MediaVF.Services.Configuration;
using MediaVF.Services.Logging;
using MediaVF.Services.Data;

namespace MediaVF.Web.BandedTogether.Server.Bands
{
    public class BandsServiceComponent : ServiceComponent, IBandsService
    {
        public BandsServiceComponent(IServiceConfigManager configManager, IComboLog log)
            : base(configManager, log) { }

        protected override void RegisterTypes()
        {
        }

        public override void Run()
        {
        }

        /// <summary>
        /// Gets all bands for a given user
        /// </summary>
        /// <param name="userID">The user id of the user for which to get bands</param>
        /// <returns>The list of bands found for the user</returns>
        public IEnumerable<Band> GetBandsForUser(int userID)
        {
            return DataManager.GetDataContext<Band>().Get<Band>("GetBandsByUserID", new object[] { userID }.ToList());
        }
        
        /// <summary>
        /// Gets a band by its ID
        /// </summary>
        /// <param name="bandID">The ID of the band to get</param>
        /// <returns>The band found for the ID</returns>
        public Band GetBandByID(int bandID)
        {
            // add id to dictionary of parameters
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("ID", bandID);

            // get bands for given parameters-
            List<Band> bands = DataManager.GetDataContext<Band>().Get<Band>(parameters, true);
            if (bands != null)
                return bands.FirstOrDefault();
            else
                return null;
        }

        /// <summary>
        /// Adds a list of bands for a user
        /// </summary>
        /// <param name="userID">The ID of the user to add bands for</param>
        /// <param name="bands">The list of bands to be added</param>
        public void AddBandsForUser(int userID, IEnumerable<Band> bandNames)
        {
            // add records for any bands that do not already exist in the db
            List<Band> bands = PopulateBandObjects(bandNames);

            // get the datacontext for bands
            DataContext dataContext = DataManager.GetDataContext<Band>();

            // get the existing bands for this user
            IEnumerable<Band> existingUserBands = GetBandsForUser(userID);

            // create list of bands that do not already exist for this user
            List<UserBand> newUserBands =
                bands.Where(b => !existingUserBands.Any(eub => string.Compare(eub.Name, b.Name, true) == 0))
                     .Select(b => new UserBand() { UserID = userID, BandID = b.ID })
                     .ToList();

            // add the new bands and save
            dataContext.AddObjects(newUserBands);
            dataContext.Save();
        }

        /// <summary>
        /// Adds new bands to the database
        /// </summary>
        /// <param name="bands"></param>
        public List<Band> PopulateBandObjects(IEnumerable<Band> bands)
        {
            // get the context for bands
            DataContext dataContext = DataManager.GetDataContext<Band>();

            // get list of existing bands in the db
            List<Band> existingBands = dataContext.GetAll<Band>(false);

            List<Band> bandsToAdd = new List<Band>();
            List<Band> populatedBands = new List<Band>();

            // go through the list of bands and find any that do not already exist in the db
            foreach (Band band in bands)
            {
                if (!populatedBands.Any(b => string.Compare(b.Name, band.Name, true) == 0))
                {
                    if (!existingBands.Any(b => string.Compare(b.Name, band.Name, true) == 0))
                    {
                        populatedBands.Add(band);
                        bandsToAdd.Add(band);
                    }
                    else
                        populatedBands.Add(existingBands.First(b => string.Compare(b.Name, band.Name, true) == 0));
                }
            }

            // add any new bands found
            if (bandsToAdd.Count > 0)
            {
                dataContext.AddObjects(bandsToAdd);
                dataContext.Save();
            }

            return populatedBands;
        }
    }
}

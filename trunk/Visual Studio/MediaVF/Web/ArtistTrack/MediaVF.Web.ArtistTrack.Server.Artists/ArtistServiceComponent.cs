using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MediaVF.Entities.ArtistTrack;
using MediaVF.Services.Components;
using MediaVF.Services.Configuration;
using MediaVF.Services.Logging;
using MediaVF.Services.Data;
using MediaVF.Web.ArtistTrack.Server.Artists;

namespace MediaVF.Web.ArtistTrack.Server.Artists
{
    public class ArtistsServiceComponent : ServiceComponent, IArtistsService
    {
        public ArtistsServiceComponent(IServiceConfigManager configManager, IComboLog log)
            : base(configManager, log) { }

        protected override void RegisterTypes()
        {
        }

        public override void Run()
        {
        }

        /// <summary>
        /// Gets all artists for a given user
        /// </summary>
        /// <param name="userID">The user id of the user for which to get artists</param>
        /// <returns>The list of artists found for the user</returns>
        public IEnumerable<Artist> GetArtistsForUser(int userID)
        {
            return DataManager.GetDataContext<Artist>().Get<Artist>("GetArtistsByUserID", new object[] { userID }.ToList());
        }
        
        /// <summary>
        /// Gets a artist by its ID
        /// </summary>
        /// <param name="artistID">The ID of the artist to get</param>
        /// <returns>The artist found for the ID</returns>
        public Artist GetArtistByID(int artistID)
        {
            // add id to dictionary of parameters
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("ID", artistID);

            // get artists for given parameters-
            List<Artist> artists = DataManager.GetDataContext<Artist>().Get<Artist>(parameters, true);
            if (artists != null)
                return artists.FirstOrDefault();
            else
                return null;
        }

        /// <summary>
        /// Adds a list of artists for a user
        /// </summary>
        /// <param name="userID">The ID of the user to add artists for</param>
        /// <param name="artists">The list of artists to be added</param>
        public void AddArtistsForUser(int userID, IEnumerable<Artist> artistNames)
        {
            // add records for any artists that do not already exist in the db
            List<Artist> artists = PopulateArtistObjects(artistNames);

            // get the datacontext for artists
            DataContext dataContext = DataManager.GetDataContext<Artist>();

            // get the existing artists for this user
            IEnumerable<Artist> existingUserArtists = GetArtistsForUser(userID);

            // create list of artists that do not already exist for this user
            List<UserArtist> newUserArtists =
                artists.Where(b => !existingUserArtists.Any(eua => string.Compare(eua.Name, b.Name, true) == 0))
                     .Select(b => new UserArtist() { UserID = userID, ArtistID = b.ID })
                     .ToList();

            // add the new artists and save
            dataContext.AddObjects(newUserArtists);
            dataContext.Save();
        }

        /// <summary>
        /// Adds new artists to the database
        /// </summary>
        /// <param name="artists"></param>
        public List<Artist> PopulateArtistObjects(IEnumerable<Artist> artists)
        {
            // get the context for artists
            DataContext dataContext = DataManager.GetDataContext<Artist>();

            // get list of existing artists in the db
            List<Artist> existingArtists = dataContext.GetAll<Artist>(false);

            List<Artist> artistsToAdd = new List<Artist>();
            List<Artist> populatedArtists = new List<Artist>();

            // go through the list of artists and find any that do not already exist in the db
            foreach (Artist artist in artists)
            {
                if (!populatedArtists.Any(a => string.Compare(a.Name, artist.Name, true) == 0))
                {
                    if (!existingArtists.Any(a => string.Compare(a.Name, artist.Name, true) == 0))
                    {
                        populatedArtists.Add(artist);
                        artistsToAdd.Add(artist);
                    }
                    else
                        populatedArtists.Add(existingArtists.First(a => string.Compare(a.Name, artist.Name, true) == 0));
                }
            }

            // add any new artists found
            if (artistsToAdd.Count > 0)
            {
                dataContext.AddObjects(artistsToAdd);
                dataContext.Save();
            }

            return populatedArtists;
        }
    }
}

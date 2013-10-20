using System;

namespace AutoTrade.MarketData.Publication
{
    public class PublicationData<T>
    {
        /// <summary>
        /// Gets or sets the unique id of the publication data
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the publish date time
        /// </summary>
        public DateTime PublishDateTime { get; set; }

        /// <summary>
        /// Gets or sets the data
        /// </summary>
        public T Data { get; set; }
    }
}
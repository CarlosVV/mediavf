namespace AutoTrade.Core.UI
{
    public class UiEventArgs<T>
    {
        public string EventID { get; private set; }

        public T EventData { get; private set; }

        public UiEventArgs(string eventID, T eventData)
        {
            EventID = eventID;
            EventData = eventData;
        }
    }
}

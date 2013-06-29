namespace AutoTrade.Core.Email
{
    interface IEmail
    {
        string From { get;  }

        string Subject { get; }

        string Body { get; }
    }
}

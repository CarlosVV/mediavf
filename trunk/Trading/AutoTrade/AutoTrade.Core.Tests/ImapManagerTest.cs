using AutoTrade.Core.Email;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTrade.Core.Tests
{
    [TestClass]
    public class ImapManagerTest
    {
        [TestMethod]
        public void Search_GmailShouldReturnResults()
        {
            var imapManager = new ImapManager("imap.gmail.com", 993, true, "evanverneyfink", "Asmo36de");

            var results = imapManager.Search(new EmailSearchCriteria { Folder = "Me", From = "evanverneyfink" });

            results.Should().NotBeEmpty();
        }
    }
}

using AutoTrade.Core.Email;
using AutoTrade.Core.Email.Imap.ImapX;
using AutoTrade.Core.Email.Imap.S22Imap;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTrade.Core.Tests
{
    [TestClass]
    public class ImapManagerTest
    {
        [TestMethod]
        public void ImapXSearch_GmailShouldReturnResults()
        {
            var imapManager = new ImapXManager("imap.gmail.com", 993, true, "evanverneyfink", "Asmo36de");

            var results = imapManager.Search(new EmailSearchCriteria { Folder = "Me", From = "evanverneyfink" });

            results.Should().NotBeEmpty();
        }

        [TestMethod]
        public void S22Search_GmailShouldReturnResults()
        {
            var imapManager = new S22ImapManager("imap.gmail.com", 993, true, "evanverneyfink", "Asmo36de");

            var results = imapManager.Search(new EmailSearchCriteria { Folder = "Me", From = "evanverneyfink" });

            results.Should().NotBeEmpty();
        }
    }
}

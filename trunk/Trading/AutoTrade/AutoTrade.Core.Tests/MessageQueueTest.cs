using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using FluentAssertions;
using MSFT = System.Messaging;
using AutoTrade.Core.Msmq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTrade.Core.Tests
{
    public class TestPayload
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public TimeSpan Time { get; set; }

        public List<ChildItem> Items { get; set; }
    }

    public class ChildItem
    {
        public Guid Id { get; set; }

        public string Text { get; set; }
        
        public decimal Value { get; set; }
    }

    [TestClass]
    public class MessageQueueTest
    {
        public const string TestNamespace = "http://schemas.autotrade.com/test";

        private const string TestQueueFullName = ".\\Private$\\" + TestQueueName;

        private const string TestQueueName = "TestQueue";

        [TestInitialize]
        public void Initialize()
        {
            if (MSFT.MessageQueue.Exists(TestQueueFullName))
                MSFT.MessageQueue.Delete(TestQueueFullName);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (MSFT.MessageQueue.Exists(TestQueueFullName))
                MSFT.MessageQueue.Delete(TestQueueFullName);
        }

        [TestMethod]
        public void Constructor_ShouldCreateQueue()
        {
            var messageQueue = new MessageQueue<TestPayload>("TestQueue", null, true);

            MSFT.MessageQueue.Exists(TestQueueFullName).Should().BeTrue();
        }

        [TestMethod]
        public void Constructor_ShouldUseExistingQueue()
        {
            MSFT.MessageQueue.Create(TestQueueFullName);

            var messageQueue = new MessageQueue<TestPayload>(TestQueueName, null, true);

            messageQueue.Path.Should().Be(TestQueueFullName);
            MSFT.MessageQueue.Exists(TestQueueFullName).Should().BeTrue();
        }

        [TestMethod]
        public void Add_ShouldWriteToQueueWithoutChildren()
        {
            var messageQueue = new MessageQueue<TestPayload>(TestQueueName, null, true);

            var payload = new TestPayload
                {
                    Id = Guid.NewGuid(),
                    Text = "Some test text",
                    Time = DateTime.Now.TimeOfDay
                };

            messageQueue.Add(payload);

            var msgQueue = new MSFT.MessageQueue(messageQueue.Path) {Formatter = new MessageFormatter<TestPayload>()};

            var msg = msgQueue.Receive(TimeSpan.FromSeconds(30));
            msg.Body.Should().NotBeNull();
            msg.Body.Should().BeOfType<TestPayload>();

            var actualPayload = (TestPayload) msg.Body;
            actualPayload.Id.Should().Be(payload.Id);
            actualPayload.Text.Should().Be(payload.Text);
            actualPayload.Time.Should().Be(payload.Time);
        }

        [TestMethod]
        public void Add_ShouldWriteToQueueWithChildren()
        {
            var messageQueue = new MessageQueue<TestPayload>(TestQueueName, null, true);

            var payload = new TestPayload
            {
                Id = Guid.NewGuid(),
                Text = "Some test text",
                Time = DateTime.Now.TimeOfDay,
                Items = new List<ChildItem>
                    {
                        new ChildItem {Id = Guid.NewGuid(), Text = "Some test text again", Value = 12.34m},
                        new ChildItem {Id = Guid.NewGuid(), Text = "Some test text again and again", Value = 56.78m},
                        new ChildItem {Id = Guid.NewGuid(), Text = "Some test text again and again and again", Value = 90.00m},
                    }
            };

            messageQueue.Add(payload);

            var msgQueue = new MSFT.MessageQueue(messageQueue.Path) { Formatter = new MessageFormatter<TestPayload>() };

            var msg = msgQueue.Receive(TimeSpan.FromSeconds(30));
            msg.Body.Should().NotBeNull();
            msg.Body.Should().BeOfType<TestPayload>();

            var actualPayload = (TestPayload)msg.Body;
            actualPayload.Id.Should().Be(payload.Id);
            actualPayload.Text.Should().Be(payload.Text);
            actualPayload.Time.Should().Be(payload.Time);
            
            actualPayload.Items.Should().NotBeNull();
            actualPayload.Items.Should().HaveCount(3);

            actualPayload.Items[0].ShouldBeEquivalentTo(payload.Items[0]);
            actualPayload.Items[1].ShouldBeEquivalentTo(payload.Items[1]);
            actualPayload.Items[2].ShouldBeEquivalentTo(payload.Items[2]);
        }
    }
}

using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System.Collections.Generic;

namespace Jinyinmao.AuthManager.Libraries
{
    public class TopicClientFactory
    {
        private static readonly Dictionary<string, TopicClient> topicClients = new Dictionary<string, TopicClient>();

        public static TopicClient InitTopicClient(string serviceBusConnectionString, string topicName)
        {
            MessagingFactory factory = MessagingFactory.CreateFromConnectionString(serviceBusConnectionString);
            factory.RetryPolicy = RetryPolicy.Default;

            TopicClient topicClient;
            if (!topicClients.TryGetValue(topicName, out topicClient))
            {
                topicClient = factory.CreateTopicClient(topicName);
                topicClients.Add(topicName, topicClient);
            }

            return topicClient;
        }
    }
}
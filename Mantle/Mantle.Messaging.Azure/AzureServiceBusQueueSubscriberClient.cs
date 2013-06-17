﻿using System;
using Mantle.Azure;
using Microsoft.ServiceBus.Messaging;

namespace Mantle.Messaging.Azure
{
    public class AzureServiceBusQueueSubscriberClient : AzureServiceBusQueueClient, ISubscriberClient
    {
        public AzureServiceBusQueueSubscriberClient(AzureServiceBusQueueSubscriberEndpoint endpoint,
                                                    IAzureServiceBusConfiguration sbConfiguration)
            : base(endpoint, sbConfiguration)
        {
        }

        public Message<T> Receive<T>()
        {
            return Receive<T>(TimeSpan.Zero);
        }

        public Message<T> Receive<T>(TimeSpan timeout)
        {
            try
            {
                BrokeredMessage brokeredMessage = QueueClient.Receive(timeout);

                if (brokeredMessage == null)
                    return null;

                return new AzureServiceBusMessage<T>(brokeredMessage.GetBody<T>(), brokeredMessage);
            }
            catch (Exception ex)
            {
                throw new MessagingException(
                    "An error occurred while attempting to read a message from the specified queue. See inner exception for more details.",
                    ex);
            }
        }
    }
}
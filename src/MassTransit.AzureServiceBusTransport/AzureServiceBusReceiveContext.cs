﻿// Copyright 2007-2014 Chris Patterson, Dru Sellers, Travis Smith, et. al.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace MassTransit.AzureServiceBusTransport
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Net.Mime;
    using System.Text;
    using System.Threading;
    using Microsoft.ServiceBus.Messaging;


    public class AzureServiceBusReceiveContext :
        ReceiveContext,
        AzureServiceBusMessageContext
    {
        static readonly ContentType DefaultContentType = new ContentType("application/vnd.masstransit+json");

        readonly byte[] _body;
        readonly Uri _inputAddress;
        readonly BrokeredMessage _message;
        readonly Stopwatch _receiveTimer;
        ContentType _contentType;
        Encoding _encoding;
        AzureServiceBusReceiveContextHeaders _headers;
        readonly CancellationTokenSource _cancellationTokenSource;

        public AzureServiceBusReceiveContext(BrokeredMessage message)
        {
            _receiveTimer = Stopwatch.StartNew();

            _message = message;

            _cancellationTokenSource = new CancellationTokenSource();
        }

        public IDictionary<string, object> Properties
        {
            get { return _message.Properties; }
        }


        public int DeliveryCount
        {
            get { return _message.DeliveryCount; }
        }

        public string Label
        {
            get { return _message.Label; }
            set { _message.Label = value; }
        }

        public long SequenceNumber
        {
            get { return _message.SequenceNumber; }
        }

        public long EnqueuedSequenceNumber
        {
            get { return _message.EnqueuedSequenceNumber; }
        }

        public Guid LockToken
        {
            get { return _message.LockToken; }
        }

        public DateTime LockedUntil
        {
            get { return _message.LockedUntilUtc; }
        }

        public string SessionId
        {
            get { return _message.SessionId; }
        }

        public long Size
        {
            get { return _message.Size; }
        }

        public MessageState State
        {
            get { return _message.State; }
        }

        public bool ForcePersistence
        {
            get { return _message.ForcePersistence; }
        }

        public string To
        {
            get { return _message.To; }
            set { _message.To = value; }
        }

        public string ReplyToSessionId
        {
            get { return _message.ReplyToSessionId; }
        }

        public string PartitionKey
        {
            get { return _message.PartitionKey; }
        }

        public string ViaPartitionKey
        {
            get { return _message.ViaPartitionKey; }
        }

        public string ReplyTo
        {
            get { return _message.ReplyTo; }
        }

        public DateTime EnqueuedTime
        {
            get { return _message.EnqueuedTimeUtc; }
        }

        public DateTime ScheduledEnqueueTime
        {
            get { return _message.ScheduledEnqueueTimeUtc; }
        }

        public Encoding ContentEncoding
        {
            get
            {
                return _encoding ?? (_encoding = string.IsNullOrWhiteSpace(ContentType.CharSet)
                    ? Encoding.UTF8
                    : Encoding.GetEncoding(ContentType.CharSet));
            }
        }

        public bool Redelivered
        {
            get { return _message.DeliveryCount > 1; }
        }

        public CancellationToken CancellationToken
        {
            get { return _cancellationTokenSource.Token; }
        }

        public Stream Body
        {
            get { return new MemoryStream(_body, 0, _body.Length, false); }
        }

        public TimeSpan ElapsedTime
        {
            get { return _receiveTimer.Elapsed; }
        }

        public Uri InputAddress
        {
            get { return _inputAddress; }
        }

        public ContentType ContentType
        {
            get { return _contentType ?? (_contentType = GetContentType()); }
        }

        public Headers Headers
        {
            get { return _headers ?? (_headers = new AzureServiceBusReceiveContextHeaders(this)); }
        }

        public void NotifyConsumed(TimeSpan elapsed, string messageType, string consumerType)
        {
            
        }

        public void NotifyFaulted(string messageType, string consumerType, Exception exception)
        {
            
        }

        ContentType GetContentType()
        {
            object contentTypeHeader;
            if (Headers.TryGetHeader("Content-Type", out contentTypeHeader))
            {
                var contentType = contentTypeHeader as ContentType;
                if (contentType != null)
                    return contentType;
                var s = contentTypeHeader as string;
                if (s != null)
                    return new ContentType(s);
            }

            return DefaultContentType;
        }
    }
}
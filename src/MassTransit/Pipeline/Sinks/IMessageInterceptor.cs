// Copyright 2007-2014 Chris Patterson, Dru Sellers, Travis Smith, et. al.
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
namespace MassTransit.Pipeline.Sinks
{
    using System;
    using System.Threading.Tasks;


    /// <summary>
    /// Intercepts a specific message type
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IMessageInterceptor<in TMessage>
        where TMessage : class
    {
        /// <summary>
        /// Called before a message is dispatched to any consumers
        /// </summary>
        /// <param name="context">The consume context</param>
        /// <returns></returns>
        Task PreDispatch(ConsumeContext<TMessage> context);

        /// <summary>
        /// Called after the message has been dispatched to all consumers
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task PostDispatch(ConsumeContext<TMessage> context);

        /// <summary>
        /// Called after the message has been dispatched to all consumers when one or more exceptions have occurred
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        Task DispatchFaulted(ConsumeContext<TMessage> context, Exception exception);
    }
}
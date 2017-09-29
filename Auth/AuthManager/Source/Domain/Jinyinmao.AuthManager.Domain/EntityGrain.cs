using Jinyinmao.AuthManager.Domain.Core.Events;
using Jinyinmao.AuthManager.Libraries;
using Microsoft.ServiceBus.Messaging;
using Moe.Lib;
using Moe.Lib.Jinyinmao;
using MoeLib.Jinyinmao.Orleans;
using Orleans;
using System;
using System.Threading.Tasks;

namespace Jinyinmao.AuthManager.Domain
{
    public abstract class EntityGrain : JinyinmaoGrain
    {
        private static readonly Lazy<string> serviceBusConnectionString = new Lazy<string>(() => App.Configurations.GetConfig<AuthSiloConfig>().ServiceBusConnectionString);

        /// <summary>
        ///     Gets or sets the last modified.
        /// </summary>
        /// <value>The last modified.</value>
        protected DateTime LastModified { get; set; }

        private static string ServiceBusConnectionString
        {
            get { return serviceBusConnectionString.Value; }
        }

        /// <summary>
        ///     This method is called at the begining of the process of deactivating a grain.
        /// </summary>
        public override async Task OnDeactivateAsync()
        {
            await this.SaveChangeAsync();
            await base.OnDeactivateAsync();
        }

        protected virtual async Task RaiseEventAsync<TEvent>(TEvent @event) where TEvent : IEvent
        {
            TopicClient topicClient = await Task.Run(() => TopicClientFactory.InitTopicClient(ServiceBusConnectionString, @event.EventName));
            await topicClient.SendAsync(new BrokeredMessage(@event.ToJson()) { SessionId = Guid.NewGuid().ToGuidString() });
        }

        protected virtual Task SaveChangeAsync()
        {
            return TaskDone.Done;
        }
    }
}
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Subscription;

namespace Uniceps.MessengerSystem
{
    public class Messenger
    {
        public static Messenger Default { get; } = new Messenger();

        private Messenger() { }

        private readonly ConcurrentDictionary<Type, List<MessengerSubscription>> _subscribers = new();
        public void Register<TMessage>(object recipient, Action<TMessage> action)
        {
            if (recipient == null) throw new ArgumentNullException(nameof(recipient));
            if (action == null) throw new ArgumentNullException(nameof(action));

            var messageType = typeof(TMessage);
            var subscription = new MessengerSubscription(
                new WeakReference(recipient),
                action.Target != null ? new WeakReference(action.Target) : null,
                action.Method
            );
            _subscribers.AddOrUpdate(messageType,
                new List<MessengerSubscription> { subscription },
                (key, list) =>
                {
                    lock (list)
                    {
                        list.Add(subscription);
                    }
                    return list;
                });
        }

        public void Send<TMessage>(TMessage message)
        {
            if (message == null) return;

            var messageType = typeof(TMessage);

            if (!_subscribers.TryGetValue(messageType, out var list)) return;

            List<MessengerSubscription> deadSubscriptions = new();

            lock (list)
            {
                foreach (var sub in list)
                {
                    if (sub.RecipientRef.IsAlive)
                    {
                        if (sub.TargetRef == null || sub.TargetRef.IsAlive)
                        {
                            var target = sub.TargetRef?.Target ?? sub.RecipientRef.Target;
                            sub.MethodInfo.Invoke(target, new object[] { message });
                        }
                    }
                    else
                    {
                        deadSubscriptions.Add(sub);
                    }
                }
                foreach (var deadSub in deadSubscriptions)
                {
                    list.Remove(deadSub);
                }
            }
        }

        private class MessengerSubscription
        {
            public WeakReference RecipientRef { get; }
            public WeakReference? TargetRef { get; }
            public MethodInfo MethodInfo { get; }

            public MessengerSubscription(WeakReference recipientRef, WeakReference? targetRef, MethodInfo methodInfo)
            {
                RecipientRef = recipientRef;
                TargetRef = targetRef;
                MethodInfo = methodInfo;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Microsoft.Practices.Prism.Events;

using MediaVF.Services.Core.Properties;

namespace MediaVF.Services.Core.Events
{
    public class ServiceEvent<T> : CompositePresentationEvent<T>
    {
        public override SubscriptionToken Subscribe(Action<T> action,
            ThreadOption threadOption,
            bool keepSubscriberReferenceAlive,
            Predicate<T> filter)
        {
            if (threadOption == ThreadOption.UIThread)
                throw new InvalidOperationException(Resources.UIEventOnService);

            return base.Subscribe(action, threadOption, keepSubscriberReferenceAlive, filter);
        }
    }
}

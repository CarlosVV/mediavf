﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18034
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AutoTrade.MarketData.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("AutoTrade.MarketData.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The data provider on the subscription is null..
        /// </summary>
        internal static string DataProviderNullException {
            get {
                return ResourceManager.GetString("DataProviderNullException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The data provider type {0} was not found registered in the dependency container..
        /// </summary>
        internal static string DataProviderTypeNotRegisteredExceptionFormat {
            get {
                return ResourceManager.GetString("DataProviderTypeNotRegisteredExceptionFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to create market data subscription. {0}.
        /// </summary>
        internal static string FailedToCreateSubscriptionExceptionFormat {
            get {
                return ResourceManager.GetString("FailedToCreateSubscriptionExceptionFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No subscriptions are currently active..
        /// </summary>
        internal static string NoSubscriptionsFoundWarning {
            get {
                return ResourceManager.GetString("NoSubscriptionsFoundWarning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Subscription not found: {0}..
        /// </summary>
        internal static string SubscriptionNotFoundExceptionFormat {
            get {
                return ResourceManager.GetString("SubscriptionNotFoundExceptionFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No subscription with id {0} is loaded in the subscription manager..
        /// </summary>
        internal static string SubscriptionNotLoadedWarningFormat {
            get {
                return ResourceManager.GetString("SubscriptionNotLoadedWarningFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An exception occurred trying to retrieve or update subscription data..
        /// </summary>
        internal static string SubscriptionUpdateException {
            get {
                return ResourceManager.GetString("SubscriptionUpdateException", resourceCulture);
            }
        }
    }
}

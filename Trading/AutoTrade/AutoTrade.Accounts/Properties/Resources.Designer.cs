﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18052
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AutoTrade.Accounts.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("AutoTrade.Accounts.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to The AccountType data has not been loaded for the account {0}..
        /// </summary>
        internal static string AccountTypeNotLoadedMessage {
            get {
                return ResourceManager.GetString("AccountTypeNotLoadedMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An existing session is currently open for account {0}. Please try again later..
        /// </summary>
        internal static string ExistingSessionMessage {
            get {
                return ResourceManager.GetString("ExistingSessionMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The amount {0} is not valid for a deposit. A deposit value must be greater than 0..
        /// </summary>
        internal static string InvalidDepositAmountMessage {
            get {
                return ResourceManager.GetString("InvalidDepositAmountMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Type {0} is not a valid account sync manager. Ensure that the type name is valid, the assembly for the type is loaded, and that the type implements the ITransactionProcessor interface..
        /// </summary>
        internal static string InvalidSyncManagerTypeMessage {
            get {
                return ResourceManager.GetString("InvalidSyncManagerTypeMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Session not found for id {0}..
        /// </summary>
        internal static string SessionNotFoundMessage {
            get {
                return ResourceManager.GetString("SessionNotFoundMessage", resourceCulture);
            }
        }
    }
}

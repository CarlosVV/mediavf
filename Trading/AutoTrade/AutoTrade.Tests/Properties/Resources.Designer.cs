﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18034
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AutoTrade.Tests.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("AutoTrade.Tests.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to Multiple methods matching the given signature were found. Type = {0}, Method = {1}.
        /// </summary>
        internal static string AmbiguousMethodExceptionFormat {
            get {
                return ResourceManager.GetString("AmbiguousMethodExceptionFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Field {0} on type {1} cannot be cast as {2}..
        /// </summary>
        internal static string FieldCannotBeCastFormat {
            get {
                return ResourceManager.GetString("FieldCannotBeCastFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Field {0} on type {1} cannot be set from an object of type {2}..
        /// </summary>
        internal static string FieldCannotBeSetFormat {
            get {
                return ResourceManager.GetString("FieldCannotBeSetFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Field {0} on type {1} is a value type and cannot be set to null..
        /// </summary>
        internal static string FieldCannotBeSetToNullFormat {
            get {
                return ResourceManager.GetString("FieldCannotBeSetToNullFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Property {0} on type {1} cannot be cast as {2}..
        /// </summary>
        internal static string PropertyCannotBeCastFormat {
            get {
                return ResourceManager.GetString("PropertyCannotBeCastFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Property {0} on type {1} does not support reading..
        /// </summary>
        internal static string PropertyCannotBeReadFormat {
            get {
                return ResourceManager.GetString("PropertyCannotBeReadFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Property {0} on type {1} cannot be set from an object of type {2}..
        /// </summary>
        internal static string PropertyCannotBeSetFormat {
            get {
                return ResourceManager.GetString("PropertyCannotBeSetFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Property {0} on type {1} is a value type and cannot be set to null..
        /// </summary>
        internal static string PropertyCannotBeSetToNullFormat {
            get {
                return ResourceManager.GetString("PropertyCannotBeSetToNullFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Property {0} on type {1} does not support writing..
        /// </summary>
        internal static string PropertyCannotBeWrittenFormat {
            get {
                return ResourceManager.GetString("PropertyCannotBeWrittenFormat", resourceCulture);
            }
        }
    }
}
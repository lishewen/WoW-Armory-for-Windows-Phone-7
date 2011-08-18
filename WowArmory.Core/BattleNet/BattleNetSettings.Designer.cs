﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.237
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WowArmory.Core.BattleNet {
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
    internal class BattleNetSettings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal BattleNetSettings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WowArmory.Core.BattleNet.BattleNetSettings", typeof(BattleNetSettings).Assembly);
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
        ///   Looks up a localized string similar to character/{0}/{1}{2}.
        /// </summary>
        internal static string BattleNet_Api_Character {
            get {
                return ResourceManager.GetString("BattleNet_Api_Character", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to guild/{0}/{1}{2}.
        /// </summary>
        internal static string BattleNet_Api_Guild {
            get {
                return ResourceManager.GetString("BattleNet_Api_Guild", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to item/{0}.
        /// </summary>
        internal static string BattleNet_Api_Item {
            get {
                return ResourceManager.GetString("BattleNet_Api_Item", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to realm/status.
        /// </summary>
        internal static string BattleNet_Api_RealmStatus {
            get {
                return ResourceManager.GetString("BattleNet_Api_RealmStatus", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to http://{0}.battle.net/api/wow/.
        /// </summary>
        internal static string BattleNet_BaseUri {
            get {
                return ResourceManager.GetString("BattleNet_BaseUri", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 56.
        /// </summary>
        internal static string BattleNet_IconSize_Large {
            get {
                return ResourceManager.GetString("BattleNet_IconSize_Large", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 36.
        /// </summary>
        internal static string BattleNet_IconSize_Medium {
            get {
                return ResourceManager.GetString("BattleNet_IconSize_Medium", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 18.
        /// </summary>
        internal static string BattleNet_IconSize_Small {
            get {
                return ResourceManager.GetString("BattleNet_IconSize_Small", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to http://{0}.media.blizzard.com/wow/icons/.
        /// </summary>
        internal static string BattleNet_IconUri {
            get {
                return ResourceManager.GetString("BattleNet_IconUri", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to http://{0}.media.blizzard.com/wow/renders/items/.
        /// </summary>
        internal static string BattleNet_ItemRenderUri {
            get {
                return ResourceManager.GetString("BattleNet_ItemRenderUri", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to eu.
        /// </summary>
        internal static string BattleNet_Region_Europe {
            get {
                return ResourceManager.GetString("BattleNet_Region_Europe", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to us.
        /// </summary>
        internal static string BattleNet_Region_USA {
            get {
                return ResourceManager.GetString("BattleNet_Region_USA", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to http://{0}.battle.net/static-render/{0}/.
        /// </summary>
        internal static string BattleNet_ThumbnailUri {
            get {
                return ResourceManager.GetString("BattleNet_ThumbnailUri", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to wp7wowarmory.
        /// </summary>
        internal static string WebRequest_Header_UserAgent {
            get {
                return ResourceManager.GetString("WebRequest_Header_UserAgent", resourceCulture);
            }
        }
    }
}

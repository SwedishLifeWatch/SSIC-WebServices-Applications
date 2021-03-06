﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ArtDatabanken.WebService.UserService {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    public sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("^[a-zåäöA-ZÅÄÖ0-9_]{3,40}$")]
        public string UserNameRegularExpression {
            get {
                return ((string)(this["UserNameRegularExpression"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("en-GB")]
        public string DefaultLocale {
            get {
                return ((string)(this["DefaultLocale"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("WebLocale")]
        public string LocaleCacheKey {
            get {
                return ((string)(this["LocaleCacheKey"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("WebCountry")]
        public string CountryCacheKey {
            get {
                return ((string)(this["CountryCacheKey"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("WebAuthorityAttributeType")]
        public string AuthorityAttributeTypeCacheKey {
            get {
                return ((string)(this["AuthorityAttributeTypeCacheKey"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1")]
        public int RoleIdForSuperAdministrator {
            get {
                return ((int)(this["RoleIdForSuperAdministrator"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("User is not authorized to perform this action. It is only super administrators of" +
            " the user administration system that are allowed to use this functionality.")]
        public string ErrorMessageIsNotSuperAdministrator {
            get {
                return ((string)(this["ErrorMessageIsNotSuperAdministrator"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("UserGroupAdministration")]
        public string AuthorityIdentifierForUserGroupAdministration {
            get {
                return ((string)(this["AuthorityIdentifierForUserGroupAdministration"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("User is not authorized to perform this action. Beside super administrators it is " +
            "only user group administrators that are allowed to use this functionality.")]
        public string ErrorMessageIsNotUserGroupAdministrator {
            get {
                return ((string)(this["ErrorMessageIsNotUserGroupAdministrator"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("^.*(?=.{8,40})(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).*$")]
        public string PasswordRegularExpression {
            get {
                return ((string)(this["PasswordRegularExpression"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5")]
        public int MaxLoginAttempt {
            get {
                return ((int)(this["MaxLoginAttempt"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("20")]
        public int MinFailedLoginWaitTime {
            get {
                return ((int)(this["MinFailedLoginWaitTime"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("ApplicationsInSoaKey")]
        public string ApplicationsInSoaCacheKey {
            get {
                return ((string)(this["ApplicationsInSoaCacheKey"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("WebApplicationAction")]
        public string ApplicationActionCacheKey {
            get {
                return ((string)(this["ApplicationActionCacheKey"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("WebApplication")]
        public string ApplicationCacheKey {
            get {
                return ((string)(this["ApplicationCacheKey"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("WebApplicationVersion")]
        public string ApplicationVersionCacheKey {
            get {
                return ((string)(this["ApplicationVersionCacheKey"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("WebAuthorityDataType")]
        public string AuthorityDataTypeCacheKey {
            get {
                return ((string)(this["AuthorityDataTypeCacheKey"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("ApplicationAuthorityDataType")]
        public string ApplicationAuthorityDataTypeCacheKey {
            get {
                return ((string)(this["ApplicationAuthorityDataTypeCacheKey"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("WebAuthorityAttribute")]
        public string AuthorityAttributeCacheKey {
            get {
                return ((string)(this["AuthorityAttributeCacheKey"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("WebAuthority")]
        public string AuthorityCacheKey {
            get {
                return ((string)(this["AuthorityCacheKey"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("WebRole")]
        public string RoleCacheKey {
            get {
                return ((string)(this["RoleCacheKey"]));
            }
        }
    }
}

﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Estimating.ProgressReporter.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.4.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=EAI-LAP-NBGAME\\SQLEXPRESS;Initial Catalog=Estimating_Dev;Integrated S" +
            "ecurity=True")]
        public string Estimating {
            get {
                return ((string)(this["Estimating"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=SPECTRUM;Integrated Security=True;Connect Timeout=30;Encrypt=False;Tr" +
            "ustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False" +
            "")]
        public string SPECTRUM {
            get {
                return ((string)(this["SPECTRUM"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=ADEAI-ALT\\TRIMBLE;Initial Catalog=Estimate_Dev;Integrated Security=Tr" +
            "ue;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationInte" +
            "nt=ReadWrite;MultiSubnetFailover=False")]
        public string Estimating_Dev {
            get {
                return ((string)(this["Estimating_Dev"]));
            }
        }
    }
}
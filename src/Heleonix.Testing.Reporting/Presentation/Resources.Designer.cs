﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Heleonix.Testing.Reporting.Presentation {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Heleonix.Testing.Reporting.Presentation.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to The key=value pairs to specify custom content replacements, i.e. header or footer, in the output reports.
        /// </summary>
        internal static string CLI_Content_Description {
            get {
                return ResourceManager.GetString("CLI_Content_Description", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The .NET CLI tool to generate human-friendly test reports from technical test results files.
        /// </summary>
        internal static string CLI_Description {
            get {
                return ResourceManager.GetString("CLI_Description", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The file &quot;{0}&quot; is not found..
        /// </summary>
        internal static string CLI_FileNotFound {
            get {
                return ResourceManager.GetString("CLI_FileNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Format of the output report.
        /// </summary>
        internal static string CLI_Format_Description {
            get {
                return ResourceManager.GetString("CLI_Format_Description", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Input files to generate reports from.
        /// </summary>
        internal static string CLI_Input_Description {
            get {
                return ResourceManager.GetString("CLI_Input_Description", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Determines whether the input files should be merged into a single output report, or every input file should have a separate generated output report.
        /// </summary>
        internal static string CLI_Megre_Description {
            get {
                return ResourceManager.GetString("CLI_Megre_Description", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Path of the output file to store the generated merged report in (see the --merge option) or path of the folder to store multiple generated reports.
        /// </summary>
        internal static string CLI_Output_Description {
            get {
                return ResourceManager.GetString("CLI_Output_Description", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The key=value pairs to specify custom styling, i.e. colors, for the output reports.
        /// </summary>
        internal static string CLI_Style_Description {
            get {
                return ResourceManager.GetString("CLI_Style_Description", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sets the verbosity level for logging.
        /// </summary>
        internal static string CLI_Verbosity_Description {
            get {
                return ResourceManager.GetString("CLI_Verbosity_Description", resourceCulture);
            }
        }
    }
}
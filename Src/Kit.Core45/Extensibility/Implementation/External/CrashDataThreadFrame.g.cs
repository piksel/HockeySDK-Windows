

//------------------------------------------------------------------------------
// This code was generated by a tool.
//
//   Tool : Bond Compiler 3.02
//   File : CrashData_types.cs
//
// Changes to this file may cause incorrect behavior and will be lost when
// the code is regenerated.
// <auto-generated />
//------------------------------------------------------------------------------


#region ReSharper warnings
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantNameQualifier
// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace
// ReSharper disable UnusedParameter.Local
// ReSharper disable RedundantUsingDirective
#endregion

namespace Piksel.HockeyApp.Extensibility.Implementation.External
{
    using System.Collections.Generic;

    
    [System.CodeDom.Compiler.GeneratedCode("gbc", "3.02")]
    internal partial class CrashDataThreadFrame
    {

        public string address { get; set; }


        public string symbol { get; set; }

        
        public IDictionary<string, string> registers { get; set; }

        public CrashDataThreadFrame()
            : this("AI.CrashDataThreadFrame", "CrashDataThreadFrame")
        {}

        protected CrashDataThreadFrame(string fullName, string name)
        {
            address = string.Empty;
            symbol = string.Empty;
            registers = new Dictionary<string, string>();
        }
    }
} // AI











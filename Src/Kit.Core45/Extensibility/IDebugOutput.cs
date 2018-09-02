﻿// <copyright file="IDebugOutput.cs" company="Microsoft">
// Copyright © Microsoft. All Rights Reserved.
// </copyright>

namespace Piksel.HockeyApp.Extensibility
{
    /// <summary>
    /// Encapsulates method call that has to be compiled with DEBUG compiler constant.
    /// </summary>
    internal interface IDebugOutput
    {
        /// <summary>
        /// Write the message to the VisualStudio output window.
        /// </summary>
        void WriteLine(string message);

        /// <summary>
        /// Checks to see if logging is enabled by an attached debugger. 
        /// </summary>
        /// <returns>true if a debugger is attached and logging is enabled; otherwise, false.</returns>
        bool IsLogging();
    }
}

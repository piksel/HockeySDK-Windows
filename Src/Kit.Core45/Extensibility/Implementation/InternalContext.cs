﻿namespace Piksel.HockeyApp.Extensibility.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using DataContracts;
    using Extensibility.Implementation.External;

    /// <summary>
    /// Encapsulates Internal information.
    /// </summary>
    internal sealed class InternalContext : IJsonSerializable
    {
        private readonly IDictionary<string, string> tags;

        internal InternalContext(IDictionary<string, string> tags)
        {
            this.tags = tags;
        }

        /// <summary>
        /// Gets or sets application insights SDK version.
        /// </summary>
        public string SdkVersion
        {
            get { return this.tags.GetTagValueOrNull(ContextTagKeys.Keys.InternalSdkVersion); }
            set { this.tags.SetStringValueOrRemove(ContextTagKeys.Keys.InternalSdkVersion, value); }
        }

        /// <summary>
        /// Gets or sets application insights agent version.
        /// </summary>
        public string AgentVersion
        {
            get { return this.tags.GetTagValueOrNull(ContextTagKeys.Keys.InternalAgentVersion); }
            set { this.tags.SetStringValueOrRemove(ContextTagKeys.Keys.InternalAgentVersion, value); }
        }

        void IJsonSerializable.Serialize(IJsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteProperty("sdkVersion", this.SdkVersion);
            writer.WriteProperty("agentVersion", this.AgentVersion);
            writer.WriteEndObject();
        }
    }
}

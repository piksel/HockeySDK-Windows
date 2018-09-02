﻿namespace Piksel.HockeyApp.DataContracts
{
    using System;
    using System.Collections.Generic;
    using Channel;
    using Extensibility.Implementation;
    using Extensibility.Implementation.External;

    /// <summary>
    /// Telemetry type used to track events.
    /// </summary>
    public sealed class EventTelemetry : ITelemetry, ISupportProperties
    {
        internal const string TelemetryName = "Event";
         
        internal readonly string BaseType = typeof(EventData).Name;
        internal readonly EventData Data;
        private readonly TelemetryContext context;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="EventTelemetry"/> class.
        /// </summary>
        public EventTelemetry()
        {
            this.Data = new EventData();
            this.context = new TelemetryContext(this.Data.properties, new Dictionary<string, string>());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventTelemetry"/> class with the given <paramref name="name"/>.
        /// </summary>
        /// <exception cref="ArgumentException">The event <paramref name="name"/> is null or empty string.</exception>
        public EventTelemetry(string name) : this()
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets or sets date and time when event was recorded.
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the value that defines absolute order of the telemetry item.
        /// </summary>
        public string Sequence { get; set; }
        
        /// <summary>
        /// Gets the context associated with the current telemetry item.
        /// </summary>
        public TelemetryContext Context
        {
            get { return this.context; }
        }
        
        /// <summary>
        /// Gets or sets the name of the event.
        /// </summary>
        public string Name
        {
            get { return this.Data.name; }
            set { this.Data.name = value; }
        }

        /// <summary>
        /// Gets a dictionary of application-defined event metrics.
        /// </summary>
        public IDictionary<string, double> Metrics
        {
            get { return this.Data.measurements; }
        }

        /// <summary>
        /// Gets a dictionary of application-defined property names and values providing additional information about this event.
        /// </summary>
        public IDictionary<string, string> Properties
        {
            get { return this.Data.properties; }
        }

        /// <summary>
        /// Sanitizes the properties based on constraints.
        /// </summary>
        void ITelemetry.Sanitize()
        {
            this.Name = this.Name.SanitizeName();
            this.Name = Utils.PopulateRequiredStringValue(this.Name, "name", typeof(EventTelemetry).FullName);
            this.Properties.SanitizeProperties();
            this.Metrics.SanitizeMeasurements();
        }
    }
}

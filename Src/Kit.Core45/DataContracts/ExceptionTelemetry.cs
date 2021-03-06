﻿namespace Piksel.HockeyApp.DataContracts
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Channel;
    using Extensibility.Implementation;
    using Extensibility.Implementation.External;
    using Extensibility.Implementation.Platform;

    /// <summary>
    /// Telemetry type used to track exceptions.
    /// </summary>
    internal sealed class ExceptionTelemetry : ITelemetry, ISupportProperties
    {
        internal const int MaxExceptionCountToSave = 10;
        internal const string TelemetryName = "Exception";
        internal readonly string BaseType = typeof(ExceptionData).Name;
        internal readonly ExceptionData Data;

        private readonly TelemetryContext context;
        private Exception exception;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionTelemetry"/> class with empty properties.
        /// </summary>
        public ExceptionTelemetry()
        {
            this.Data = new ExceptionData();
            this.context = new TelemetryContext(this.Data.properties, new Dictionary<string, string>());
            this.HandledAt = default(ExceptionHandledAt);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionTelemetry"/> class with empty properties.
        /// </summary>
        /// <param name="exception">Exception instance.</param>
        public ExceptionTelemetry(Exception exception)
            : this()
        {
            if (exception == null)
            {
                exception = new Exception(Utils.PopulateRequiredStringValue(null, "message", typeof(ExceptionTelemetry).FullName));
            }

            this.Exception = exception;
        }

        /// <summary>
        /// Gets or sets date and time when telemetry was recorded.
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
        /// Gets or sets the value indicated where the exception was handled.
        /// </summary>
        public ExceptionHandledAt HandledAt
        {
            get
            {
                ExceptionHandledAt result;
                return Enum.TryParse<ExceptionHandledAt>(this.Data.handledAt, out result) ? result : ExceptionHandledAt.Unhandled;
            }

            set
            {
                this.Data.handledAt = value.ToString();
            }
        }
        
        /// <summary>
        /// Gets or sets the original exception tracked by this <see cref="ITelemetry"/>.
        /// </summary>
        public Exception Exception
        {
            get 
            {
                return this.exception;
            }

            set 
            { 
                this.exception = value;
                this.UpdateExceptions(value);
            }
        }

        /// <summary>
        /// Gets a dictionary of application-defined exception metrics.
        /// </summary>
        public IDictionary<string, double> Metrics
        {
            get { return this.Data.measurements; }
        }

        /// <summary>
        /// Gets properties.
        /// </summary>
        public IDictionary<string, string> Properties
        {
            get { return this.Data.properties; }
        }
        
        /// <summary>
        /// Gets or sets Exception severity level.
        /// </summary>
        public SeverityLevel? SeverityLevel
        {
            get { return this.Data.severityLevel; }
            set { this.Data.severityLevel = value; }
        }

        internal IList<ExceptionDetails> Exceptions
        {
            get { return this.Data.exceptions; }
        }

        /// <summary>
        /// Sanitizes the properties based on constraints.
        /// </summary>
        void ITelemetry.Sanitize()
        {
            // Sanitize on the ExceptionDetails stack information for raw stack and parsed stack is done while creating the object in ExceptionConverter.cs
            this.Properties.SanitizeProperties();
            this.Metrics.SanitizeMeasurements();
        }

        private static void ConvertExceptionTree(Exception exception, ExceptionDetails parentExceptionDetails, List<ExceptionDetails> exceptions)
        {
            if (exception == null)
            {
                exception = new Exception(Utils.PopulateRequiredStringValue(null, "message", typeof(ExceptionTelemetry).FullName));
            }

            ExceptionDetails exceptionDetails = PlatformSingleton.Current.GetExceptionDetails(exception, parentExceptionDetails);
            exceptions.Add(exceptionDetails);

            AggregateException aggregate = exception as AggregateException;
            if (aggregate != null)
            {
                foreach (Exception inner in aggregate.InnerExceptions)
                {
                    ExceptionTelemetry.ConvertExceptionTree(inner, exceptionDetails, exceptions);
                }
            }
            else if (exception.InnerException != null)
            {
                ExceptionTelemetry.ConvertExceptionTree(exception.InnerException, exceptionDetails, exceptions);
            }
        }

        private void UpdateExceptions(Exception exception)
        {
            // collect the set of exceptions detail info from the passed in exception
            List<ExceptionDetails> exceptions = new List<ExceptionDetails>();
            ExceptionTelemetry.ConvertExceptionTree(exception, null, exceptions);

            // trim if we have too many, also add a custom exception to let the user know we're trimed
            if (exceptions.Count > MaxExceptionCountToSave)
            {
                // TODO: when we localize these messages, we should consider not using InvariantCulture
                // create our "message" exception.
                InnerExceptionCountExceededException countExceededException = new InnerExceptionCountExceededException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "The number of inner exceptions was {0} which is larger than {1}, the maximum number allowed during transmission. All but the first {1} have been dropped.",
                        exceptions.Count,
                        MaxExceptionCountToSave));

                // remove all but the first N exceptions
                exceptions.RemoveRange(MaxExceptionCountToSave, exceptions.Count - MaxExceptionCountToSave);
                
                // we'll add our new exception and parent it to the root exception (first one in the list)
                exceptions.Add(PlatformSingleton.Current.GetExceptionDetails(countExceededException, exceptions[0]));
            }
            
            this.Data.exceptions = exceptions;
        }
    }
}

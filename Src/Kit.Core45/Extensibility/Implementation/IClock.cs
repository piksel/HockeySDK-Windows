namespace Piksel.HockeyApp.Extensibility.Implementation
{
    using System;

    internal interface IClock
    {
        DateTimeOffset Time { get; }
    }
}

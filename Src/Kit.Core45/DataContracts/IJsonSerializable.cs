namespace Piksel.HockeyApp.DataContracts
{
    /// <summary>
    /// Represents objects that support serialization to JSON.
    /// </summary>
    internal interface IJsonSerializable
    {
        /// <summary>
        /// Writes JSON representation of the object to the specified <paramref name="writer"/>.
        /// </summary>
        void Serialize(IJsonWriter writer);
    }
}
﻿// <copyright file="StorageBase.cs" company="Microsoft">
// Copyright © Microsoft. All Rights Reserved.
// </copyright>

namespace Piksel.HockeyApp.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Extensibility.Implementation.Tracing;
    using Channel;

    internal abstract class BaseStorageService
    {
        /// <summary>
        /// Peeked transmissions dictionary (maps file name to its full path). Holds all the transmissions that were peeked.
        /// </summary>
        /// <remarks>
        /// Note: The value (=file's full path) is not required in the Storage implementation. 
        /// If there was a concurrent Abstract Data Type Set it would have been used instead. 
        /// However, since there is no concurrent Set, dictionary is used and the second value is ignored.    
        /// </remarks>
        protected IDictionary<string, string> peekedTransmissions;

        /// <summary>
        /// Gets or sets the maximum size of the storage in bytes. When limit is reached, the Enqueue method will drop new transmissions. 
        /// </summary>
        internal ulong CapacityInBytes { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of files. When limit is reached, the Enqueue method will drop new transmissions. 
        /// </summary>
        internal uint MaxFiles { get; set; }

        internal abstract string FolderName { get; }

        /// <summary>
        /// Initializes the <see cref="BaseStorageService"/>
        /// </summary>
        /// <param name="uniqueFolderName">A folder name. Under this folder all the transmissions will be saved.</param>
        internal abstract void Init(string uniqueFolderName);

        internal abstract StorageTransmission Peek();

        internal abstract void Delete(StorageTransmission transmission);

        internal abstract Task EnqueueAsync(Transmission transmission);

        protected void OnPeekedItemDisposed(string fileName)
        {
            try
            {
                if (this.peekedTransmissions.ContainsKey(fileName))
                {
                    this.peekedTransmissions.Remove(fileName);
                }
            }
            catch (Exception e)
            {
                CoreEventSource.Log.LogVerbose("Failed to remove the item from storage items. Exception: " + e.ToString());
            }
        }
    }
}

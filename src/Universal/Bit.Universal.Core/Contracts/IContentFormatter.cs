﻿using System.Diagnostics.CodeAnalysis;

namespace Bit.Core.Contracts
{
    /// <summary>
    /// It serialize/deSerialize objects to/from json/xml etc based on what implementation is provided.
    /// </summary>
    public interface IContentFormatter
    {
        string Serialize<T>([AllowNull] T obj);

        T Deserialize<T>(string objAsStr);
    }
}
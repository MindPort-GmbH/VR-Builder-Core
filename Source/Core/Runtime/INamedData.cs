// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021 MindPort GmbH

using System.Runtime.Serialization;
using VRBuilder.Core.Attributes;

namespace VRBuilder.Core
{
    /// <summary>
    /// Data structure with an <see cref="IStep"/>'s name.
    /// </summary>
    public interface INamedData : IData
    {
        /// <summary>
        /// <see cref="IStep"/>'s name.
        /// </summary>
        [DataMember]
        [HideInProcessInspector]
        string Name { get; set; }
    }
}

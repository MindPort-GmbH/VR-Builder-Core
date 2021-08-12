// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021 MindPort GmbH

namespace VRBuilder.Core
{
    /// <summary>
    /// A chapter is a high-level grouping of several <see cref="IStep"/>s.
    /// </summary>
    public interface IChapter : IEntity, IDataOwner<IChapterData>
    {
        /// <summary>
        /// Utility data which is used by Training SDK custom editors.
        /// </summary>
        ChapterMetadata ChapterMetadata { get; }
    }
}

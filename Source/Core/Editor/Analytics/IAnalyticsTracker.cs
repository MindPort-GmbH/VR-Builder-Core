// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021 MindPort GmbH

namespace VRBuilder.Editor.Analytics
{
    internal interface IAnalyticsTracker
    {
        /// <summary>
        /// Session id in use.
        /// </summary>
        string SessionId { get; }

        /// <summary>
        /// Sends given data.
        /// </summary>
        void Send(AnalyticsEvent data);

        /// <summary>
        /// Send a start event.
        /// </summary>
        void SendSessionStart();
    }
}

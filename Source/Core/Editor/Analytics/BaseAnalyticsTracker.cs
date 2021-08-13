// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021 MindPort GmbH

using System;
using System.Globalization;
using UnityEditor;

namespace VRBuilder.Editor.Analytics
{
    /// <summary>
    /// Abstract analytics handler, which handles the SessionId
    /// </summary>
    internal abstract class BaseAnalyticsTracker : IAnalyticsTracker
    {
        public const string KeySessionId = "Innoactive.Creator.Analytics.SessionID";

        public string SessionId { get; }

        internal BaseAnalyticsTracker()
        {
            if (EditorPrefs.HasKey(KeySessionId))
            {
                SessionId = EditorPrefs.GetString(KeySessionId);
            }
            else
            {
                SessionId = Guid.NewGuid().ToString();
            }
        }

        public abstract void Send(AnalyticsEvent data);

        public abstract void SendSessionStart();

        protected string GetLanguage()
        {
            return CultureInfo.InstalledUICulture.Name;
        }
    }
}

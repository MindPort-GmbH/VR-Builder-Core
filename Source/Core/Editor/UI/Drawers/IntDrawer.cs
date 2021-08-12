// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021 MindPort GmbH

using System;
using UnityEditor;
using UnityEngine;

namespace VRBuilder.Editor.UI.Drawers
{
    /// <summary>
    /// Training drawer for int values.
    /// </summary>
    [DefaultTrainingDrawer(typeof(int))]
    internal class IntDrawer : AbstractDrawer
    {
        /// <inheritdoc />
        public override Rect Draw(Rect rect, object currentValue, Action<object> changeValueCallback, GUIContent label)
        {
            rect.height = EditorDrawingHelper.SingleLineHeight;

            int value = (int)currentValue;
            int newValue = EditorGUI.IntField(rect, label, value);

            if (value != newValue)
            {
                ChangeValue(() => newValue, () => value, changeValueCallback);
            }

            return rect;
        }
    }
}

using System;
using UnityEditor;
using UnityEngine;
using VRBuilder.Core.Properties;
using VRBuilder.Core.SceneObjects;
using VRBuilder.Editor.UndoRedo;

namespace VRBuilder.Editor.UI.Drawers
{
    internal class FloatPropertyDrawer : UniqueNameReferenceDrawer
    {
        string propertyName = "";

        /// <inheritdoc />
        public override Rect Draw(Rect rect, object currentValue, Action<object> changeValueCallback, GUIContent label)
        {
            rect = base.Draw(rect, currentValue, changeValueCallback, label);

            UniqueNameReference reference = currentValue as UniqueNameReference;

            if (string.IsNullOrEmpty(reference.UniqueName))
            {
                Rect newLine = AddNewRectLine(ref rect);

                GUILayout.BeginArea(newLine);
                GUILayout.BeginHorizontal();

                propertyName = EditorGUILayout.TextField("New property", propertyName, GUILayout.Height(EditorDrawingHelper.SingleLineHeight));

                if (GUILayout.Button("Create", GUILayout.Width(64), GUILayout.Height(EditorDrawingHelper.SingleLineHeight)))
                {
                    GameObject dataObject = GameObject.Find("[PROCESS_DATA]");
                    if (dataObject == null)
                    {
                        dataObject = new GameObject("[PROCESS_DATA]");
                    }

                    GameObject property = new GameObject(propertyName);
                    property.AddComponent<FloatValueProperty>();
                    property.transform.SetParent(dataObject.transform);

                    string oldUniqueName = reference.UniqueName;
                    string newUniqueName = GetIDFromSelectedObject(property, typeof(FloatValueProperty), oldUniqueName);

                    if (reference.UniqueName != newUniqueName)
                    {
                        RevertableChangesHandler.Do(
                            new ProcessCommand(
                                () =>
                                {
                                    reference.UniqueName = newUniqueName;
                                    changeValueCallback(reference);
                                },
                                () =>
                                {
                                    reference.UniqueName = oldUniqueName;
                                    changeValueCallback(reference);
                                }),
                            isUndoOperation ? undoGroupName : string.Empty);

                        if (isUndoOperation)
                        {
                            RevertableChangesHandler.CollapseUndoOperations(undoGroupName);
                        }

                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.EndArea();
            }

            return rect;            
        }
    }
}

using System;
using System.Reflection;
using VRBuilder.Core;
using UnityEngine;

namespace VRBuilder.Editor.UI.Drawers
{
    [DefaultTrainingDrawer(typeof(TransitionCollection))]
    internal class TransitionCollectionDrawer : DataOwnerDrawer
    {
        public override GUIContent GetLabel(MemberInfo memberInfo, object memberOwner)
        {
            return null;
        }

        public override GUIContent GetLabel(object value, Type declaredType)
        {
            return null;
        }
    }
}

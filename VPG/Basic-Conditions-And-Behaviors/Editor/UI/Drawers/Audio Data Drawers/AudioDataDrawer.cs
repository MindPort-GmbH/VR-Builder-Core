using System;
using System.Reflection;
using VPG.Core.Audio;
using VPG.Editor.UI.Drawers;
using UnityEngine;

namespace VPG.Editor.Core.UI.Drawers
{
    /// <summary>
    /// Training drawer for <see cref="IAudioData"/> members.
    /// </summary>
    [DefaultTrainingDrawer(typeof(IAudioData))]
    public class AudioDataDrawer : ObjectDrawer
    {
        protected string tooltip;
        protected Texture image;
        
        /// <inheritdoc />
        public override Rect Draw(Rect rect, object currentValue, Action<object> changeValueCallback, GUIContent label)
        {
            tooltip = label.tooltip;
            image = label.image;
            // Has to run with a null label to not show two labels, dont ask me why.
            return base.Draw(rect, currentValue, changeValueCallback, GUIContent.none);
        }

        /// <inheritdoc />
        protected override void CheckValidationForValue(object currentValue, MemberInfo info, GUIContent label)
        {
            label.image = image;
            label.tooltip = tooltip;
        }
    }
}

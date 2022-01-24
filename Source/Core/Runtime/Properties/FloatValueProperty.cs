using VRBuilder.Core.SceneObjects;

namespace VRBuilder.Core.Properties
{
    public class FloatValueProperty : ProcessSceneObjectProperty, IValueProperty<float>
    {
        private float value;

        public float GetValue()
        {
            return value;
        }

        public void SetValue(float value)
        {
            this.value = value;
        }
    }
}

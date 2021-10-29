using UnityEngine;

namespace VRBuilder.Core.Properties
{
    public interface IPathProperty : ISceneObjectProperty
    {
        Vector3 GetPoint(float t);

        Vector3 GetDirection(float t);
    }
}

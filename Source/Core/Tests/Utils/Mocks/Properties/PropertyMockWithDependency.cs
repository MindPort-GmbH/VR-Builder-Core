using VRBuilder.Core.Properties;
using UnityEngine;

namespace VRBuilder.Tests.Utils.Mocks
{
    /// <summary>
    /// Property requiring a <see cref="PropertyMock"/>.
    /// </summary>
    [RequireComponent(typeof(PropertyMock))]
    public class PropertyMockWithDependency : TrainingSceneObjectProperty
    {

    }
}

using Unity.Entities;
using UnityEngine;

public class PresentationGameObject : IComponentData
{
    public GameObject Instance;
}

public struct HasLinkedCompanionTag : IComponentData { }

public class PresentationGameObjectCleanUp : ICleanupComponentData
{
    public GameObject Instance;
}
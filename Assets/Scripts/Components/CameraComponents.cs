using Unity.Entities;
using UnityEngine;

public struct CameraFollowEntity : IComponentData
{
    public Entity Value;
}

public class PresentationGameObject : IComponentData
{
    public GameObject Instance;
}

public struct InitedTag : IComponentData { }

public class PresentationGameObjectCleanUp : ICleanupComponentData
{
    public GameObject Instance;
}
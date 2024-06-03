using Unity.Entities;
using UnityEngine;

public struct CameraFollowEntity : IComponentData
{
    public Entity Value;
}

public class PresentationGameObject : IComponentData
{
    public GameObject Value;
}

public struct InitedTag : IComponentData { }
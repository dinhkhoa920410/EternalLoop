using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[UpdateAfter(typeof(TransformSystemGroup))]
[RequireMatchingQueriesForUpdate]
public partial struct PresentationObjectTransformUpdateSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (localToWorld, transform) in SystemAPI.Query<RefRO<LocalToWorld>, SystemAPI.ManagedAPI.UnityEngineComponent<Transform>>()
                     .WithAll<InitedTag>())
        {
            transform.Value.position = localToWorld.ValueRO.Position;
            transform.Value.rotation = localToWorld.ValueRO.Rotation;
        }
    }
}
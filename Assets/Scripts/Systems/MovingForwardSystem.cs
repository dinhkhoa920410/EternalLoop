using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

public partial struct MovingForwardSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;
        foreach (var (transform, speed) in SystemAPI
                     .Query<RefRW<LocalTransform>, ProjectileSpeed>()
                     .WithAll<Simulate>())
        {
                transform.ValueRW.Position += transform.ValueRW.Up() * speed.Value * deltaTime;
        }
    }
}

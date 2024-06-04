using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateBefore(typeof(TransformSystemGroup))]
public partial struct MovingWithInputSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;
        foreach (var (transform, moveInput, moveSpeed) in SystemAPI
                     .Query<RefRW<LocalTransform>, PlayerMoveInput, PlayerMoveSpeed>()
                     .WithAll<PlayerTag>()
                     .WithAll<Simulate>())
        {
            if (math.lengthsq(moveInput.Value) > float.Epsilon)
            {
                transform.ValueRW.Position.xy += moveInput.Value * moveSpeed.Value * deltaTime;
                
                var forward = new float3(moveInput.Value.x, moveInput.Value.y, 0f);
                transform.ValueRW.Rotation = quaternion.LookRotationSafe(math.forward(), forward);
                
            }
        }
    }
}
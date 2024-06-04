using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateBefore(typeof(TransformSystemGroup))]
public partial struct FollowTargetSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;

        foreach (var (transform, moveSpeed, target, currentState) in SystemAPI
                     .Query<RefRW<LocalTransform>, RefRO<Movable>, RefRO<Target>, RefRO<CurrentState>>()
                     .WithAll<EnemyTag>()
                     .WithAll<Simulate>())
        {
            if (currentState.ValueRO.Value != BehaviorState.Moving) continue;
            
            var targetPosition = SystemAPI.GetComponent<LocalTransform>(target.ValueRO.Value).Position;
            var selfPosition = transform.ValueRO.Position;
            var distanceSquareToTarget = math.distancesq(targetPosition, selfPosition);
            if(distanceSquareToTarget < 0.5f) continue;
            
            var moveDirection = math.normalize (targetPosition - selfPosition);
            var moveVector = moveDirection * moveSpeed.ValueRO.Value * deltaTime;
            transform.ValueRW.Position += moveVector;
            transform.ValueRW.Rotation = quaternion.LookRotationSafe(math.back(), moveDirection);
        }
    }
}
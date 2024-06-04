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
        
        foreach (var (transform, moveSpeed) in SystemAPI
                     .Query<RefRW<LocalTransform>, Movable>()
                     .WithAll<EnemyTag>()
                     .WithAll<Simulate>())
        {
            var nearestPlayerPosition = float3.zero;
            var nearestPlayerDistance = -1f;
            var selfPosition = transform.ValueRW.Position;
            foreach (var playerTransform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<PlayerTag>())
            {
                var playerPosition = playerTransform.ValueRO.Position;
                var distanceToPlayer = math.distance(playerPosition, selfPosition);
                if (nearestPlayerDistance < 0 || nearestPlayerDistance > distanceToPlayer)
                {
                    nearestPlayerPosition = playerPosition;
                    nearestPlayerDistance = distanceToPlayer;
                }
            }
            
            if(nearestPlayerDistance < 0.001f) continue;
            
            var moveDirection = math.normalize (nearestPlayerPosition - selfPosition);
            var moveVector = moveDirection * moveSpeed.Value * deltaTime;
            transform.ValueRW.Position += moveVector;
            transform.ValueRW.Rotation = quaternion.LookRotationSafe(math.back(), moveDirection);
        }
    }
    
    [BurstCompile]
    public partial struct MovingJob : IJobEntity
    {
        public float DeltaTime;

        [BurstCompile]
        private void Execute(ref LocalTransform transform, in PlayerMoveInput moveInput, PlayerMoveSpeed moveSpeed)
        {
            if (math.lengthsq(moveInput.Value) > float.Epsilon)
            {
                transform.Position.xy += moveInput.Value * moveSpeed.Value * DeltaTime;
                
                var forward = new float3(moveInput.Value.x, moveInput.Value.y, 0f);
                transform.Rotation = quaternion.LookRotationSafe(math.back(), forward);
                
            }
        }
    }
}
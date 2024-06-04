using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct SearchingTargetSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecbBS = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
            .CreateCommandBuffer(state.WorldUnmanaged);
        
        foreach (var (localTransform, enemyEntity) in SystemAPI.Query<RefRO<LocalTransform>>().
                     WithAll<Simulate>().WithAll<EnemyTag>().WithNone<Target>().WithEntityAccess())
        {
            Entity targetEntity = default;
            var nearestPlayerDistance = -1f;
            var selfPosition = localTransform.ValueRO.Position;
            foreach (var (playerTransform, playerEntity) in SystemAPI.Query<RefRO<LocalTransform>>()
                         .WithAll<PlayerTag>().WithEntityAccess())
            {
                var playerPosition = playerTransform.ValueRO.Position;
                var distanceToPlayer = math.distance(playerPosition, selfPosition);
                if (targetEntity == default || nearestPlayerDistance > distanceToPlayer)
                {
                    nearestPlayerDistance = distanceToPlayer;
                    targetEntity = playerEntity;
                }
            }
            ecbBS.AddComponent(enemyEntity, new Target
            {
                Value = targetEntity
            });
        }
    }
}
using Unity.Burst;
using Unity.Entities;

[UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
[UpdateAfter(typeof(SimulationSystemGroup))]
public partial struct ActualDestroyEntitySystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        var ecbEs = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        foreach (var (isDestroyedTag, entity) in SystemAPI.Query<IsDestroyedTag>()
                     .WithAll<Simulate>()
                     .WithEntityAccess())
        {
            ecbEs.DestroyEntity(entity);
        }
    }
}
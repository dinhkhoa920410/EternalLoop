using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

public partial struct DestroyOnTimerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecbBs = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        
        foreach (var (timeToDestroy, entity) in SystemAPI.Query<RefRW<TimeToDestroy>>().WithAll<Simulate>().WithEntityAccess())
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            timeToDestroy.ValueRW.Value -= deltaTime;
            if (timeToDestroy.ValueRW.Value < 0)
            {
                ecbBs.AddComponent<IsDestroyedTag>(entity);
            }
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }
}

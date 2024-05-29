using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;

[UpdateInGroup(typeof(PhysicsSystemGroup))]
[UpdateAfter(typeof(DamageOnCollisionSystem))]
public partial struct DestroyOnCollisionSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<SimulationSingleton>();
        state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        var destroyOnCollisionJob = new DestroyOnCollisionJob
        {
            DestroyOnCollisionLookup = SystemAPI.GetComponentLookup<DestroyOnCollisionTag>(true),
            EcbBs = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged)
        };
        var simulationSingleton = SystemAPI.GetSingleton<SimulationSingleton>();
        state.Dependency = destroyOnCollisionJob.Schedule(simulationSingleton, state.Dependency);
    }

    public struct DestroyOnCollisionJob : ITriggerEventsJob
    {
        [ReadOnly] public ComponentLookup<DestroyOnCollisionTag> DestroyOnCollisionLookup;

        public EntityCommandBuffer EcbBs;
        
        public void Execute(TriggerEvent triggerEvent)
        {
            if(DestroyOnCollisionLookup.HasComponent(triggerEvent.EntityA))
                EcbBs.AddComponent<IsDestroyedTag>(triggerEvent.EntityA);
            if(DestroyOnCollisionLookup.HasComponent(triggerEvent.EntityB))
                EcbBs.AddComponent<IsDestroyedTag>(triggerEvent.EntityB);
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }
}
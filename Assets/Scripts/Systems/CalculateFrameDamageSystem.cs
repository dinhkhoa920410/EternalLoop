using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics.Systems;

[UpdateInGroup(typeof(PhysicsSystemGroup))]
[UpdateAfter(typeof(DamageOnCollisionSystem))]
public partial struct CalculateFrameDamageSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecbEOS = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        
        foreach (var (receivedDamages, currentHP, entity) in SystemAPI
                     .Query<DynamicBuffer<ReceivedDamage>, RefRW<CurrentHp>>()
                     .WithAll<Simulate>()
                     .WithEntityAccess())
        {
            if (!receivedDamages.IsEmpty)
            {
                var totalDamage = 0f;
                foreach (var receivedDamage in receivedDamages)
                {
                    totalDamage += receivedDamage.Value;
                }
                receivedDamages.Clear();
                var newHp = currentHP.ValueRW.Value - totalDamage;
                if (newHp <= 0)
                {
                    newHp = 0;
                    ecbEOS.AddComponent(entity, new IsDestroyedTag());
                    // ecbEOS.SetComponentEnabled<Simulate>(entity, false);
                    ecbEOS.DestroyEntity(entity);
                }
                currentHP.ValueRW.Value = newHp;
            }
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }
}
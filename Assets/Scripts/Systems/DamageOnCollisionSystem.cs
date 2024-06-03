using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;


    [UpdateInGroup(typeof(PhysicsSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    public partial struct DamageOnCollisionSystem : ISystem
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
            var damageOnTriggerJob = new DamageOnTriggerJob
            {
                DamageSourceLookUp = SystemAPI.GetComponentLookup<DamageSource>(true),
                // DamageSourceLookUp = SystemAPI.GetComponentLookup<DamageSource>(true),
                AlreadyDamageEntityLookUp = SystemAPI.GetBufferLookup<AlreadyDamagedEntity>(true),
                ReceivedDamagesLookUp = SystemAPI.GetBufferLookup<ReceivedDamage>(true),
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged)
            };
            var simulationSingleton = SystemAPI.GetSingleton<SimulationSingleton>();
            state.Dependency = damageOnTriggerJob.Schedule(simulationSingleton, state.Dependency);
        }

        public struct DamageOnTriggerJob : ITriggerEventsJob
        {
            [ReadOnly] public ComponentLookup<DamageSource> DamageSourceLookUp;
            // [ReadOnly] public ComponentLookup<Team> DamageSource;
            [ReadOnly] public BufferLookup<AlreadyDamagedEntity> AlreadyDamageEntityLookUp;
            [ReadOnly] public BufferLookup<ReceivedDamage> ReceivedDamagesLookUp;

            public EntityCommandBuffer ECB;

            public void Execute(TriggerEvent triggerEvent)
            {
                Entity damageDealingEntity;
                Entity damageReceivingEntity;
                
                if (DamageSourceLookUp.HasComponent(triggerEvent.EntityA)
                    && ReceivedDamagesLookUp.HasBuffer(triggerEvent.EntityB))
                {
                    damageDealingEntity = triggerEvent.EntityA;
                    damageReceivingEntity = triggerEvent.EntityB;
                }
                else if (DamageSourceLookUp.HasComponent(triggerEvent.EntityB)
                        && ReceivedDamagesLookUp.HasBuffer(triggerEvent.EntityA))
                {
                    damageDealingEntity = triggerEvent.EntityB;
                    damageReceivingEntity = triggerEvent.EntityA;
                }
                else
                {
                    return;
                }
                
                //Don't apply damage multiple times
                var alreadyDamagedEntities = AlreadyDamageEntityLookUp[damageDealingEntity];
                foreach (var alreadyDamagedEntity in alreadyDamagedEntities)
                {
                    if (alreadyDamagedEntity.Value.Equals(damageReceivingEntity)) return;
                }

                var damageSource = DamageSourceLookUp[damageDealingEntity];
                ECB.AppendToBuffer(damageReceivingEntity, new ReceivedDamage {Value = damageSource.DamageAmount});
                ECB.AppendToBuffer(damageDealingEntity, new AlreadyDamagedEntity {Value = damageReceivingEntity});
                ECB.AddComponent(damageReceivingEntity, new IsDamagedTag());
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
    }

using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace DefaultNamespace.Systems
{
    public partial struct CastSkillSystem : ISystem
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
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
            
            foreach (var (cooldownTimer, playerTransform, bulletPrefab, range, cooldown) in SystemAPI
                         .Query<RefRW<CooldownTimer>, RefRO<LocalTransform>, BulletPrefab, Range, Cooldown>()
                         .WithAll<Simulate>())
            {
                var deltaTime = SystemAPI.Time.DeltaTime;
                cooldownTimer.ValueRW.Value -= deltaTime;
                if (cooldownTimer.ValueRO.Value > 0)
                    return;
                    
                var playerPosition = playerTransform.ValueRO.Position;
                var nearestEnemySquareDistance = float.MaxValue;
                var nearestEnemyPosition = float3.zero;
                var hasInRangeEnemy = false;
                foreach (var enemyTransform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<EnemyTag>().WithAll<Simulate>())
                {
                    var enemyPosition = enemyTransform.ValueRO.Position;
                    var squareDistanceToEnemy = math.distancesq(playerPosition, enemyPosition);
                    if (squareDistanceToEnemy < range.Value)
                    {
                        if (nearestEnemySquareDistance > squareDistanceToEnemy)
                        {
                            nearestEnemySquareDistance = squareDistanceToEnemy;
                            nearestEnemyPosition = enemyPosition;
                            hasInRangeEnemy = true;
                        }
                    }
                }
            
                if (hasInRangeEnemy)
                {
                    var newBullet = ecb.Instantiate(bulletPrefab.Value);
                    var newBulletTransform = LocalTransform.FromPosition(playerPosition);
                    newBulletTransform.Rotation = quaternion.LookRotationSafe(math.forward(), nearestEnemyPosition-playerPosition);
                    
                    ecb.SetComponent(newBullet, newBulletTransform);
                    cooldownTimer.ValueRW.Value = cooldown.Value;
                }
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
    }
}
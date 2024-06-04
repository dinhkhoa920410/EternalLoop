using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct SpawnEnemySystem : ISystem
{
    private double _nextSpawnTime;
    private Random _random;
    private Entity _enemySpawnerEntity;
    private bool _isInitialized;
    
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<BeginInitializationEntityCommandBufferSystem.Singleton>();
        state.RequireForUpdate<EnemySpawnerTag>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (SystemAPI.Time.ElapsedTime < _nextSpawnTime)
            return;
        if (!_isInitialized)
        {
            _enemySpawnerEntity = SystemAPI.GetSingletonEntity<EnemySpawnerTag>();
            _random = Random.CreateFromIndex((uint) _enemySpawnerEntity.GetHashCode());
            _isInitialized = true;
        }
        
        var ecbBI = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>()
            .CreateCommandBuffer(state.WorldUnmanaged);
        
        var spawnCooldown = state.EntityManager.GetComponentData<SpawnCooldown>(_enemySpawnerEntity);
        var rangeFromPlayer = state.EntityManager.GetComponentData<RangeFromPlayer>(_enemySpawnerEntity);
        var enemyDataBufferLookUp = SystemAPI.GetBufferLookup<EnemyData>(true);
        enemyDataBufferLookUp.TryGetBuffer(_enemySpawnerEntity, out var enemyDataBuffer);
        var enemyData = enemyDataBuffer[0];
        var newEnemy = ecbBI.Instantiate(enemyData.Prefab);
            
        foreach (var playerTransform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<PlayerTag>().WithAll<Simulate>())
        {
            var playerPosition = playerTransform.ValueRO.Position;
            var randomOffsetX = _random.NextFloat(rangeFromPlayer.Min, rangeFromPlayer.Max) * (_random.NextBool() ? 1f : -1f);
            var randomOffsetY = _random.NextFloat(rangeFromPlayer.Min, rangeFromPlayer.Max) * (_random.NextBool() ? 1f : -1f);
            var randomPosition = math.float3(playerPosition.x + randomOffsetX, playerPosition.y + randomOffsetY, 0);
            var newEnemyTransform = LocalTransform.FromPosition(randomPosition);
            
            ecbBI.SetComponent(newEnemy, newEnemyTransform);
            _nextSpawnTime = SystemAPI.Time.ElapsedTime + spawnCooldown.Value;
        }
    }
}
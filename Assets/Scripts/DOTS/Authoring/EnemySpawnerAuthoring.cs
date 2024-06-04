using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemySpawnerAuthoring : MonoBehaviour
{
    [SerializeField] private List<EnemyScriptableObject> enemyScriptableObjects;
    [SerializeField] private int minDistanceFromPlayer;
    [SerializeField] private int maxDistanceFromPlayer;
    [SerializeField] private float cooldown;
    
    private class EnemySpawnerAuthoringBaker : Baker<EnemySpawnerAuthoring>
    {
        public override void Bake(EnemySpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddBuffer<EnemyData>(entity);
            foreach (var enemyData in authoring.enemyScriptableObjects)
            {
                AppendToBuffer(entity, new EnemyData
                {
                    Prefab = GetEntity(enemyData.prefab, TransformUsageFlags.Dynamic),
                    Health = enemyData.health,
                    Attack = enemyData.attack,
                    Defense = enemyData.defense,
                    MoveSpeed = enemyData.moveSpeed,
                    Experience = enemyData.experience
                });
            }
            AddComponent(entity, new RangeFromPlayer
            {
                Min = authoring.minDistanceFromPlayer,
                Max = authoring.maxDistanceFromPlayer
            });
            AddComponent(entity, new SpawnCooldown
            {
                Value = authoring.cooldown
            });
            AddComponent<EnemySpawnerTag>(entity);
        }
    }
}
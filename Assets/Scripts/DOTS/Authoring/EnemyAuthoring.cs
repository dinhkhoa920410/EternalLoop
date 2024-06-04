using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics.Authoring;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyAuthoring : MonoBehaviour
{
    public float moveSpeed;
    public float maxHealthPoint;
    
    public class EnemyBaker : Baker<EnemyAuthoring>
    {
        public override void Bake(EnemyAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<EnemyTag>(entity);
            AddComponent(entity, new Movable
            {
                Value = authoring.moveSpeed
            });
            AddComponent(entity, new Damageable
            {
                MaxHp = authoring.maxHealthPoint
            });
            AddComponent(entity, new CurrentHp
            {
                Value = authoring.maxHealthPoint
            });
            AddBuffer<ReceivedDamage>(entity);
        }
    }
}

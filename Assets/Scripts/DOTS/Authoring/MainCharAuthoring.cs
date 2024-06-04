using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class MainCharAuthoring : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float range;
    [SerializeField] private float cooldown;
    
    
    public class MainCharBaker : Baker<MainCharAuthoring>
    {
        public override void Bake(MainCharAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<PlayerTag>(entity);
            AddComponent<PlayerMoveInput>(entity);
            AddComponent(entity, new PlayerMoveSpeed
            {
                Value = authoring.moveSpeed,
            });
            AddComponent(entity, new BulletPrefab
            {
                Value = GetEntity(authoring.bullet, TransformUsageFlags.Dynamic)
            });
            AddComponent(entity, new Range
            {
                Value = authoring.range
            });
            AddComponent(entity, new Cooldown
            {
                Value = authoring.cooldown
            });
            AddComponent(entity, new CooldownTimer
            {
                Value = authoring.cooldown
            });
        }
    }
}

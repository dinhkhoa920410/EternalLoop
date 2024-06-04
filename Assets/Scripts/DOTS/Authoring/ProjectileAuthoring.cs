using Unity.Entities;
using UnityEngine;


    public class ProjectileAuthoring : MonoBehaviour
    {
        [SerializeField] private float damage;
        [SerializeField] private float speed;
        [SerializeField] private float duration;
        [SerializeField] private bool isPiercing;
        
        private class ProjectileAuthoringBaker : Baker<ProjectileAuthoring>
        {
            
            
            public override void Bake(ProjectileAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new DamageSource
                {
                    DamageAmount = authoring.damage
                });
                AddComponent(entity, new ProjectileSpeed
                {
                    Value = authoring.speed
                });
                AddComponent(entity, new Timer
                {
                    Value = authoring.duration
                });
                AddBuffer<AlreadyDamagedEntity>(entity);
                AddComponent(entity, new TimeToDestroy
                {
                    Value = authoring.duration
                });
                if(!authoring.isPiercing)
                    AddComponent<DestroyOnCollisionTag>(entity);
            }
        }
    }

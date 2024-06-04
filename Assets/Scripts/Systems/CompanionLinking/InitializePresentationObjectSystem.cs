using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial struct InitializePresentationObjectSystem : ISystem
{
    
    
    public void OnUpdate(ref SystemState state)
    {
        foreach (var entity in SystemAPI.QueryBuilder()
                     .WithAll<LocalToWorld, PresentationGameObject>()
                     .WithNone<InitedTag>()
                     .Build().ToEntityArray(Allocator.Temp))
        {
            var presentationGameObject = SystemAPI.ManagedAPI.GetComponent<PresentationGameObject>(entity);

            var gameObject = Object.Instantiate(presentationGameObject.Instance);

            var components = gameObject.GetComponents(typeof(Component));
            foreach (var component in components)
            {
                if(component != null)
                    state.EntityManager.AddComponentObject(entity, component);
            }
            
            gameObject.AddComponent<EntityGameObject>().AssignEntity(entity, state.World);
            state.EntityManager.AddComponentData(entity, new InitedTag());

            var localToWorld = SystemAPI.GetComponent<LocalToWorld>(entity);

            gameObject.transform.position = localToWorld.Position;
            gameObject.transform.rotation = localToWorld.Rotation;
        }
    }
}
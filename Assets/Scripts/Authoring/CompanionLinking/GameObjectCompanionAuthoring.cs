using Unity.Entities;
using UnityEngine;


public class GameObjectCompanionAuthoring : MonoBehaviour
{
    [SerializeField] private GameObject Prefab;
    
    
    private class GameObjectCompanionAuthoringBaker : Baker<GameObjectCompanionAuthoring>
    {
        public override void Bake(GameObjectCompanionAuthoring authoring)
        {
            Entity bakingEntity = GetEntity(TransformUsageFlags.WorldSpace);
            AddComponentObject(bakingEntity, new PresentationGameObject
            {
                Instance = authoring.Prefab
            });
        }
    }
}
using Unity.Entities;
using UnityEngine;

public class FollowPlayerCameraAuthoring : MonoBehaviour
{
    [SerializeField] private Transform cameraTarget;
    
    private class FollowPlayerCameraAuthoringBaker : Baker<FollowPlayerCameraAuthoring>
    {
        public override void Bake(FollowPlayerCameraAuthoring authoring)
        {
            var bakingEntity = GetEntity(authoring, TransformUsageFlags.Dynamic);
            
            AddComponent(bakingEntity, new CameraFollowEntity
            {
                Value = GetEntity(authoring.cameraTarget, TransformUsageFlags.Dynamic)
            });
        }
    }
}

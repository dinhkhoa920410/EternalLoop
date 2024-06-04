using Unity.Entities;
using UnityEngine;

public class CameraFollowAuthoring : MonoBehaviour
{
    [SerializeField] private Transform cameraTarget;
    
    private class FollowPlayerCameraAuthoringBaker : Baker<CameraFollowAuthoring>
    {
        public override void Bake(CameraFollowAuthoring authoring)
        {
            var bakingEntity = GetEntity(authoring, TransformUsageFlags.None);
            
            AddComponent(bakingEntity, new CameraFollowEntity
            {
                Value = GetEntity(authoring.cameraTarget, TransformUsageFlags.None)
            });
        }
    }
}

using Cinemachine;
using Unity.Entities;
using UnityEngine;

public partial struct CameraFollowAssignmentSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<CameraFollowEntity>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var ecbBI = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach (var (cinemachineCamera, cameraFollowEntity, entity) in SystemAPI
                     .Query<SystemAPI.ManagedAPI.UnityEngineComponent<CinemachineVirtualCamera>, CameraFollowEntity>()
                     .WithEntityAccess())
        {
            cinemachineCamera.Value.Follow = SystemAPI.ManagedAPI.GetComponent<Transform>(cameraFollowEntity.Value);
        }
    }
}
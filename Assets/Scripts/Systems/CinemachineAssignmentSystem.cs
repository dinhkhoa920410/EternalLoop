using Cinemachine;
using Unity.Entities;
using UnityEngine;

public partial struct CinemachineAssignmentSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<CameraFollowEntity>();
    }

    public void OnUpdate(ref SystemState state)
    {
        foreach (var (cinemachineCamera, cameraFollowEntity) in SystemAPI
                     .Query<SystemAPI.ManagedAPI.UnityEngineComponent<CinemachineVirtualCamera>, CameraFollowEntity>())
        {
            cinemachineCamera.Value.Follow = SystemAPI.ManagedAPI.GetComponent<Transform>(cameraFollowEntity.Value);
        }
    }
}
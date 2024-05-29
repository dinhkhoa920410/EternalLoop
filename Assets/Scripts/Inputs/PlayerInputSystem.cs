
using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
public partial class GetPlayerInputSystem : SystemBase
{
    private PlayerMovement _playerMovement;
    private Entity _playerEntity;
    
    protected override void OnCreate()
    {
        RequireForUpdate<PlayerTag>();
        RequireForUpdate<PlayerMoveInput>();

        _playerMovement = new PlayerMovement();
    }

    protected override void OnStartRunning()
    {
        _playerMovement.Enable();
        _playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
    }

    protected override void OnStopRunning()
    {
        _playerMovement.Disable();
        _playerEntity = Entity.Null;
    }

    protected override void OnUpdate()
    {
        var currentMoveInput = _playerMovement.Default.PlayerMovement.ReadValue<Vector2>();
        SystemAPI.SetSingleton(new PlayerMoveInput {Value = currentMoveInput});
    }
}

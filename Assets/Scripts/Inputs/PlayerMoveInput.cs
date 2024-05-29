using Unity.Entities;
using Unity.Mathematics;

public struct PlayerMoveInput : IComponentData
{
    public float2 Value;
}

public struct PlayerMoveSpeed : IComponentData
{
    public float Value;
}

public struct PlayerTag : IComponentData
{
}

public struct EnemyTag : IComponentData
{
}

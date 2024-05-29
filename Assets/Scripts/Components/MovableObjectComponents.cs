
using Unity.Entities;

public struct Movable : IComponentData
{
    public float Value;
}

public struct ProjectileSpeed : IComponentData
{
    public float Value;
}

public struct Timer : IComponentData
{
    public float Value;
}
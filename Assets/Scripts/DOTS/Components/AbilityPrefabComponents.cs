using Unity.Entities;

public struct BulletPrefab : IComponentData
{
    public Entity Value;
}

public struct TimeToDestroy : IComponentData
{
    public float Value;
}

public struct Cooldown : IComponentData
{
    public float Value;
}

public struct CooldownTimer : IComponentData
{
    public float Value;
}
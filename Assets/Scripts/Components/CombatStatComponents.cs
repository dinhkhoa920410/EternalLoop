using Unity.Entities;

public struct Damageable : IComponentData
{
    public float MaxHp;
}

public struct CurrentHp : IComponentData
{
    public float Value;
}

public struct DamageSource : IComponentData
{
    public Entity Source;
    public float DamageAmount;
}

public struct Range : IComponentData
{
    public float Value;
}

public struct AlreadyDamagedEntity : IBufferElementData
{
    public Entity Value;
}

public struct ReceivedDamage : IBufferElementData
{
    public float Value;
}

public struct IsDestroyedTag : IComponentData { }

public struct DestroyOnCollisionTag : IComponentData { }
using System.Collections.Generic;
using Unity.Entities;

public enum AttackType
{
    Melee,
    Charge,
    Cast
}

public struct EnemyPrefab : IComponentData
{
    public Entity Value;
}

public struct RangeFromPlayer : IComponentData
{
    public int Min;
    public int Max;
}

public struct EnemyData : IBufferElementData
{
    public Entity Prefab;
    public float Health;
    public float Attack;
    public float Defense;
    public float MoveSpeed;
    public float Experience;
}

public struct Attack : IBufferElementData
{
    public AttackType AttackType;
    public Entity AttackPrefab;
    public float Cooldown;
    public float Range;
}

public struct SpawnCooldown : IComponentData
{
    public float Value;
}

public struct EnemySpawnerTag : IComponentData {}
using Unity.Entities;

public enum BehaviorState
{
    Idle,
    SearchingTarget,
    Moving,
    Attacking,
    Disable,
    Dead
}
public struct CurrentState : IComponentData
{
    public BehaviorState Value;
}

public struct Target : IComponentData
{
    public Entity Value;
}
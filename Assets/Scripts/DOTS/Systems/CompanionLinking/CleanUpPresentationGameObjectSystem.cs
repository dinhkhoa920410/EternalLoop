using Unity.Entities;

public partial struct CleanUpPresentationGameObjectSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<BeginInitializationEntityCommandBufferSystem.Singleton>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var ecbBis = SystemAPI
            .GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>()
            .CreateCommandBuffer(state.WorldUnmanaged);
        
        foreach (var (presentationGameObjectCleanUp, entity) in SystemAPI
                     .Query<PresentationGameObjectCleanUp>()
                     .WithEntityAccess())
        {
            if(presentationGameObjectCleanUp.Instance == null) continue;
            presentationGameObjectCleanUp.Instance.SetActive(false);
            
            // ecbBis.RemoveComponent<PresentationGameObjectCleanUp>(entity);
        }
    }
}
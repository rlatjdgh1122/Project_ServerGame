public interface INetworkObjectController
{

    public ulong GetOwner();
    public bool IsOwner();
    public bool IsServer();
    public bool IsClient();
    public void Despawn();
    
}

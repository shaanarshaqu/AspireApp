namespace WebApi2.Manager.Interface
{
    public interface IFlowTestManager
    {
        Task<bool> CalliningAuthorizedFlow();
    }
}

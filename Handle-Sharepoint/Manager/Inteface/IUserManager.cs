using Handle_Sharepoint.Data.Models;

namespace Handle_Sharepoint.Manager.Inteface
{
    public interface IUserManager
    {
        Task<List<User>> GetAllUsers();
        Task<bool> AddUser(User user);
    }
}

using JWTAPI.Models;

namespace JWTAPI.Interface
{
    public interface IUser
    {

        public List<UserInfo> GetUserDetails();
        public UserInfo GetUserDetails(int id);
        public void AddUser(UserInfo User);
        public void UpdateUser(UserInfo User);
        public UserInfo DeleteUser(int id);
        public bool CheckUser(int id);
        public void AddLogs(Logs Log);
    }
}

using JWTAPI.Interface;
using JWTAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace JWTAPI.Repository
{
    public class UserRepository : IUser
    {
        readonly DatabaseContext _dbContext = new();


        public UserRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<UserInfo> GetUserDetails()
        {
            try
            {
                return _dbContext.UserInfos.ToList();
            }
            catch
            {
                throw;
            }
        }

        public UserInfo GetUserDetails(int id)
        {
            try
            {
                UserInfo? users = _dbContext.UserInfos.Find(id);
                if (users != null)
                {
                    return users;
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch
            {
                throw;
            }
        }

        public void AddUser(UserInfo users)
        {
            try
            {
                _dbContext.UserInfos.Add(users);
                _dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public void AddLogs(Logs logs)
        {
            try
            {
                _dbContext.Loging.Add(logs);
                _dbContext.SaveChanges();

            }
            catch
            {
                throw;
            }
        }

        public void UpdateUser(UserInfo users)
        {
            try
            {
                _dbContext.Entry(users).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public UserInfo DeleteUser(int id)
        {
            try
            {
                UserInfo? users = _dbContext.UserInfos.Find(id);

                if (users != null)
                {
                    _dbContext.UserInfos.Remove(users);
                    _dbContext.SaveChanges();
                    return users;
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch
            {
                throw;
            }
        }

        public bool CheckUser(int id)
        {
            return _dbContext.UserInfos.Any(e => e.UserId == id);
        }
    }
}

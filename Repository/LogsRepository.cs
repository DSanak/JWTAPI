using JWTAPI.Interface;
using JWTAPI.Models;

namespace JWTAPI.Repository
{
    public class LogsRepository : ILogs
    {
        readonly DatabaseContext _dbContext = new();


        public LogsRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Logs> GetLogs()
        {
            try
            {
                return _dbContext.Loging.ToList();
            }
            catch
            {
                throw;
            }
        }

        public bool CheckLogs(int id)
        {
            return _dbContext.Loging.Any(e => e.userID == id);
        }
    }
}
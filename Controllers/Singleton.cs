namespace JWTAPI.Controllers
{
    public sealed class Singleton
    {
        private static Singleton instance = null;
        private static readonly object padlock = new object();
        public int saveIdusera;

        Singleton()
        {
            saveIdusera = 1;
        }

        public static Singleton Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Singleton();
                    }
                    return instance;
                }
            }
        }
    }
}

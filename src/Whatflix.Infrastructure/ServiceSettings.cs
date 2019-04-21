namespace Whatflix.Infrastructure
{
    public class ServiceSettings
    {
        public Connection Whatfix_Db { get; set; }
    }

    public class Connection
    {
        public string ConnectionString { get; set; }
        public string Name { get; set; }
    }
}

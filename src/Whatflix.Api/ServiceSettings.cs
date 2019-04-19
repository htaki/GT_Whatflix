namespace Whatflix.Api
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

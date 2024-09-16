namespace TestItemsWebAPI.Configurations
{
    public class DbOptions
    {
        public const string ConnectionSection = "ConnectionStrings";
        public const string DefaultSection = "DefaultConnection";
        public string DefaultConnection { get; set; }
    }
}

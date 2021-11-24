namespace flexGateway.Shared
{
    public class AdapterConfigurationModel
    {
        public string Name { get; set; }
        public string TypeFullName { get; set; }
        public string JsonConfiguration { get; set; }
        public bool IsSource { get; set; }
    }
}

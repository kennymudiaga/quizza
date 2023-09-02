namespace Quizza.Common.Web.Configuration
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class ConfigSectionNameAttribute : Attribute
    {
        public ConfigSectionNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}

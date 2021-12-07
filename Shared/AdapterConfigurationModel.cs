using System.ComponentModel.DataAnnotations;

namespace flexGateway.Shared
{
    public class AdapterConfigurationModel
    {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string TypeFullName { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string JsonConfiguration { get; set; }
        
        [Required]
        public bool IsSource { get; set; }
    }
}

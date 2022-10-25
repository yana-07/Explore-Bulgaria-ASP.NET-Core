using System.ComponentModel.DataAnnotations;

namespace ExploreBulgaria.Data.Common.Models
{
    public class BaseModel<Tkey> : IAuditInfo
    {
        [Key]
        public Tkey Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace LogisticsSystem.API.Context
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string? Tag { get; set; }
        public DateTime InsertDate { get; set; } = DateTime.Now;
    }
}

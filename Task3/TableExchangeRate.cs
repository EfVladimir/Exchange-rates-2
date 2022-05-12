namespace Task3
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TableExchangeRate")]
    public partial class TableExchangeRate
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public double Value { get; set; }
    }
}

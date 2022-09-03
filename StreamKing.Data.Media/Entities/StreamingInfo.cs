using System.ComponentModel.DataAnnotations;

namespace StreamKing.Data.Media
{
    public class StreamingInfo
    {
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Country { get; set; }

    }
}

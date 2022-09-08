using System.ComponentModel.DataAnnotations;

namespace Whyrule.Models
{
    public class NewsPost
    {
        public int Id { get; set; }
        public string Title { get; set; }

        [DataType(DataType.Date)]
        public DateTime PostDate { get; set; }
        public string Content { get; set; }
        public string Tags { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TasksAPI.Models
{
    public class Tasks<T>
    {
        
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MinLength(5, ErrorMessage ="El mensaje debe de ser mayor de 5 letras")]
        public string Description { get; set; }
        
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
        public T AdditionalData { get; set; }

    }
}

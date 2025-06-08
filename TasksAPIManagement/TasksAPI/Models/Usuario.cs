using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TasksAPI.Models
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Nombre { get; set; }
        [EmailAddress(ErrorMessage = "El email debe de tener un formato correcto")]
        public string Email { get; set; }
        [MinLength(10)]
        public string password { get; set; }
        [Required]

        public DateTime FechaNacimiento { get; set; }





    }
}

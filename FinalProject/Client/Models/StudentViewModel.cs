using System.ComponentModel.DataAnnotations;

namespace Client.Models
{
    public class StudentViewModel
    {
        [Required(ErrorMessage = "Student is empty")]
        public Student Student { get; set; } = null!;
        [Required(ErrorMessage = "Classes is empty")]
        public List<Class> Classes { get; set; } = null!;
    }
}

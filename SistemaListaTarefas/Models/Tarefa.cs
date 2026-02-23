using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaListaTarefas.Models
{
    public class Tarefa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 

        [Required(ErrorMessage = "O nome da tarefa é obrigatório.")]
        [Display(Name = "Nome da Tarefa")]
        public string Nome { get; set; } 

        [Required(ErrorMessage = "O custo é obrigatório.")]
        [Range(0, double.MaxValue, ErrorMessage = "O custo deve ser maior ou igual a zero.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Custo { get; set; } 

        [Required(ErrorMessage = "A data-limite é obrigatória.")]
        [Display(Name = "Data Limite")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataLimite { get; set; } 

        [Required]
        public int Ordem { get; set; } 
    }
}
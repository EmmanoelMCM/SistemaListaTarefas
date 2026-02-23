using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaListaTarefas.Models
{
    public class Tarefa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Identificador (Chave Primária) [cite: 10]

        [Required(ErrorMessage = "O nome da tarefa é obrigatório.")]
        [Display(Name = "Nome da Tarefa")]
        public string Nome { get; set; } // Não permite duplicidade [cite: 11]

        [Required(ErrorMessage = "O custo é obrigatório.")]
        [Range(0, double.MaxValue, ErrorMessage = "O custo deve ser maior ou igual a zero.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Custo { get; set; } // Valor fracionário >= 0 [cite: 12]

        [Required(ErrorMessage = "A data-limite é obrigatória.")]
        [Display(Name = "Data Limite")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataLimite { get; set; } // Exibição sempre em DD/MM/AAAA [cite: 13]

        [Required]
        public int Ordem { get; set; } // Campo numérico para ordenação [cite: 14]
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ClientAdmin.Models.Validators;

namespace ClientAdmin.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required, StringLength(150)]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = string.Empty;

        [Required, Display(Name = "CPF"), Cpf]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Informe 11 dígitos.")]
        public string CPF { get; set; } = string.Empty;

        [Required, DataType(DataType.Date)]
        [Display(Name = "Data de Nascimento")]
        public DateTime DataNascimento { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data de Cadastro")]
        public DateTime DataCadastro { get; set; } = DateTime.Today;

        [Range(0, double.MaxValue, ErrorMessage = "A renda deve ser >= 0")]
        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Renda Familiar")]
        public decimal? RendaFamiliar { get; set; }

        [NotMapped]
        public string ClasseRenda =>
            !RendaFamiliar.HasValue ? "" :
            RendaFamiliar.Value <= 980m ? "A" :
            RendaFamiliar.Value <= 2500m ? "B" : "C";
            
    }
}

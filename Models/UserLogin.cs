using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace GestApp_API.Models
{
    public class UserLogin
    {
        [Key] // Define Id como chave primária
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo Nome é obrigatório")] // Nome é obrigatório
        public string Nome { get; set; }
        [Required] // Login é obrigatório e Garante que o Login seja único
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        [Timestamp]
        public DateTime CreateDate { get; set; }
        [Timestamp]
        public DateTime UpdateDate { get; set; }
    }
}

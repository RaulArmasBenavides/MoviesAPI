﻿using System.ComponentModel.DataAnnotations;

namespace ApiMovies.Core.Entities
{
    public class Categoria
    {
        [Key]
        public int  Id { get; set; }

        [Required]
        public string Nombre { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}

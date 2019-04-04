﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication.Core.Domain.Entities;

namespace WebApplication.Core.Domain
{
    public class Voivodeship : EntityInt
    {
        // Pola
        [Required] [Column(TypeName = "nvarchar(30)")]
        public string Name { get; protected set; }

        // Konstruktory
        public Voivodeship(string Name)
        {
            Id = null;
            this.Name = Name;
        }
    }
}
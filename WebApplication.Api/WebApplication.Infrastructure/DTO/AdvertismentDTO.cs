﻿using System;

namespace WebApplication.Infrastructure.DTO
{
    public class AdvertismentDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public decimal Price { get; set; }
        public decimal Size { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }

        public ImageDTO Image { get; set; }
    }
}
﻿namespace WebApplication.Infrastructure.Commands
{
    public class CreateAdv
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public int City { get; set; }
        public string Street { get; set; }
        public float Size { get; set; }
        public string Category { get; set; }
        public int? Floor { get; set; }
    }
}

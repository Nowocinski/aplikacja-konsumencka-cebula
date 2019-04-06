using System;

namespace WebApplication.Infrastructure.Commands
{
    public class ImageToAdv
    {
        public Guid Advertisement { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

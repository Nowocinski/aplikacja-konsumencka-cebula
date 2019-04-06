using System.Collections.Generic;

namespace WebApplication.Infrastructure.DTO
{
    public class AdvertisementsWithPageToEndDTO
    {
        public IEnumerable<AdvertismentDTO> Advertisement { get; set; }
        public int PageToEnd { get; set; }
    }
}

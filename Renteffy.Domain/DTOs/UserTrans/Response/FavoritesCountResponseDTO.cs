using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.UserTrans.Response
{
    public class FavoritesCountResponseDTO
    {
        public int PostId { get; set; }
        public int FavoritesCount { get; set; }
        public int IsFavorite { get; set; }
    }
}

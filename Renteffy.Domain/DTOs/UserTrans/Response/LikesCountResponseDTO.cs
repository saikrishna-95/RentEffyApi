using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.UserTrans.Response
{
    public class LikesCountResponseDTO
    {
        public int PostId { get; set; }
        public int LikesCount { get; set; }
        public int IsLiked { get; set; }
    }
}

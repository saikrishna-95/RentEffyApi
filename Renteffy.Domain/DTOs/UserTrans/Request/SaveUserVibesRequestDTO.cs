using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.UserTrans.Request
{
    public class SaveUserVibesRequestDTO
    {
        public int UserId { get; set; }
        public List<int> VibeIds { get; set; } = new List<int>();
    }
}

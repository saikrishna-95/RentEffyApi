using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.UserTrans.Response
{
    public class ReceiptResultDto
    {
        public string LocalPath { get; set; } = default!;
        public string CloudUrl { get; set; } = default!;
    }
}

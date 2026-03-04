using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.UserTrans.Response
{
    public class MasterDataResponseDTO
    {
        public List<AmenitesResponseDto> Amenities { get; set; }
        public List<BedTypeResponseDto> BedTypes { get; set; }
        public List<CategoryResponseDto> Categories { get; set; }
        public List<FloorResponseDto> Floors { get; set; }
        public List<FoodResponseDto> FoodTypes { get; set; }
        public List<PgTypeResponseDto> PgTypes { get; set; }
        public List<RoomResponseDto> Rooms { get; set; }
        public List<StayingPeriodResponseDto> StayingPeriods { get; set; }
    }
}

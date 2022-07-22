using System;
using System.Collections.Generic;
using System.Text;

namespace DealershipManagement.DataTransferObjects.Common
{
    public class ListRequestDto
    {
        public int Offset { get; set; } = 0;
        public string SearchBy { get; set; }
    }
}

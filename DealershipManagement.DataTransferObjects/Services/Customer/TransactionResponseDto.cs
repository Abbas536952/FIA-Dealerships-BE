using System;
using System.Collections.Generic;
using System.Text;

namespace DealershipManagement.DataTransferObjects.Services.Customer
{
    public class TransactionResponseDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int VehicleId { get; set; }
        public string VehicleDetails { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime Date { get; set; }
    }
}

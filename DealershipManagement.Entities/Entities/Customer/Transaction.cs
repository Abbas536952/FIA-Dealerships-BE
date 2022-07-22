using System;
using System.Collections.Generic;
using System.Text;
using Template.Repository.Entities;

namespace DealershipManagement.Entities.Customer
{
    public class Transaction : AuditableDeletableEntity
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int VehicleId { get; set; }
        public decimal TotalCost { get; set; }
        public Customer Customer { get; set; }
        public Entities.Vehicle.Vehicle Vehicle { get; set; }
    }
}

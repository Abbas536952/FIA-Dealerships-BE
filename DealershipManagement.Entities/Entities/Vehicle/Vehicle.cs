using DealershipManagement.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Template.Repository.Entities;

namespace DealershipManagement.Entities.Vehicle
{
    public class Vehicle : AuditableDeletableEntity
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string VIN { get; set; }
        public int Year { get; set; }
        public decimal Mileage { get; set; }
        public DateTime? SalesDate { get; set; }
        public VehicleStatus Status { get; set; }
        public string ViewableLink { get; set; }
        public decimal Cost { get; set; }
        public string Description { get; set; }
        public TransmissionType TransmissionType { get; set; }
        public string Registration { get; set; }
        public int ExteriorReport { get; set; }
        public int InteriorReport { get; set; }
        public int TransmissionReport { get; set; }
        public int ACReport { get; set; }
        public int EngineReport { get; set; }
        public int SuspensionReport { get; set; }
        public Customer.Customer Customer { get; set; }
    }
}

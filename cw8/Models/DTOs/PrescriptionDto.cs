using System;
using System.Collections.Generic;

namespace cw8.Models.DTOs
{
    public class PrescriptionDto{
        public DateTime Date {get; set;}
        public DateTime DueDate {get; set;}
        public Doctor Doctor {get; set;}
        public Patient Patient {get; set;}
        public List<MedicamentDto> Medicaments {get; set;}
    }
}
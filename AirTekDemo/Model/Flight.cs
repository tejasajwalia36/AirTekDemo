using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirTekDemo.Model
{
    public class Flight
    {
        public int FlightId { get; set; }
        public string DepartureCode { get; set; }
        public string DepartureDetail { get; set; }
        public string ArrivalCode { get; set; }
        public string ArrivalDetail { get; set; }
        public int Day { get; set; }
        public int OrderCapacity { get; set; }
        public bool HasOrderCapacity => this.OrderCapacity > 0;
    }
}

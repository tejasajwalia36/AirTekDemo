using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirTekDemo.Model
{
    public class Orders
    {
        public Dictionary<string, OrderDetail> OrderList { get; set; }
    }
    public class OrderDetail
    {
        public string? Destination { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaltyBus.Models
{
    public class Bus
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public int RouteNumber { get; set; }
        public bool Arrived { get; set; }
        public DateTime Scheduled { get; set; }
        public DateTime Estimated { get; set; }

        public int LateBy { get; set; }
      //  public ICollection<Bet> Bets { get; set; }
    }
}

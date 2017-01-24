using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SaltyBus.Models;

namespace SaltyBus.Models
{
    public class SaltyRepository : ISaltyRepository
    {
        private ILogger<SaltyRepository> _logger;
        private SaltyContext _context;

        public SaltyRepository(SaltyContext context, ILogger<SaltyRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public IEnumerable<Bus> GetAllBusses()
        {
            return _context.Buses.ToList();
        }

        public void AddBus(Bus bus)
        {
            _context.Add(bus);
        }

        public void UpdateBus(Bus bus)
        {
            var foundBus = _context.Buses.FirstOrDefault(s => s.Key == bus.Key);
            if (foundBus != null && foundBus.Scheduled < bus.Scheduled)
            {
                bus.Id = foundBus.Id;
                DateTime centralNow = TimeZoneInfo.ConvertTime(System.DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"));


                if (bus.Estimated > centralNow)
                {
                   var delta = bus.Estimated - bus.Scheduled;
                    bus.LateBy = delta.Minutes;
                }
                else
                {
                    bus.Arrived = true;
                }
                    foundBus = bus;
                _context.Entry(foundBus).State = EntityState.Modified;
            }
            else
            {
                AddBus(bus);
            }

        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}

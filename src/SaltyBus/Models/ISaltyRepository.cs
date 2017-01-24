using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaltyBus.Models
{
    public interface ISaltyRepository
    {
        IEnumerable<Bus> GetAllBusses();
        //IEnumerable<Bus> Get

        void AddBus(Bus bus);

        void UpdateBus(Bus bus);
        Task<bool> SaveChangesAsync();
    }
}

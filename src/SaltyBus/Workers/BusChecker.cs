using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using SaltyBus.Models;

namespace WebApplication2.Workers
{
    public class BusChecker
    {
        private ISaltyRepository _repo;
        private string _apiKey;
        private ILogger<BusChecker> _logger;

        public BusChecker(ISaltyRepository repository, ILogger<BusChecker> logger )
        {
            _repo = repository;
            _apiKey = Environment.GetEnvironmentVariable("winnipegTransitApiKey");
            _logger = logger;
        }

        public async Task<string> GetBuses()
        {
            string centralZoneId = "Central Standard Time";
            TimeZoneInfo centralZone = TimeZoneInfo.FindSystemTimeZoneById(centralZoneId);
            var url = "http://api.winnipegtransit.com/v2/stops/10582/schedule.json?api-key=" + _apiKey;
            using (var client = new HttpClient())
            {
                var response =
                    await client.GetStringAsync(url);

                if (!string.IsNullOrWhiteSpace(response))
                {
                    var json = JObject.Parse(response);
                    var allRoutes = json["stop-schedule"]["route-schedules"];
                    foreach (var route in allRoutes)
                    {
                        var nextBusses = route["scheduled-stops"];
                        foreach (var bus in nextBusses)
                        {
                            var newEstimated =
                                 TimeZoneInfo.ConvertTime(DateTime.Parse(bus["times"]["arrival"]["estimated"].ToString()), centralZone);
                            var newScheduled =
                                 TimeZoneInfo.ConvertTime(DateTime.Parse(bus["times"]["arrival"]["scheduled"].ToString()), centralZone);


                            Bus newBus = new Bus()
                            {
                                Estimated = newEstimated,
                                Scheduled = newScheduled,
                                Key = bus["key"].ToString(),
                                RouteNumber = (int)route["route"]["number"],
                               
                            };
                            
                            _repo.UpdateBus(newBus);
                        }
                    }
                    await _repo.SaveChangesAsync();
                }
                return response;
            }
           
        }
       

    }
}

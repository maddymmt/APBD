using System.Collections.Generic;
using System.Threading.Tasks;
using cw7.Models.DTOs;

namespace cw7.Services
{
    public interface ITripService
    {
        public Task<IEnumerable<TripDto>> GetTrips();
        public Task DeleteClient(int id);
        public Task AddClient(Models.DTOs.Client dto);
        public Task AddTripToClient(ClientTripDto dto);

    }
}
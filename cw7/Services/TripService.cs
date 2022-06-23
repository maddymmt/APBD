using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cw7.Models;
using cw7.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Country = cw7.Models.DTOs.Country;

namespace cw7.Services
{
    public class TripService : ITripService
    {
        private readonly masterContext _context;

        public TripService(masterContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TripDto>> GetTrips()
        {
            return await _context.Trips.OrderBy(e => e.DateFrom).Select(e => new TripDto
            {
                Name = e.Name,
                Description = e.Description,
                DateFrom = e.DateFrom,
                DateTo = e.DateTo,
                MaxPeople = e.MaxPeople,
                Countries = e.CountryTrips.Select(e => new Country
                {
                    Name = e.IdCountryNavigation.Name
                }).ToList(),
                Clients = e.ClientTrips.Select(e => new Models.DTOs.Client
                {
                    FirstName = e.IdClientNavigation.FirstName,
                    LastName = e.IdClientNavigation.LastName
                }).ToList()
            }).ToListAsync();
        }

        public async Task DeleteClient(int id)
        {
            //Końcówka powinna najpierw sprawdzić czy klient nie posiada żadnych
            // przypisanych wycieczek. Jeśli klient posiada co najmniej jedną przypisaną
            // wycieczkę – zwracamy błąd i usunięcie nie dochodzi do skutku

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                throw new Exception("Nie ma takiego klienta");
            }

            var isInTrip = await _context.ClientTrips.AnyAsync(e => e.IdClient == client.IdClient);
            if (isInTrip)
            {
                throw new Exception("Klient posiada przypisane wycieczki");
            }


            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();


            // var client = new Models.Client
            // {
            //     IdClient = id
            // };

            // var entry = _context.Entry(client);
            // _context.Remove(entry);
            // entry.State = EntityState.Deleted;
            // await _context.SaveChangesAsync();
        }


        public async Task AddClient(Models.DTOs.Client dto)
        {
            var client = new Models.Client
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
        }

        public async Task AddTripToClient(ClientTripDto dto)
        {
            var trip = await _context.Trips.FindAsync(dto.IdTrip);
            if (trip == null)
            {
                throw new Exception("Nie ma takiej wycieczki");
            }

            var pesel = await _context.Clients.FindAsync(dto.Pesel);
            if (pesel == null)
            {
                var client1 = new Models.Client()
                {
                    IdClient = _context.Clients.Max(cl => cl.IdClient) + 1,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    Telephone = dto.Telephone,
                    Pesel = dto.Pesel
                };
                _context.Clients.Add(client1);
                // await _context.SaveChangesAsync();

                var client = _context.Clients.FirstOrDefault(c => c.Pesel == dto.Pesel);
                if (_context.ClientTrips.FirstOrDefault(c => c.IdClient == client.IdClient && c.IdTrip == dto.IdTrip) !=
                    null)
                {
                    throw new Exception("Klient już posiada tą wycieczkę");
                }

                var clientTrip = _context.ClientTrips.FirstOrDefault(clt => clt.IdTrip == trip.IdTrip);
                if (clientTrip == null)
                {
                    clientTrip = new ClientTrip()
                    {
                        IdTrip = trip.IdTrip,
                        PaymentDate = DateTime.Now
                    };
                    _context.ClientTrips.Add(clientTrip);
                }
            }
        }
        
        // public async Task AddCountry(Models.DTOs.Country dto)
        // {
        //     var country = new Models.Country
        //     {
        //         Name = dto.Name
        //     };
        //     await _context.AddAsync(country);
        //     await _context.SaveChangesAsync();
        //
        //
        //     var client = await _context.Clients.Where(e => e.Pesel == "dto.Pesel").FirstOrDefaultAsync();
        //     if (client is null)
        //     {
        //     }
        //
        //     var isInTrip = await _context.ClientTrips.AnyAsync(e => e.IdClient == client.IdClient);
        // }
    }
}
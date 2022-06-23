using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using cw4.Models;
using Microsoft.Extensions.Configuration;

namespace cw4.Services
{
    public class AnimalService : IAnimalService
    {
        private readonly IConfiguration _configuration;


        public AnimalService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int AddAnimal(Animal animal)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText =
                    $"insert into animal (idanimal, name, description, category, area) values (@1, @2, @3, @4, @5)";
                command.Parameters.AddWithValue("@1", animal.IdAnimal);
                command.Parameters.AddWithValue("@2", animal.Name);
                command.Parameters.AddWithValue("@3", animal.Description);
                command.Parameters.AddWithValue("@4", animal.Category);
                command.Parameters.AddWithValue("@5", animal.Area);
                connection.Open();

                return command.ExecuteNonQuery();
            }
        }

        public int UpdateAnimal(Animal animal, int IdAnimal)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText =
                    $"update animal set name = @2, description = @3, category = @4, area = @5 where idanimal = @1";
                command.Parameters.AddWithValue("@1", IdAnimal);
                command.Parameters.AddWithValue("@2", animal.Name);
                command.Parameters.AddWithValue("@3", animal.Description);
                command.Parameters.AddWithValue("@4", animal.Category);
                command.Parameters.AddWithValue("@5", animal.Area);
                connection.Open();

                return command.ExecuteNonQuery();
            }
        }

        public int DeleteAnimal(Animal animal, int IdAnimal)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText =
                    $"delete from animal where idanimal = @1";
                command.Parameters.AddWithValue("@1", IdAnimal);
                connection.Open();

                return command.ExecuteNonQuery();
            }
        }

        public List<Animal> GetAnimals(string orderBy)
        {
            var result = new List<Animal>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = $"select * from animal order by {orderBy}";
                connection.Open();

                var dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    var animal = new Animal
                    {
                        IdAnimal = int.Parse(dataReader["idanimal"].ToString()),
                        Name = dataReader["name"].ToString(),
                        Description = dataReader["description"].ToString(),
                        Area = dataReader["area"].ToString()
                    };
                    result.Add(animal);
                }
            }
            return result;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cw4.Models;

namespace cw4.Models
{
    public class Animal
    {
        public int IdAnimal {get; set;}
        public string Name {get; set;}
        public string? Description {get; set;}
        public string Category {get; set;}

        public string Area {get; set;}
    }
}
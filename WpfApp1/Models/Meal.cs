﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    class Meal
    {
        public int Id { get; set; }
        public int DatesId { get; set; }
        public string Name { get; set; }
        public int Week { get; set; }
        public string Day { get; set; }

        public string Fullinformation
        {
            get
            {

                return $"{Week} {Day} {Name}";

            }
        }


    }
}

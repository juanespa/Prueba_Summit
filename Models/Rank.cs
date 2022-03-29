using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prueba_1.Models
{
    public class Rank
    {
        public string rank { get; set; }
        public string item { get; set; }
        public string repo_name { get; set; }
        public string stars { get; set; }

    }

    public class resp
    {
        public int cantidad { get; set; }
        public bool orden { get; set; }
        public string search { get; set; }
        public int Value { get; set; }
    }
}
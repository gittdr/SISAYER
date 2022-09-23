using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.Models
{
    interface IBebidaAlcoholica
    {
        int Alcohol { get; set; }
        void MaxRecomendado();
    }
}

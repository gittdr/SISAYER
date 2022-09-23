using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.Models
{
    internal class Cerveza : Bebidas,IBebidaAlcoholica
    {
        public int Alcohol { get; set; }
        public Cerveza(int Cantidad, string Nombre="Cerveza") : base(Nombre, Cantidad)
        {

        }

        public void MaxRecomendado()
        {
            Console.WriteLine("El maximo son 30");
        }
    }
}

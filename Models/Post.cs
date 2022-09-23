using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.Models
{
    public class Post
    {
        public int userId { get; set; }
        public string id { get; set; }
        public string title { get; set; }
        public string body { get; set; }

        public string nombre { get; set; }
        public string serie { get; set; }

        public string idSucursal { get; set; }

        public Boolean nomina { get; set; }

        public Boolean produccion { get; set; }

        public string NOMBRE { get; set; }

        public string TIPOSUCURSAL { get; set; }

        public string motivo { get; set; }

        public List<string> uuid { get; set; }

        public string status { get; set; }

        public string descripcion { get; set; }

        
    }
}

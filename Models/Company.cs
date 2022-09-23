using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.Models
{
    public class Company
    {
        public Company(String ID, String Dir1, String Dir2, String Dir3, String cp, String Ciudad, String lat, String lng)
        {
            this.ID = ID;
            this.Dir1 = Dir1;
            this.Dir2 = Dir2;
            this.Dir3 = Dir3;
            this.cp = cp;
            this.Ciudad = Ciudad;
            this.lat = lat;
            this.lng = lng;
        }

        public String ID { get; set; }
        public String Dir1 { get; set; }
        public String Dir2 { get; set; }
        public String Dir3 { get; set; }
        public String cp { get; set; }
        public String Ciudad { get; set; }
        public String lat { get; set; }
        public String lng { get; set; }
    }




    public class ErroresCP
    {

        public ErroresCP(String Fecha, String Folio, String Erro1, String Erro2, String Erro3, String Erro4, String Erro5, String Erro6, String Erro7)
        {
            this.Fecha = Fecha;
            this.Folio = Folio;
            this.Erro1 = Erro1;
            this.Erro2 = Erro2;
            this.Erro3 = Erro3;
            this.Erro4 = Erro4;
            this.Erro5 = Erro5;
            this.Erro6 = Erro6;
            this.Erro7 = Erro7;
        }

        public String Fecha { get; set; }
        public String Folio { get; set; }
        public String Erro1 { get; set; }
        public String Erro2 { get; set; }
        public String Erro3 { get; set; }
        public String Erro4 { get; set; }
        public String Erro5 { get; set; }
        public String Erro6 { get; set; }
        public String Erro7 { get; set; }

    }


    public class CartaPorterest
    {

        public CartaPorterest(String Folio, String Serie, String UUID, String Pdf_xml_descarga, String Pdf_descargaFactura, String xlm_descargaFactura, String cancelFactura, String LegNum, String Fecha, String Total, String Moneda, String RFC, String Origen, String Destino)
        {
            this.Folio = Folio;
            this.Serie = Serie;
            this.UUID = UUID;
            this.Pdf_xml_descarga = Pdf_xml_descarga;
            this.Pdf_descargaFactura = Pdf_descargaFactura;
            this.xlm_descargaFactura = xlm_descargaFactura;
            this.cancelFactura = cancelFactura;
            this.LegNum = LegNum;
            this.Fecha = Fecha;
            this.Total = Total;
            this.Moneda = Moneda;
            this.RFC = RFC;
            this.Origen = Origen;
            this.Destino = Destino;
        }

        public String Serie { get; set; }
        public String Folio { get; set; }
        public String UUID { get; set; }
        public String Pdf_xml_descarga { get; set; }
        public String Pdf_descargaFactura { get; set; }
        public String xlm_descargaFactura { get; set; }
        public String cancelFactura { get; set; }
        public String LegNum { get; set; }
        public String Fecha { get; set; }

        public String Total { get; set; }
        public String Moneda { get; set; }
        public String RFC { get; set; }
        public String Origen { get; set; }
        public String Destino { get; set; }

    }



}

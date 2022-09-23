using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ConsoleApp2.Models
{
    public class Functions
    {
        public static HttpPostedFile archivoenv;
        public Functions()
        {
            //
            // TODO: Agregar aquí la lógica del constructor
            //
        }
        public string cambioFormatoFecha(string fecha)
        {
            string[] split = fecha.Split('/');
            if (split.Count() > 0 && split.Count() == 3)
            {
                return split[2] + "-" + split[1] + "-" + split[0];
            }
            return "";
        }
        public string EncodeTo64(string text)
        {
            byte[] toEncodeAsBytes
                  = System.Text.ASCIIEncoding.ASCII.GetBytes(text);
            string returnValue
                  = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }
        public string DecodeFrom64(string text)
        {

            byte[] encodedDataAsBytes
                = System.Convert.FromBase64String(text);
            string returnValue =
               System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);
            return returnValue;
        }

        public string archivos(HttpPostedFile archivo)
        {
            string filearchivo = archivo.FileName.ToString();
            archivoenv = archivo;
            return filearchivo;
        }

        public string CreateRandomPassword()
        {
            string _allowedChars = "abcdefghijkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ23456789!@$?";
            Byte[] randomBytes = new Byte[10];
            char[] chars = new char[10];
            int allowedCharCount = _allowedChars.Length;

            for (int i = 0; i < 10; i++)
            {
                Random randomObj = new Random();
                randomObj.NextBytes(randomBytes);
                chars[i] = _allowedChars[(int)randomBytes[i] % allowedCharCount];
            }

            return new string(chars);
        }

        public void EnvioMailserver(string cuerpo, string correo)
        {
            MailMessage correos = new MailMessage();
            SmtpClient smtp = new SmtpClient();

            correos.From = new MailAddress("correoremitente", "asunto", System.Text.Encoding.UTF8);
            correos.To.Add(correo);
            //correos.To.Add("soporte@laintegral.com.mx");
            correos.To.Add("correo que recibe");
            correos.SubjectEncoding = System.Text.Encoding.UTF8;
            correos.Subject = "asunto";

            correos.Body = "<B>" + cuerpo + "</B>";
            correos.BodyEncoding = System.Text.Encoding.UTF8;
            correos.IsBodyHtml = true;
            correos.Priority = MailPriority.High; //>> prioridad

            smtp.Credentials = new System.Net.NetworkCredential("correoremitente", "password");
            smtp.Port = 25;
            smtp.Host = "132.140.160.13243";
            smtp.EnableSsl = false;

            smtp.Send(correos);
        }

        public void EnvioGMail(string cuerpo, string correo, string asunto)
        {
            try
            {
                MailMessage correos = new MailMessage();
                SmtpClient smtp = new SmtpClient();

                correos.From = new MailAddress(correo, "GALAN MEN", System.Text.Encoding.UTF8);
                correos.To.Add(correo);
                //correos.To.Add(correo2);
                //correos.To.Add(correo);
                correos.SubjectEncoding = System.Text.Encoding.UTF8;
                correos.Subject = asunto;

                correos.Body = "<B>" + cuerpo + "</B>";
                correos.BodyEncoding = System.Text.Encoding.UTF8;
                correos.IsBodyHtml = true;
                correos.Priority = MailPriority.High; //>> prioridad

                smtp.Credentials = new System.Net.NetworkCredential("facturasumo@gmail.com", "164875xbox");
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;

                smtp.Send(correos);
            }
            catch { }
        }

        public string GetPublicIP()
        {
            try
            {
                string url = "http://checkip.dyndns.org";
                System.Net.WebRequest req = System.Net.WebRequest.Create(url);
                System.Net.WebResponse resp = req.GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                string response = sr.ReadToEnd().Trim();
                string[] a = response.Split(':');
                string a2 = a[1].Substring(1);
                string[] a3 = a2.Split('<');
                string a4 = a3[0];
                return a4;
            }
            catch { return "127.0.0.0"; }
        }

    }



}

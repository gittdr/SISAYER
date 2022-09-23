using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using tss = Tamir.SharpSsh;


namespace ConsoleApp2.Models
{
    public class UploadFile
    {
        //Properties
        //public const String host = "173.205.254.88";
        //public const String host = "200.33.10.53";

        public String host = null;
        public Int32 port = 0;
        public const String pwd = "tdr";





        public String user;
        //Int32 port = 22;


        //Constructor
        public UploadFile()
        {

            host = System.Web.Configuration.WebConfigurationManager.AppSettings["servidor"].ToString();
            port = Int32.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["port"].ToString());

        }

        //Methods

        public void modificaServer(string serie)
        {
            if (serie.Equals("TDRT"))
            {
                user = "usr_tdrt";
            }
            else if (serie.Equals("TDRA"))
            {
                user = "usr_nct";
            }
            else if (serie.Equals("TDRL"))
            {
                user = "usr_tdrl";
            }
            else if (serie.Equals("NCT"))
            {
                user = "usr_nct";
            }
            else if (serie.Equals("TDRLD"))
            {
                user = "usr_tdrl";
            }
        }

        public string prubeftp(string file, string path, string tipo)
        {

            string error = "";
            try
            {
                modificaServer(tipo);

                FileInfo toUpload = new FileInfo(path);
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(host + "/" + toUpload.Name);

                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(user, pwd);
                request.UsePassive = false;
                request.UseBinary = true;

                Stream ftpStream = request.GetRequestStream();
                FileStream files = File.OpenRead(path);

                int length = 1024;
                byte[] buffer = new byte[length];
                int bytesRead = 0;

                do
                {
                    bytesRead = files.Read(buffer, 0, length);
                    ftpStream.Write(buffer, 0, bytesRead);
                } while (bytesRead != 0);
                files.Close();
                ftpStream.Close();

            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error connecting to server");
                error = "FALLA AL SUBIR";
                throw ex;
            }
            //try
            //{

            //    modificaServer(tipo);

            //    FileInfo toUpload = new FileInfo(file);
            //    tss.SshTransferProtocolBase sftpClient;
            //    sftpClient = new tss.Scp(host, user);
            //    sftpClient.Password = pwd;
            //    //connect to server
            //    sftpClient.Connect(port);
            //    MessageBox.Show(" connecting to server success");


            //    //subir archivo

            //    sftpClient.Put(@"C:\Users\fidele gutember\"+ toUpload.Name, "/");

            //    //close connection
            //    sftpClient.Close();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Error connecting to server");

            //     throw ex;
            //}
            return error;
        }

        //Se suben varios facturas a la vez
        public void subeArchivo(ArrayList nct, ArrayList tdra, ArrayList tdrl, ArrayList tdrt, string path)
        {
            if (nct.Count != 0)
            {

                //string file, string path, string tipo
                modificaServer("NCT");

                //subir archivo

                for (int i = 0; tdrt.Count > i; i++)
                {


                    ArrayList archivo = ((ArrayList)tdrt[i]);
                    FileInfo toUpload = new FileInfo(archivo[0].ToString());
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(host + "/" + toUpload.Name);

                    request.Method = WebRequestMethods.Ftp.UploadFile;
                    request.Credentials = new NetworkCredential(user, pwd);
                    request.UsePassive = false;
                    request.UseBinary = true;
                    //ArrayList archivo = ((ArrayList)tdrt[i]);
                    Stream ftpStream = request.GetRequestStream();
                    FileStream files = File.OpenRead(archivo[0].ToString());

                    int length = 5048;
                    byte[] buffer = new byte[length];
                    int bytesRead = 0;
                    do
                    {
                        bytesRead = files.Read(buffer, 0, length);
                        ftpStream.Write(buffer, 0, bytesRead);
                    } while (bytesRead != 0);

                    files.Close();
                    ftpStream.Close();

                }


            }

            if (tdra.Count != 0)
            {

                //string file, string path, string tipo
                modificaServer("TDRA");

                //subir archivo
                for (int i = 0; tdrt.Count > i; i++)
                {


                    ArrayList archivo = ((ArrayList)tdrt[i]);
                    FileInfo toUpload = new FileInfo(archivo[0].ToString());
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(host + "/" + toUpload.Name);

                    request.Method = WebRequestMethods.Ftp.UploadFile;
                    request.Credentials = new NetworkCredential(user, pwd);
                    request.UsePassive = false;
                    request.UseBinary = true;
                    //ArrayList archivo = ((ArrayList)tdrt[i]);
                    Stream ftpStream = request.GetRequestStream();
                    FileStream files = File.OpenRead(archivo[0].ToString());

                    int length = 5048;
                    byte[] buffer = new byte[length];
                    int bytesRead = 0;
                    do
                    {
                        bytesRead = files.Read(buffer, 0, length);
                        ftpStream.Write(buffer, 0, bytesRead);
                    } while (bytesRead != 0);

                    files.Close();
                    ftpStream.Close();

                }


            }

            if (tdrl.Count != 0)
            {

                //string file, string path, string tipo
                modificaServer("TDRL");
                tss.SshTransferProtocolBase sftpClient;
                sftpClient = new tss.Sftp(host, user);
                sftpClient.Password = pwd;

                //connect to server
                sftpClient.Connect(port);

                //subir archivo
                for (int i = 0; i < tdrl.Count; i++)
                {
                    ArrayList archivo = ((ArrayList)tdrt[i]);
                    FileInfo toUpload = new FileInfo(archivo[0].ToString());
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(host + "/" + toUpload.Name);

                    request.Method = WebRequestMethods.Ftp.UploadFile;
                    request.Credentials = new NetworkCredential(user, pwd);
                    request.UsePassive = false;
                    request.UseBinary = true;
                    //ArrayList archivo = ((ArrayList)tdrt[i]);
                    Stream ftpStream = request.GetRequestStream();
                    FileStream files = File.OpenRead(archivo[0].ToString());

                    int length = 5048;
                    byte[] buffer = new byte[length];
                    int bytesRead = 0;
                    do
                    {
                        bytesRead = files.Read(buffer, 0, length);
                        ftpStream.Write(buffer, 0, bytesRead);
                    } while (bytesRead != 0);

                    files.Close();
                    ftpStream.Close();

                }


            }

            if (tdrt.Count != 0)
            {
                modificaServer("TDRT");
                //string file, string path, string tipo 
                for (int i = 0; tdrt.Count > i; i++)
                {


                    ArrayList archivo = ((ArrayList)tdrt[i]);
                    FileInfo toUpload = new FileInfo(archivo[0].ToString());
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(host + "/" + toUpload.Name);

                    request.Method = WebRequestMethods.Ftp.UploadFile;
                    request.Credentials = new NetworkCredential(user, pwd);
                    request.UsePassive = false;
                    request.UseBinary = true;
                    //ArrayList archivo = ((ArrayList)tdrt[i]);
                    Stream ftpStream = request.GetRequestStream();
                    FileStream files = File.OpenRead(archivo[0].ToString());

                    int length = 5048;
                    byte[] buffer = new byte[length];
                    int bytesRead = 0;
                    do
                    {
                        bytesRead = files.Read(buffer, 0, length);
                        ftpStream.Write(buffer, 0, bytesRead);
                    } while (bytesRead != 0);

                    files.Close();
                    ftpStream.Close();

                }


            }
        }


        public void subeArchivojr(ArrayList nct, ArrayList tdra, ArrayList tdrl, ArrayList tdrt)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://www.contoso/test.htm");
            request.Method = WebRequestMethods.Ftp.UploadFile;
            // this example assumes the ftp site uses anonymous logon.
            request.Credentials = new NetworkCredential("anonymous", "janeDoe@contoso.com");
            //copy the contents of the file to request stream.
            StreamReader sourceStream = new StreamReader("testfile.txt");
            byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
            sourceStream.Close();
            request.ContentLength = fileContents.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(fileContents, 0, fileContents.Length);
            requestStream.Close();

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Console.WriteLine("upload File Complete, status{0}", response.StatusDescription);

            response.Close();




        }

        //try
        //{
        //    modificaServer(tipo);



        //    String ftpurl = host + "/" + file; // e.g. ftp://serverip/foldername/foldername

        //    String ftpusername = user; // e.g. username
        //    String ftppassword = pwd; // e.g. password

        //    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(host);
        //    request.Method = WebRequestMethods.Ftp.UploadFile;
        //    request.Credentials = new NetworkCredential(ftpusername, ftppassword);
        //    request.UsePassive = true;
        //    request.UseBinary = true;
        //    request.KeepAlive = true;
        //    request.Proxy = null;
        //    //RUTA DONDE ESTA HUBICADO EL VIDEO
        //    FileStream stream = System.IO.File.OpenRead(path);
        //    byte[] buffer = new byte[stream.Length];
        //    stream.Read(buffer, 0, buffer.Length);
        //    stream.Close();
        //    Stream reqStream = request.GetRequestStream();
        //    reqStream.Write(buffer, 0, buffer.Length);
        //    reqStream.Flush();
        //    reqStream.Close();
        //}

        //public void uploadTxt(string file)
        //{
        //    try
        //    {
        //        JSch jsch = new JSch();
        //        //Iniciar sesión con el servidor
        //        Session session = jsch.getSession(user, host, 22);


        //        //Password
        //        session.setPassword(pwd);

        //        session.connect();
        //        int tiempo = session.getTimeout();

        //        Channel channel = session.openChannel("sftp");
        //        channel.connect();
        //        ChannelSftp c = (ChannelSftp)channel;

        //        c.put(root + file, file);
        //        session.disconnect();
        //    }
        //    catch (Exception e)
        //    {
        //        string error = e.ToString();
        //    }
        //}
        //public void Upload(string fileToUpload,string tipo)
        //{


        //        modificaServer(tipo);

        //         // e.g. ftp://serverip/foldername/foldername

        //        String ftpusername = user; // e.g. username
        //        String ftppassword = pwd; // e.g. password

        //        FileInfo toUpload = new FileInfo(fileToUpload);

        //        String ftpurl = ("ftp://10.176.163.68:1091" + "/" + toUpload.Name);

        //    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpurl);
        //        request.Method = WebRequestMethods.Ftp.UploadFile;
        //        request.Credentials = new NetworkCredential("usr_tdr", "tdr");
        //    request.UsePassive = false;
        //    request.UseBinary = true;
        //    request.KeepAlive = false;


        //    FileStream stream = File.OpenRead(fileToUpload);
        //    byte[] buffer = new byte[stream.Length];
        //    stream.Read(buffer, 0, buffer.Length);
        //    stream.Close();
        //    stream.Close();
        //    try
        //    {       
        //            System.IO.Stream reqstrm = request.GetRequestStream();
        //            reqstrm.Write(buffer, 0, buffer.Length);
        //            reqstrm.Close();
        //            MessageBox.Show("Upload successfully");

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }


        //}

    }

}

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
    public class ModelFact
    {
        private const string facturas = "select folio as Folio,fhemision as Fecha, nombrecliente as Cliente, idreceptor from VISTA_fe_Header";
        private const string facturasClientes = "select distinct  idreceptor from vista_fe_header";
        private const string facturasPorProcesar = "select * from VISTA_fe_Header where  idreceptor not in ('liverpol','GLOBALIV','LIVERTIJ','SFERALIV','FACTUMLV','LIVERDED')";
        private const string facturasPorProcesarLivepool = "select * from VISTA_fe_Header where idreceptor in ('liverpol','GLOBALIV','LIVERTIJ','SFERALIV','FACTUMLV','LIVERDED')";
        private const string facturaAdendaReferencia = "select ref_number, ref_type from referencenumber where ord_hdrnumber = @orden and (ref_type = 'ADEHOJ' or ref_type = 'ADEPED' or ref_type = 'LPROV')";
        private const string datosFactura = "select * from VISTA_fe_Header where folio = @factura";
        private const string detalle = "select * from vista_Fe_detail where folio = @factura";
        private const string detalle33 = "select * from vista_Fe_detail where folio = @factura";
        private const string invoice = "select ivh_invoicestatus,ivh_mbnumber,ivh_ref_number from invoiceheader where ivh_invoicenumber = @factura";
        private const string updateTrans = "update invoiceheader set ivh_ref_number = @idComprobante where ivh_invoicenumber = @fact";
        private const string updateTransMaster = "update invoiceheader set ivh_ref_number = @idComprobante where ivh_mbnumber  = @master";
        private const string insertaGeneradas = "insert into VISTA_Fe_generadas (nmaster,invoice,serie,idreceptor,fhemision,total,moneda,rutapdf,rutaxml,imaging,bandera,\r\n            provfact,status,ultinvoice,hechapor,orden,rfc) values (@master,@factura,@serie,@idreceptor,@fhemision,@total,@moneda,\r\n            @rutapdf,@rutaxml,@imaging,@bandera,@provfactura,@status,@ultinvoice,@hechapor,@orden,@rfc)";
        private const string parmFactura = "( select case when(select ivh_mbnumber from invoiceheader with (nolock) where ivh_invoicenumber = @factura) = 0 then @factura else (select max(ivh_invoicenumber) from invoiceheader with (nolock) where ivh_mbnumber = (select ivh_mbnumber from invoiceheader with (nolock) where ivh_mbnumber != 0 and ivh_invoicenumber = @factura)) end)";
        private const string masterFactura = "select * from vista_fe_header where ultinvoice = @parmFact";
        private const string minInvoice = "select invoice from vista_fe_header where ultinvoice = @parmFact";
        private const string P_fact = "@factura";
        private const string P_idComprobante = "@idComprobante";
        private const string P_master = "@master";
        private const string P_fact2 = "@fact";
        private const string P_pfact = "@parmFact";
        private const string P_invoice = "@lastInvoice";
        private const string P_serie = "@serie";
        private const string P_idReceptor = "@idreceptor";
        private const string P_fhemision = "@fhemision";
        private const string P_total = "@total";
        private const string P_moneda = "@moneda";
        private const string P_rutaPdf = "@rutapdf";
        private const string P_rutaXML = "@rutaxml";
        private const string P_imaging = "@imaging";
        private const string P_bandera = "@bandera";
        private const string P_provFact = "@provfactura";
        private const string P_status = "@status";
        private const string P_ultinvoice = "@ultinvoice";
        private const string P_hechapor = "@hechapor";
        private const string P_orden = "@orden";
        private const string P_rfc = "@rfc";
        private string _ConnectionString;

        public ModelFact()
        {
            this._ConnectionString = new Connection().connectionString;
        }

        public DataTable getFacturas()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("select folio as Folio,fhemision as Fecha, nombrecliente as Cliente, idreceptor from VISTA_fe_Header", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.CommandTimeout = 1000;
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }
        public DataTable getLeg()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("SELECT segmento FROM segmentosportimbrar_JR WHERE estatus = 1 and billto = 'SAYER' or estatus = 1 and billto = 'SAYFUL'", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.CommandTimeout = 1000;
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }
        public DataTable SelectLegHeader(string orseg)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                connection.Open();
                using (SqlCommand selectCommand = new SqlCommand("sp_obtener_order_header", connection))
                {

                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandTimeout = 100000;
                    selectCommand.Parameters.AddWithValue("@leg", (object)orseg);
                    selectCommand.ExecuteNonQuery();
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            //selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            connection.Close();
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }
        public DataTable UpdateOrderHeader(string orheader, string fecha)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                connection.Open();
                using (SqlCommand selectCommand = new SqlCommand("sp_update_orderheader", connection))
                {

                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandTimeout = 100000;
                    selectCommand.Parameters.AddWithValue("@orheader", (object)orheader);
                    selectCommand.Parameters.AddWithValue("@fecha", (object)fecha);
                    selectCommand.ExecuteNonQuery();
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            //selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            connection.Close();
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }
        public void OrderHeader(string leg, string rfecha)
        {
            string cadena2 = @"Data source=172.24.16.112; Initial Catalog=TMWSuite; User ID=sa; Password=tdr9312;Trusted_Connection=false;MultipleActiveResultSets=true";
            //DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(cadena2))
            {

                using (SqlCommand selectCommand = new SqlCommand("sp_Order_Header_JC", connection))
                {

                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandTimeout = 100121220;
                    selectCommand.Parameters.AddWithValue("@leg", leg);
                    selectCommand.Parameters.AddWithValue("@fecha", rfecha);
                    try
                    {
                        connection.Open();
                        selectCommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        string message = ex.Message;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

        }
        public DataTable UpdateLeg(string leg, string tipom)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                connection.Open();
                using (SqlCommand selectCommand = new SqlCommand("sp_obtener_segmento_actualizado", connection))
                {
                    
                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandTimeout = 1000;
                    selectCommand.Parameters.AddWithValue("@leg", (object)leg);
                    selectCommand.Parameters.AddWithValue("@tipom", (object)tipom);
                    selectCommand.ExecuteNonQuery();
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            //selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            connection.Close();
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }
        public DataTable GetSegmentoRepetido(string leg)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                connection.Open();
                using (SqlCommand selectCommand = new SqlCommand("sp_obtener_segmento_repetido", connection))
                {

                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandTimeout = 1000;
                    selectCommand.Parameters.AddWithValue("@leg", (object)leg);
                    selectCommand.ExecuteNonQuery();
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            //selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            connection.Close();
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }
        public DataTable getFacturasClientes()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("select distinct  idreceptor from vista_fe_header", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.CommandTimeout = 1000;
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }

        public DataTable getFacturasGeneradas()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("select * from vista_fe_generadas where fhemision >'2019-01-01' and rutapdf not like '%:9050%' order by 5 desc", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.CommandTimeout = 1000;
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }

        public DataTable getFacturasPorProcesar(string billto)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("select * from VISTA_fe_Header where idreceptor = @idreceptor and  idreceptor not in ('liverpol','GLOBALIV','LIVERTIJ','SFERALIV','FACTUMLV','MERCANLV','LIVERDED')", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.CommandTimeout = 1000;
                    selectCommand.Parameters.AddWithValue("@idreceptor", (object)billto);
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }
        public DataTable getOrder(string segmento)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("select * from VISTA_fe_Header where idreceptor = @idreceptor and  idreceptor not in ('liverpol','GLOBALIV','LIVERTIJ','SFERALIV','FACTUMLV','MERCANLV','LIVERDED')", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.CommandTimeout = 1000;
                    selectCommand.Parameters.AddWithValue("@segmento", (object)segmento);
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }

        public DataTable getFacturasPorProcesarLivepool()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("select * from VISTA_fe_Header where idreceptor in ('liverpol','GLOBALIV','LIVERTIJ','SFERALIV','FACTUMLV','MERCANLV','LIVERDED')", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.CommandTimeout = 1000;
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }

        public DataTable getFacturaAdendaReferencia(string Ord)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("select ref_number, ref_type from referencenumber where ord_hdrnumber = @orden and (ref_type = 'ADEHOJ' or ref_type = 'ADEPED' or ref_type = 'LPROV')", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.CommandTimeout = 1000;
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            selectCommand.Parameters.AddWithValue("@orden", (object)Ord);
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }

        public DataTable getDatosFacturas(string fact)
        {
            DataTable dataTable1 = new DataTable();
            DataTable dataTable2 = new DataTable();
            string str = "";
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("( select case when(select ivh_mbnumber from invoiceheader with (nolock) where ivh_invoicenumber = @factura) = 0 then @factura else (select max(ivh_invoicenumber) from invoiceheader with (nolock) where ivh_mbnumber = (select ivh_mbnumber from invoiceheader with (nolock) where ivh_mbnumber != 0 and ivh_invoicenumber = @factura)) end)", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.Parameters.AddWithValue("@factura", (object)fact);
                    selectCommand.CommandTimeout = 1000;
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable1);
                            selectCommand.Connection.Close();
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                            selectCommand.Connection.Close();
                        }
                    }
                }
                if (dataTable1.Rows.Count != 0 && dataTable1 != null)
                    str = dataTable1.Rows[0].ItemArray[0].ToString();
                using (SqlCommand selectCommand = new SqlCommand("select * from vista_fe_header where ultinvoice = @parmFact", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.Parameters.AddWithValue("@parmFact", (object)str);
                    selectCommand.CommandTimeout = 1000;
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable2);
                            selectCommand.Connection.Close();
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                            selectCommand.Connection.Close();
                        }
                    }
                }
            }
            return dataTable2;
        }

        public DataTable getDetalle(string p)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("select * from vista_Fe_detail where folio = @factura", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.Parameters.AddWithValue("@factura", (object)p);
                    selectCommand.CommandTimeout = 1000;
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                            selectCommand.Connection.Close();
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }

        public DataTable getDetalle33(string p)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("select * from vista_Fe_detail where folio = @factura", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.Parameters.AddWithValue("@factura", (object)p);
                    selectCommand.CommandTimeout = 1000;
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                            selectCommand.Connection.Close();
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }

        public DataTable getInvoice(string fact)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("select ivh_invoicestatus,ivh_mbnumber,ivh_ref_number from invoiceheader where ivh_invoicenumber = @factura", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.Parameters.AddWithValue("@factura", (object)fact);
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                            selectCommand.Connection.Close();
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }

        public void updateFactura(string fact, string comprobante, int mbnumber)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand(mbnumber != 0 ? "update invoiceheader set ivh_ref_number = @idComprobante where ivh_mbnumber  = @master" : "update invoiceheader set ivh_ref_number = @idComprobante where ivh_invoicenumber = @fact", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.Parameters.AddWithValue("@idComprobante", (object)comprobante);
                    if (mbnumber == 0)
                        selectCommand.Parameters.AddWithValue("@fact", (object)fact);
                    else
                        selectCommand.Parameters.AddWithValue("@master", (object)mbnumber);
                    selectCommand.Parameters.AddWithValue("@factura", (object)fact);
                    using (new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            selectCommand.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
        }
        public DataTable getUser(string user)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("select top 1 usr_password from  tlbUserAccess tu inner join ttsusers ttu on usr_userid= ttu.usr_userid where usr_mail =@user and usr_access = 'Y'", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.CommandTimeout = 1000;
                    selectCommand.Parameters.AddWithValue("@user", (object)user);
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }
        public DataTable getLastInvoice(string ivh)
        {
            DataTable dataTable1 = new DataTable();
            DataTable dataTable2 = new DataTable();
            string str = "";
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("( select case when(select ivh_mbnumber from invoiceheader with (nolock) where ivh_invoicenumber = @factura) = 0 then @factura else (select max(ivh_invoicenumber) from invoiceheader with (nolock) where ivh_mbnumber = (select ivh_mbnumber from invoiceheader with (nolock) where ivh_mbnumber != 0 and ivh_invoicenumber = @factura)) end)", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.Parameters.AddWithValue("@factura", (object)ivh);
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable1);
                            selectCommand.Connection.Close();
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                            selectCommand.Connection.Close();
                        }
                    }
                }
                if (dataTable1.Rows.Count != 0 && dataTable1 != null)
                    str = dataTable1.Rows[0].ItemArray[0].ToString();
                using (SqlCommand selectCommand = new SqlCommand("select invoice from vista_fe_header where ultinvoice = @parmFact", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.Parameters.AddWithValue("@parmFact", (object)str);
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable2);
                            selectCommand.Connection.Close();
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                            selectCommand.Connection.Close();
                        }
                    }
                }
            }
            return dataTable2;
        }

        public void correcionGeneradas(
      string fact,
      string serie,
      string rutaPdf,
      string rutaXML,
      string UID)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("update vista_fe_generadas set rutapdf=@rutapdf, rutaxml=@rutaxml, UID=@UID where invoice = @factura and serie = @serie", connection))
                {
                    selectCommand.CommandType = CommandType.Text;

                    selectCommand.Parameters.AddWithValue("@factura", (object)fact);
                    selectCommand.Parameters.AddWithValue("@serie", (object)serie);

                    selectCommand.Parameters.AddWithValue("@rutapdf", (object)rutaPdf);
                    selectCommand.Parameters.AddWithValue("@rutaxml", (object)rutaXML);

                    selectCommand.Parameters.AddWithValue("@UID", (object)UID);
                    using (new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            selectCommand.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
        }



        public void actualizaGeneradas(
          string folioFactura,
          string serieFactura,
          string uuidFactura,
          string pdf_xml_descargaFactura,
          string pdf_descargaFactura,
          string xlm_descargaFactura,
          string cancelFactura,
          string LegNum,
          string Fecha,
          string Total,
          string Moneda,
          string RFC,
          string Origen,
          string Destino)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("INSERT INTO [dbo].[VISTA_Carta_Porte]([Folio],[Serie],[UUID],[Pdf_xml_descarga],[Pdf_descargaFactura],[xlm_descargaFactura],[cancelFactura],[LegNum],[Fecha],[Total],[Moneda],[RFC],[Origen],[Destino])VALUES(@Folio, @Serie, @UUID, @Pdf_xml_descarga, @Pdf_descargaFactura, @xlm_descargaFactura, @cancelFactura, @LegNum, @Fecha, @Total, @Moneda, @RFC, @Origen, @Destino)", connection))
                {
                    selectCommand.Parameters.AddWithValue("@Folio", (object)folioFactura);
                    selectCommand.Parameters.AddWithValue("@Serie", (object)serieFactura);
                    selectCommand.Parameters.AddWithValue("@UUID", (object)uuidFactura);
                    selectCommand.Parameters.AddWithValue("@Pdf_xml_descarga", (object)pdf_xml_descargaFactura);
                    selectCommand.Parameters.AddWithValue("@Pdf_descargaFactura", (object)pdf_descargaFactura);
                    selectCommand.Parameters.AddWithValue("@xlm_descargaFactura", (object)xlm_descargaFactura);
                    selectCommand.Parameters.AddWithValue("@cancelFactura", (object)cancelFactura);
                    selectCommand.Parameters.AddWithValue("@LegNum", (object)LegNum);
                    selectCommand.Parameters.AddWithValue("@Fecha", (object)Fecha);
                    selectCommand.Parameters.AddWithValue("@Total", (object)Total);
                    selectCommand.Parameters.AddWithValue("@Moneda", (object)Moneda);
                    selectCommand.Parameters.AddWithValue("@RFC", (object)RFC);
                    selectCommand.Parameters.AddWithValue("@Origen", (object)Origen);
                    selectCommand.Parameters.AddWithValue("@Destino", (object)Destino);
                    using (new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            selectCommand.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
        }


        public void ErrorGeneradasCP(
            string Fecha,
            string Folio,
            string Erro1,
            string Erro2,
            string Erro3,
            string Erro4,
            string Erro5,
            string Erro6,
            string Erro7)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("INSERT INTO [dbo].[VISTA_Carta_Porte_Errores]([Fecha],[Folio],[Erro1],[Erro2],[Erro3],[Erro4],[Erro5],[Erro6],[Erro7])VALUES(@Fecha, @Folio, @Erro1, @Erro2, @Erro3, @Erro4, @Erro5, @Erro6, @Erro7)", connection))
                {
                    selectCommand.Parameters.AddWithValue("@Fecha", (object)Fecha);
                    selectCommand.Parameters.AddWithValue("@Folio", (object)Folio);
                    selectCommand.Parameters.AddWithValue("@Erro1", (object)Erro1);
                    selectCommand.Parameters.AddWithValue("@Erro2", (object)Erro2);
                    selectCommand.Parameters.AddWithValue("@Erro3", (object)Erro3);
                    selectCommand.Parameters.AddWithValue("@Erro4", (object)Erro4);
                    selectCommand.Parameters.AddWithValue("@Erro5", (object)Erro5);
                    selectCommand.Parameters.AddWithValue("@Erro6", (object)Erro6);
                    selectCommand.Parameters.AddWithValue("@Erro7", (object)Erro7);
                    using (new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            selectCommand.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
        }
    }
}

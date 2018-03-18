using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Cas.Common
{
    public static class DataUtil
    {
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    var v = Props[i].GetValue(item, null);
                    if (v != null && v.GetType().ToString() == "System.DateTime")
                    {
                        values[i] = Convert.ToDateTime(v).ToString("dd/MM/yyyy hh:mm:ss");
                    }
                    else if (v != null && v.GetType().ToString() == "System.Decimal")
                    {
                        values[i] = Convert.ToDecimal(v).ToString("#,##0");                        
                    }
                    else
                    {
                        values[i] = v;
                    }                    
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        public static string GetHashString(string password)
        {
            System.Security.Cryptography.SHA512Managed HashTool = new System.Security.Cryptography.SHA512Managed();
            Byte[] PasswordAsByte = System.Text.Encoding.UTF8.GetBytes(password);
            Byte[] EncryptedBytes = HashTool.ComputeHash(PasswordAsByte);
            HashTool.Clear();
            return Convert.ToBase64String(EncryptedBytes);
        }
    }

    public static class DateFormat
    {
        public const string JsonDateFormat = "yyyy-MM-ddTHH:mm:ssZ";        
    }

    public static class ServicesNamespace
    {
        public const string CarLogService = "http://www.kiatnakinbank.com/services/KKCAR/CARLogService";
    }
    public static class SystemName
    {
        public const string BulkCreateActivityLog = "BulkCreateActivityLog";
        public const string HttpCreateActivityLog = "HttpCreateActivityLog";
    }
    public static class Batch
    {
        public const string HttpCreateActivityLog = "CAR_BATCH_001";
        public const string BulkCreateActivityLog = "CAR_BATCH_004";
    }
    public static class BatchStatus
    {
        public const string Success = "Success";
        public const string Fail = "Fail";
        public const string Processing = "Processing";
        public const string Idle = "Idle";
        public const string SemiSuccess = "SemiSuccess";
    }
    public static class BatchResponse
    {
        public const string Code000 = "CAS-I-000";
        public const string Message000 = "Success";
        public const string Code100 = "CAS-E-100";
        public const string Message100 = "Internal Error";
        public const string Code101 = "CAS-E-101";
        public const string Message101 = "Data Required";
        public const string Code102 = "CAS-E-102";
        public const string Message102 = "One of these data required";
        public const string Code103 = "CAS-E-103";
        public const string Message103 = "Invalid Data";
        public const string Code201 = "CAS-E-201";
        public const string Message201 = "Invalid Security Key";
        public const string Code202 = "CAS-E-202";
        public const string Message202 = "No Permission";
        public const string Code203 = "CAS-E-203";
        public const string Message203 = "Invalid Service Name";
        public const string Code300 = "CAS-E-300";
        public const string Message300 = "No Data Found";
        public const string Code401 = "CAS-E-401";
        public const string Message401 = "Invalid Total Record";
        public const string Code402 = "CAS-E-402";
        public const string Message402 = "Invalid JSON Format";
        public const string Code403 = "CAS-E-403";
        public const string Message403 = "Duplicated Data";
    }

    public static class ExceptionService
    {
        public static string GetMessage(Exception ex)
        {
            if (ex.InnerException != null)
            {
                return GetMessage(ex.InnerException);
            }
            else
            {
                return ex.Message;
            }
        }
        public static string GetToString(Exception ex)
        {
            if (ex.InnerException != null)
            {
                return GetToString(ex.InnerException);
            }
            else
            {
                return ex.ToString();
            }
        }
    }
}

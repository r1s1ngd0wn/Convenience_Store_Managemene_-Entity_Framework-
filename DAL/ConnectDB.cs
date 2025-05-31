// DAL/ConnectDB.cs
using System;
using System.Data;
using System.Data.SqlClient;

namespace Convenience_Store_Management.DAL
{
    public class ConnectDB
    {
        public readonly string strCon = "Data Source= (local);Initial Catalog=QuanLyBanHang;Integrated Security=True;TrustServerCertificate=True";

        public SqlConnection conn = null;
        public SqlCommand comm = null;
        public SqlDataAdapter da = null;
        public SqlTransaction tran = null;

        public ConnectDB()
        {
            conn = new SqlConnection(strCon);
            comm = conn.CreateCommand();
        }

        public void OpenConnection()
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Lỗi khi mở kết nối cơ sở dữ liệu: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi không xác định khi mở kết nối: " + ex.Message, ex);
            }
        }

        public void CloseConnection()
        {
            try
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Lỗi khi đóng kết nối cơ sở dữ liệu: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi không xác định khi đóng kết nối: " + ex.Message, ex);
            }
        }

        public void BeginTransaction()
        {
            OpenConnection();
            tran = conn.BeginTransaction();
            comm.Transaction = tran;
        }

        public void CommitTransaction()
        {
            if (tran != null)
            {
                tran.Commit();
                tran = null;
            }
            CloseConnection();
        }

        public void RollbackTransaction()
        {
            if (tran != null)
            {
                tran.Rollback();
                tran = null;
            }
            CloseConnection();
        }

        public DataSet ExecuteQueryDataSet(string strSQL, CommandType ct)
        {
            // This method needs to handle its own connection if no transaction is active
            // and pass the transaction if one IS active.
            // Simplified: Always use current connection/transaction if exists, otherwise open/close.
            bool wasClosed = conn.State == ConnectionState.Closed;
            if (wasClosed && tran == null) // Only open if not already open for a transaction
            {
                OpenConnection();
            }

            try
            {
                comm.CommandText = strSQL;
                comm.CommandType = ct;
                comm.Connection = conn; // Ensure command uses the shared connection
                if (tran != null) // Ensure command uses the shared transaction if active
                {
                    comm.Transaction = tran;
                }
                da = new SqlDataAdapter(comm);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
            catch (SqlException ex)
            {
                throw new Exception("Lỗi truy vấn dữ liệu: " + ex.Message, ex);
            }
            finally
            {
                if (wasClosed && tran == null) // Only close if we opened it and no transaction is active
                {
                    CloseConnection();
                }
            }
        }

        public bool MyExecuteNonQuery(string strSQL, CommandType ct, ref string error)
        {
            bool f = false;
            // The connection is expected to be open if BeginTransaction was called,
            // otherwise, it needs to be opened for non-transactional single operations.
            bool wasClosed = conn.State == ConnectionState.Closed;
            if (wasClosed && tran == null) // Only open if not already open for a transaction
            {
                OpenConnection();
            }

            try
            {
                comm.CommandText = strSQL;
                comm.CommandType = ct;
                comm.Connection = conn; // Ensure command uses the shared connection
                if (tran != null) // Ensure command uses the shared transaction if active
                {
                    comm.Transaction = tran;
                }
                comm.ExecuteNonQuery();
                f = true;
            }
            catch (SqlException ex)
            {
                error = ex.Message;
            }
            catch (Exception ex)
            {
                error = "Lỗi không xác định: " + ex.Message;
            }
            finally
            {
                // Only close the connection if it was originally closed and no transaction is active.
                // If a transaction is active (tran != null), the BLL's Commit/Rollback will close it.
                if (wasClosed && tran == null)
                {
                    CloseConnection();
                }
            }
            return f;
        }
    }
}
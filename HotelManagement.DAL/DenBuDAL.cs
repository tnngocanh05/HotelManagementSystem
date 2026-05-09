using HotelManagement.DTO;
using System.Data.SqlClient;

namespace HotelManagement.DAL
{
    public class DenBuDAL
    {
        private DBConnection db = new DBConnection();

        public bool ThemDenBu(DenBuDTO denBu)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = @"
                    INSERT INTO DenBu
                    (
                        MaDatPhong,
                        NoiDung,
                        SoTien
                    )
                    VALUES
                    (
                        @MaDatPhong,
                        @NoiDung,
                        @SoTien
                    )";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@MaDatPhong", denBu.MaDatPhong);
                cmd.Parameters.AddWithValue("@NoiDung", denBu.NoiDung);
                cmd.Parameters.AddWithValue("@SoTien", denBu.SoTien);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public decimal TinhTienDenBu(int maDatPhong)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT ISNULL(SUM(SoTien),0)
                    FROM DenBu
                    WHERE MaDatPhong = @MaDatPhong";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@MaDatPhong", maDatPhong);

                object result = cmd.ExecuteScalar();

                return result != null ? (decimal)result : 0;
            }
        }
    }
}
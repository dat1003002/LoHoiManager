using System;
using System.Configuration;

namespace LoHoiManager
{
    public class Configuration
    {
        private static Configuration _instance;
        private string _connectionString;

        // Singleton pattern để đảm bảo chỉ có một instance
        public static Configuration Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Configuration();
                }
                return _instance;
            }
        }

        // Constructor riêng để tải chuỗi kết nối
        private Configuration()
        {
            LoadConnectionString();
        }

        private void LoadConnectionString()
        {
            try
            {
                // Lấy chuỗi kết nối từ app.config
                _connectionString = ConfigurationManager.ConnectionStrings["MyDb"]?.ConnectionString;
                if (string.IsNullOrEmpty(_connectionString))
                {
                    throw new ConfigurationErrorsException("Chuỗi kết nối 'MyDb' không tồn tại trong app.config.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi đọc chuỗi kết nối: " + ex.Message);
            }
        }

        // Phương thức lấy chuỗi kết nối
        public string GetConnectionString()
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("Chuỗi kết nối chưa được tải.");
            }
            return _connectionString;
        }
    }
}
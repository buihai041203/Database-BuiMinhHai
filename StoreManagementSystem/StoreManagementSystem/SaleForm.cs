using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace StoreManagementSystem
{
    public partial class SaleForm : Form
    {
        private string currentView = "Product"; // Biến trạng thái hiển thị hiện tại

        public SaleForm()
        {
            InitializeComponent();

            // Maximize the form to fill the screen
            this.WindowState = FormWindowState.Maximized;

            // Center the form on the screen
            this.StartPosition = FormStartPosition.CenterScreen;

            // Disable resizing (Fixed size)
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            // Optional: Disable the maximize button as well if you don't want the user to maximize it
            this.MaximizeBox = false;
        }

        private void SaleForm_Load(object sender, EventArgs e)
        {
            currentView = "Product"; // Đặt mặc định là hiển thị dữ liệu sản phẩm
            LoadProductData();       // Gọi phương thức để tải dữ liệu sản phẩm
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            string searchText = textBoxSearch.Text; // Lấy nội dung tìm kiếm từ TextBox

            if (string.IsNullOrEmpty(searchText))
            {
                if (currentView == "Product") LoadProductData();
                else if (currentView == "History") LoadHistoryData();
                else if (currentView == "Customer") LoadCustomerData();
            }
            else
            {
                if (currentView == "Product") SearchProduct(searchText);
                else if (currentView == "History") SearchHistory(searchText);
                else if (currentView == "Customer") SearchCustomer(searchText);
            }

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            currentView = "Product";
            LoadProductData(); // Hiển thị dữ liệu sản phẩm
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            currentView = "History";
            LoadHistoryData(); // Hiển thị dữ liệu lịch sử giao dịch
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            currentView = "Customer";
            LoadCustomerData(); // Hiển thị dữ liệu thông tin khách hàng
        }

        private void LoadProductData()
        {
            string query = "SELECT * FROM Product"; // Lấy tất cả dữ liệu từ bảng Product

            using (SqlConnection conn = new SqlConnection(Connection.SQLConnection))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt; // Gán DataTable vào DataGridView
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải dữ liệu sản phẩm: " + ex.Message);
                }
            }
        }

        private void LoadHistoryData()
        {
            string query = @"
        SELECT c.CustomerName, p.Name AS ProductName, ph.Quantity, ph.PurchaseDate
        FROM PurchaseHistory ph
        JOIN Customer c ON ph.CustomerID = c.CustomerID
        JOIN Product p ON ph.ProductCode = p.Code";  // Lấy tên sản phẩm từ bảng Product

            using (SqlConnection conn = new SqlConnection(Connection.SQLConnection))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải dữ liệu lịch sử giao dịch: " + ex.Message);
                }
            }
        }

        private void LoadCustomerData()
        {
            string query = "SELECT  CustomerName, PhoneNumber, Email, Address FROM Customer";

            using (SqlConnection conn = new SqlConnection(Connection.SQLConnection))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải dữ liệu khách hàng: " + ex.Message);
                }
            }
        }

        private void SearchProduct(string searchText)
        {
            string query = "SELECT * FROM Product WHERE Code LIKE @searchText OR Name LIKE @searchText";

            using (SqlConnection conn = new SqlConnection(Connection.SQLConnection))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@searchText", "%" + searchText + "%");

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tìm kiếm sản phẩm: " + ex.Message);
                }
            }
        }

        private void SearchHistory(string searchText)
        {
            string query = @"
        SELECT c.CustomerName, p.Name AS ProductName, ph.Quantity, ph.PurchaseDate
        FROM PurchaseHistory ph
        JOIN Customer c ON ph.CustomerID = c.CustomerID
        JOIN Product p ON ph.ProductCode = p.Code
        WHERE c.CustomerName LIKE @searchText OR p.Name LIKE @searchText OR ph.ProductCode LIKE @searchText";

            using (SqlConnection conn = new SqlConnection(Connection.SQLConnection))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@searchText", "%" + searchText + "%");

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dataGridView1.DataSource = dt; // Gán DataTable vào DataGridView
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tìm kiếm lịch sử giao dịch: " + ex.Message);
                }
            }
        }



        private void SearchCustomer(string searchText)
        {
            string query = @"
        SELECT CustomerID, CustomerName, PhoneNumber, Email, Address
        FROM Customer
        WHERE CustomerName LIKE @searchText OR PhoneNumber LIKE @searchText";

            using (SqlConnection conn = new SqlConnection(Connection.SQLConnection))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@searchText", "%" + searchText + "%");

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dataGridView1.DataSource = dt; // Gán DataTable vào DataGridView
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tìm kiếm khách hàng: " + ex.Message);
                }
            }
        }


        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginApp loginForm = new LoginApp();
            loginForm.Show();
        }
    }
}

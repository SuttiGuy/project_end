using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project1
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            string connectionString = "Server=localhost;Database=phone;User Id=root;Password=;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            string selectQuery = "SELECT * FROM phone";
            MySqlCommand selectCommand = new MySqlCommand(selectQuery, connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter(selectCommand);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            dataGridView1.DataSource = dataTable;

            connection.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedRows.Count > 0)
            {
                string brand = dataGridView1.SelectedRows[0].Cells["brand"].Value.ToString();
                string modele = dataGridView1.SelectedRows[0].Cells["modele"].Value.ToString();
                string price = dataGridView1.SelectedRows[0].Cells["price"].Value.ToString();
                string stock = dataGridView1.SelectedRows[0].Cells["stock"].Value.ToString();

                DataTable selectedItems = new DataTable();
                selectedItems.Columns.Add("brand");
                selectedItems.Columns.Add("modele");
                selectedItems.Columns.Add("price");
                selectedItems.Columns.Add("stock");

                selectedItems.Rows.Add(brand, modele, price, stock);

                dataGridView2.DataSource = selectedItems;
            }
            else
            {
                MessageBox.Show("โปรดเลือกรายการที่คุณต้องการเลือกและซื้อ");
            }


            if (dataGridView1.SelectedRows.Count > 0)
            {
                string brand = dataGridView1.SelectedRows[0].Cells["brand"].Value.ToString();
                string stock = dataGridView1.SelectedRows[0].Cells["stock"].Value.ToString();
                string price = dataGridView1.SelectedRows[0].Cells["price"].Value.ToString();

                DataTable selectedItems = new DataTable();
                selectedItems.Columns.Add("brand");
                selectedItems.Columns.Add("stock");
                selectedItems.Columns.Add("price");

                selectedItems.Rows.Add(brand, stock, price);

                dataGridView2.DataSource = selectedItems;
            }




        }

        private void button2_Click(object sender, EventArgs e)
        {
            string connectionString = "Server=localhost;Database=phone;User Id=root;Password=;";
            MySqlConnection connection = new MySqlConnection(connectionString);

            if (dataGridView1.SelectedRows.Count > 0)
            {
                // ดึงข้อมูลจากแถวที่ถูกเลือก
                int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["id"].Value);
                string brand = dataGridView1.SelectedRows[0].Cells["brand"].Value.ToString();
                string modele = dataGridView1.SelectedRows[0].Cells["modele"].Value.ToString();
                string price = dataGridView1.SelectedRows[0].Cells["price"].Value.ToString();
                int stock = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["stock"].Value);
                int checkstock = stock;
                int userbuy = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["userbuy"].Value);

                if (stock > 0)
                {
                    // ลด stock 
                    stock--;
                    userbuy++;

                    // ทำสิ่งที่คุณต้องการทำเมื่อคลิกปุ่ม "เลือกและซื้อ" ตรงนี้
                    // คุณสามารถเรียกฟอร์มหรือเปิดหน้าต่างใหม่เพื่อทำรายการเพิ่มเติม

                    // เช่น แสดงข้อความแจ้งเตือน
                    MessageBox.Show(" คุณได้ทำการสั่งซื้อ " + brand + modele + " ราคา : " + price + " บาท ");

                    // อัปเดต stock ใน DataGridView
                    dataGridView1.SelectedRows[0].Cells["stock"].Value = stock; // อัปเดต stock ในฐานข้อมูล (ถ้าต้องการ)
                    dataGridView1.SelectedRows[0].Cells["userbuy"].Value = userbuy; // อัปเดต userbuy ในฐานข้อมูล (ถ้าต้องการ)
                    UpdateStockInDatabase(id, stock, userbuy);
                }
                else
                {
                    MessageBox.Show("สินค้าหมดสต็อกแล้ว");
                }
            }
            else
            {
                // ถ้าไม่มีแถวที่ถูกเลือก ให้แจ้งเตือนให้ผู้ใช้เลือกแถวก่อน
                MessageBox.Show("โปรดเลือกรายการที่คุณต้องการเลือกและซื้อ");
            }
        }

        private void UpdateStockInDatabase(int id, int stock, int userbuy)
        {
            string connectionString = "Server=localhost;Database=phone;User Id=root;Password=;";
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                string updateQuery = "UPDATE phone SET stock = @new_stock , userbuy = @new_userbuy WHERE id = @new_id";
                MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection);
                updateCommand.Parameters.AddWithValue("@new_id", id);
                updateCommand.Parameters.AddWithValue("@new_stock", stock);
                updateCommand.Parameters.AddWithValue("@new_userbuy", userbuy);

                updateCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log it or show an error message)
                MessageBox.Show("Error updating stock in the database: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }


        private void label10_Click(object sender, EventArgs e)
        {
            Form1_1 form1_1 = new Form1_1(); // เปลี่ยน Form เป็นชื่อของฟอร์มที่ต้องการเปิด
            form1_1.Show();
        }

    }
}

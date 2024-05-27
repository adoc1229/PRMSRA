using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using MySql.Data.MySqlClient;

namespace ParkEase
{
    public partial class AdminUsers : Form
    {
        private string connectionString = "server=127.0.0.1;port=3306;username=root;password=;database=parkeasedatabase;";
        public AdminUsers()
        {
            InitializeComponent();
            tbxEmpID.Hide();
            tbxPN.Hide();
            tbxEmpName.Hide();
            tbxCN.Hide();
            btnAddEmployee.Hide();
            cbxVType.Hide();
            label2.Hide();
            label3.Hide();
            labelUsers.Hide();
        }
        MySqlDataAdapter da;
        MySqlCommand cmd;
        DataSet ds;
        int indexRow;
        MySqlConnection myConn;
        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            string platenumber = tbxPN.Text;
            string vehicletype = cbxVType.Text;
            string fullName = tbxEmpName.Text;
            string contanct = tbxCN.Text;
            string passw = tbxPass1.Text;

            Random rand = new Random();
            string empID = "EMP" + rand.Next(100, 999).ToString();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO employees (EmployeeID, EmployeeName, VehicleType, PlateNumber, ContactNumber, Password) VALUES (@EmpID, @EmpName, @VType, @PlateNumber, @Contanct, @Passwords)";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EmpID", empID);
                    command.Parameters.AddWithValue("@EmpName", fullName);
                    command.Parameters.AddWithValue("@VType", vehicletype);
                    command.Parameters.AddWithValue("@PlateNumber", platenumber);
                    command.Parameters.AddWithValue("@Contanct", contanct);
                    command.Parameters.AddWithValue("@Passwords", passw);
                    command.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Employee registered successfully! Employee ID: " + empID.ToString());
            lbEmps_Click(sender, e);
            tbxEmpName.Clear();
            tbxCN.Clear();
            tbxPN.Clear();

        }
        private void tbxUserID_TextChanged(object sender, EventArgs e)
        {
            string searchValue = tbxUserID.Text;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT UserID, VehicleOwner, VehicleType, PlateNumber FROM users WHERE UserID LIKE @userID";
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection))
                {
                    adapter.SelectCommand.Parameters.AddWithValue("@userID", "%" + searchValue + "%");
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet);
                    dgvUsers.DataSource = dataSet.Tables[0];
                }
            }
        }
        private void tbxEmpID_TextChanged(object sender, EventArgs e)
        {
            string searchValue = tbxEmpID.Text;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT EmployeeID, EmployeeName, VehicleType, PlateNumber FROM employees WHERE EmployeeID LIKE @empID";
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection))
                {
                    adapter.SelectCommand.Parameters.AddWithValue("@empID", "%" + searchValue + "%");
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet);
                    dgvUsers.DataSource = dataSet.Tables[0];
                }
            }
        }
        private void lbUsers_Click(object sender, EventArgs e)
        {
            lbUsers.ForeColor = Color.DimGray;
            lbEmps.ForeColor = Color.Gray;
            label3.Hide();
            label2.Show();
            panel3.BackColor = Color.FromArgb(95, 74, 211);
            panel4.BackColor = Color.Silver;
            tbxUserID.Show();
            tbxEmpID.Hide();
            tbxPN.Hide();
            tbxEmpName.Hide();
            tbxCN.Hide();
            btnAddEmployee.Hide();
            cbxVType.Hide();
            labelUsers.Show();
            labelEmployees.Hide();
            btnDelete.Show();
            btnDeleteEmp.Hide();
            tbxPass1.Hide();

            try
            {
                myConn = new MySqlConnection("server=127.0.0.1;port=3306;username=root;password=;database=parkeasedatabase;");

                string query = "SELECT UserID, VehicleOwner, VehicleType, PlateNumber, ContactNumber FROM users";

                da = new MySqlDataAdapter(query, myConn);

                ds = new DataSet();
                myConn.Open();
                da.Fill(ds, "users");
                dgvUsers.DataSource = ds.Tables["users"];

                int userCount = countUsers();
                labelUsers.Text = "" + userCount.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
               
                
            }
        }
        private void lbEmps_Click(object sender, EventArgs e)
        {
            lbUsers.ForeColor = Color.Gray;
            lbEmps.ForeColor = Color.DimGray;
            label2.Hide();
            label3.Show();
            panel4.BackColor = Color.FromArgb(95, 74, 211);
            panel3.BackColor = Color.Silver;
            tbxUserID.Hide();
            tbxEmpID.Show();
            tbxEmpName.Show();
            btnAddEmployee.Show();
            tbxCN.Show();
            cbxVType.Show();
            labelUsers.Hide();
            labelEmployees.Show();
            tbxPN.Show();
            btnDelete.Hide();
            btnDeleteEmp.Show();
            tbxPass1.Show();

            try
            {
                myConn = new MySqlConnection("server=127.0.0.1;port=3306;username=root;password=;database=parkeasedatabase;");

                string query = "SELECT EmployeeID, EmployeeName, VehicleType, PlateNumber, ContactNumber FROM employees";

                da = new MySqlDataAdapter(query, myConn);

                ds = new DataSet();
                myConn.Open();
                da.Fill(ds, "employees");
                dgvUsers.DataSource = ds.Tables["employees"];

                int empCount = countEmployees();
                labelEmployees.Text = "" + empCount;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
        }
        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            try
            {
                string userID = tbxUserID.Text.Trim();

                if (!string.IsNullOrEmpty(userID))
                {
                    string query = "DELETE FROM users WHERE UserID = @UserID";
                    cmd = new MySqlCommand(query, myConn);
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    myConn.Open();
                    cmd.ExecuteNonQuery();
                    myConn.Close();

                    lbUsers_Click(sender, e);
                    MessageBox.Show("Data deleted successfully!");
                }
                else
                {
                    MessageBox.Show("Please select a user to delete.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void btnDeleteEmp_Click(object sender, EventArgs e)
        {
            try
            {
                string empID = tbxEmpID.Text.Trim();

                if (!string.IsNullOrEmpty(empID))
                {
                    string query = "DELETE FROM employees WHERE EmployeeID = @EmpID";
                    cmd = new MySqlCommand(query, myConn);
                    cmd.Parameters.AddWithValue("@EmpID", empID);
                    myConn.Open();
                    cmd.ExecuteNonQuery();
                    myConn.Close();

                    lbEmps_Click(sender, e);
                    MessageBox.Show("Data deleted successfully!");
                }
                else
                {
                    MessageBox.Show("Please select an employee to delete.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private int countUsers()
        {
            int count = 0;
            try
            {
                myConn = new MySqlConnection("server=127.0.0.1;port=3306;username=root;password=;database=parkeasedatabase;");

                myConn.Open();
                string query = "SELECT * FROM users";
                MySqlCommand cmd = new MySqlCommand(query, myConn);
                MySqlDataReader reader = cmd.ExecuteReader();
                
                while(reader.Read()){
                    count++;
                }

                reader.Close();
                    
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error counting users: " + ex.Message);
            }
            finally
            {
                myConn.Close();
            }
            return count;
        }
        private int countEmployees()
        {
            int count = 0;
            try
            {
                myConn = new MySqlConnection("server=127.0.0.1;port=3306;username=root;password=;database=parkeasedatabase;");

                myConn.Open();
                string query = "SELECT * FROM employees";
                MySqlCommand cmd = new MySqlCommand(query, myConn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    count++;
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error counting employees: " + ex.Message);
            }
            finally
            {
                myConn.Close();
            }
            return count;
        }
        private void AdminUsers_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;

        }
        private void lbUsers_MouseLeave(object sender, EventArgs e)
        {
            Label hoveredLabel = sender as Label;
            if (hoveredLabel != null)
            {
                hoveredLabel.ForeColor = Color.Gray; 
            }
        }
        private void lbEmps_MouseLeave(object sender, EventArgs e)
        {
            Label hoveredLabel = sender as Label;
            if (hoveredLabel != null)
            {
                hoveredLabel.ForeColor = Color.Gray; 
            }
        }
        private void lbUsers_MouseEnter(object sender, EventArgs e)
        {
            Label hoveredLabel = sender as Label;
            if (hoveredLabel != null)
            {
                hoveredLabel.ForeColor = Color.DimGray;
            }
        }
        private void lbEmps_MouseEnter(object sender, EventArgs e)
        {
            Label hoveredLabel = sender as Label;
            if (hoveredLabel != null)
            {
                hoveredLabel.ForeColor = Color.DimGray;
            }
        }
    }
}

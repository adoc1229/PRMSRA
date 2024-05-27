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
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using MySql.Data.MySqlClient;

namespace ParkEase
{
    public partial class Home : Form
    {
        private string connectionString = "server=127.0.0.1;port=3306;username=root;password=;database=parkeasedatabase;";
        private int totalSlots = 136;
        private int occupiedSlots = 0;
        private int reservedSlots = 0;
        private int availableSlots = 0;
        public Home()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            bar1.Value = 0;
            LoadDataGrid();
            UpdateSlotCounts();
        }
        MySqlConnection myConn;
        MySqlDataAdapter da;
        MySqlCommand cmd;
        DataSet ds;
        int indexRow;
        private void UpdateSlotCounts()
        {
            using (MySqlConnection connection = new MySqlConnection("server=127.0.0.1;port=3306;username=root;password=;database=parkeasedatabase;"))
            {
                
                try
                {
                    connection.Open();

                    string availableQuery = "SELECT * FROM parking WHERE Status = 'AVAILABLE'";
                    MySqlCommand availableCommand = new MySqlCommand(availableQuery, connection);
                    availableSlots = Convert.ToInt32(availableCommand.ExecuteScalar());

                    string reservedQuery = "SELECT * FROM parking WHERE Status = 'RESERVED'";
                    MySqlCommand reservedCommand = new MySqlCommand(reservedQuery, connection);
                    reservedSlots = Convert.ToInt32(reservedCommand.ExecuteScalar());

                    string occupiedQuery = "SELECT * FROM parking WHERE Status = 'OCCUPIED'";
                    MySqlCommand occupiedCommand = new MySqlCommand(occupiedQuery, connection);
                    occupiedSlots = Convert.ToInt32(occupiedCommand.ExecuteScalar());

                    int totalUsedSlots = occupiedSlots + reservedSlots;
                    int totalAvailableSlots = totalSlots - totalUsedSlots;

                    double occupiedPercentage = (double)occupiedSlots / totalSlots * 100;
                    double reservedPercentage = (double)reservedSlots / totalSlots * 100;

                    bar1.Value = (int)(occupiedPercentage + reservedPercentage);
                    labelAvailable.Text = $"{totalAvailableSlots}";
                    labelReserved.Text = $"{reservedSlots}";
                    labelOccupied.Text = $"{occupiedSlots}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        private void LoadDataGrid()
        {
            try
            {
                myConn = new MySqlConnection("server=127.0.0.1;port=3306;username=root;password=;database=parkeasedatabase;");

                string query = "SELECT ParkingCode, Status FROM parking";

                da = new MySqlDataAdapter(query, myConn);

                ds = new DataSet();
                myConn.Open();
                da.Fill(ds, "parking");
                dgvParking.DataSource = ds.Tables["parking"];
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
        private void btnAll_Click(object sender, EventArgs e)
        {
            LoadDataGrid();
        }
        private void rdbAvailable_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                myConn = new MySqlConnection("server=127.0.0.1;port=3306;username=root;password=;database=parkeasedatabase;");

                string query = "SELECT ParkingCode, Status FROM parking WHERE Status = 'AVAILABLE'";

                da = new MySqlDataAdapter(query, myConn);

                ds = new DataSet();
                myConn.Open();
                da.Fill(ds, "parking");
                dgvParking.DataSource = ds.Tables["parking"];
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
        private void rdbReserved_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                myConn = new MySqlConnection("server=127.0.0.1;port=3306;username=root;password=;database=parkeasedatabase;");

                string query = "SELECT ParkingCode, Status FROM parking WHERE Status = 'RESERVED'";

                da = new MySqlDataAdapter(query, myConn);

                ds = new DataSet();
                myConn.Open();
                da.Fill(ds, "parking");
                dgvParking.DataSource = ds.Tables["parking"];
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
        private void rdbOccupied_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                myConn = new MySqlConnection("server=127.0.0.1;port=3306;username=root;password=;database=parkeasedatabase;");

                string query = "SELECT ParkingCode, Status FROM parking WHERE Status = 'OCCUPIED'";

                da = new MySqlDataAdapter(query, myConn);

                ds = new DataSet();
                myConn.Open();
                da.Fill(ds, "parking");
                dgvParking.DataSource = ds.Tables["parking"];
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
        private void Home_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
        }
    }
}

using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ParkEase
{
    public partial class ParkingStatus : Form
    {

        private string connectionString = "server=127.0.0.1;port=3306;username=root;password=;database=parkeasedatabase;";
        public ParkingStatus()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }
        MySqlConnection myConn;
        MySqlDataAdapter da;
        MySqlCommand cmd;
        DataSet ds;
        int indexRow;
        private void ParkingStatus_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;

        }
        private void tbxTN_TextChanged(object sender, EventArgs e)
        {
            string searchValue = tbxTicket.Text;
            DateTime selectedDate = datetimepicker1.Value.Date;
            string formattedDate = selectedDate.ToString("yyyy-MM-dd");

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT TicketNumber, DATE_FORMAT(TimeIn, '%H:%i') AS TimeIn, DATE_FORMAT(TimeOut, '%H:%i') AS TimeOut, ParkingFee, ParkingDate FROM parking_log WHERE TicketNumber LIKE @TN AND ParkingDate = @Date";
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection))
                {
                    adapter.SelectCommand.Parameters.AddWithValue("@TN", "%" + searchValue + "%");
                    adapter.SelectCommand.Parameters.AddWithValue("@Date", selectedDate);
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet);
                    dgvParkingLog.DataSource = dataSet.Tables[0];
                }
            }
        }
        private void btnLoad_Click_1(object sender, EventArgs e)
        {
            panel4.BackColor = Color.FromArgb(95, 74, 211);
            panel3.BackColor = Color.Silver;
            panel1.BackColor = Color.Silver;
            lbLog.ForeColor = Color.DimGray;
            lbBooking.ForeColor = Color.Gray;
            lbSales.ForeColor = Color.Gray;

            try
            {
                myConn = new MySqlConnection("server=127.0.0.1;port=3306;username=root;password=;database=parkeasedatabase;");

                da = new MySqlDataAdapter("SELECT TicketNumber, DATE_FORMAT(TimeIn, '%H:%i') AS TimeIn, DATE_FORMAT(TimeOut, '%H:%i') AS TimeOut, ParkingFee, ParkingDate FROM parking_log", myConn);

                ds = new DataSet();
                myConn.Open();
                da.Fill(ds, "parking_log");
                dgvParkingLog.DataSource = ds.Tables["parking_log"];
                myConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void btnClearLog_Click(object sender, EventArgs e)
        {
            try
            {
                myConn = new MySqlConnection("server=127.0.0.1;port=3306;username=root;password=;database=parkeasedatabase;");


                string deleteEmployeesQuery = "DELETE FROM bookings";
                MySqlCommand deleteEmployeesCmd = new MySqlCommand(deleteEmployeesQuery, myConn);

                string updateParkingStatusQuery = "UPDATE parking SET Status='AVAILABLE'";
                MySqlCommand updateParkingStatusCmd = new MySqlCommand(updateParkingStatusQuery, myConn);

                string deleteSalesReportQuery = "DELETE FROM sales_report";
                MySqlCommand deleteSalesReportCmd = new MySqlCommand(deleteSalesReportQuery, myConn);

                myConn.Open();

                int rowsAffectedEmployees = deleteEmployeesCmd.ExecuteNonQuery();
                int rowsAffectedParkingStatus = updateParkingStatusCmd.ExecuteNonQuery();
                int rowsAffectedSalesReport = deleteSalesReportCmd.ExecuteNonQuery();

                myConn.Close();

                if (rowsAffectedEmployees > 0)
                {
                    MessageBox.Show("All records cleared successfully.");
                    btnLoad_Click_1(sender, e);
                    dgvParkingLog.DataSource = null;
                }
                else if (rowsAffectedSalesReport > 0)
                {
                    MessageBox.Show("All records cleared successfully.");
                    btnLoad_Click_1(sender, e);
                    dgvParkingLog.DataSource = null;
                }
                else
                {
                    MessageBox.Show("No records found to clear.");
                }

                myConn.Open();

                string selectQuery = "SELECT ParkingDate, SUM(ParkingFee) AS TotalFee FROM parking_log GROUP BY ParkingDate";
                MySqlCommand selectCommand = new MySqlCommand(selectQuery, myConn);
                MySqlDataReader reader = selectCommand.ExecuteReader();

                while (reader.Read())
                {
                    DateTime date = Convert.ToDateTime(reader["ParkingDate"]);
                    decimal totalFee = Convert.ToDecimal(reader["TotalFee"]);
                 
                    string insertQuery = "INSERT INTO sales_report (ParkingDate, TotalSales) VALUES (@Date, @TotalSales)";
                    MySqlCommand insertCommand = new MySqlCommand(insertQuery, myConn);
                    
                    insertCommand.Parameters.AddWithValue("@Date", date);
                    insertCommand.Parameters.AddWithValue("@TotalSales", totalFee);
                    
                    insertCommand.ExecuteNonQuery();
                    
                    
                }
             

                MessageBox.Show("Sales report updated successfully.");
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message + ex.StackTrace);
            }
            
        }
        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            string ticketNumber = tbxTicket.Text;

            string query = "SELECT ParkingCode, UserID FROM bookings WHERE TicketNumber = @TicketNumber";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@TicketNumber", ticketNumber);

                try
                {
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        string parkingCode = reader["ParkingCode"].ToString();
                        string userID = reader["UserID"].ToString();
                       
                        DateTime currentTime = DateTime.Now;
                        DateTime timeOut = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, currentTime.Minute, 0);
                        
                        string timeInQuery = "SELECT TimeIn FROM parking_log WHERE TicketNumber = @TicketNumber";
                        MySqlCommand timeInCommand = new MySqlCommand(timeInQuery, connection);
                        timeInCommand.Parameters.AddWithValue("@TicketNumber", ticketNumber);
                        reader.Close();
                        DateTime timeIn = Convert.ToDateTime(timeInCommand.ExecuteScalar());
                        TimeSpan parkingDuration = timeOut.TimeOfDay - timeIn.TimeOfDay;
                        double durationMinutes = parkingDuration.TotalMinutes;
                        

                        double parkingFee;
                        const double initialFee = 30;
                        const double subsequentFeePerMinute = 20;

                        if (durationMinutes <= 3)
                        {
                            parkingFee = initialFee;
                        }
                        else
                        {

                            parkingFee = initialFee;

                            parkingFee += (durationMinutes - 3) * subsequentFeePerMinute;
                        }
                       
                        parkingFee *= 1.12;
                        reader.Close();

                        string userTypeQuery = "SELECT UserType FROM bookings WHERE TicketNumber = @TicketNumber";
                        MySqlCommand userTypeCommand = new MySqlCommand(userTypeQuery, connection);
                        userTypeCommand.Parameters.AddWithValue("@TicketNumber", ticketNumber);
                        string userType = userTypeCommand.ExecuteScalar()?.ToString();

                        if (userType == "Senior Citizen")
                        {

                            parkingFee *= 0.8;
                        }
                        else if (userType == "Employee")
                        {
                            parkingFee = 0;
                        }

                        double discount = 0;

                        if (userType == "Senior Citizen")
                        {
                            discount = 0.8;
                        }
                        else if (userType == "Employee")
                        {
                            discount = 1;
                        }

                        string updateParkingLogQuery = "UPDATE parking_log SET TimeOut = @TimeOut, ParkingFee = @ParkingFee WHERE TicketNumber = @TicketNumber";
                        MySqlCommand updateParkingLogCommand = new MySqlCommand(updateParkingLogQuery, connection);
                        updateParkingLogCommand.Parameters.AddWithValue("@TimeOut", timeOut.ToShortTimeString());
                        updateParkingLogCommand.Parameters.AddWithValue("@ParkingFee", parkingFee);
                        updateParkingLogCommand.Parameters.AddWithValue("@TicketNumber", ticketNumber);
                        updateParkingLogCommand.ExecuteNonQuery();

                        string plateNumber = "";
                        string vehicleType = "";
                        string userInfoQuery = "SELECT PlateNumber, VehicleType FROM users WHERE UserID = @UserID";
                        MySqlCommand userInfoCommand = new MySqlCommand(userInfoQuery, connection);
                        userInfoCommand.Parameters.AddWithValue("@UserID", userID);
                        MySqlDataReader userInfoReader = userInfoCommand.ExecuteReader();

                        if (userInfoReader.Read())
                        {
                            plateNumber = userInfoReader["PlateNumber"].ToString();
                            vehicleType = userInfoReader["VehicleType"].ToString();
                            reader.Close();
                        }

                        userInfoReader.Close();

                        Random random = new Random();
                        string serialNumber = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 10).Select(s => s[random.Next(s.Length)]).ToArray());

                        using (MySqlTransaction transaction = connection.BeginTransaction())
                        {
                            MySqlCommand deleteCommand = connection.CreateCommand();
                            deleteCommand.Transaction = transaction;

                            try
                            {
                                deleteCommand.CommandText = "DELETE FROM bookings WHERE TicketNumber = @TicketNumber";
                                deleteCommand.Parameters.AddWithValue("@TicketNumber", ticketNumber);
                                deleteCommand.ExecuteNonQuery();

                                transaction.Commit();

                                string updateParkingQuery = "UPDATE parking SET Status = 'AVAILABLE' WHERE ParkingCode = @ParkingCode";
                                MySqlCommand updateParkingCommand = new MySqlCommand(updateParkingQuery, connection);
                                updateParkingCommand.Parameters.AddWithValue("@ParkingCode", parkingCode);
                                updateParkingCommand.ExecuteNonQuery();

                                string receiptDirectory = @"C:\Users\romeo\source\repos\ParkEase\Receipts";
                                string receiptFileName = Path.Combine(receiptDirectory, $"Receipt_{ticketNumber}.pdf");

                                if (!Directory.Exists(receiptDirectory))
                                {
                                    Directory.CreateDirectory(receiptDirectory);
                                }

                                using (FileStream fs = new FileStream(receiptFileName, FileMode.Create, FileAccess.Write, FileShare.None))
                                {
                                    Document doc = new Document();
                                    PdfWriter.GetInstance(doc, fs);
                                    doc.Open();

                                    doc.Add(new Paragraph("\n\n"));
                                    doc.Add(new Paragraph("                                                                    Kai Mall"));
                                    doc.Add(new Paragraph("                                          Bagong Silang, Caloocan City, 1428 NCR"));
                                    doc.Add(new Paragraph("                                          Parking Reservation and Management System"));
                                    doc.Add(new Paragraph("                                                         Tel. No. 417-7735 to 38  Fax"));
                                    doc.Add(new Paragraph("                                                    VAT REG. TIN #. 005-255-946 000"));
                                    doc.Add(new Paragraph($"                                                              Serial # {serialNumber}"));
                                    doc.Add(new Paragraph("                                                      Permit # 0111-082-89826-000"));
                                    doc.Add(new Paragraph("                                                                     MN 110193842"));
                                    doc.Add(new Paragraph("\n\n"));
                                    doc.Add(new Paragraph($"Date: {DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}"));
                                    doc.Add(new Paragraph($"Ticket #: {ticketNumber}"));
                                    doc.Add(new Paragraph($"Time In: {timeIn.ToShortTimeString()}"));
                                    doc.Add(new Paragraph($"Time Out: {timeOut.ToShortTimeString()}"));
                                    doc.Add(new Paragraph($"Elapsed: {parkingDuration}"));
                                    doc.Add(new Paragraph($"Plate #: {plateNumber}"));
                                    doc.Add(new Paragraph($"User Type: {userType}"));
                                    doc.Add(new Paragraph($"Vehicle Type: {vehicleType}"));
                                    doc.Add(new Paragraph("----------------------------------------------------------------------------------------------------------------------------------"));
                                    doc.Add(new Paragraph($"Charge:                    {parkingFee / 1.12:F2}"));
                                    doc.Add(new Paragraph($"Vat Tax:                    {parkingFee * 0.12:F2}"));
                                    doc.Add(new Paragraph($"Discount:                   {discount * 100}%"));
                                    doc.Add(new Paragraph("Payment Method:     CASH"));
                                    doc.Add(new Paragraph($"Total:                         {parkingFee}"));
                                    doc.Add(new Paragraph("\n\n"));
                                    doc.Add(new Paragraph("                                                    THIS IS AN UNOFFICIAL RECEIPT"));
                                    doc.Add(new Paragraph("                                                      THANK YOU... GOD BLESS!!!"));
                                    doc.Close();
                                }
                                MessageBox.Show("Receipt is done.");
                                System.Diagnostics.Process.Start(receiptFileName);
                                tbxTicket.Clear();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                MessageBox.Show("Error: " + ex.Message +ex.StackTrace , "Error");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ticket number not found.", "Error");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error");
                }
            }
        }
        private void lbLog_Click(object sender, EventArgs e)
        {
            tbxTicket.Show();
            tbxParkingTicket.Hide();
            panel4.BackColor = Color.FromArgb(95, 74, 211);
            panel3.BackColor = Color.Silver;
            panel1.BackColor = Color.Silver;
            lbLog.ForeColor = Color.DimGray;
            lbBooking.ForeColor = Color.Gray;
            lbSales.ForeColor = Color.Gray;

            try
            {
                myConn = new MySqlConnection("server=127.0.0.1;port=3306;username=root;password=;database=parkeasedatabase;");

                DateTime selectedDate = datetimepicker1.Value.Date;
                string formattedDate = selectedDate.ToString("yyyy-MM-dd");


                //string selectQuery = $"SELECT TicketNumber, DATE_FORMAT(TimeIn, 'Short Time') AS TimeIn, DATE_FORMAT(TimeOut, 'Short Time') AS TimeOut, ParkingFee, ParkingDate FROM parking_log WHERE ParkingDate = #{selectedDate.ToShortDateString()}#";
                string selectQuery = $"SELECT TicketNumber, DATE_FORMAT(TimeIn, '%H:%i') AS TimeIn, DATE_FORMAT(TimeOut, '%H:%i') AS TimeOut, ParkingFee, ParkingDate FROM parking_log WHERE ParkingDate = '{formattedDate}'";

                da = new MySqlDataAdapter(selectQuery, myConn);

                ds = new DataSet();
                myConn.Open();
                da.Fill(ds, "parking_log");
                dgvParkingLog.DataSource = ds.Tables["parking_log"];
                myConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message + ex.StackTrace);
            }
        }
        private void lbBooking_Click(object sender, EventArgs e)
        {
            tbxTicket.Hide();
            tbxParkingTicket.Show();
            panel3.BackColor = Color.FromArgb(95, 74, 211);
            panel4.BackColor = Color.Silver;
            panel1.BackColor = Color.Silver;
            lbBooking.ForeColor = Color.DimGray;
            lbLog.ForeColor = Color.Gray;
            lbSales.ForeColor = Color.Gray;

            try
            {
                myConn = new MySqlConnection("server=127.0.0.1;port=3306;username=root;password=;database=parkeasedatabase;");

                da = new MySqlDataAdapter("SELECT ParkingCode, UserID, TicketNumber, UserType, VehicleType, Status FROM bookings", myConn);

                ds = new DataSet();
                myConn.Open();
                da.Fill(ds, "bookings");
                dgvParkingLog.DataSource = ds.Tables["bookings"];
                myConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void lbSales_Click(object sender, EventArgs e)
        {
            panel1.BackColor = Color.FromArgb(95, 74, 211);
            panel4.BackColor = Color.Silver;
            panel3.BackColor = Color.Silver;
            lbSales.ForeColor = Color.DimGray;
            lbBooking.ForeColor = Color.Gray;
            lbLog.ForeColor = Color.Gray;

            try
            {
                myConn = new MySqlConnection("server=127.0.0.1;port=3306;username=root;password=;database=parkeasedatabase;");

                da = new MySqlDataAdapter("SELECT ParkingDate, TotalSales FROM sales_report", myConn);

                ds = new DataSet();
                myConn.Open();
                da.Fill(ds, "sales_report");
                dgvParkingLog.DataSource = ds.Tables["sales_report"];
                myConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void datetimepicker_ValueChanged(object sender, EventArgs e)
        {
            tbxTicket.Show();
            tbxParkingTicket.Hide();
            panel4.BackColor = Color.FromArgb(95, 74, 211);
            panel3.BackColor = Color.Silver;
            panel1.BackColor = Color.Silver;
            lbLog.ForeColor = Color.DimGray;
            lbBooking.ForeColor = Color.Gray;
            lbSales.ForeColor = Color.Gray;
            try
            {
                myConn = new MySqlConnection("server=127.0.0.1;port=3306;username=root;password=;database=parkeasedatabase;");

                DateTime selectedDate = datetimepicker1.Value.Date;
                string formattedDate = selectedDate.ToString("yyyy-MM-dd");

              //  string selectQuery = $"SELECT TicketNumber, Format(TimeIn, 'Short Time') AS TimeIn, Format(TimeOut, 'Short Time') AS TimeOut, ParkingFee, ParkingDate FROM parking_log WHERE ParkingDate = #{selectedDate.ToShortDateString()}#";
                string selectQuery = $"SELECT TicketNumber, DATE_FORMAT(TimeIn, '%H:%i') AS TimeIn, DATE_FORMAT(TimeOut, '%H:%i') AS TimeOut, ParkingFee, ParkingDate FROM parking_log WHERE ParkingDate = '{formattedDate}'";
                da = new MySqlDataAdapter(selectQuery, myConn);

                ds = new DataSet();
                myConn.Open();
                da.Fill(ds, "parking_log");
                dgvParkingLog.DataSource = ds.Tables["parking_log"];
                myConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void tbxParkingTicket_TextChanged(object sender, EventArgs e)
        {
            string searchValue = tbxTicket.Text;
            DateTime selectedDate = datetimepicker1.Value.Date;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT ParkingCode, UserID, TicketNumber, UserType, VehicleType, Status FROM bookings WHERE TicketNumber LIKE @TN";
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection))
                {
                    adapter.SelectCommand.Parameters.AddWithValue("@TN", "%" + searchValue + "%");
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet);
                    dgvParkingLog.DataSource = dataSet.Tables[0];
                }
            }
        }
    }
}

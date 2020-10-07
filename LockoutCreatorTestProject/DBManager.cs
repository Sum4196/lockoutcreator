using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Diagnostics;

namespace LockoutCreator
{
    class DBManager
    {
        public static string GetDatabaseFilePath()
        {
            string dbFile = Program.GlobalVars.dbFile;
            if (File.Exists(Directory.GetCurrentDirectory() + "\\HIGH_VOLTAGE LOCKOUTS.mdb"))
            {
                dbFile = (String)Directory.GetCurrentDirectory() + "\\HIGH_VOLTAGE LOCKOUTS.mdb";
            }
            else if (File.Exists("G:\\Elec\\Lockouts\\HIGH_VOLTAGE LOCKOUTS.mdb"))
            {
                dbFile = "G:\\Elec\\Lockouts\\HIGH_VOLTAGE LOCKOUTS.mdb";
            }
            else if (File.Exists("G:\\Lockouts\\HIGH_VOLTAGE LOCKOUTS.mdb"))
            {
                dbFile = "G:\\Lockouts\\HIGH_VOLTAGE LOCKOUTS.mdb";
            }
            else if (File.Exists("G:\\HIGH_VOLTAGE LOCKOUTS.mdb"))
            {
                dbFile = "G:\\HIGH_VOLTAGE LOCKOUTS.mdb";
            }
            else
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.InitialDirectory = (String)Directory.GetCurrentDirectory();
                    openFileDialog.Filter = "Access Database Files (*.mdb, *.accdb)|*.mdb;*.accdb";
                    openFileDialog.FilterIndex = 1;
                    openFileDialog.RestoreDirectory = true;
                    DialogResult result = openFileDialog.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        //Get the path of specified file
                        dbFile = openFileDialog.FileName;
                        openFileDialog.Dispose();
                    }
                    if (result != DialogResult.OK)
                    {
                        // Messagebox saying that a database needs to be selected to run the program, then close.
                        MessageBox.Show("A database must be selected in order to use this program.  The program will now close.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        openFileDialog.Dispose();
                        Environment.Exit(0);
                    }
                }
            }

            return dbFile;
        }

        public static bool TestConnectToElecDB(string databaseFilePath)
        {
            bool connection = false;
            string connTestString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + databaseFilePath + ";";
            string queryTestString = "SELECT * FROM LOCKOUT;";

            // start connection
            using (OleDbConnection connectionTest = new OleDbConnection(connTestString))
            {
                // create command to execute.  IE: what query to run on what connection string.
                OleDbCommand command = new OleDbCommand(queryTestString, connectionTest);

                // attempt to connect with try/catch
                try
                {
                    connectionTest.Open();
                    connection = true;
                    connectionTest.Close();
                }
                catch (Exception e)
                {
                    // Display error to user here.
                    MessageBox.Show("Error: Connection to the database has failed.  Please ensure you have the Access Database driver installed and the database you are accessing is closed.  The following message may help in solving the issue.");
                    MessageBox.Show("Error Message (TestConnectToElecDB): " + e);
                    connection = false;
                }
            }

            return connection;

        }

        public static DataTable GetLockoutIDs(string databaseFilePath)
        {
            string connString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + databaseFilePath + ";";
            string queryString = "SELECT LOCKID FROM LOCKOUT;";
            DataTable lockoutDataTable = new DataTable();

            using (OleDbConnection connection = new OleDbConnection(connString))
            {
                OleDbCommand command = new OleDbCommand(queryString, connection);

                try
                {
                    connection.Open();

                    // create data adapter
                    OleDbDataAdapter da = new OleDbDataAdapter(command);
                    da.Fill(lockoutDataTable);
                    connection.Close();
                    da.Dispose();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error Message (GetLockoutIDs): ", e);
                }
            }

            return lockoutDataTable;

        }

        public static DataTable GetLockoutDataFromDB(string databaseFilePath, string queryText)
        {
            string connString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + databaseFilePath + ";";
            DataTable lockoutDataTable = new DataTable();

            using (OleDbConnection connection = new OleDbConnection(connString))
            {
                OleDbCommand command = new OleDbCommand(queryText, connection);

                try
                {
                    connection.Open();
                    // create data adapter
                    // Debugging purposes
                    Console.WriteLine("current variables: " + databaseFilePath + " || query" + queryText);
                    Console.WriteLine("creating database adapter");
                    OleDbDataAdapter da = new OleDbDataAdapter(command);
                    // Debugging purposes
                    Console.WriteLine("filling table object with data");
                    da.Fill(lockoutDataTable);
                    // Debugging purposes
                    Console.WriteLine("closing connection");
                    connection.Close();
                    da.Dispose();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error Message (GetLockoutDataFromDB): ", e);
                }
            }

            return lockoutDataTable;

        }
     
    }
}

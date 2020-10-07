using System;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using System.Diagnostics;
using System.Threading;
//using Word = Microsoft.Office.Interop.Word;

namespace LockoutCreator
{

    public static class Program
    {
        // Global variables that are used throughout the program across files.
        public static class GlobalVars
        {
            public static string dbFile = "";
            public static int progress = 0;
            public static DataTable lockoutIDtable = new DataTable();
            public static LockoutInputForm form1;
            public static bool submitPressed = false;
            public static string documentSaveFileNamePath;
        }

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);

            // Debugging purposes
            Console.WriteLine("Checking .NET Framework version.");
            // Checks for .NET Framework version 4.5 or higher is installed.
            InstallationChecker.CheckForDOTNETFramework();

            // Debugging purposes
            Console.WriteLine("Checking office installation and Access Database Engine installation.");
            // Checks for Access Database Engine Driver and shows link to download if not installed as well as checks for Office installation.
            InstallationChecker.CheckForOfficeInstallation();

            // Debugging purposes
            Console.WriteLine("getting database file path.");
            // Attempt to find database file/get file from user
            GlobalVars.dbFile = DBManager.GetDatabaseFilePath();

            // Debugging purposes
            Console.WriteLine("testing connection to database.");
            // Test connection to database
            DBManager.TestConnectToElecDB(GlobalVars.dbFile);

            // Debugging purposes
            Console.WriteLine("getting user input.");
            // Get user input
            Application.Run(GlobalVars.form1 = new LockoutInputForm());

        }

        // Helper function for the program that gathers data and starts the document creation.
        public static void ProgramHelper(String lockoutID, String lockoutDate, String lockTime, String unlockTime)
        {
            // Initiates local variables being used in the ProgramHelper function.
            string dbFile = GlobalVars.dbFile;
            string lockoutIDinput = lockoutID;
            DateTime lockoutDateinput;
            DateTime lockTimeinput;
            DateTime unlockTimeinput;

            // Checks if the lockoutID given is null or empty (which was already done in the form file), but if it somehow got past that, this will catch it.
            if (String.IsNullOrEmpty(lockoutIDinput) == true)
            {
                //Console.WriteLine("The lockout id was not entered.  Please enter a lockout id.");
                MessageBox.Show("The lockout id was not entered.  Please enter a lockout id.", "Lockout ID Not Entered", MessageBoxButtons.OK);
                return;
            }

            // These three if/else statements set the values to the current date/time if no date/time was given in the form.
            if (String.IsNullOrEmpty(lockoutDate) == true) { lockoutDateinput = DateTime.Now; }
            else { lockoutDateinput = DateTime.Parse(lockoutDate); }

            if (String.IsNullOrEmpty(lockTime) == true) { lockTimeinput = DateTime.Now; }
            else { lockTimeinput = DateTime.Parse(lockTime); }

            if (String.IsNullOrEmpty(unlockTime) == true) { unlockTimeinput = DateTime.Now; }
            else { unlockTimeinput = DateTime.Parse(unlockTime); }

            // Normalizes the text for easy comparing of given lockoutID to actual lockout IDs in the database.
            string lockoutIDUpper = lockoutIDinput.ToUpper();

            // Gets list of lockout IDs to then use and make sure that the lockout ID given is in the database.
            DataTable resultTable = DBManager.GetLockoutIDs(dbFile);
            bool contains = resultTable.AsEnumerable().Any(row => lockoutIDUpper == row.Field<String>("LOCKID").ToUpper());

            // Gets table data from Access database using given lockoutID.
            string queryText = "SELECT '" + lockoutIDinput + "' FROM LOCKOUT;";
            DataTable lockoutIDCheck = DBManager.GetLockoutDataFromDB(dbFile, queryText);

            // Secondary check to ensure that the lockout ID exists in the database and that there is data for that lockout ID and not a blank entry.
            // If the lockout ID is not found AND/OR there is no data for that lockout ID, the program exits.
            if (lockoutIDCheck == null || contains == false)
            {
                //Console.WriteLine($"The lockout id '{lockoutIDinput}' could not be found.  Please rerun the program and try again or check your database file to ensure the lockout id exists.");
                MessageBox.Show($"The lockout id '{lockoutIDinput}' could not be found.  Please rerun the program and try again or check your database file to ensure the lockout id exists.");
                Environment.Exit(0);
            }
            else
            {
                // Debugging purposes
                Console.WriteLine("Getting data from database.");

                // Get data from database for Word document creation.
                DataTable lockoutDataTable = DBManager.GetLockoutDataFromDB(dbFile, $"SELECT LOCKTEXT.ITEM AS PRINTITEM, IIf(LOCKTEXT.[ACTION] IS NULL,TEXT,[ACTION].[ACTION] & '.  ' & LOCATION.LOCATION & ' ' & TEXT) AS PRINTDESC, IIf(LOCKTEXT.LINECONTENTS IS NULL AND LOCKTEXT.[ACTION]=0,'ELEC',VOLTAGE) AS PRLC, LOCKTEXT.ISOL AS PRISOL, LOCKTEXT.LOCK AS PRLOCKBY, LOCKTEXT.UNLOCK AS PRUNLOCKBY FROM (REVIEW RIGHT JOIN LOCKOUT ON REVIEW.RECID = LOCKOUT.REVIEW) LEFT JOIN((LOCATION RIGHT JOIN([ACTION] RIGHT JOIN LOCKTEXT ON [ACTION].[RECID] = [LOCKTEXT].[ACTION]) ON LOCATION.RECID = LOCKTEXT.LOCATION) LEFT JOIN VOLTAGES ON LOCKTEXT.LINECONTENTS = VOLTAGES.VOLTID) ON LOCKOUT.LOCKID = LOCKTEXT.LOCKID WHERE UCASE(LOCKOUT.LOCKID)='{lockoutIDinput}' AND(LOCKTEXT.ITEM <> 0 OR NOT NULL) ORDER BY LOCKTEXT.ITEM;");
                DataTable lockoutInfoTable = DBManager.GetLockoutDataFromDB(dbFile, $"SELECT LOCKOUT.LOCKID, LOCKTEXT.ITEM, LOCKOUT.WORK_LOC AS AREA, LOCKOUT.LOCKS, LOCKOUT.WORK_DESC AS HEADING, LOCKTEXT.ITEM AS PRINTITEM, REVIEW.REVIEW, IIf(LOCKTEXT.[ACTION] Is Null, TEXT, [ACTION].[ACTION] & LOCATION.LOCATION & '.   ' & TEXT) AS PRINTDESC, IIf(LOCKTEXT.LINECONTENTS Is Null And LOCKTEXT.[ACTION] = 0, 'ELEC', VOLTAGE) AS PRLC, LOCKTEXT.ISOL AS PRISOL, LOCKTEXT.LOCK AS PRLOCKBY, LOCKTEXT.UNLOCK AS PRUNLOCKBY, VOLTAGES.VOLTAGE FROM(REVIEW RIGHT JOIN LOCKOUT ON REVIEW.RECID = LOCKOUT.REVIEW) LEFT JOIN((LOCATION RIGHT JOIN ([ACTION] RIGHT JOIN LOCKTEXT ON [ACTION].RECID = LOCKTEXT.[ACTION]) ON LOCATION.RECID = LOCKTEXT.LOCATION) LEFT JOIN VOLTAGES ON LOCKTEXT.LINECONTENTS = VOLTAGES.VOLTID) ON LOCKOUT.LOCKID = LOCKTEXT.LOCKID WHERE LOCKOUT.LOCKID = '{lockoutIDinput}' AND (LOCKTEXT.ITEM <> 0 OR NOT NULL) ORDER BY LOCKTEXT.ITEM;");

                // Debugging purposes
                Console.WriteLine("creating word document.");

                // Starts the document creation section.
                DocumentCreation.CreateDocument(lockoutDataTable, lockoutInfoTable, lockoutID, lockoutDateinput, lockTimeinput, unlockTimeinput);

                /* Doesn't seem to work for now because of thread issues.
                // Open word document after creation in a new process, allowing this program to finish and close and allows the user to edit the lockout right away without having to go to their Documents folder.
                //System.Diagnostics.Process.Start(GlobalVars.documentSaveFileNamePath);
                Word.Application ap = new Word.Application();
                ap.Visible = true;
                Word.Document document = ap.Documents.Open(GlobalVars.documentSaveFileNamePath);*/

                // After document is created, the program is closed.
                Environment.Exit(0);
            }
        }
    }
}
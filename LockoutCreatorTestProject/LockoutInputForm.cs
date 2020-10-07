using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LockoutCreator
{
    public partial class LockoutInputForm : Form
    {
        // Local variables that are used in local functions of this form.
        BackgroundWorker bWorker;
        string lockoutID;
        string lockoutDate;
        string lockTime;
        string unlockTime;

        public LockoutInputForm()
        {
            // Initializes the form and the background worker for the progress bar to function while the document is being created.
            InitializeComponent();
            bWorker = new BackgroundWorker();
            bWorker.DoWork += new DoWorkEventHandler(BWorker_DoWork);
            bWorker.ProgressChanged += new ProgressChangedEventHandler(BWorker_ProgressChanged);
            //bWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bWorker_RunWorkerCompleted);
            bWorker.WorkerReportsProgress = true;

            // Set lockout date to date format (minus time)
            lockoutDatePicker.Format = DateTimePickerFormat.Short;

            // Set lock and unlock time DateTimePicker boxes to short date and time format
            lockTimePicker.Format = DateTimePickerFormat.Custom;
            lockTimePicker.CustomFormat = "M/d/yyyy h:mmtt";
            unlockTimePicker.Format = DateTimePickerFormat.Custom;
            unlockTimePicker.CustomFormat = "M/d/yyyy h:mmtt";

            // This grabs a list of the lockout IDs from the DBManager.GetLockoutIDs function and adds them to the combobox list items with a for loop.
            DataTable lockoutIDs = DBManager.GetLockoutIDs(Program.GlobalVars.dbFile);
            for (int i = 0; i < lockoutIDs.Rows.Count; i++) { lockoutIDComboBox.Items.Add(lockoutIDs.Rows[i][0]); }

        }

        // Background worker completed function that isn't being used, but may be helpful in the future if you wanted to run a task after the progress bar gets to 100%.
        /*private void bWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            throw new NotImplementedException();
        } */

        // Background worker code that runs when the progress is changed which is determined by the bWorker.Async function called later on.
        // Updates the progress bar percentage and label with percentage.
        private void BWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pBar1.Value = Program.GlobalVars.progress;
            lblStatus.Text = pBar1.Value.ToString() + "%";
        }

        // Background worker function that is called when async is called, which ultimately calls the Program.ProgramHelper function.
        private void BWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Program.ProgramHelper(lockoutID, lockoutDate, lockTime, unlockTime);
        }

        // When submit button clicked, pulls user input for use in the Word document.
        private void SubmitButton_Click(object sender, EventArgs e)
        {
            lockoutID = lockoutIDComboBox.Text;
            if (String.IsNullOrEmpty(lockoutDatePicker.Text) == true) { lockoutDate = DateTime.Today.ToString(); }
            else { lockoutDate = lockoutDatePicker.Text; }
            if (String.IsNullOrEmpty(lockTimePicker.Text) == true) { lockTime = DateTime.Now.ToString(); }
            else { lockTime = lockTimePicker.Text; }
            if (String.IsNullOrEmpty(unlockTimePicker.Text) == true) { unlockTime = DateTime.Now.ToString(); }
            else { unlockTime = unlockTimePicker.Text; }

            // Allows user to not enter a lockout ID and retry without having to rerun the program.  This disables all user input if the lockout ID given
            //  is in the list, otherwise it does nothing which allows the user to choose another lockout ID.
            if (String.IsNullOrEmpty(lockoutID) != true)
            {
                bWorker.RunWorkerAsync();
                Program.GlobalVars.submitPressed = true;
                submitButton.Enabled = false;
                lockoutIDComboBox.Enabled = false;
                lockoutDatePicker.Enabled = false;
                lockTimePicker.Enabled = false;
                unlockTimePicker.Enabled = false;
            }
        }

        // When called, updates the progress bar on the UI using the background worker.
        public void SetProgress(int progress)
        {
            bWorker.ReportProgress(progress);
            //pBar1.Value = progress;
            //pBar1.Invalidate();
            //pBar1.Update();
            //pBar1.Refresh();
            //Application.DoEvents();
        }

        // Disables the closing of the form while the Word document is being created. Otherwise, the form can be closed normally.
        private void LockoutInputForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Program.GlobalVars.submitPressed == true)
            {
                e.Cancel = true;
            }
        }
    }
}

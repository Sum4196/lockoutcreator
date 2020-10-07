using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;
using Microsoft.Office.Core;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LockoutCreator
{
    public static class DocumentCreation
    {
        public static void CreateDocument(System.Data.DataTable lData, System.Data.DataTable lInfo, String lockoutID, DateTime lockoutDate, DateTime lockTime, DateTime unlockTime)
        {
            // Debugging purposes
            Console.WriteLine("Starting word document creation.");

            // Starts the progress bar and sets the progress to 1%.
            Program.GlobalVars.progress = 1;
            Program.GlobalVars.form1.SetProgress(Program.GlobalVars.progress);

            // Missing object variable that is used for filling in required function arguments for Word functions that are not necessary to be populated for this circumstance.
            object missing = System.Reflection.Missing.Value;
            //object endOfDoc = "\\endofdoc"; /* \endofdoc is a predefined bookmark; may be used in the future, just uncomment if needed for adding anything to the end of the word document. */

            // Debugging purposes
            Console.WriteLine("Starting word.");

            //Start Word and create a new document based on the template in the Resources folder inside this project's folder.
            Word._Application wordApp;
            Word._Document doc;

            // Debugging purposes
            Console.WriteLine("Initializing word object.");
            wordApp = new Word.Application();
            Console.WriteLine("wordApp: " + wordApp);

            // checking to see if i can get the office version this way
            Console.WriteLine("word version: " + wordApp.Version); //returns number, like 16.0, but not bit value

            // Debugging purposes
            Console.WriteLine("setting word to invisible.");
            wordApp.Visible = false; // set to false before releasing.
            // Debugging purposes
            Console.WriteLine("loading template.");
            Console.WriteLine("path1: " + System.AppDomain.CurrentDomain.BaseDirectory);
            Console.WriteLine("path2: " + System.AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\lockoutTemplate.dot");
            Console.WriteLine("docTemplate path: " + Path.GetFullPath(System.AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\lockoutTemplate.dot"));

            object docTemplate = Path.GetFullPath(System.AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\lockoutTemplate.dot");
            
            // Debugging purposes
            Console.WriteLine("creating document from template.");
            Console.WriteLine("output: " + wordApp.Documents.Add(ref docTemplate, ref missing, ref missing, ref missing));
            doc = wordApp.Documents.Add(ref docTemplate, ref missing, ref missing, ref missing);

            // Debugging purposes
            Console.WriteLine("Blank Word document created from template.");

            // Sets the progress to 15%.
            Program.GlobalVars.progress = 15;
            Program.GlobalVars.form1.SetProgress(Program.GlobalVars.progress);

            // Creates a new Word.Table object at the given table in the template so that we can modify that table with the following code such as changing the font name to fit the rest of the document.
            Word.Table dataTable = doc.Tables[4];
            dataTable.Range.Font.Name = "Arial";

            // Sets the progress to 20%.
            //Program.GlobalVars.progress = 20;
            //Program.GlobalVars.form1.SetProgress(Program.GlobalVars.progress);

            // Debugging purposes
            Console.WriteLine("Starting table creation.");

            // Populates the table in word with data from database.
            for (int i = 0; i < lData.Rows.Count; i++)
            {
                if (i <= 15) { Program.GlobalVars.progress = 15; }
                else if (i > 15 && i < 40) { Program.GlobalVars.progress = i; }
                else if (i >= 40) { Program.GlobalVars.progress = 40; }

                Program.GlobalVars.form1.SetProgress(Program.GlobalVars.progress);

                for (int k = 0; k < 6; k++)
                {
                    dataTable.Cell(i+2, k+1).Range.Text = lData.Rows[i][k].ToString();
                }
                dataTable.Rows.Add(ref missing);
            }

            //Removes extra row at the end of the table.
            dataTable.Rows.Last.Delete();

            // Sets the progress bar to 40%.
            //Program.GlobalVars.progress = 40;
            //Program.GlobalVars.form1.SetProgress(Program.GlobalVars.progress);

            // Debugging purposes
            Console.WriteLine("Starting table shading loop.");

            //Shades the table cells that contain "//" in the word document.
            for (int i = 0; i <= dataTable.Rows.Count; i++)
            {
                if (i <= 41) { Program.GlobalVars.progress = 41; }
                else if(i > 41 && i < 94) { Program.GlobalVars.progress = i; }
                else if(i >= 94) { Program.GlobalVars.progress = 94; }
                
                Program.GlobalVars.form1.SetProgress(Program.GlobalVars.progress);

                for (int k=0; k <= dataTable.Columns.Count; k++)
                {
                    if (dataTable.Cell(i, k).Range.Text.Contains("//"))
                    {
                        dataTable.Cell(i, k).Range.Text = "";
                        dataTable.Cell(i, k).Shading.ForegroundPatternColor = Word.WdColor.wdColorGray375;
                    }
                }
            }

            // Sets the progress to 95%.
            Program.GlobalVars.progress = 95;
            Program.GlobalVars.form1.SetProgress(Program.GlobalVars.progress);

            // Debugging purposes
            Console.WriteLine("Setting word bookmarks to specified values.");

            // Sets the text at the bookmarked locations in the word document with default values if none given, or with the values given by the user from the form.
            doc.Bookmarks["LockoutDate"].Range.Text = String.Format("{0:MM/dd/yyyy}", lockoutDate);
            doc.Bookmarks["LockTime"].Range.Text = lockTime.ToString("g");
            doc.Bookmarks["UnlockTime"].Range.Text = unlockTime.ToString("g");
            doc.Bookmarks["LockoutID"].Range.Text = lockoutID;
            doc.Bookmarks["PrintDateTime"].Range.Text = DateTime.Now.ToString();
            doc.Bookmarks["Title1"].Range.Text = lInfo.Rows[0][2].ToString();
            doc.Bookmarks["Title2"].Range.Text = lInfo.Rows[0][4].ToString();
            doc.Bookmarks["Title3"].Range.Text = lInfo.Rows[0][6].ToString();
            doc.Bookmarks["LocksNeeded"].Range.Text = lInfo.Rows[0][3].ToString();

            // Creates the full file path to the user's documents folder and replacing characters that aren't allowed for Windows filenames.
            object documentSaveFileName = Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents\\") + (lockoutID + "--" + DateTime.Now.ToString()).Replace("/", "-").Replace(" ", "_").Replace(":", "-");
            Program.GlobalVars.documentSaveFileNamePath = documentSaveFileName.ToString();

            // Debugging purposes
            Console.WriteLine("Saving word document.");

            // Saves the word document to the user's documents folder
            doc.SaveAs2(ref documentSaveFileName, ref missing, ref missing, ref missing, ref missing, ref missing,
            ref missing, ref missing, ref missing, ref missing, ref missing,
            ref missing, ref missing, ref missing, ref missing, ref missing);

            // Closes the word document and sets the variable to null as well as closing Microsoft Word and sets that variable to null as well.
            doc.Close();
            doc = null;
            wordApp.Quit();
            wordApp = null;

            // Sets the progress to 100% as we are now done, waits one second before displaying the messagebox showing the user where the file was saved.
            Program.GlobalVars.progress = 100;
            Program.GlobalVars.form1.SetProgress(Program.GlobalVars.progress);
            System.Threading.Thread.Sleep(1000);
            MessageBox.Show("The lockout document has been saved to: " + documentSaveFileName + ".docx" + "\nThe program will now close.");

        }
    }
}

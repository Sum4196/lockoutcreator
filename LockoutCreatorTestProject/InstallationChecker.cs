using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace LockoutCreator
{
    class InstallationChecker
    { 
        // Check all possible office installation folders for Word and Access IF NEEDED after some testing.
        // list of all default installation directories of office below.

        /*
            Office 2007
            	Windows 64-bit:
            		C:\Program Files\Microsoft Office\Office12\
            	Windows 32-bit:
            		C:\Program Files (x86)\Microsoft Office\Office12\
            
            Office 2010
            	Windows 64-bit:
            		C:\Program Files\Microsoft Office\Office14\
            	Windows 32-bit:
            		C:\Program Files (x86)\Microsoft Office\Office14\
            
            Click-To-Run
            	Windows 64-bit:
            		C:\Program Files\Microsoft Office 14\ClientX64\Root\Office14\
            	Windows 32-bit:
            		C:\Program Files (x86)\Microsoft Office 14\ClientX86\Root\Office14\
            
            Office 2013
            	Windows 64-bit:
            		C:\Program Files\Microsoft Office\Office15\
            	Windows 32-bit:
            		C:\Program Files (x86)\Microsoft Office\Office15\
            
            Click-To-Run
            	Windows 64-bit:
            		C:\Program Files\Microsoft Office 15\ClientX64\Root\Office15\
            	Windows 32-bit:
            		C:\Program Files (x86)\Microsoft Office 15\ClientX86\Root\Office15\
            
            Office 2016
            	Windows 64-bit:
            		C:\Program Files\Microsoft Office\Office16\
            	Windows 32-bit:
            		C:\Program Files (x86)\Microsoft Office\Office16\
            
            Click-To-Run
            	Windows 64-bit:
            		C:\Program Files\Microsoft Office 16\ClientX64\Root\Office16\
            	Windows 32-bit:
            		C:\Program Files (x86)\Microsoft Office 16\ClientX86\Root\Office16\
         */

        /*  Office 2007 - Office 365
            C:\Program Files\Microsoft Office\Office12\
            C:\Program Files (x86)\Microsoft Office\Office12\
            C:\Program Files\Microsoft Office\Office14\
            C:\Program Files (x86)\Microsoft Office\Office14\
            C:\Program Files\Microsoft Office 14\ClientX64\Root\Office14\
            C:\Program Files (x86)\Microsoft Office 14\ClientX86\Root\Office14\
            C:\Program Files\Microsoft Office\Office15\
            C:\Program Files (x86)\Microsoft Office\Office15\
            C:\Program Files\Microsoft Office 15\ClientX64\Root\Office15\
            C:\Program Files (x86)\Microsoft Office 15\ClientX86\Root\Office15\
            C:\Program Files\Microsoft Office\Office16\
            C:\Program Files (x86)\Microsoft Office\Office16\
            C:\Program Files\Microsoft Office 16\ClientX64\Root\Office16\
            C:\Program Files (x86)\Microsoft Office 16\ClientX86\Root\Office16\
            C:\Program Files\Microsoft Office\root\Office16\
        */

        // Return true if Office is installed.
        public static void CheckForOfficeInstallation()
        {
            string officeBitness = null;
            // Check for bitness of office 2016+
            if (String.IsNullOrEmpty(officeBitness))
            {
                officeBitness = (String)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Office\ClickToRun\Configuration", "Platform", null);
            }
            // Office 2016
            if (String.IsNullOrEmpty(officeBitness))
            {
                officeBitness = (String)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Office\16.0\Outlook", "Bitness", null);
            }
            // Office 2013
            if (String.IsNullOrEmpty(officeBitness))
            {
                officeBitness = (String)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Office\15.0\Outlook", "Bitness", null);
            }
            // Office 2010
            if (String.IsNullOrEmpty(officeBitness))
            {
                officeBitness = (String)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Office\14.0\Outlook", "Bitness", null);
            }
            // Debugging purposes
            Console.WriteLine("officeBitness: " + officeBitness);

            // Uses the result of officeBitness to determine if office is not installed.
            if (officeBitness == null)
            {
                DialogResult officeMsgBox = MessageBox.Show("Office installation not found.  Please ensure you have Microsoft Office 2010 or later installed.  Would you like to continue anyway? (May not work)", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if(officeMsgBox == DialogResult.Yes)
                {
                    ;
                }
                else
                {
                    Environment.Exit(0);
                }
            }

            // Gets a boolean variable result from the CheckForAccessDBEngine function.
            bool accessDBEngineInstalled = CheckForAccessDBEngine();

            // Uses accessDBEngineInstalled boolean to determine if the Access Database Driver needs to be installed or not and uses the previous officeBitness variable to determine which version of the Access Database Engine needs to be installed.
            if (accessDBEngineInstalled == false)
            {
                if (officeBitness == "x64")
                {
                    // 64-bit Access Database Engine download message box
                    DialogResult result = MessageBox.Show("The Access Database Engine is not installed.\nWould you like to download it now? (64-bit)\nThe application will now close.", "Access Database Engine Error", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        // Opens the default browser with the given URL to download the 64-bit version of the Access Database Engine driver and closes the program.
                        System.Diagnostics.Process.Start("https://download.microsoft.com/download/2/4/3/24375141-E08D-4803-AB0E-10F2E3A07AAA/AccessDatabaseEngine_X64.exe");
                        Environment.Exit(0);
                    }
                    else { Environment.Exit(0); }

                }
                else if (officeBitness == "x86")
                {
                    // 32-bit Access Database Engine download message box
                    DialogResult result = MessageBox.Show("The Access Database Engine is not installed.\nWould you like to download it now? (32-bit)\nThe application will now close.", "Access Database Engine Error", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        // Opens the default browser with the given URL to download the 32-bit version of the Access Database Engine driver and closes the program.
                        System.Diagnostics.Process.Start("https://download.microsoft.com/download/2/4/3/24375141-E08D-4803-AB0E-10F2E3A07AAA/AccessDatabaseEngine.exe");
                        Environment.Exit(0);
                    }
                    else { Environment.Exit(0); }

                }
                else
                {
                    // Office bitness was unable to be determined, and the Access database engine driver was not found.
                    DialogResult accessDBEngineresult = MessageBox.Show("The Access Database Engine driver was not found and the version of Microsoft Office could not be determined.  Please install the Access Database Engine driver (2010) in order to use this software.\n\n32-bit: Click 'Yes' to download the 32-bit Access Database Engine driver. \n\n64-bit: Click 'No' to download the 64-bit Access Database Engine driver.", "Error", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error);
                    if (accessDBEngineresult == DialogResult.Yes)
                    {
                        // 32-bit
                        System.Diagnostics.Process.Start("https://download.microsoft.com/download/2/4/3/24375141-E08D-4803-AB0E-10F2E3A07AAA/AccessDatabaseEngine.exe");
                        Environment.Exit(0);
                    }
                    else if (accessDBEngineresult == DialogResult.No)
                    {
                        // 64-bit
                        System.Diagnostics.Process.Start("https://download.microsoft.com/download/2/4/3/24375141-E08D-4803-AB0E-10F2E3A07AAA/AccessDatabaseEngine_X64.exe");
                        Environment.Exit(0);
                    }
                    else if (accessDBEngineresult == DialogResult.Cancel)
                    {
                        // quit program
                        Environment.Exit(0);
                    }
                    else
                    {
                        // quit program
                        Environment.Exit(0);
                    }
                }

            }
        }

        // Checks to see if the Access Database Engine driver is installed.
        public static bool CheckForAccessDBEngine()
        {
            // Initializes necessary variables and grabs the registry key needed to determine if the driver is installed or not.
            string AccessDBAsValue = "";
            RegistryKey rkACDBKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Classes\Installer\Products");
            bool llretval = false;

            // If the registry key is empty or doesn't exist, the driver isn't installed, otherwise it searches to make sure that it's actually installed and not just leftover from a previous installation.
            if (rkACDBKey != null)
            {
                int lnSubKeyCount = 0;
                lnSubKeyCount = rkACDBKey.SubKeyCount;
                foreach (string subKeyName in rkACDBKey.GetSubKeyNames())
                {
                    using (RegistryKey RegSubKey = rkACDBKey.OpenSubKey(subKeyName))
                    {
                        foreach (string valueName in RegSubKey.GetValueNames())
                        {
                            if (valueName.ToUpper() == "PRODUCTNAME")
                            {
                                AccessDBAsValue = (string)RegSubKey.GetValue(valueName.ToUpper());
                                if (AccessDBAsValue.Contains("Access database engine"))
                                {
                                    llretval = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (llretval)
                    {
                        break;
                    }
                }
            }

            // Returns a boolean (true/false) if the driver was found or not.
            return llretval;
        }

        public static void CheckForDOTNETFramework()
        {
            // Check for .net framework version 4.5+
            string dotnetVersion = Convert.ToString(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full", "Version", null));
            var netver = dotnetVersion.Split(new[] { '.' }, 3);
            double dotNetVersionAsDouble = Convert.ToDouble(string.Join("", netver[0] + "." + netver[1]));
            
            if (dotNetVersionAsDouble >= (double)4.5)
            {
                ;
            }
            else
            {
                MessageBox.Show("The .NET Framework, version 4.5 or higher, is required to run this program.  Please install .NET Framework version 4.5 or higher and re-run the program.");
                Environment.Exit(0);
            }
        }

    }
}

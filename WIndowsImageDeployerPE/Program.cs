﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Windows.Forms;
namespace WIndowsImageDeployerPE
{
    internal class Program
    {
        int left = Console.CursorLeft;
        int top = Console.CursorTop;
        Microsoft.VisualBasic.Devices.ComputerInfo ComputerInfo = new Microsoft.VisualBasic.Devices.ComputerInfo();
        string version = "1.5";
        [STAThread]

        public static void Main(string[] args)
        {


            WIndowsImageDeployerPE.Program program = new WIndowsImageDeployerPE.Program();
            List<string> drive_array = new List<string>();
            int driveindex = 0;
            int drivesel;
            string drivename = " ";
            List<int> disbaledoptions = new List<int>();

            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
            Console.WriteLine("--------Windows 11 Deployer-----------------------");
            Console.WriteLine(Resource1.logo);

            //  Console.WriteLine("Created by Carson Games");
            Console.WriteLine("\nVersion : " + program.version + "\n");
            Console.WriteLine("--------------------------------------------------");
            program.DisplaySystemInfo();
            Console.WriteLine("--------------------------------------------------");





            if (!program.isWinPE())
            {
                Console.WriteLine("\nWinPE Not Detected! Launching ISO Downloader");


                program.Download_Save();

            }


            else
            {
                //if (program.isUEFI())
                //{
                //    Console.WriteLine("UEFI is currently not supported. Program will now exit.");
                //    Environment.Exit(0);

                //}
                while (true)
                {

                    Console.WriteLine("\nSelect a option below.");

                    Console.WriteLine("(1) Install using the currently loaded image.");

                    //Console.WriteLine("(2) Download & Update the current image.");

                    //if (!program.IsConnectedToInternet())
                    //{


                    //}
                    //Console.SetCursorPosition(0, Console.CursorTop - 1);
                    //Console.ForegroundColor = ConsoleColor.DarkGray;
                    //Console.WriteLine("(2) Download & Update the current image. -  No internet ");
                    //disbaledoptions.Add(2);

                    //Console.ForegroundColor = ConsoleColor.White;

                    Console.WriteLine("(2) Exit.");

                    string choice = Console.ReadLine();
                    if (disbaledoptions.Contains(Int32.Parse(choice)))
                    {
                        Console.WriteLine("This option is diabled.");


                    }
                    else
                    {
                        if (choice == "1")
                        {
                            break;
                        }
                        if (choice == "2")
                        {
                            Console.WriteLine("Now exiting. Run exit to reboot.");
                            Environment.Exit(0);

                        }
                    }


                }

                Console.WriteLine("Getting Disks.");


                //    DriveInfo[] drives = DriveInfo.GetDrives();
                //    foreach (DriveInfo drive in drives)
                //    {
                //        if (drive.IsReady)
                //        {
                //            string label = drive.VolumeLabel;
                //            if (label == "" || label == " ")
                //            {
                //                label = "(no label)";
                //            }
                //            drive_array.Add(label + " - " + drive.TotalSize / 1000 / 1024 + "MB");
                //        }
                //    }
                //    foreach (string drive in drive_array)
                //    {
                //        Console.WriteLine("[" + drive_array.IndexOf(drive) + "] " + drive);

                //    }
                //    while (true)
                //    {
                //        try
                //        {
                //            Console.WriteLine("Pick a drive you would like to use.");
                //            int temp = Int32.Parse(Console.ReadLine());

                //            driveindex = temp;
                //            temp = 0;
                //            Console.WriteLine(drive_array[driveindex]);
                //            break;



                //        }
                //        catch (Exception e)
                //        {
                //            Console.WriteLine("Invalid Selection. Try again.");
                //        }
                //    }

                //    drivename = drive_array[driveindex].Split('-')[0];
                //    Console.WriteLine(drivename);
                //    Console.ReadLine();
                //}
                // execute DiskPart programatically
                Process process = new Process();
                process.StartInfo.FileName = "diskpart.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.StandardInput.WriteLine("list disk");
                process.StandardInput.WriteLine("exit");

                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();


                // extract information from output
                string table = output.Split(new string[] { "DISKPART>" }, StringSplitOptions.None)[1];
                var rows = table.Split(new string[] { "\n" }, StringSplitOptions.None);
                //for (int i = 3; i < rows.Length; i++)
                //{
                //    // if (rows[i].Contains("Disk"))
                //    //  {
                //    int index = Int32.Parse(rows[i].Split(new string[] { " " }, StringSplitOptions.None)[3]);
                //    string label = rows[i].Split(new string[] { " " }, StringSplitOptions.None)[16] + " " + rows[i].Split(new string[] { " " }, StringSplitOptions.None)[17];
                //    // long size = 0;


                //foreach (DriveInfo drive in DriveInfo.GetDrives())
                //{

                //    if (drive.IsReady && drive.VolumeLabel == label)
                //    {
                //        size = (long)(drive.TotalSize / 1000 / 1024);
                //    }
                //}
                Console.WriteLine(output);
                //    }



                Console.WriteLine("Select a disk number");
                drivesel = program.GetIndexOfDrive(Console.ReadLine());
                if (drivesel == -1)
                {
                    Console.WriteLine("Invalid Selection.");
                }
                else
                {
                    Console.WriteLine("Are you sure you want to continue with " + drivesel + "?");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ALL DATA ON THAT DRIVE WILL BE FORMATTED!!!");
                    Console.ForegroundColor = ConsoleColor.White;
                    string answer = Console.ReadLine();
                    if (answer == "Y" || answer == "y")
                    {


                    }
                    else
                    {
                        Console.WriteLine("Starting over");
                    }


                    Console.WriteLine("Getting Windows Editions..");

                    Process process1 = new Process();
                    process1.StartInfo.FileName = "dism.exe";
                    process1.StartInfo.Arguments = "/Get-WimInfo /WimFile:install.wim";
                    process1.StartInfo.UseShellExecute = false;
                    process1.StartInfo.CreateNoWindow = true;
                    process1.StartInfo.RedirectStandardInput = true;
                    process1.StartInfo.RedirectStandardOutput = true;
                    process1.Start();
                    string output1 = process1.StandardOutput.ReadToEnd();

                    process1.WaitForExit();

                    Console.WriteLine(output1);

                    Console.WriteLine("Select the edition of windows you would like installed.");
                    answer = Console.ReadLine();

                    int indexsel = int.Parse(answer);








                    Console.WriteLine(" (1/4) Formatting Drive");
                    if (program.Format(drivesel))
                    {

                        Console.WriteLine(" (2/4) Deploying Image - This may take awhile");

                        if (program.Install(indexsel))
                        {

                            Console.WriteLine(" (3/4) Adding BCD Boot Records");
                            if (program.BCDRecords(drivesel))
                            {
                                Console.WriteLine(" (4/4) Copying Update Patch");
                                if (program.CopyPatch())
                                {
                                    Console.WriteLine("Windows 11 has been deployed!\nRebooting ");

                                    process.StartInfo.FileName = "wpeutil.exe";
                                    process.StartInfo.Arguments = "reboot";
                                    process.StartInfo.UseShellExecute = false;
                                    process.StartInfo.CreateNoWindow = true;
                                    process.StartInfo.RedirectStandardInput = true;
                                    process.StartInfo.RedirectStandardOutput = true;
                                    process.Start();

                                    Environment.Exit(0);

                                }
                            }

                        }
                        else
                        {
                            program.Error();
                        }

                    }
                }




            }

        }
        public void Error()
        {
            Console.WriteLine("An error occured while deploying windows! Please look at the above error for more info. \n\n Type 'exit' to reboot.");
            Environment.Exit(0);
        }

        public bool isWinPE()
        {
            using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            using (var key = hklm.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\WinPE"))
            {
                if (key == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        //public void wc_DownloadProgressChanged(Object sender, DownloadProgressChangedEventArgs e)

        //{
        ////    Console.WriteLine("Downloading Image. - " + e.ProgressPercentage + "% complete.");
        ////    Console.SetCursorPosition(0, Console.CursorTop - 1);



        //}
        //public bool wc_DownloadProgressFinsihed(Object sender, DownloadStringCompletedEventArgs e)

        //{

        //    //Console.WriteLine("Completed!");


        //    //return true;

        //}
        //public bool UpdateFile()
        //{

        //    WebClient webClient = new WebClient();
        //    webClient.DownloadProgressChanged += wc_DownloadProgressChanged;

        //    webClient.DownloadFileAsync(new Uri()


        //}



        //public bool IsConnectedToInternet()
        //{
        //    string host = "carsongames.com";
        //    Ping p = new Ping();
        //    try
        //    {
        //        PingReply reply = p.Send(host, 3000);
        //        if (reply.Status == IPStatus.Success)
        //            return true;
        //    }
        //    catch { }
        //    return false;
        //}
        public bool isUEFI()
        {
            if (Environment.GetEnvironmentVariable("firmware_type") == "UEFI" || Environment.GetEnvironmentVariable("firmware_type") == "EFI")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Format(int index)
        {
            Process process = new Process();
            if (isUEFI())
            {
                Console.WriteLine("--Start of format-- ");
                process.StartInfo.FileName = "diskpart.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.StandardInput.WriteLine("select disk " + index);
                Console.WriteLine("  Formatting Drive " + index);
                process.StandardInput.WriteLine("clean");
                Console.WriteLine("  Converting to GPT");

                process.StandardInput.WriteLine("convert gpt");
                Console.WriteLine("  Creating EFI Partition");

                process.StandardInput.WriteLine("create part efi size=500");
                Console.WriteLine("  Formatting EFI as FAT32");

                process.StandardInput.WriteLine("format fs=fat32");
                process.StandardInput.WriteLine("assign letter=s");
                Console.WriteLine("  Creating Windows Partition");

                process.StandardInput.WriteLine("create part pri");
                Console.WriteLine("  Formatting Windows Partition");

                process.StandardInput.WriteLine("format quick fs=ntfs");
                process.StandardInput.WriteLine("assign letter=w");



                process.StandardInput.WriteLine("exit");

                Console.WriteLine("--End of format-- ");

                string output1 = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                return true;
            }
            else
            {
                Console.WriteLine("--Start of format-- ");

                process.StartInfo.FileName = "diskpart.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.StandardInput.WriteLine("select disk " + index);
                Console.WriteLine("  Formatting Drive " + index);
                process.StandardInput.WriteLine("clean");

                Console.WriteLine("  Creating System Partition");

                process.StandardInput.WriteLine("create partition primary size=100");
                Console.WriteLine("  Formatting System as NTFS " + index);

                process.StandardInput.WriteLine("format quick fs=ntfs label=System");
                process.StandardInput.WriteLine("assign letter=S");
                Console.WriteLine("  Creating Windows Partition");

                process.StandardInput.WriteLine("create partition primary");
                Console.WriteLine("  Removing 650 MB from Windows");

                process.StandardInput.WriteLine("shrink minimum=650");
                Console.WriteLine("  Formatting Windows as NTFS " + index);

                process.StandardInput.WriteLine("format quick fs=ntfs label=Windows");
                process.StandardInput.WriteLine("assign letter=W");
                Console.WriteLine("  Creating Recovery Partition");

                process.StandardInput.WriteLine("create partition primary");
                Console.WriteLine("  Formatting Recovery as NTFS " + index);

                process.StandardInput.WriteLine("format quick fs=ntfs label=Recovery");
                process.StandardInput.WriteLine("assign letter=R");
                Console.WriteLine("  Marking Recovery as Hidden " + index);

                process.StandardInput.WriteLine("set id=27");

                process.StandardInput.WriteLine("exit");
                Console.WriteLine("--End of format-- ");


                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                return true;
            }
        }

        public bool Install(int index)
        {

            Process process = new Process();
            process.StartInfo.FileName = "dism.exe";
            process.StartInfo.Arguments = $"/apply-image /imagefile:install.wim /index:{index} /ApplyDir:W:\\";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();



            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($" An Error occured while applying the image. \n\n DISM error {process.ExitCode}");
                Console.ForegroundColor = ConsoleColor.White;

                return false;
            }

            return true;
        }
        void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data.ToString());
        }
        public bool BCDRecords(int index)
        {
            Process process = new Process();
            if (isUEFI())
            {

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.FileName = "x:\\windows\\system32\\bcdboot.exe";
                process.StartInfo.Arguments = "w:\\Windows /s S:";
                process.Start();
                string output1 = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                return true;
            }
            else
            {

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.FileName = "x:\\windows\\system32\\bcdboot.exe";
                process.StartInfo.Arguments = " w:\\windows /s S: /f ALL";
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                return true;
            }
        }
        public bool CopyPatch()
        {
            Directory.CreateDirectory(@"W:\Windows\Setup\Scripts");

            File.Copy("SetupComplete.cmd", @"W:\Windows\Setup\Scripts\SetupComplete.cmd");
            return true;

        }



        public int GetIndexOfDrive(string drive)
        {
            drive = drive.Replace(":", "").Replace(@"\", "");

            // execute DiskPart programatically
            Process process = new Process();
            process.StartInfo.FileName = "diskpart.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            process.StandardInput.WriteLine("list volume");
            process.StandardInput.WriteLine("exit");
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            // extract information from output
            string table = output.Split(new string[] { "DISKPART>" }, StringSplitOptions.None)[1];
            var rows = table.Split(new string[] { "\n" }, StringSplitOptions.None);
            for (int i = 3; i < rows.Length; i++)
            {
                if (rows[i].Contains("Volume"))
                {
                    int index = Int32.Parse(rows[i].Split(new string[] { " " }, StringSplitOptions.None)[3]);
                    string label = rows[i].Split(new string[] { " " }, StringSplitOptions.None)[3];

                    if (label.Equals(drive))
                    {
                        return index;
                    }
                }
            }

            return -1;
        }
        public void DisplaySystemInfo()
        {
            Console.WriteLine("System Information: ");

            Console.WriteLine("System Memory: " + ComputerInfo.TotalPhysicalMemory / 1000 / 1024 + " MB");
            var key = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0\");

            Console.WriteLine("CPU: " + key?.GetValue("ProcessorNameString").ToString() ?? "Not Found");

            Console.WriteLine("Firmware Type : " + Environment.GetEnvironmentVariable("firmware_type"));
        }
        public void Download_Save()
        {
            try
            {



                progress progress = new progress();
                progress.ShowDialog();
                progress.Dispose();
                Console.WriteLine("Finished! Press any key to exit.");
                Console.ReadKey();
                Application.Exit();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }




        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {

            Console.SetCursorPosition(left, top);
            Console.Write("Downloaded Percentage: {0}", e.ProgressPercentage);


        }

        private void Client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {

        }
    }
}


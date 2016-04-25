using System;
using System.Windows.Forms;
using System.Management;
using System.IO;

namespace TheDecryptor
{
    public partial class Form1 : Form
    {
        ManagementObjectCollection USBCollection;
        UsbDrive selectedDrive;
        FileInfo[] availableFiles;

        PasswordBox pb = new PasswordBox();
        int nofilesdecrypted;
        private class UsbDrive
        {
            public String driveLabel;  //name assigned by user
            public String driveLetter;
            public String modelInfo;
            public String serialNumber;

            public override string ToString()
            {
                return driveLetter + " " + driveLabel + " " + modelInfo;
            }

        }

        public Form1()
        {
            InitializeComponent();
            try
            {
                LoadDrives();
                loadAllCriptoredFiles(selectedDrive.driveLetter);
                
            }
            catch (Exception)
            {
                MessageBox.Show("No USB drive attached.");
                
                
            }
            
        }

        public void LoadDrives()  //working one final one
        {

            USBCollection = new ManagementObjectSearcher("select * from Win32_DiskDrive where InterfaceType='USB'").Get();

            foreach (ManagementObject drive in USBCollection)
            {

                try
                {

                    //listBox1.Items.Add("Serial Num : " + drive["SerialNumber"].ToString());


                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    continue;
                }

                foreach (ManagementObject partition in new ManagementObjectSearcher("ASSOCIATORS OF {Win32_DiskDrive.DeviceID='" + drive["DeviceID"] + "'} WHERE AssocClass = Win32_DiskDriveToDiskPartition").Get())
                {

                    // associate partitions with logical disks (drive letter volumes)
                    foreach (ManagementObject disk in new ManagementObjectSearcher("ASSOCIATORS OF {Win32_DiskPartition.DeviceID='" + partition["DeviceID"] + "'} WHERE AssocClass =Win32_LogicalDiskToPartition").Get())
                    {

                        UsbDrive ud = new UsbDrive();
                        ud.driveLabel = (string)disk["VolumeName"];
                        ud.driveLetter = (string)disk["Name"];
                        ud.modelInfo = (string)drive["model"];
                        ud.serialNumber = (string)drive["SerialNumber"];

                        //availableUsbs.Add(ud);


                        comboBox1.Items.Add(ud);
                        comboBox1.SelectedIndex = 0;
                        selectedDrive = (UsbDrive)comboBox1.SelectedItem;

                        //label2.Text = "availablr usbs : " + availableUsbs.Count.ToString();

                    }

                }


            }

        }

        private void loadAllCriptoredFiles(string driveLetter)
        {
            DirectoryInfo di = new DirectoryInfo(@driveLetter);//"C:\"
            FileInfo[] fi = di.GetFiles();
            //FileInfo[] fiFiltered = di.GetFiles("*.encrypt");
            //availableFiles = di.GetFiles("*.encrypt", SearchOption.AllDirectories);

            availableFiles = di.GetFiles("*.encrypt");
            foreach (FileInfo f in availableFiles)
            {
                listBox1.Items.Add(f.FullName);  //Name
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            try
            {

                pb.ShowDialog();

                //MessageBox.Show(pb.pwordok.ToString());

                if (pb.pwordok)
                {

                    UsbDrive ud = (UsbDrive)comboBox1.SelectedItem;
                    //MessageBox.Show(ud.serialNumber + pb.password.Trim());
                    string secretKey = ud.serialNumber + pb.password.Trim();
                    TheDecryptor.MyEncryptor enc = new TheDecryptor.MyEncryptor(secretKey);



                    foreach (FileInfo f in availableFiles)
                    {
                        string fileToDecrypt = f.FullName;
                        int iPosition;
                        iPosition = fileToDecrypt.LastIndexOf(".encrypt");
                        if ((iPosition == -1) || (iPosition != (fileToDecrypt.Length - 8)))
                        {
                            MessageBox.Show("Corrupt file :"+f+". Moving to the nextfile.");
                            continue;
                        }

                        //strOutputFile = the file path minus the last 8 characters (.encrypt)
                        string decryptedFileName = f.Name.Substring(0, fileToDecrypt.Length - 11);
                        //MessageBox.Show(decryptedFileName);
                        //Assign strOutputFile to the position after the last "\" in the path.
                        iPosition = decryptedFileName.LastIndexOf("jnd");
                        if (iPosition == -1)
                        {
                            decryptedFileName = decryptedFileName + ".dat";
                        }
                        else
                        {
                            decryptedFileName = decryptedFileName.Replace("jnd", ".");
                        }

                        //Directory.CreateDirectory("path/to/your/dir");

                        if (!System.IO.Directory.Exists(Path.GetTempPath() + "cryptor"))
                            System.IO.Directory.CreateDirectory(System.IO.Path.GetTempPath() + "cryptor");

                        string temp_path = Path.GetTempPath() + "cryptor" + "\\";
                        //MessageBox.Show(fileToDecrypt + " | " + temp_path + decryptedFileName);
                        //File.SetAttributes(fileToDecrypt, FileAttributes.Normal);
                        try
                        {
                            enc.Decrypt(fileToDecrypt, temp_path + decryptedFileName);
                            nofilesdecrypted++;
                        }
                        catch (Exception ex)
                        {
                            //MessageBox.Show("File : "+f+" either password pr usb didn't match. Moving to the next file.");
                            continue ;
                        }
                        
                        //MessageBox.Show(System.IO.Directory.Exists(Path.GetTempPath() + "cryptor").ToString());
                    }

                    
                    
                    VirtualDriveCreator.MapDrive('X', Path.GetTempPath() + "cryptor");

                    if (nofilesdecrypted != 0)
                    {
                        MessageBox.Show("Files with matching usb and pasword were decryped successfully.");
                        writeLog("SUCCESSFUL", nofilesdecrypted);
                    }

                    else
                    {
                        MessageBox.Show("No files decrypted as didn't match the original usb and pasword.");
                        writeLog("UNSUCCESSFUL", nofilesdecrypted);
                    }
                        
                }
            }
            catch (Exception ex)
            {
                //writeLog("FAILED",nofilesdecrypted);
                MessageBox.Show(ex.Message);
            }
        }

        private void writeLog(string status,int fileCount)
        {
            UsbDrive ud = (UsbDrive)comboBox1.SelectedItem;
            string logpath = ud.driveLetter + "\\" + "criptor_log.txt";
                 

            if (!File.Exists(logpath))
            {
                using (File.Create(Application.StartupPath + @"\Client.config.xml")) ;
                
            }

            File.AppendAllText(logpath, status + " decrypting attempt recorded with " + fileCount + " files at " + DateTime.Now + Environment.NewLine);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            VirtualDriveCreator.UnmapDrive('X');
            System.IO.DirectoryInfo di = new DirectoryInfo(System.IO.Path.GetTempPath() + "cryptor");

            if (di.Exists)
            {


                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }

                System.IO.Directory.Delete(System.IO.Path.GetTempPath() + "cryptor");

            }
            

        }


    }

    
}

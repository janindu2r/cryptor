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
       
        public Form1()
        {
            InitializeComponent();
                LoadDrives();

                if (selectedDrive == null)
                {
                    button1.Enabled = false;
                    return;
                }
                    
                loadAllCriptoredFiles(selectedDrive.driveLetter);
                
        }

        public void LoadDrives()  //working one final one
        {

            USBCollection = new ManagementObjectSearcher("select * from Win32_DiskDrive where InterfaceType='USB'").Get();

            foreach (ManagementObject drive in USBCollection)
            {

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

                        comboBox1.Items.Add(ud);
                        comboBox1.SelectedIndex = 0;
                        selectedDrive = (UsbDrive)comboBox1.SelectedItem;
                    }
                }
            }
        }

        private void loadAllCriptoredFiles(string driveLetter)
        {
            DirectoryInfo di = new DirectoryInfo(@driveLetter);//"C:\"
            FileInfo[] fi = di.GetFiles();

            availableFiles = di.GetFiles("*.ccp");
            listBox1.Items.Clear();
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

                if (pb.pwordok)
                {

                    UsbDrive ud = (UsbDrive)comboBox1.SelectedItem;
                    string secretKey = ud.serialNumber + pb.password.Trim();
                    TheDecryptor.MyEncryptor enc = new TheDecryptor.MyEncryptor(secretKey);

                    foreach (FileInfo f in availableFiles)
                    {
                        string fileToDecrypt = f.FullName;
                        int iPosition;
                        iPosition = fileToDecrypt.LastIndexOf(".ccp");
                        if ((iPosition == -1) || (iPosition != (fileToDecrypt.Length - 4)))
                        {
                            MessageBox.Show("Corrupt file :"+f+". Moving to the nextfile.");
                            continue;
                        }

                        //strOutputFile = the file path minus the last 8 characters (.ccp)
                        string decryptedFileName = f.Name.Substring(0, fileToDecrypt.Length - 7);
                        //MessageBox.Show(decryptedFileName);
                        //Assign strOutputFile to the position after the last "\" in the path.
                        iPosition = decryptedFileName.LastIndexOf("ccp");
                        if (iPosition == -1)
                        {
                            decryptedFileName = decryptedFileName + ".dat";
                        }
                        else
                        {
                            decryptedFileName = decryptedFileName.Replace("ccp", ".");
                        }


                        if (!System.IO.Directory.Exists(Path.GetTempPath() + "cryptor"))
                            System.IO.Directory.CreateDirectory(System.IO.Path.GetTempPath() + "cryptor");

                        string temp_path = Path.GetTempPath() + "cryptor" + "\\";
                        try
                        {
                            enc.Decrypt(fileToDecrypt, temp_path + decryptedFileName);
                            nofilesdecrypted++;
                        }
                        catch (Exception ex)
                        {
                            continue ;
                        }
                        
                    }

                    
                    
                   

                    if (nofilesdecrypted != 0)
                    {
                        VirtualDriveCreator.MapDrive('X', Path.GetTempPath() + "cryptor");
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {            
            selectedDrive = (UsbDrive)comboBox1.SelectedItem;
            
            if(selectedDrive!=null)
                loadAllCriptoredFiles(selectedDrive.driveLetter);
        }

    }
    
}

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
            LoadDrives();
            loadAllCriptoredFiles(selectedDrive.driveLetter);
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
                    MessageBox.Show(e.ToString());
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
                UsbDrive ud = (UsbDrive)comboBox1.SelectedItem;
                MessageBox.Show(ud.serialNumber + txtPword.Text.Trim());
                string secretKey = ud.serialNumber + txtPword.Text.Trim();
                TheDecryptor.MyEncryptor enc = new TheDecryptor.MyEncryptor(secretKey);
            


                foreach (FileInfo f in availableFiles)
                {
                    string fileToDecrypt = f.FullName;
                    int iPosition;
                    iPosition = fileToDecrypt.LastIndexOf(".encrypt");
                    if ((iPosition == -1) || (iPosition != (fileToDecrypt.Length - 8)))
                    {
                        MessageBox.Show("Invalid file. Please select proper encrypted file.");
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
                    
                    string temp_path = Path.GetTempPath()+"cryptor"+"\\";
                    //MessageBox.Show(fileToDecrypt + " | " + temp_path + decryptedFileName);
                    //File.SetAttributes(fileToDecrypt, FileAttributes.Normal);
                    enc.Decrypt(fileToDecrypt, temp_path + decryptedFileName);               
                    //MessageBox.Show(System.IO.Directory.Exists(Path.GetTempPath() + "cryptor").ToString());
                }

                MessageBox.Show("Files decrypted successfully.");
                VirtualDriveCreator.MapDrive('X', Path.GetTempPath()+"cryptor");
                Console.WriteLine(VirtualDriveCreator.GetDriveMapping('X'));
                //VirtualDriveCreator.UnmapDrive('X');

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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

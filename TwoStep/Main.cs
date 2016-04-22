using System;
using System.IO;
using System.Management;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Two_Step_File_Protector
{

    public partial class Main : Form
    {
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
        ManagementObjectCollection USBCollection;
        List<UsbDrive> availableUsbs = new List<UsbDrive>();

        UsbDrive currentSelectedDrive;

        string fileToEncrypt;  //selected file by browse
        string encryptedFileName; //fileNameafterEncryption
        string fileToDecrypt;
        string decryptedFileName;
        //System.Management.ManagementClass USBClass = new ManagementClass("Win32_DiskDrive");

        public Main()
        {
            USBCollection = new ManagementObjectSearcher("select * from Win32_DiskDrive where InterfaceType='USB'").Get();
            InitializeComponent();
           // PidVidMeth();
           LoadDrives();
           currentSelectedDrive = (UsbDrive)comboBox1.SelectedItem;
           //GetDriveLetters();
           //testMeth();

            //}
           // MyMethod();
            
        }

        //public void GetDriveLetters()
        //{
        //    DriveInfo[] drives = DriveInfo.GetDrives();

        //    foreach (DriveInfo d in drives)
        //    {
        //        try
        //        {
        //            listBox2.Items.Add(d.DriveType);
        //            listBox2.Items.Add(d.DriveFormat);
        //            listBox2.Items.Add(d.Name);
        //            listBox2.Items.Add(d.RootDirectory);
        //            listBox2.Items.Add(d.VolumeLabel);

        //            listBox2.Items.Add("");
        //        }
        //        catch (Exception e)
        //        {
        //            listBox2.Items.Add("");
        //            continue;

        //        }

        //    }
        //}

        //public void advnc()
        //{
        //    foreach (ManagementObject drive in new ManagementObjectSearcher("select * from Win32_DiskDrive where InterfaceType='USB'").Get())
        //    {
        //        foreach (ManagementObject partition in new ManagementObjectSearcher("ASSOCIATORS OF {Win32_DiskDrive.DeviceID='" + drive["DeviceID"] + "'} WHERE AssocClass = Win32_DiskDriveToDiskPartition").Get())
        //        {
        //            //foreach (ManagementObject partition in new ManagementObjectSearcher("ASSOCIATORS OF {Win32_DiskDrive.Model='" + "SanDisk Cruzer Contour USB Device" + "'} WHERE AssocClass = Win32_DiskDriveToDiskPartition").Get())
        //            //if (drive["Model"].ToString() == "SanDisk Cruzer Contour USB Device")
        //            //{
        //            listBox1.Items.Add("Partition=" + partition["Name"]);

        //            // associate partitions with logical disks (drive letter volumes)
        //            foreach (ManagementObject disk in new ManagementObjectSearcher("ASSOCIATORS OF {Win32_DiskPartition.DeviceID='" + partition["DeviceID"] + "'} WHERE AssocClass =Win32_LogicalDiskToPartition").Get())
        //            {
        //                listBox1.Items.Add("Disk=" + disk["Name"]);
        //            }
        //            //}
        //        }
        //    }
        //}

        //public void PidVidMeth()
        //{
        //    ManagementObjectSearcher mosDisks = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");

        //    // Loop through each object (disk) retrieved by WMI

        //    foreach (ManagementObject moDisk in mosDisks.Get())
        //    {

        //        // Add the HDD to the list (use the Model field as the item's caption)

        //        listBox1.Items.Add(moDisk["model"].ToString());

                
        //    }



        //}

        //public void testMeth()
        //{
        //    ManagementObjectSearcher mosDisks = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");

        //    // Loop through each object (disk) retrieved by WMI

        //    foreach (ManagementObject moDisk in mosDisks.Get())
        //    {

        //        // Add the HDD to the list (use the Model field as the item's caption)

        //        foreach(PropertyData p in moDisk.Properties)
        //        {
        //            listBox3.Items.Add(p.Name+" "+p.Value);
        //        }


        //        listBox3.Items.Add("");
        //    }
            
        //}

        public void LoadDrives()  //working one final one
        {
            //USBClass = new ManagementClass("Win32_DiskDrive");
            //USBCollection = USBClass.GetInstances();
            //USBCollection = new ManagementObjectSearcher("select * from Win32_DiskDrive where InterfaceType='USB'").Get();
            

            foreach (ManagementObject drive in USBCollection)
            {
                
                //listBox1.Items.Add("Name : " + drive["model"]);
                try
                {

                    listBox1.Items.Add("Serial Num : " + drive["SerialNumber"].ToString());
                    

                }
                catch (Exception)
                {
                    continue;
                }
                //listBox1.Items.Add("DeviceID : "+drive["deviceid"].ToString());
                //listBox1.Items.Add("MediaType : " + drive["MediaType"].ToString());
                //listBox1.Items.Add("Desc : " + drive["Description"].ToString());

                foreach (ManagementObject partition in new ManagementObjectSearcher("ASSOCIATORS OF {Win32_DiskDrive.DeviceID='" + drive["DeviceID"] + "'} WHERE AssocClass = Win32_DiskDriveToDiskPartition").Get())
                {
                    //foreach (ManagementObject partition in new ManagementObjectSearcher("ASSOCIATORS OF {Win32_DiskDrive.Model='" + "SanDisk Cruzer Contour USB Device" + "'} WHERE AssocClass = Win32_DiskDriveToDiskPartition").Get())
                    //if (drive["Model"].ToString() == "SanDisk Cruzer Contour USB Device")
                    //{
                    //listBox1.Items.Add("Partition=" + partition["Label"]);
                    //foreach (PropertyData p in partition.Properties)
                    //{
                    //    listBox3.Items.Add(p.Name + " " + p.Value);
                    //}

                    // associate partitions with logical disks (drive letter volumes)
                    foreach (ManagementObject disk in new ManagementObjectSearcher("ASSOCIATORS OF {Win32_DiskPartition.DeviceID='" + partition["DeviceID"] + "'} WHERE AssocClass =Win32_LogicalDiskToPartition").Get())
                    {
                        //listBox1.Items.Add("Drive Letter : " + disk["Name"]);
                        //listBox1.Items.Add("Drive Label : " + disk["VolumeName"]);

                        //foreach (PropertyData p in disk.Properties)
                        //{
                        //    listBox3.Items.Add(p.Name + " " + p.Value);
                        //    //drive.Properties.Add("DriveLetter", disk["Name"]);
                        //}
                        UsbDrive ud = new UsbDrive();
                        ud.driveLabel = (string) disk["VolumeName"];
                        ud.driveLetter = (string)disk["Name"];
                        ud.modelInfo = (string)drive["model"];
                        ud.serialNumber =(string) drive["SerialNumber"];

                        availableUsbs.Add(ud);

                        //comboBox1.Items.Add(drive["model"] + " " + disk["Name"] + " " + disk["VolumeName"]);
                        comboBox1.Items.Add(ud);
                        comboBox1.SelectedIndex = 0;

                        label2.Text = "availablr usbs : "+  availableUsbs.Count.ToString()  ;
                        
                    }
                    //}
                }
                
             //   listBox1.Items.Add("Signature : " + usb["Signature"].ToString());


                //listBox1.Items.Add(usb.GetText(new TextFormat()));
                //Console.WriteLine(usb.GetText(new TextFormat()));

                //listBox1.Items.Add("Serial Num : "+usb["SerialNumber"].ToString());

                //string deviceId = usb["deviceid"].ToString();
                //Console.WriteLine(deviceId);
                //listBox1.Items.Add(deviceId);

                //int vidIndex = deviceId.IndexOf("VID_");
                //string startingAtVid = deviceId.Substring(vidIndex + 4); // + 4 to remove "VID_"                    
                //string vid = startingAtVid.Substring(0, 4); // vid is four characters long
                //Console.WriteLine("VID: " + vid);
                //listBox1.Items.Add("VID: " + vid);

                //int pidIndex = deviceId.IndexOf("PID_");
                //string startingAtPid = deviceId.Substring(pidIndex + 4); // + 4 to remove "PID_"                    
                //string pid = startingAtPid.Substring(0, 4); // pid is four characters long
                //Console.WriteLine("PID: " + pid);
                //listBox1.Items.Add("PID: " + pid);
            }
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //listBox1.Items.Clear();
            //listBox2.Items.Clear();
            //listBox3.Items.Clear();
            //LoadDrives();
            //GetDriveLetters();
            //testMeth();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Form2().Visible = true;
        }

        private void btnBrowseEncrypt_Click(object sender, EventArgs e)
        {
            //Setup the open dialog.
            openFileDialog1.FileName = "";
            openFileDialog1.Title = "Choose a file to encrypt";
            openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.Filter = "";
            // openFileDialog1.Filter = "Encrypted Files (*.encrypt) | *.encrypt";

           
            string fileExtension = "";

            //Find out if the user chose a file.
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileToEncrypt = openFileDialog1.FileName;

                txtFileToEncrypt.Text = fileToEncrypt;

                int extenstionDotPosition;
                /* Get the position of the last "\" in the OpenFileDialog.FileName path.
                * -1 is when the character your searching for is not there.
                * IndexOf searches from left to right.*/
                extenstionDotPosition = fileToEncrypt.LastIndexOf(".");
                if (extenstionDotPosition == -1)
                {
                    MessageBox.Show("Invalid type of file. Please select proper file.");
                    btnFileEncrypt.Enabled = false;
                    return;
                }

                //strOutputFile = the file path minus the last 8 characters (.encrypt)
                fileExtension = fileToEncrypt.Substring(extenstionDotPosition, (fileToEncrypt.Length - extenstionDotPosition));
                
                //Assign strOutputFile to the position after the last "\" in the path.  
                string encryptedFileExtension = "jnd"+fileExtension.Substring(1) + ".encrypt"; //this adds real ext to the file name end then.encrypt  eg : picjndjpg.encrypt  * jnd prefix is added to identify the original extension when decrypting

                string justFileName = fileToEncrypt.Substring(fileToEncrypt.LastIndexOf("\\")+1);
                encryptedFileName = justFileName.Replace(fileExtension, encryptedFileExtension);

               MessageBox.Show(fileExtension+" | "+encryptedFileExtension+ " | " + encryptedFileName);
                //txtDestinationEncrypt.Text = strOutputEncrypt;
                //Update buttons
                btnFileEncrypt.Enabled = true;
            }

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnFileEncrypt_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedItem==null)
                {
                    MessageBox.Show("No USB Flash drive attached. Please attach one.");
                    return;
                }

                UsbDrive selectedUsb = (UsbDrive)comboBox1.SelectedItem;
                string secretKey  =   selectedUsb.serialNumber; // the usb serial number is used as the private key

                EncryptDecrypt.MyEncryptor enc = new EncryptDecrypt.MyEncryptor(secretKey);
                enc.Encrypt(fileToEncrypt, selectedUsb.driveLetter+encryptedFileName);
               // MessageBox.Show(fileToEncrypt + " || " + selectedUsb.driveLetter +"\\"+ encryptedFileName); return;
                MessageBox.Show("File Encrypted Successfully.");
                //enc.Encrypt(txtFileToEncrypt.Text, "D:\\Personal"+ "\\");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentSelectedDrive = (UsbDrive)comboBox1.SelectedItem;
            MessageBox.Show("Selected : " + currentSelectedDrive.serialNumber);
        }

        private void btnBrowseDecrypt_Click(object sender, EventArgs e)
        {
            //Setup the open dialog.
            openFileDialog1.FileName = "";
            openFileDialog1.Title = "Choose a file to decrypt";
            openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.Filter = "Encrypted Files (*.encrypt) | *.encrypt";



            //Find out if the user chose a file.
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileToDecrypt = openFileDialog1.FileName;
                //txtFileToDecrypt.Text = strFileToDecrypt;
                int iPosition;
                /* Get the position of the last "\" in the OpenFileDialog.FileName path.
                * -1 is when the character your searching for is not there.
                * IndexOf searches from left to right.*/
                iPosition = fileToDecrypt.LastIndexOf(".encrypt");
                if ((iPosition == -1) || (iPosition != (fileToDecrypt.Length - 8)))
                {
                    MessageBox.Show("Invalid file. Please select proper encrypted file.");
                }

                //strOutputFile = the file path minus the last 8 characters (.encrypt)
                decryptedFileName = fileToDecrypt.Substring(0, fileToDecrypt.Length - 8);
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
                MessageBox.Show(fileToDecrypt+ " | "+decryptedFileName );
                //txtDestinationDecrypt.Text = strOutputDecrypt;
                //Update buttons
                btnFileDecrypt.Enabled = true;

            }
        }

        private void btnFileDecrypt_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedItem== null)
                {
                    txtFileToDecrypt.Text = "";
                    MessageBox.Show("select the usb original where the file was put. Otherwise can't decrypt");
                    return;
                }
                UsbDrive ud = (UsbDrive)comboBox1.SelectedItem;
                                
                string secretKey = ud.serialNumber;
                EncryptDecrypt.MyEncryptor enc = new EncryptDecrypt.MyEncryptor(secretKey);
                enc.Decrypt(fileToDecrypt, decryptedFileName);
                MessageBox.Show("File decrypted successfully.");
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                //MessageBox.Show("This isn't the USB drive that was used to encrypt this file. Try selecting another.");
                MessageBox.Show(ex.Message);
            }
        }
    }
}

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

    public partial class Form1 : Form
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

        public Form1()
        {
            USBCollection = new ManagementObjectSearcher("select * from Win32_DiskDrive where InterfaceType='USB'").Get();
            InitializeComponent();
           LoadDrives();
           currentSelectedDrive = (UsbDrive)comboBox1.SelectedItem;

        }

        public void LoadDrives()  //working one final one
        {
         
            

            foreach (ManagementObject drive in USBCollection)
            {
                
                try
                {

                    listBox1.Items.Add("Serial Num : " + drive["SerialNumber"].ToString());
                    

                }
                catch (Exception)
                {
                    continue;
                }

                foreach (ManagementObject partition in new ManagementObjectSearcher("ASSOCIATORS OF {Win32_DiskDrive.DeviceID='" + drive["DeviceID"] + "'} WHERE AssocClass = Win32_DiskDriveToDiskPartition").Get())
                {
                    
                    foreach (ManagementObject disk in new ManagementObjectSearcher("ASSOCIATORS OF {Win32_DiskPartition.DeviceID='" + partition["DeviceID"] + "'} WHERE AssocClass =Win32_LogicalDiskToPartition").Get())
                    {
                  
                        UsbDrive ud = new UsbDrive();
                        ud.driveLabel = (string) disk["VolumeName"];
                        ud.driveLetter = (string)disk["Name"];
                        ud.modelInfo = (string)drive["model"];
                        ud.serialNumber =(string) drive["SerialNumber"];

                        availableUsbs.Add(ud);

            
                        comboBox1.Items.Add(ud);
                        comboBox1.SelectedIndex = 0;

                        label2.Text = "availablr usbs : "+  availableUsbs.Count.ToString()  ;
                        
                    }
                  
                }
                
            
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

           
            string fileExtension = "";

            //Find out if the user chose a file.
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileToEncrypt = openFileDialog1.FileName;

                txtFileToEncrypt.Text = fileToEncrypt;

                int extenstionDotPosition;
                extenstionDotPosition = fileToEncrypt.LastIndexOf(".");
                if (extenstionDotPosition == -1)
                {
                    MessageBox.Show("Invalid type of file. Please select proper file.");
                    btnFileEncrypt.Enabled = false;
                    return;
                }

   
                fileExtension = fileToEncrypt.Substring(extenstionDotPosition, (fileToEncrypt.Length - extenstionDotPosition));
       
                string encryptedFileExtension = "jnd"+fileExtension.Substring(1) + ".encrypt"; 

                string justFileName = fileToEncrypt.Substring(fileToEncrypt.LastIndexOf("\\")+1);
                encryptedFileName = justFileName.Replace(fileExtension, encryptedFileExtension);

               MessageBox.Show(fileExtension+" | "+encryptedFileExtension+ " | " + encryptedFileName);
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
                int iPosition;
                iPosition = fileToDecrypt.LastIndexOf(".encrypt");
                if ((iPosition == -1) || (iPosition != (fileToDecrypt.Length - 8)))
                {
                    MessageBox.Show("Invalid file. Please select proper encrypted file.");
                }

                decryptedFileName = fileToDecrypt.Substring(0, fileToDecrypt.Length - 8);
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
                MessageBox.Show(ex.Message);
            }
        }
    }
}

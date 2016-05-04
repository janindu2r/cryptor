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
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;

namespace Two_Step_File_Protector
{

    public partial class Main : Form
    {
        private const int WM_DEVICECHANGE = 0x219;
        private const int DBT_DEVICEARRIVAL = 0x8000;
        private const int DBT_DEVICEREMOVECOMPLETE = 0x8004;
        private const int DBT_DEVTYP_VOLUME = 0x00000002;
             
        ManagementObjectCollection USBCollection;
        List<UsbDrive> availableUsbs = new List<UsbDrive>();

        PasswordBox pb = new PasswordBox();

        UsbDrive currentSelectedDrive;
        HashSet<string> filesToEncrypt = new HashSet<string>();
        
        ManagementScope msScope = new ManagementScope("root\\CIMV2");


        public Main()
        {    
           InitializeComponent();
           LoadDrives();
           currentSelectedDrive = (UsbDrive)comboBox1.SelectedItem;
        }
             

        public void LoadDrives()  //working one final one
        {
            
            try
            {
                USBCollection = new ManagementObjectSearcher("select * from Win32_DiskDrive where InterfaceType='USB'").Get();
                availableUsbs.Clear();
                comboBox1.Items.Clear();
                foreach (ManagementObject drive in USBCollection)
                {

                    foreach (ManagementObject partition in new ManagementObjectSearcher("ASSOCIATORS OF {Win32_DiskDrive.DeviceID='" + drive["DeviceID"] + "'} WHERE AssocClass = Win32_DiskDriveToDiskPartition").Get())
                    {
                        foreach (ManagementObject disk in new ManagementObjectSearcher("ASSOCIATORS OF {Win32_DiskPartition.DeviceID='" + partition["DeviceID"] + "'} WHERE AssocClass =Win32_LogicalDiskToPartition").Get())
                        {
                            UsbDrive ud = new UsbDrive();
                            ud.driveLabel = (string)disk["VolumeName"];
                            ud.driveLetter = (string)disk["Name"];
                            ud.modelInfo = (string)drive["model"];
                            ud.serialNumber = (string)drive["SerialNumber"];

                            availableUsbs.Add(ud);

                            comboBox1.Items.Add(ud);
                            comboBox1.SelectedIndex = 0;


                        }
                    }
                }
            }
            catch (Exception)
            {
                LoadDrives();
            }
            
        }
                
        private void btnBrowseEncrypt_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Title = "Choose files to encrypt";
            openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.Filter = "";
            openFileDialog1.Multiselect = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK & openFileDialog1.FileNames.Length>0)
            {
               
                foreach (string f in openFileDialog1.FileNames)
                {
                    filesToEncrypt.Add(f);
                }
               
                listBox1.Items.Clear();
                foreach (string f in filesToEncrypt)
                {
                    listBox1.Items.Add(f);
                }

                btnFileEncrypt.Enabled = true;
            }
        }           

        private string getEncryptFileName(string fname)
        {
            string fileExtension = "";

            string fileToEncrypt = fname;
            string encryptedFileName;

            int extenstionDotPosition;
            
            extenstionDotPosition = fileToEncrypt.LastIndexOf(".");
            if (extenstionDotPosition == -1)
            {
                MessageBox.Show("Invalid type of file. Please select proper file.");
                btnFileEncrypt.Enabled = false;
                return "";
            }

            fileExtension = fileToEncrypt.Substring(extenstionDotPosition, (fileToEncrypt.Length - extenstionDotPosition));

            string encryptedFileExtension = "ccp" + fileExtension.Substring(1) + ".ccp"; //this adds real ext to the file name end then.ccp  eg : picccpjpg.ccp  * jnd prefix is added to identify the original extension when decrypting

            string justFileName = fileToEncrypt.Substring(fileToEncrypt.LastIndexOf("\\") + 1);
            encryptedFileName = justFileName.Replace(fileExtension, encryptedFileExtension);

            return encryptedFileName;
        }

        private void enabledisableComponents(bool v)
        {
            if (v)
            {
                foreach (Control ctrl in this.Controls)
                {
                    ctrl.Enabled = true;
                } 
            }
            else
            {
                foreach (Control ctrl in this.Controls)
                {
                    ctrl.Enabled = false;
                } 
            }
               
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
                if (filesToEncrypt.Count == 0)
                {
                    MessageBox.Show("Select one or more files.");
                    return;
                }
                else 
                {
                    pb.ShowDialog();
                    if (!pb.pwordok)
                    {
                        
                        return;
                    }
                   
                }

                label4.Text = "Please Wait...";
                enabledisableComponents(false);
                label4.Enabled = true;

                UsbDrive selectedUsb = (UsbDrive)comboBox1.SelectedItem;
                string secretKey = selectedUsb.serialNumber + pb.password;
                MyEncryptor enc = new MyEncryptor(secretKey);


                foreach (string fileToEncrypt in filesToEncrypt)
                {
                    
                    string encryptedFileName = getEncryptFileName(fileToEncrypt);

                    if (encryptedFileName == "")
                    {
                        MessageBox.Show("Invalid File : " + filesToEncrypt);
                        continue;
                    }
                    
                    enc.Encrypt(fileToEncrypt, selectedUsb.driveLetter + encryptedFileName);


                    FileInfo ff = new FileInfo(selectedUsb.driveLetter + encryptedFileName);
                    File.SetAttributes(ff.FullName, FileAttributes.System);
                    File.SetAttributes(ff.FullName, FileAttributes.Hidden);
                }

                //Copying the decrypting module
                FileInfo fsrc = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "TheDecryptor.exe");
                FileInfo fdest = new FileInfo(selectedUsb.driveLetter + @"\" + "TheDecryptor.exe");

                if (!fdest.Exists)
                    fsrc.CopyTo(fdest.FullName);

                enabledisableComponents(true);
                label4.Text = "";
                MessageBox.Show("All the files Encrypted Successfully.");
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentSelectedDrive = (UsbDrive)comboBox1.SelectedItem;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            filesToEncrypt.Clear();
            listBox1.Items.Clear();
            btnFileEncrypt.Enabled = false;
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            switch (m.Msg)
            {
                case WM_DEVICECHANGE:
                    switch ((int)m.WParam)
                    {
                        case DBT_DEVICEARRIVAL:

                            int devType = Marshal.ReadInt32(m.LParam, 4);
                            if (devType == DBT_DEVTYP_VOLUME)
                            {
                                label4.Text = "Please Wait.Detecting newly inserted usd drive";
                                enabledisableComponents(false);
                                label4.Enabled = true;

                                LoadDrives();
                                enabledisableComponents(true);
                                label4.Text = "";
                            }
                            break;

                        case DBT_DEVICEREMOVECOMPLETE:
                            label4.Text = "Please Wait.Recognizing removed usb drive";
                                enabledisableComponents(false);
                                label4.Enabled = true;
                            LoadDrives();
                            LoadDrives();
                                enabledisableComponents(true);
                                label4.Text = "";
                            break;
                    }
                    break;
            }
        }

    }
}

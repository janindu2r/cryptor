using System;
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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            advnc();
        }

        public void advnc()
        {
            foreach (ManagementObject drive in new ManagementObjectSearcher("select * from Win32_DiskDrive where InterfaceType='USB'").Get())
            {
                foreach (ManagementObject partition in new ManagementObjectSearcher("ASSOCIATORS OF {Win32_DiskDrive.DeviceID='" + drive["DeviceID"] + "'} WHERE AssocClass = Win32_DiskDriveToDiskPartition").Get())
                {
                    //foreach (ManagementObject partition in new ManagementObjectSearcher("ASSOCIATORS OF {Win32_DiskDrive.Model='" + "SanDisk Cruzer Contour USB Device" + "'} WHERE AssocClass = Win32_DiskDriveToDiskPartition").Get())
                    //if (drive["Model"].ToString() == "SanDisk Cruzer Contour USB Device")
                    //{
                        listBox1.Items.Add("Partition=" + partition["Name"]);

                        // associate partitions with logical disks (drive letter volumes)
                        foreach (ManagementObject disk in new ManagementObjectSearcher("ASSOCIATORS OF {Win32_DiskPartition.DeviceID='" + partition["DeviceID"] + "'} WHERE AssocClass =Win32_LogicalDiskToPartition").Get())
                        {
                            listBox1.Items.Add("Disk=" + disk["Name"]);
                        }
                    //}
                }
            }
        }
    }

    
}

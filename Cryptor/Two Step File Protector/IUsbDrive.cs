using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Two_Step_File_Protector
{
    interface IUsbDrive
    {
        string driveLabel
        {
            get;
            set;
        }

        string driveLetter
        {
            get;
            set;
        }
        string modelInfo
        {
            get;
            set;
        }
        string serialNumber
        {
            get;
            set;
        }

        string ToString();
    }

}

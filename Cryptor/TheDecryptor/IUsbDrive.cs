using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheDecryptor
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

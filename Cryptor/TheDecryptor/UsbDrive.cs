using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheDecryptor
{
    class UsbDrive : IUsbDrive
    {
        string DriveLabel;  //name assigned by user
        string DriveLetter;
        string ModelInfo;
        string SerialNumber;

        public override string ToString()
        {
            return driveLetter + " " + driveLabel;
        }


        public string driveLabel
        {
            get
            {
                return DriveLabel;
            }

            set
            {
                DriveLabel = value;
            }
        }
        public string driveLetter
        {
            get
            {
                return DriveLetter;
            }

            set
            {
                DriveLetter = value;
            }
        }
        public string modelInfo
        {
            get
            {
                return ModelInfo;
            }

            set
            {
                ModelInfo = value;
            }
        }
        public string serialNumber
        {
            get
            {
                return SerialNumber;
            }

            set
            {
                SerialNumber = value;
            }
        }
    }
}

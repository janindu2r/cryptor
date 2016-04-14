using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Two_Step_File_Protector
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            string msg = "first program";
            byte[] bytes =  Encoding.Default.GetBytes(msg);

            for (int i = 0; i < bytes.Length; i++)
                Console.WriteLine(Encoding.Default.GetString(bytes));


            //FileInfo f = new FileInfo("C:/Users/Janindu/Desktop/CEPA Description.txt");  
            //using (BinaryReader br = new BinaryReader(f.OpenRead()))
            //{
            //    Console.WriteLine(br.ReadInt32());
            //    Console.WriteLine(br.ReadString());
            //}   

            FileInfo fi = new FileInfo("champu.dat");
            using (BinaryWriter bw = new BinaryWriter(fi.OpenWrite()))
            {
                int x = 007;
                string str = "hello champu ,one day you will become doomkatu";

                bw.Write(x);
                bw.Write(str);
            }

            //Reading  
            FileInfo f = new FileInfo("champu.dat");
            using (BinaryReader br = new BinaryReader(fi.OpenRead()))
            {
                Console.WriteLine(br.ReadByte());
               // Console.WriteLine(br.ReadInt32());
               // Console.WriteLine(br.ReadString());
            }
            Console.ReadLine();   
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dTools;


namespace dTools.Console
{
    class Program
    {
        public string Name { get; set; }
        public int Age { get; set; }

        static void Main(string[] args)
        {
            var file = @"C:\Users\H12727182\Desktop\TEST\T1F3L1.xml";
            for (int i = 0; i < 99999; i++)
            {
                var fff = new FileInfo(file);
            }
            int a = 1;
            for (int i = 45803; i < 99999; i++)
            {
                System.Console.WriteLine(i);
                File.Copy(file, $@"C:\Users\H12727182\Desktop\TEST\T1F3L{i}.xml");
            }

            var f= FileHelper.GetDirectoryAllFiles(@"C:\AMATAAA");
            var json = new
            {
                name = "张三",
                age = 18
            }.ToJson();

            System.Console.WriteLine(json);

            var entity = json.ToObject<Program>();

            System.Console.WriteLine(entity.Name);
            System.Console.WriteLine(entity.Age);


            var name = "";

            if (name.IsEmpty())
            {
                System.Console.WriteLine("name 是空");
            }

            var mail = "3977077343@qq.com";
            if (mail.IsEmail())
            {
                System.Console.WriteLine("邮件格式正确");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dTools.Console
{
    class Program
    {
        public string Name { get; set; }
        public int Age { get; set; }

        static void Main(string[] args)
        {
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

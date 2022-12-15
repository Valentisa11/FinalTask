using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FinalTask
{
    internal class Program
    {
        static void Main()
        {
            const string file = "Students.dat"; //путь к файлу с архивом сущностей Student
            const string desctopfile = "Students"; //Имя каталогу в котором будут созданы файлы по группам
            Student[]? students = null;

            HashSet<string> grouphas = new(); //структура для проверки повторения групп
            BinaryFormatter formatter = new();

            //десериализация
            if (File.Exists(file))
                using (var fs = new FileStream(file, FileMode.Open))
                {
                    students = (Student[])formatter.Deserialize(fs);
                    Console.WriteLine("Объект десериализован");
                }

            var Dir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory); //Путь к Рабочему столу

            if (Directory.Exists(Dir)) //проверяем есть ли папка Рабочий стол
            {
                try
                {
                    var desctopdir = new DirectoryInfo(Dir);
                    if (Directory.Exists(string.Concat(Dir, @$"\{desctopfile}")))
                        Directory.Delete(string.Concat(Dir, @$"\{desctopfile}"), true); //если каталог Students уже есть, удаляем его

                    var newdir = desctopdir.CreateSubdirectory(desctopfile); //создаем новый каталог на рабочем столе

                    if (students is not null)
                        foreach (Student student in students)
                        {
                            if (student.Group is not null)
                                if (!grouphas.Contains(student.Group)) //файл с наименованием группы еще не создавался?
                                {
                                    grouphas.Add(student.Group);

                                    var fileInfo = new FileInfo(string.Concat(Dir, @$"\{desctopfile}\", student.Group, ".txt"));

                                    if (!fileInfo.Exists)
                                    {

                                        using (StreamWriter sw = fileInfo.CreateText()) //создаем файл с наименованием группы и записываем первого студента
                                        {
                                            sw.WriteLine($"{student.Name}, {student.DateOfBirth:D}");
                                        }
                                    }
                                }
                                else
                                {
                                    var fileInfo = new FileInfo(string.Concat(Dir, @$"\{desctopfile}\", student.Group, ".txt"));

                                    if (fileInfo.Exists)
                                    {
                                        using (StreamWriter sw = fileInfo.AppendText()) //открываем файл и дописывает остальных студентов 
                                        {
                                            sw.WriteLine($"{student.Name}, {student.DateOfBirth:D}");
                                        }
                                    }
                                }
                        }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message); //при наличии ошибки, выводим ее
                }
            }
            Console.ReadKey();
        }
    }
}
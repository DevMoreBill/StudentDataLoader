using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace StudentDataLoader
{
    class Student
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public DateTime DateOfBirth { get; set; }
        public decimal AverageGrade { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Путь к бинарному файлу
            string binaryFilePath = @"C:binaryfile.bin"; // путь

            // Создаем директорию Students на рабочем столе
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string studentsDirectory = Path.Combine(desktopPath, "Students");
            Directory.CreateDirectory(studentsDirectory);

            // Чтение данных из бинарного файла 
            List<Student> students = ReadStudentsFromBinary(binaryFilePath);

            // Разбиваем студентов по группам и записываем в текстовые файлы
            foreach (var group in students.GroupBy(s => s.Group))
            {
                string groupFileName = Path.Combine(studentsDirectory, $"{group.Key}.txt");

                using (StreamWriter writer = new StreamWriter(groupFileName))
                {
                    foreach (var student in group)
                    {
                        writer.WriteLine($"{student.Name}, {student.DateOfBirth.ToShortDateString()}, {student.AverageGrade}");
                    }
                }
            }

            Console.WriteLine("Данные студентов обработаны");
            Console.ReadKey();
        }

        static List<Student> ReadStudentsFromBinary(string filePath)
        {
            List<Student> students = new List<Student>();

            using (StreamReader reader = new StreamReader(filePath, Encoding.Default))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Разбиваем строку на части
                    string[] parts = line.Split(',');

                    // Проверяем, что у нас достаточное количество данных
                    if (parts.Length >= 4)
                    {
                        // Создаем экземпляр Student
                        Student student = new Student
                        {
                            Name = parts[0].Trim(),
                            Group = parts[1].Trim(),
                            DateOfBirth = DateTime.Parse(parts[2].Trim()),
                            AverageGrade = decimal.Parse(parts[3].Trim())
                        };

                        // Добавляем студента в список
                        students.Add(student);
                    }
                    else
                    {
                        Console.WriteLine($"Ошибка: неверный формат данных в строке: {line}");
                    }
                }
            }

            return students;
        }
    }
}

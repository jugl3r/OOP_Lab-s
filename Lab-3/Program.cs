using System;
using System.Collections.Generic;
using System.IO;

namespace Lab3
{
    // Типы данных
    public class MyData1
    {
        public int Id { get; set; }
        public string TextData { get; set; }
        public override string ToString() { return string.Format("ID: {0}, Text: {1}", Id, TextData); }
    }

    public class MyData2
    {
        public double Value { get; set; }
        public string Description { get; set; }
        public override string ToString() { return string.Format("Value: {0}, Desc: {1}", Value, Description); }
    }

    // Абстрактный базовый класс
    public abstract class AbstractFile
    {
        public string Name { get; set; }
        protected bool IsOpen { get; set; }
        protected int Position { get; set; }
        
        public AbstractFile(string name)
        {
            Name = name;
            IsOpen = false;
            Position = 0;
        }

        public abstract void Open();
        public abstract void Close();
        public abstract void Seek(int pos);
        public abstract void Read();
        public abstract void Write(object data);
        
        public int GetPosition()
        {
            return Position;
        }

        public abstract int GetLength();

        // Виртуальный (абстрактный) метод для вывода полей класса
        public abstract void PrintInfo();
    }

    // Производный класс 1
    public class MyDataFile1 : AbstractFile
    {
        public string HeaderInfo { get; set; }
        private List<MyData1> fileData;

        public MyDataFile1(string name, string header) : base(name)
        {
            HeaderInfo = header;
            fileData = new List<MyData1>();
        }

        public override void Open()
        {
            IsOpen = true;
            Position = 0;
            Console.WriteLine(string.Format("Файл '{0}' (Data1) открыт. Заголовок: {1}", Name, HeaderInfo));
        }

        public override void Close()
        {
            IsOpen = false;
            Console.WriteLine(string.Format("Файл '{0}' закрыт.", Name));
        }

        public override void Seek(int pos)
        {
            if (!IsOpen) { Console.WriteLine("Ошибка: Файл закрыт."); return; }
            if (pos >= 0 && pos <= fileData.Count)
            {
                Position = pos;
                Console.WriteLine(string.Format("Курсор перемещен на позицию {0}", Position));
            }
            else
            {
                Console.WriteLine("Ошибка: Неверная позиция.");
            }
        }

        public override void Read()
        {
            if (!IsOpen) { Console.WriteLine("Ошибка: Файл закрыт."); return; }
            if (Position < fileData.Count)
            {
                Console.WriteLine(string.Format("Прочитано на позиции {0}: {1}", Position, fileData[Position].ToString()));
                Position++;
            }
            else
            {
                Console.WriteLine("Достигнут конец файла.");
            }
        }

        public override void Write(object data)
        {
            if (!IsOpen) { Console.WriteLine("Ошибка: Файл закрыт."); return; }
            if (data is MyData1)
            {
                if (Position == fileData.Count)
                {
                    fileData.Add((MyData1)data);
                }
                else
                {
                    fileData[Position] = (MyData1)data;
                }
                Console.WriteLine("Данные успешно записаны.");
                Position++;
            }
            else
            {
                Console.WriteLine("Ошибка: Неверный тип данных для MyDataFile1.");
            }
        }

        public override int GetLength()
        {
            return fileData.Count;
        }

        public override void PrintInfo()
        {
            Console.WriteLine(string.Format("Файл: {0,-12} | Тип: Data1 | Заголовок: {1,-20} | Размер: {2}", Name, HeaderInfo, GetLength()));
        }
    }

    // Производный класс 2
    public class MyDataFile2 : AbstractFile
    {
        public string MetaHeader { get; set; }
        private List<MyData2> fileData;

        public MyDataFile2(string name, string header) : base(name)
        {
            MetaHeader = header;
            fileData = new List<MyData2>();
        }

        public override void Open()
        {
            IsOpen = true;
            Position = 0;
            Console.WriteLine(string.Format("Файл '{0}' (Data2) открыт. Заголовок: {1}", Name, MetaHeader));
        }

        public override void Close()
        {
            IsOpen = false;
            Console.WriteLine(string.Format("Файл '{0}' закрыт.", Name));
        }

        public override void Seek(int pos)
        {
            if (!IsOpen) { Console.WriteLine("Ошибка: Файл закрыт."); return; }
            if (pos >= 0 && pos <= fileData.Count)
            {
                Position = pos;
                Console.WriteLine(string.Format("Курсор перемещен на позицию {0}", Position));
            }
            else
            {
                Console.WriteLine("Ошибка: Неверная позиция.");
            }
        }

        public override void Read()
        {
            if (!IsOpen) { Console.WriteLine("Ошибка: Файл закрыт."); return; }
            if (Position < fileData.Count)
            {
                Console.WriteLine(string.Format("Прочитано на позиции {0}: {1}", Position, fileData[Position].ToString()));
                Position++;
            }
            else
            {
                Console.WriteLine("Достигнут конец файла.");
            }
        }

        public override void Write(object data)
        {
            if (!IsOpen) { Console.WriteLine("Ошибка: Файл закрыт."); return; }
            if (data is MyData2)
            {
                if (Position == fileData.Count)
                {
                    fileData.Add((MyData2)data);
                }
                else
                {
                    fileData[Position] = (MyData2)data;
                }
                Console.WriteLine("Данные успешно записаны.");
                Position++;
            }
            else
            {
                Console.WriteLine("Ошибка: Неверный тип данных для MyDataFile2.");
            }
        }

        public override int GetLength()
        {
            return fileData.Count;
        }

        public override void PrintInfo()
        {
            Console.WriteLine(string.Format("Файл: {0,-12} | Тип: Data2 | Метаданные: {1,-19} | Размер: {2}", Name, MetaHeader, GetLength()));
        }
    }

    // Класс Folder
    public class Folder
    {
        public List<AbstractFile> Files { get; private set; }

        public Folder()
        {
            Files = new List<AbstractFile>();
        }

        public void AddFile(AbstractFile f)
        {
            Files.Add(f);
            Console.WriteLine(string.Format("Файл '{0}' добавлен в папку.", f.Name));
        }

        public void PrintFiles()
        {
            Console.WriteLine("\n--- Содержимое папки ---");
            if (Files.Count == 0) Console.WriteLine("Папка пуста.");
            foreach (var f in Files)
            {
                // Единообразная работа с элементами коллекции через виртуальный метод
                f.PrintInfo();
            }
            Console.WriteLine("------------------------\n");
        }

        public void SortByName()
        {
            Files.Sort((f1, f2) => string.Compare(f1.Name, f2.Name));
            Console.WriteLine("Файлы отсортированы по имени.");
        }

        public void SortByLength()
        {
            Files.Sort((f1, f2) => f1.GetLength().CompareTo(f2.GetLength()));
            Console.WriteLine("Файлы отсортированы по размеру.");
        }

        public void SaveToFile(string path)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(path))
                {
                    foreach (var f in Files)
                    {
                        string header = "";
                        if (f is MyDataFile1) header = ((MyDataFile1)f).HeaderInfo;
                        if (f is MyDataFile2) header = ((MyDataFile2)f).MetaHeader;
                        
                        sw.WriteLine(string.Format("{0}|{1}|{2}|{3}", f.GetType().Name, f.Name, f.GetLength(), header));
                    }
                }
                Console.WriteLine("Метаданные папки успешно сохранены в файл: " + path);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при сохранении: " + ex.Message);
            }
        }

        public void LoadFromFile(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("Файл не найден.");
                return;
            }
            try
            {
                Files.Clear();
                string[] lines = File.ReadAllLines(path);
                foreach (string line in lines)
                {
                    string[] parts = line.Split('|');
                    if (parts.Length >= 4)
                    {
                        string type = parts[0];
                        string name = parts[1];
                        // int length = int.Parse(parts[2]);
                        string header = parts[3];

                        if (type == "MyDataFile1")
                            Files.Add(new MyDataFile1(name, header));
                        else if (type == "MyDataFile2")
                            Files.Add(new MyDataFile2(name, header));
                    }
                }
                Console.WriteLine("Папка успешно загружена из файла.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при чтении: " + ex.Message);
            }
        }
    }

    class Program
    {
        static Folder currentFolder = new Folder();
        static AbstractFile activeFile = null;

        static void Main(string[] args)
        {
            
            while (true)
            {
                Console.WriteLine("=== ЛАБОРАТОРНАЯ РАБОТА 3: ФАЙЛОВАЯ СИСТЕМА ===");
                Console.WriteLine("1. Создать DataFile1");
                Console.WriteLine("2. Создать DataFile2");
                Console.WriteLine("3. Вывести содержимое папки");
                Console.WriteLine("4. Сортировать папку по имени");
                Console.WriteLine("5. Сортировать папку по длине");
                Console.WriteLine("6. Сохранить папку в TXT");
                Console.WriteLine("7. Загрузить папку из TXT");
                Console.WriteLine("8. Работа с файлом (открыть, читать, писать...)");
                Console.WriteLine("0. Выход");
                Console.Write("Выбор: ");
                
                string choice = Console.ReadLine();
                if (choice == "0") break;

                switch (choice)
                {
                    case "1":
                        Console.Write("Имя файла: ");
                        string n1 = Console.ReadLine();
                        Console.Write("Заголовок: ");
                        string h1 = Console.ReadLine();
                        currentFolder.AddFile(new MyDataFile1(n1, h1));
                        break;
                    case "2":
                        Console.Write("Имя файла: ");
                        string n2 = Console.ReadLine();
                        Console.Write("Заголовок: ");
                        string h2 = Console.ReadLine();
                        currentFolder.AddFile(new MyDataFile2(n2, h2));
                        break;
                    case "3":
                        currentFolder.PrintFiles();
                        break;
                    case "4":
                        currentFolder.SortByName();
                        break;
                    case "5":
                        currentFolder.SortByLength();
                        break;
                    case "6":
                        Console.Write("Имя текстового файла: ");
                        currentFolder.SaveToFile(Console.ReadLine());
                        break;
                    case "7":
                        Console.Write("Имя текстового файла: ");
                        currentFolder.LoadFromFile(Console.ReadLine());
                        break;
                    case "8":
                        WorkWithFile();
                        break;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }
                Console.WriteLine();
            }
        }

        static void WorkWithFile()
        {
            if (currentFolder.Files.Count == 0)
            {
                Console.WriteLine("В папке нет файлов.");
                return;
            }

            Console.WriteLine("\n--- Список файлов ---");
            for (int i = 0; i < currentFolder.Files.Count; i++)
            {
                Console.WriteLine(string.Format("{0}. {1} ({2})", i + 1, currentFolder.Files[i].Name, currentFolder.Files[i].GetType().Name));
            }
            Console.Write("Выберите номер файла: ");
            int idx;
            if (!int.TryParse(Console.ReadLine(), out idx) || idx < 1 || idx > currentFolder.Files.Count)
            {
                Console.WriteLine("Неверный номер.");
                return;
            }

            activeFile = currentFolder.Files[idx - 1];

            while (true)
            {
                Console.WriteLine(string.Format("\n--- Работа с файлом '{0}' ---", activeFile.Name));
                Console.WriteLine("1. Открыть (Open)");
                Console.WriteLine("2. Записать данные (Write)");
                Console.WriteLine("3. Прочитать данные (Read)");
                Console.WriteLine("4. Переместить курсор (Seek)");
                Console.WriteLine("5. Узнать текущую позицию (GetPosition)");
                Console.WriteLine("6. Узнать длину файла (GetLength)");
                Console.WriteLine("7. Закрыть (Close)");
                Console.WriteLine("0. Назад");
                Console.Write("Действие: ");

                string action = Console.ReadLine();
                if (action == "0") break;

                switch (action)
                {
                    case "1":
                        activeFile.Open();
                        break;
                    case "2":
                        if (activeFile is MyDataFile1)
                        {
                            MyData1 d1 = new MyData1();
                            Console.Write("Введите ID (целое число): ");
                            int id;
                            if (int.TryParse(Console.ReadLine(), out id))
                            {
                                d1.Id = id;
                                Console.Write("Введите текст: ");
                                d1.TextData = Console.ReadLine();
                                activeFile.Write(d1);
                            }
                            else Console.WriteLine("Ошибка формата ID.");
                        }
                        else if (activeFile is MyDataFile2)
                        {
                            MyData2 d2 = new MyData2();
                            Console.Write("Введите Value (число): ");
                            double val;
                            if (double.TryParse(Console.ReadLine(), out val))
                            {
                                d2.Value = val;
                                Console.Write("Введите описание: ");
                                d2.Description = Console.ReadLine();
                                activeFile.Write(d2);
                            }
                            else Console.WriteLine("Ошибка формата Value.");
                        }
                        break;
                    case "3":
                        activeFile.Read();
                        break;
                    case "4":
                        Console.Write("Введите позицию: ");
                        int pos;
                        if (int.TryParse(Console.ReadLine(), out pos))
                            activeFile.Seek(pos);
                        break;
                    case "5":
                        Console.WriteLine("Текущая позиция: " + activeFile.GetPosition());
                        break;
                    case "6":
                        Console.WriteLine("Длина файла: " + activeFile.GetLength());
                        break;
                    case "7":
                        activeFile.Close();
                        break;
                }
            }
        }
    }
}

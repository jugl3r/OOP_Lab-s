using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Lab2
{
    class DictionaryNode
    {
        public string EnglishWord { get; set; }
        public string RussianWord { get; set; }
        public DictionaryNode Left { get; set; }
        public DictionaryNode Right { get; set; }

        public DictionaryNode(string eng, string rus)
        {
            EnglishWord = eng;
            RussianWord = rus;
            Left = null;
            Right = null;
        }
    }

    class BinaryDictionaryTree
    {
        public string DictionaryName { get; private set; }
        private DictionaryNode root;

        // Конструктор с параметрами для задания имени словаря
        public BinaryDictionaryTree(string name)
        {
            DictionaryName = name;
            root = null;
        }

        // Конструктор по умолчанию
        public BinaryDictionaryTree()
        {
            DictionaryName = "Безымянный словарь";
            root = null;
        }

        public void Add(string eng, string rus)
        {
            eng = eng.ToLower();
            if (root == null)
            {
                root = new DictionaryNode(eng, rus);
                return;
            }

            DictionaryNode current = root;
            while (true)
            {
                int cmp = string.Compare(eng, current.EnglishWord);
                if (cmp < 0)
                {
                    if (current.Left == null)
                    {
                        current.Left = new DictionaryNode(eng, rus);
                        break;
                    }
                    current = current.Left;
                }
                else if (cmp > 0)
                {
                    if (current.Right == null)
                    {
                        current.Right = new DictionaryNode(eng, rus);
                        break;
                    }
                    current = current.Right;
                }
                else
                {
                    // Update translation if word exists
                    current.RussianWord = rus;
                    break;
                }
            }
        }

        public string Search(string eng)
        {
            eng = eng.ToLower();
            DictionaryNode current = root;
            while (current != null)
            {
                int cmp = string.Compare(eng, current.EnglishWord);
                if (cmp == 0) return current.RussianWord;
                if (cmp < 0) current = current.Left;
                else current = current.Right;
            }
            return null;
        }

        public bool Remove(string eng)
        {
            eng = eng.ToLower();
            DictionaryNode parent = null;
            DictionaryNode current = root;
            
            while (current != null)
            {
                int cmp = string.Compare(eng, current.EnglishWord);
                if (cmp == 0) break;
                parent = current;
                if (cmp < 0) current = current.Left;
                else current = current.Right;
            }

            if (current == null) return false; // Not found

            // Case 1: Leaf node
            if (current.Left == null && current.Right == null)
            {
                if (parent == null) root = null;
                else if (parent.Left == current) parent.Left = null;
                else parent.Right = null;
            }
            // Case 2: Node with 1 child (Right only)
            else if (current.Left == null)
            {
                if (parent == null) root = current.Right;
                else if (parent.Left == current) parent.Left = current.Right;
                else parent.Right = current.Right;
            }
            // Case 3: Node with 1 child (Left only)
            else if (current.Right == null)
            {
                if (parent == null) root = current.Left;
                else if (parent.Left == current) parent.Left = current.Left;
                else parent.Right = current.Left;
            }
            // Case 4: Node with 2 children
            else
            {
                // Find successor (smallest node in the right subtree)
                DictionaryNode successorParent = current;
                DictionaryNode successor = current.Right;

                while (successor.Left != null)
                {
                    successorParent = successor;
                    successor = successor.Left;
                }

                current.EnglishWord = successor.EnglishWord;
                current.RussianWord = successor.RussianWord;

                if (successorParent.Left == successor)
                    successorParent.Left = successor.Right;
                else
                    successorParent.Right = successor.Right;
            }

            return true;
        }

        public void PrintInOrder()
        {
            if (root == null)
            {
                Console.WriteLine("Словарь пуст.");
                return;
            }
            InOrderTraversal(root);
        }

        private void InOrderTraversal(DictionaryNode node)
        {
            if (node != null)
            {
                InOrderTraversal(node.Left);
                Console.WriteLine(string.Format("{0} - {1}", node.EnglishWord, node.RussianWord));
                InOrderTraversal(node.Right);
            }
        }
    }

    class Program
    {
        static BinaryDictionaryTree dictionary = new BinaryDictionaryTree("Главный Англо-Русский Словарь");

        static bool IsEnglishWord(string word)
        {
            if (string.IsNullOrWhiteSpace(word)) return false;
            return Regex.IsMatch(word, @"^[a-zA-Z\s\-]+$");
        }

        static bool IsRussianWord(string word)
        {
            if (string.IsNullOrWhiteSpace(word)) return false;
            return Regex.IsMatch(word, @"^[а-яА-ЯёЁ\s\-]+$");
        }

        static void Main(string[] args)
        {

            while (true)
            {
                Console.WriteLine("\n=== " + dictionary.DictionaryName.ToUpper() + " (Дерево) ===");
                Console.WriteLine("1. Добавить слово");
                Console.WriteLine("2. Удалить слово");
                Console.WriteLine("3. Найти перевод");
                Console.WriteLine("4. Вывести весь словарь (последовательный доступ)");
                Console.WriteLine("5. Загрузить словарь из файла");
                Console.WriteLine("0. Выход");
                Console.Write("Выбор: ");
                
                string choice = Console.ReadLine();

                if (choice == "0") break;

                switch (choice)
                {
                    case "1":
                        Console.Write("Введите английское слово: ");
                        string eng = Console.ReadLine();
                        if (!IsEnglishWord(eng))
                        {
                            Console.WriteLine("Ошибка: Английское слово должно состоять только из английских букв!");
                            break;
                        }
                        Console.Write("Введите перевод: ");
                        string rus = Console.ReadLine();
                        if (!IsRussianWord(rus))
                        {
                            Console.WriteLine("Ошибка: Перевод должен состоять только из русских букв!");
                            break;
                        }
                        dictionary.Add(eng, rus);
                        Console.WriteLine("Слово добавлено.");
                        break;
                    case "2":
                        Console.Write("Введите английское слово для удаления: ");
                        string delEng = Console.ReadLine();
                        if (!IsEnglishWord(delEng))
                        {
                            Console.WriteLine("Ошибка: Введено не английское слово!");
                            break;
                        }
                        if (dictionary.Remove(delEng))
                            Console.WriteLine("Слово удалено из словаря.");
                        else
                            Console.WriteLine("Слово не найдено.");
                        break;
                    case "3":
                        Console.Write("Введите английское слово: ");
                        string searchEng = Console.ReadLine();
                        if (!IsEnglishWord(searchEng))
                        {
                            Console.WriteLine("Ошибка: Введено не английское слово!");
                            break;
                        }
                        string foundRus = dictionary.Search(searchEng);
                        if (foundRus != null)
                            Console.WriteLine(string.Format("Перевод: {0}", foundRus));
                        else
                            Console.WriteLine("Слово не найдено.");
                        break;
                    case "4":
                        Console.WriteLine("\n--- Содержимое словаря ---");
                        dictionary.PrintInOrder();
                        Console.WriteLine("--------------------------");
                        break;
                    case "5":
                        LoadFromFile();
                        break;
                    default:
                        Console.WriteLine("Неверный ввод.");
                        break;
                }
            }
        }

        static void LoadFromFile()
        {
            Console.Write("Введите имя файла (например, dictionary.txt): ");
            string filename = Console.ReadLine();
            if (!File.Exists(filename))
            {
                Console.WriteLine("Файл не найден.");
                return;
            }

            try
            {
                string[] lines = File.ReadAllLines(filename);
                int count = 0;
                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    
                    string[] parts = line.Split(new char[] { '-' }, 2);
                    if (parts.Length == 2)
                    {
                        string eng = parts[0].Trim();
                        string rus = parts[1].Trim();
                        if (IsEnglishWord(eng) && IsRussianWord(rus))
                        {
                            dictionary.Add(eng, rus);
                            count++;
                        }
                        else
                        {
                            Console.WriteLine(string.Format("Пропущена некорректная строка: {0} - {1}", eng, rus));
                        }
                    }
                }
                Console.WriteLine(string.Format("Успешно загружено слов: {0}", count));
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Ошибка при чтении файла: {0}", ex.Message));
            }
        }
    }
}

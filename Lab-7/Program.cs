using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace lab_7
{
    // Создаем класс для обработки текста
    class TextProcessor
    {
        // Поля класса для хранения уникальной информации объекта
        private string filePath;
        private int sentencesToRead;

        // Конструктор класса
        public TextProcessor(string path, int count)
        {
            this.filePath = path;
            this.sentencesToRead = count;
        }

        // Метод для выполнения основной задачи
        public void ProcessAndPrintReversed()
        {
            Console.WriteLine($"\n--- Обработка файла: {filePath} (чтение {sentencesToRead} предложений) ---");

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Ошибка: Файл {filePath} не найден.");
                return;
            }

            List<string> sentences = new List<string>();
            StringBuilder currentSentence = new StringBuilder();

            using (StreamReader reader = new StreamReader(filePath, Encoding.UTF8))
            {
                int charCode;
                while ((charCode = reader.Read()) != -1)
                {
                    char ch = (char)charCode;
                    currentSentence.Append(ch);

                    if (ch == '.' || ch == '!' || ch == '?')
                    {
                        string sentence = currentSentence.ToString().TrimStart(' ', '\t', '\n', '\r');
                        if (sentence.Length > 0)
                        {
                            sentences.Add(sentence);
                        }
                        
                        currentSentence.Length = 0;

                        if (sentences.Count == sentencesToRead)
                        {
                            break;
                        }
                    }
                }
            }

            if (sentences.Count == 0)
            {
                Console.WriteLine("В файле не найдено ни одного предложения.");
            }
            else
            {
                Console.WriteLine("Предложения в обратном порядке:");
                for (int i = sentences.Count - 1; i >= 0; i--)
                {
                    Console.WriteLine(sentences[i]);
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            TextProcessor processor = new TextProcessor("infile.txt", 3);

            // Вызываем метод обработки
            processor.ProcessAndPrintReversed();
        }
    }
}
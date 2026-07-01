using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ZooProject
{
    class Program
    {
        // Името на текстовия файл, където се пазят данните
        static string filePath = "animals.txt";
        static List<Animal> zooAnimals = new List<Animal>();

        static void Main(string[] args)
        {
           
            // Зареждане на първоначалните данни от файла
            LoadDataFromFile();

            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("=== СИСТЕМА ЗА УПРАВЛЕНИЕ НА ЗООЛОГИЧЕСКА ГРАДИНА ===");
                Console.WriteLine("1. Добавяне на ново животно");
                Console.WriteLine("2. Промяна на статуса на наличност");
                Console.WriteLine("3. Проверка на информация за конкретно животно");
                Console.WriteLine("4. Справка за всички животни");
                Console.WriteLine("5. Изход");
                Console.Write("Изберете опция (1-5): ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        AddNewAnimal();
                        break;
                    case "2":
                        ChangeAvailability();
                        break;
                    case "3":
                        CheckAnimalInfo();
                        break;
                    case "4":
                        ShowAllAnimals();
                        break;
                    case "5":
                        exit = true;
                        Console.WriteLine("Благодарим ви, че използвахте системата! Довиждане.");
                        break;
                    default:
                        Console.WriteLine("Невалиден избор. Натиснете бутон за опит...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        // Логика за зареждане на данни при стартиране
        static void LoadDataFromFile()
        {
            if (!File.Exists(filePath))
            {
                // Ако файлът не съществува, го създаваме празен
                File.Create(filePath).Close();
                return;
            }

            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] parts = line.Split(';');
                if (parts.Length == 6)
                {
                    string id = parts[0];
                    string species = parts[1];
                    string name = parts[2];
                    int age = int.Parse(parts[3]);
                    string habitat = parts[4];
                    bool availability = bool.Parse(parts[5]);

                    zooAnimals.Add(new Animal(id, species, name, age, habitat, availability));
                }
            }
        }

        // Логика за автоматично записване на актуалния списък във файла
        static void SaveDataToFile()
        {
            List<string> linesToSave = zooAnimals.Select(a => a.ToFileRow()).ToList();
            File.WriteAllLines(filePath, linesToSave);
        }

        // Функционалност 1: Добавяне на ново животно
        static void AddNewAnimal()
        {
            Console.WriteLine("--- Добавяне на ново животно ---");

            Console.Write("Въведете уникално ID: ");
            string id = Console.ReadLine();

            // Проверка за дублиране на ID
            if (zooAnimals.Any(a => a.AnimalID.Equals(id, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("Грешка: Вече съществува животно с това ID!");
                Console.ReadKey();
                return;
            }

            Console.Write("Вид: ");
            string species = Console.ReadLine();

            Console.Write("Име: ");
            string name = Console.ReadLine();

            Console.Write("Възраст: ");
            if (!int.TryParse(Console.ReadLine(), out int age))
            {
                Console.WriteLine("Грешка: Невалиден формат за възраст!");
                Console.ReadKey();
                return;
            }

            Console.Write("Местообитание: ");
            string habitat = Console.ReadLine();

            Console.Write("Налично ли е за разглеждане (true/false): ");
            if (!bool.TryParse(Console.ReadLine(), out bool availability))
            {
                Console.WriteLine("Грешка: Невалиден статус! Въведете 'true' или 'false'.");
                Console.ReadKey();
                return;
            }

            // Добавяне в колекцията
            Animal newAnimal = new Animal(id, species, name, age, habitat, availability);
            zooAnimals.Add(newAnimal);

            // Автоматична актуализация на файла
            SaveDataToFile();

            Console.WriteLine("\nЖивотното е добавено успешно и файлът е актуализиран!");
            Console.ReadKey();
        }

        // Функционалност 2: Промяна на наличността
        static void ChangeAvailability()
        {
            Console.WriteLine("--- Промяна на статуса на наличност ---");
            Console.Write("Въведете ID на животното: ");
            string id = Console.ReadLine();

            Animal animal = zooAnimals.FirstOrDefault(a => a.AnimalID.Equals(id, StringComparison.OrdinalIgnoreCase));

            if (animal != null)
            {
                Console.WriteLine($"Текущ статус на {animal.Name} ({animal.Species}): {(animal.Availability ? "Налично" : "Неналично")}");
                Console.Write("Въведете новия статус (true за налично / false за неналично): ");

                if (bool.TryParse(Console.ReadLine(), out bool newStatus))
                {
                    animal.Availability = newStatus;

                    // Автоматична актуализация на файла
                    SaveDataToFile();

                    Console.WriteLine("Статусът е променен успешно и файлът е обновен!");
                }
                else
                {
                    Console.WriteLine("Грешка: Невалидна булева стойност.");
                }
            }
            else
            {
                Console.WriteLine("Животно с такова ID не е намерено.");
            }
            Console.ReadKey();
        }

        // Функционалност 3: Проверка на информацията за конкретно животно
        static void CheckAnimalInfo()
        {
            Console.WriteLine("--- Проверка на информация ---");
            Console.Write("Въведете ID на животното: ");
            string id = Console.ReadLine();

            Animal animal = zooAnimals.FirstOrDefault(a => a.AnimalID.Equals(id, StringComparison.OrdinalIgnoreCase));

            if (animal != null)
            {
                Console.WriteLine("\nИнформация за животното:");
                Console.WriteLine(animal.ToString());
            }
            else
            {
                Console.WriteLine("Животно с такова ID не е намерено.");
            }
            Console.ReadKey();
        }

        // Функционалност 4: Справка за всички животни
        static void ShowAllAnimals()
        {
            Console.WriteLine("--- Списък на всички животни в зоопарка ---");

            if (zooAnimals.Count == 0)
            {
                Console.WriteLine("Зоопаркът е празен в момента.");
            }
            else
            {
                foreach (var animal in zooAnimals)
                {
                    Console.WriteLine(animal.ToString());
                    Console.WriteLine(new string('-', 60)); // Визуален разделител
                }
            }
            Console.ReadKey();
        }
    }
}
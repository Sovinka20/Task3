using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class Program
    {
        //Подключение внешнего txt-файла
        const string f = "Test.txt";
        static ConcurrentDictionary<string, int> openWith;


        public static async Task Main()
        {
            /*"Задание: Напишите консольное приложение на C#, которое на вход принимает большой текстовый файл." +
                " На выходе создает текстовый файл с перечислением всех уникальных слов, встречающихся в тесте, " +
                "и количеством их употреблений, отсортированный в порядке убывания количества употреблений.*/
            string line;

            //Символы, которые необходимо удалить, с "-" - проблема, она может быть частью слова
            char[] delimiterChars = { '-', '/', '"', '(', ')','*', '[', ']', ' ', ',',
                    '.', ':', ';', '!', '?', '\t',
                    '1', '2', '3', '4', '5', '6', '7', '8', '9', '0'};
            int initialCapacity = 101;

            Console.WriteLine("Многопоточная реализация");

            //Подключение внешнего txt-файла

            //Время работы кода
            Stopwatch stopWatch = Stopwatch.StartNew();
            int numProcs = Environment.ProcessorCount;
            int concurrencyLevel = numProcs * 2;
            //Создание словаря
            openWith = new ConcurrentDictionary<string, int>(concurrencyLevel, initialCapacity);

            //Подключение содержимого txt-файла
            using (StreamReader r = new StreamReader(f))
            {
               // Calculate(r, openWith);
                //var t = Task.Run(() => Calculate(r, openWith));
               // t.Wait();

                await Task.Run(() => {
                    //Проверка анализируемой строки на наполняемость
                    while ((line = r.ReadLine()) != null)
                    {
                        //Перевод всех букв в строчный формат
                        line = line.ToLower();

                        //Разбиение строки на слова
                        string[] words = line.Split(delimiterChars);
                        //Заполнение словаря
                        foreach (string word in words)
                        {
                            if (word != "")
                            {
                                if (!openWith.ContainsKey(word))
                                {
                                    //Запись уникального слова в словарь и установка значения 1
                                    openWith.TryAdd(word, 1);
                                }
                                else
                                {
                                    //Повышении значения словаря, соответствующего ключу на 1
                                    openWith[word] += 1;
                                }
                            }
                        }

                    }
                });


            }

            //Запись содержимого словаря в txt-файл 
            using (var writer = new StreamWriter("dict.txt"))
            {
                //Linq реализация сортировки по значениям словаря по убыванию
                foreach (var kvp in openWith.OrderByDescending(x => x.Value))
                {
                    writer.WriteLine($"{kvp.Key}\t{kvp.Value}");
                }
            }
            //Вывод таймера
            Console.WriteLine("Time: " + stopWatch.ElapsedMilliseconds + "мс");
            Console.ReadKey();


            //Однопоточная реализация 
            //Single_threaded_method();
            //Многопоточная реализация 
           // Multithreaded_method();

            void Single_threaded_method()
            {
                Console.WriteLine("Однопоточная реализация");
                //Подключение внешнего txt-файла

                //Время работы кода
               // Stopwatch stopWatch = Stopwatch.StartNew();
                //int numProcs = Environment.ProcessorCount;
               // int concurrencyLevel = numProcs * 2;
                //Создание словаря
                ConcurrentDictionary<string, int> openWith = new ConcurrentDictionary<string, int>(concurrencyLevel, initialCapacity);

                //Подключение содержимого txt-файла
                using (StreamReader r = new StreamReader(f))
                {


                    //Проверка анализируемой строки на наполняемость
                    while ((line = r.ReadLine()) != null)
                    {
                        //Перевод всех букв в строчный формат
                        line = line.ToLower();

                        //Разбиение строки на слова
                        string[] words = line.Split(delimiterChars);
                        //Заполнение словаря
                        foreach (string word in words)
                        {
                            if (word != "")
                            {
                                if (!openWith.ContainsKey(word))
                                {
                                    //Запись уникального слова в словарь и установка значения 1
                                    openWith.TryAdd(word, 1);
                                }
                                else
                                {
                                    //Повышении значения словаря, соответствующего ключу на 1
                                    openWith[word] += 1;
                                }
                            }
                        }
                    }
                }
                //Запись содержимого словаря в txt-файл 
                using (var writer = new StreamWriter("dict.txt"))
                {
                    //Linq реализация сортировки по значениям словаря по убыванию
                    foreach (var kvp in openWith.OrderByDescending(x => x.Value))
                    {
                        writer.WriteLine($"{kvp.Key}\t{kvp.Value}");
                    }
                }
                //Вывод таймера
                Console.WriteLine("Time: " + stopWatch.ElapsedMilliseconds + "мс");
                Console.ReadKey();
            }

            void Multithreaded_method()
            {
              /* Console.WriteLine("Многопоточная реализация");

                //Подключение внешнего txt-файла

                //Время работы кода
                Stopwatch stopWatch = Stopwatch.StartNew();
                int numProcs = Environment.ProcessorCount;
                int concurrencyLevel = numProcs * 2;
                //Создание словаря
                openWith = new ConcurrentDictionary<string, int>(concurrencyLevel, initialCapacity);

                //Подключение содержимого txt-файла
                using (StreamReader r = new StreamReader(f))
                {
                    Calculate(r, openWith);
                    var t = Task.Run(() => Calculate(r, openWith));
                    t.Wait();

                    await Task.Run(() => {
                        // Just loop.
                        int ctr = 0;
                        for (ctr = 0; ctr <= 1000000; ctr++)
                        { }
                        Console.WriteLine("Finished {0} loop iterations",
                                          ctr);
                    });


                }

                //Запись содержимого словаря в txt-файл 
                using (var writer = new StreamWriter("dict.txt"))
                {
                    //Linq реализация сортировки по значениям словаря по убыванию
                    foreach (var kvp in openWith.OrderByDescending(x => x.Value))
                    {
                        writer.WriteLine($"{kvp.Key}\t{kvp.Value}");
                    }
                }
                //Вывод таймера
                Console.WriteLine("Time: " + stopWatch.ElapsedMilliseconds + "мс");
                Console.ReadKey();
                */
            }

            void Calculate(StreamReader r, ConcurrentDictionary< string, int> openWith)
            {
               
                }



                
                

            }

        }
        
    }


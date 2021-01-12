using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

#pragma warning disable CS0162

namespace AVS5
{
    public class Program
    {
        //######################
        //#                    #
        //#                    #
        //#     НАСТРОЙКИ      #
        //#                    #
        //#                    #
        //######################
        #region ConfigurationParams
        /// <summary>
        /// true - сначала все вопросы перемешиваются, потом из них берутся первые n штук.
        /// false - сначала из исходного упорядоченного списка берётся n тестов, потом они перемешиваются.
        /// Как работает <see cref="ShuffleThenTake"/>?
        /// Пусть у нас есть последовательность чисел (номера вопросов) 123456789.
        /// Допустим, необходимо потренеровать 5 вопросов.
        /// Если <see cref="ShuffleThenTake"/> = true, то произойдет следующее:
        /// 123456789 ---(вопросы перемешиваются)---> 476592138 ---(берется 5 штук)---> 47659 - пул вопросов.
        /// Если <see cref="ShuffleThenTake"/> = false, то произойдет следующее:
        /// 123456789 ---(берём 5 штук)---> 12345 ---(вопросы перемешиваются)---> 43125 - пул вопросов.
        /// Если необходимо потренеровать, например, с 20 по 80 вопрос, то делаем следующее:
        /// Устанавливаем <see cref="ShuffleThenTake"/> в false,
        /// <see cref="FirstQuestion"/> приравниваем числу 20.
        /// Затем в программе указываем, что нужно 60 вопросов (80-20).
        /// </summary>
        private static bool ShuffleThenTake = true; 
        /// <summary>
        /// true - результат ответа показывается сразу, после его введения.
        /// false - показывается только итоговый результат в конце теста.
        /// </summary>
        private const bool ShowResultInstantly = true;  
        /// <summary>
        /// Номер вопроса, с которого будет начинаться отбор тестов.
        /// Следует использовать, если хотите прорешать определённый вариант.
        /// Работает, если <see cref="ShuffleThenTake"/> сброшен.
        /// </summary>
        private static int FirstQuestion;
        /// <summary>
        /// Расположение файла с вопросами.
        /// </summary>
        private const string LOCATION = "avs_demo.txt";
        /// <summary>
        /// true - варианты ответов распологаются в случайном порядке.
        /// false - варианты ответов стоят на одном месте.
        /// </summary>
        public static bool RandomizeAnswers = true;
        #endregion ConfigurationParams

        private static List<Question> questions = new ();

        /// <summary>
        /// Выводит в консоль конфигурационные значения
        /// </summary>
        private static void PrintSettings()
        {
            Console.WriteLine("\n***ТЕКУЩИЕ НАСТРОЙКИ***\n");
            
            Console.WriteLine($"{nameof(ShuffleThenTake)} = {ShuffleThenTake} (перемешать все тесты перед тем, как выбрать n штук)");
            Console.WriteLine($"{nameof(ShowResultInstantly)} = {ShowResultInstantly} (мгновенное отображать правильность ответа на вопрос)");
            Console.WriteLine($"{nameof(RandomizeAnswers)} = {RandomizeAnswers} (рандомизация вариантов ответа)");
            
            if(!ShuffleThenTake)
                Console.WriteLine($"{nameof(FirstQuestion)} = {FirstQuestion} (пропустить заданное количество вопросов)");
            
            Console.WriteLine($"\nНастроить данные параметры и прочитать более точное описание можно в начале программы\n");
        }

        //Проверка диапазона включает граничные значения
        /// <summary>
        /// Цикл считывания числа с консоли.
        /// Если введенную строку удается преобразовать в число, возвращает это число.
        /// Иначе выводится сообщение в консоль и предлагается ввести число
        /// </summary>
        /// <param name="min">Левая граница</param>
        /// <param name="max">Правая граница</param>
        /// <returns>Число в промежутке [<paramref name="min"/>; <paramref name="max"/>]</returns>
        private static int EnterIntInRange(int min, int max)
        {
            int result;
            
            while (!int.TryParse(Console.ReadLine(), out result) || !(result >= min && result <= max))
                // todo: add delegate for printing error?
                Console.Write("Неверный ввод, повторите еще раз: ");
            
            return result;
        }

        private static bool Init()
        {
            Console.OutputEncoding = Encoding.UTF8;
            string[] lines;
            try
            {
                lines = File.ReadAllLines(LOCATION);
            }
            catch (Exception)
            {
                // todo: delegate error printing
                Console.WriteLine($"Файл по пути \"{LOCATION}\" не найден");
                return false;
            }
            for (int i = 0; i < lines.Length; i += 4)
            {
                try
                {
                    questions.Add(new Question { Text = lines[i], Variants = lines[i + 1], RightAnswer = int.Parse(lines[i + 2]) });
                }
                catch (Exception)
                {
                    Console.WriteLine($"Error on question {(i / 4) + 1}, line {i + 1} - {i + 3}");
                    return false;
                }
            }
            if (ShuffleThenTake)
                questions.Shuffle();
            Console.WriteLine($"Тесты успешно загружены ({questions.Count} шт.)");
            // todo: delegate printing conf params?
            PrintSettings();
            return true;
        }

        private static int amountOfTests = -1;
        
        private static void TestSetup()
        {
            int amountOfTests;
            
            Console.WriteLine("Нажмите на любую клавишу...");
            Console.ReadKey();
            Console.Clear();

            if (Program.amountOfTests < 1 || Program.amountOfTests >= questions.Count)
            {
                Console.Write(!ShuffleThenTake
                    ? $"Выберите количество вопросов (1-{questions.Count - FirstQuestion}) (пропущено {FirstQuestion} вопросов): "
                    : $"Выберите количество вопросов (1-{questions.Count}): ");
                amountOfTests = EnterIntInRange(1, questions.Count);
            }

            else
            {
                amountOfTests = Program.amountOfTests;
            }
            
            Console.WriteLine(!ShuffleThenTake
                ? $"\nБудут использованы вопросы №{FirstQuestion + 1} - {FirstQuestion + amountOfTests} из исходного списка\n"
                : $"\nВопросы будут выбраны из всего списка\n");

            var questionsForTest = ShuffleThenTake 
                ? questions.Take(amountOfTests).ToList() 
                : questions.Skip(FirstQuestion).Take(amountOfTests).ToList();
            
            if (!ShuffleThenTake)
                questionsForTest.Shuffle();
            
            BeginTest(questionsForTest);
        }

        private static void BeginTest(List<Question> questionsForTest)
        {
            Console.WriteLine("Нажмите на любую клавишу для начала теста ...");
            Console.ReadKey();
            Console.Clear();
            
            for (var i = 0; i < questionsForTest.Count; i++)
            {
                Console.Clear();
                Console.WriteLine($"Вопрос {i + 1} из {questionsForTest.Count}\n");
                Console.WriteLine(questionsForTest[i]);
                Console.Write("Ваш ответ (1-5): ");
                
                questionsForTest[i].ChosenAnswer = EnterIntInRange(1, 5);
                questionsForTest[i].IsRight = questionsForTest[i].RightAnswer == questionsForTest[i].ChosenAnswer;
                
                if (!ShowResultInstantly) 
                    continue;
                
                Console.WriteLine();
                if (questionsForTest[i].IsRight)
                    Console.WriteLine("Правильно");
                
                else
                {
                    Console.WriteLine("Неправильно");
                    Console.WriteLine($"Правильный ответ: {questionsForTest[i].RightAnswer}");
                }
                
                Console.WriteLine("\nНажмите на любую клавишу...");
                Console.ReadKey();
            }
            
            EndTest(questionsForTest);
        }

        private static void EndTest(List<Question> questionsForTest)
        {
            Console.Clear();
            var amountOfRightAnswers = questionsForTest.Count(q => q.IsRight);
            
            Console.WriteLine($"Результат {amountOfRightAnswers} из {questionsForTest.Count} ({(int)((double)amountOfRightAnswers / questionsForTest.Count * 100)}%)\n");
            Console.WriteLine("Выберите опцию:");
            Console.WriteLine("1 - Просмотр всех отвеченных вопросов");
            Console.WriteLine("2 - Просмотр вопросов с неверным ответом");
            Console.WriteLine("3 - Выход");
            
            var option = EnterIntInRange(1, 3);
            
            Console.Clear();
            switch (option)
            {
                case 1:
                {
                    foreach (var q in questionsForTest)
                    {
                        Console.WriteLine(!q.IsRight
                            ? "\n*************WRONG*************"
                            : "\n*******************************");
                        Console.WriteLine(q);
                        Console.WriteLine($"\nПравльный ответ: {q.RightAnswer}\nВаш ответ: {q.ChosenAnswer}\n");
                    }

                    Console.WriteLine("Нажмите любую клавишу для выхода");
                    Console.ReadKey();
                    break;
                }
                
                case 2:
                {
                    if (!questionsForTest.Any(question => !question.IsRight))
                    {
                        Console.WriteLine("Empty list");
                        break;
                    }

                    foreach (var q in questionsForTest.Where(question => !question.IsRight))
                    {
                        Console.WriteLine("\n*******************************");
                        Console.WriteLine(q);
                        Console.WriteLine($"Правльный ответ: {q.RightAnswer}\nВаш ответ: {q.ChosenAnswer}\n");
                    }

                    Console.WriteLine("Нажмите любую клавишу для выхода");
                    Console.ReadKey();
                    break;
                }
                
                case 3:
                    break;
            }
        }


        static void Main(string[] args)
        {
            SetOptions(args);
            if (Init())
                TestSetup();
            else
            {
                Console.WriteLine("Нажмите любую клавишу для выхода");
                Console.ReadKey();
            }
        }
        
        // amountOfQuestions skipFirstNQuestions -s -r
        // -s - сбросить SHUFFLETHENTAKE
        // -r - сбросить RANDOMIZEANSWERS
        private static void SetOptions(string[] args)
        {
            try
            {
                int.TryParse(args[0], out amountOfTests);
                int.TryParse(args[1], out FirstQuestion);
            }

            catch (IndexOutOfRangeException)
            {
                
            }

            foreach (var arg in args)
            {
                if (arg == "-s")
                {
                    ShuffleThenTake = false;
                    break;
                }

                if (arg == "-r")
                {
                    RandomizeAnswers = false;
                    break;
                }
            }
        }
    }
}

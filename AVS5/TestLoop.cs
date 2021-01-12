using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AVS5
{
    public class TestLoop
    {
        private readonly List<Question> _questions = new ();
        private readonly TestOptions _options = new(); 

        /// <summary>
        /// Выводит в консоль конфигурационные значения
        /// </summary>
        private void PrintSettings()
        {
            Console.WriteLine("\n***ТЕКУЩИЕ НАСТРОЙКИ***\n");
            
            Console.WriteLine($"{nameof(_options.ShuffleThenTake)} = {_options.ShuffleThenTake} (перемешать все тесты перед тем, как выбрать n штук)");
            Console.WriteLine($"{nameof(_options.ShowResultInstantly)} = {_options.ShowResultInstantly} (мгновенное отображать правильность ответа на вопрос)");
            Console.WriteLine($"{nameof(_options.IsRandomOrder)} = {_options.IsRandomOrder} (рандомизация вариантов ответа)");
            
            if(!_options.ShuffleThenTake)
                Console.WriteLine($"{nameof(_options.FirstQuestion)} = {_options.FirstQuestion} (пропустить заданное количество вопросов)");
            
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
        private int EnterIntInRange(int min, int max)
        {
            int result;
            
            while (!int.TryParse(Console.ReadLine(), out result) || !(result >= min && result <= max))
                // todo: add delegate for printing error?
                Console.Write("Неверный ввод, повторите еще раз: ");
            
            return result;
        }

        private  bool Init()
        {
            Console.OutputEncoding = Encoding.UTF8;
            
            string[] lines;
            try
            {
                lines = File.ReadAllLines(TestOptions.LOCATION);
            }
            
            catch (Exception)
            {
                // todo: delegate error printing
                Console.WriteLine($"Файл по пути \"{TestOptions.LOCATION}\" не найден");
                return false;
            }
            
            for (var i = 0; i < lines.Length; i += 4)
            {
                try
                {
                    var question = new Question(
                        lines[i], 
                        lines[i + 1], 
                        int.Parse(lines[i + 2]),
                        _options.IsRandomOrder);
                    _questions.Add(question);
                }
                
                catch (Exception)
                {
                    Console.WriteLine($"Error on question {(i / 4) + 1}, line {i + 1} - {i + 3}");
                    return false;
                }
            }
            
            if (_options.ShuffleThenTake)
                _questions.Shuffle();
            
            Console.WriteLine($"Тесты успешно загружены ({_questions.Count} шт.)");
            
            // todo: delegate printing conf params?
            PrintSettings();
            return true;
        }

        private int amountOfTests = -1;
        
        private void TestSetup()
        {
            int amountOfTests;
            
            Console.WriteLine("Нажмите на любую клавишу...");
            Console.ReadKey();
            Console.Clear();

            if (this.amountOfTests < 1 || this.amountOfTests >= _questions.Count)
            {
                Console.Write(!_options.ShuffleThenTake
                    ? $"Выберите количество вопросов (1-{_questions.Count - _options.FirstQuestion}) (пропущено {_options.FirstQuestion} вопросов): "
                    : $"Выберите количество вопросов (1-{_questions.Count}): ");
                amountOfTests = EnterIntInRange(1, _questions.Count);
            }

            else
            {
                amountOfTests = this.amountOfTests;
            }
            
            Console.WriteLine(!_options.ShuffleThenTake
                ? $"\nБудут использованы вопросы №{_options.FirstQuestion + 1} - {_options.FirstQuestion + amountOfTests} из исходного списка\n"
                : $"\nВопросы будут выбраны из всего списка\n");

            var questionsForTest = _options.ShuffleThenTake 
                ? _questions.Take(amountOfTests).ToList() 
                : _questions.Skip(_options.FirstQuestion).Take(amountOfTests).ToList();
            
            if (!_options.ShuffleThenTake)
                questionsForTest.Shuffle();
            
            BeginTest(questionsForTest);
        }

        private void BeginTest(List<Question> questionsForTest)
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
                
                if (!_options.ShowResultInstantly) 
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

        private void EndTest(List<Question> questionsForTest)
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
        
        // amountOfQuestions skipFirstNQuestions -s -r
        // -s - сбросить SHUFFLETHENTAKE
        // -r - сбросить RANDOMIZEANSWERS
        private  void SetOptions(string[] args)
        {
            try
            {
                int.TryParse(args[0], out amountOfTests);
                int.TryParse(args[1], out _options.FirstQuestion);
            }

            catch (IndexOutOfRangeException)
            {
                
            }

            foreach (var arg in args)
            {
                if (arg == "-s")
                {
                    _options.ShuffleThenTake = false;
                    break;
                }

                if (arg == "-r")
                {
                    _options.IsRandomOrder = false;
                    break;
                }
            }
        }

        public void Start(string[] args)
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
    }
}
namespace AVS5.Client.Console.Playground
{
    public static class MessageConstants
    {
        public static readonly string InputNumber = "Введите номер ответа";
        public static readonly string RepeatAgain = "Повторите еще раз";
        
        public static readonly string YourAnswer = "Ваш ответ";
        public static readonly string RightAnswer = "Правильный ответ";

        public static readonly string Correct = "Правильно";
        public static readonly string Incorrect = "Неправильно";

        public static readonly string NullOrEmptyError =
            "Была введена пустая строка или строка, состоящая целиком из пробелов";
        
        public static readonly string WrongInputFormatError =
            "Какое-то из чисел было введено в неверном формате";
        
        public static readonly string OverflowInputError =
            "Какое-то из чисел вышло за заданные системой рамки";
        
        public static readonly string Result = "Итог";
        public static readonly string NoWrongAnswers = "Ни единого неправильного ответа";
    }
}
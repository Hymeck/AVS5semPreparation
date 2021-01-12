using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AVS5
{
    public class Question
    {
        public readonly string Text;
        public string Variants;
        public int RightAnswer;
        public readonly bool IsRandomized;
        
        public int ChosenAnswer;
        public bool IsRight;

        public Question(string text, string variants, int rightAnswer, bool isRandomized)
        {
            Text = text;
            Variants = variants;
            RightAnswer = rightAnswer;
            IsRandomized = isRandomized;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(Text);
            if (!IsRandomized)
            {
                foreach (var s in Variants.Split(';'))
                    sb.AppendLine(s.Trim());
                return sb.ToString();
            }
            else
            {
                //  Randomize possible answers
                var splittedVariants = Variants.Split(';').Where(s => s != "").Select(s => s).ToArray();
                var rightAnswerText = splittedVariants[RightAnswer - 1];
                splittedVariants = new List<string>(splittedVariants).Shuffle().ToArray();
                RightAnswer = Array.IndexOf(splittedVariants, rightAnswerText) + 1;
                Variants = "";
                for (var i = 0; i < splittedVariants.Length; i++)
                {
                    sb.AppendLine($"{i + 1}) {splittedVariants[i].Trim().Substring(3)}");
                    Variants += string.Format($"{ i + 1}) { splittedVariants[i].Trim().Substring(3)};");
                }
                return sb.ToString();
            }
        }
    }
}

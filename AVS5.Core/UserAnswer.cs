using System.Collections.Generic;

namespace AVS5.Core
{
    public record UserAnswer(int QuestionNumber, IList<int> ChosenAnswers, bool IsRight);
}
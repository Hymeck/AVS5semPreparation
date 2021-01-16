using System.Collections.Generic;
using AVS5.Data.Dto;

namespace AVS5.Data
{
    public interface IDataProvider<out T> where T : QuestionDto
    {
        IEnumerable<T> GetData();
    }
}
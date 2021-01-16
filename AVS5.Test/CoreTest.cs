using System.Collections.Generic;
using System.Linq;
using AVS5.Core;
using AVS5.Data;
using AVS5.Data.Dto;
using NUnit.Framework;

namespace AVS5.Test
{
    public class CoreTest
    {
        private IDataProvider<QuestionDto> _data;
        private IList<QuestionDto> _questions;
        private string _location = "avs_demo.txt";
        
        [SetUp]
        public void Setup()
        {
            _data = new QuestionDataProvider(_location);
            _questions = _data.GetData().ToList();
        }
    }
}
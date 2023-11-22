using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerTests
{
    public class TestUnusedPrivateFields
    {
        private readonly int _field;
        private readonly int _field1;
        private readonly int _field2;

        TestUnusedPrivateFields()
        {
            _field = 0;
            _field1 = 1;
            _field2 = 2;
        }
        public void main()
        {
            Console.WriteLine(_field2);
        }

    }
}

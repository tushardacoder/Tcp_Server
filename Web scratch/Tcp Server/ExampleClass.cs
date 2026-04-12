using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



    public class TestService
    {
        public readonly TestChildService _testService;

        public TestService(TestChildService testChildService)
        {
            _testService = testChildService;

        }

        public void print()
        {
            Console.WriteLine("hello from Testservices 1");
            _testService.print();
        }

    }





public class TestChildService
    {
        public void print()
        {
            Console.WriteLine("Hello from Techservices");
        }
    }



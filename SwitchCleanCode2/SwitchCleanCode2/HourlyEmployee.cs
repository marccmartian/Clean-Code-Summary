using System;
using System.Collections.Generic;
using System.Text;

namespace SwitchCleanCode2
{
    class HourlyEmployee : Employee
    {
        public HourlyEmployee(string type) : base(type) { }

        public override double CalculatePay()
        {
            return 200.52;
        }

        public override void DeliverPay()
        {
            Console.WriteLine("Hourly pay sucessfull");
        }

        public override String IsPayDay()
        {
            return "Quincena";
        }
    }
}

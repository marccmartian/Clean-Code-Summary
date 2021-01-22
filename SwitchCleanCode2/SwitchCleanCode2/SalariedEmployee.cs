using System;
using System.Collections.Generic;
using System.Text;

namespace SwitchCleanCode2
{
    class SalariedEmployee : Employee
    {
        public SalariedEmployee(string type) : base(type) { }

        public override double CalculatePay()
        {
            return 300.53;
        }

        public override void DeliverPay()
        {
            Console.WriteLine("Salaried pay sucessfull");
        }

        public override String IsPayDay()
        {
            return "Fin de mes";
        }
    }
}

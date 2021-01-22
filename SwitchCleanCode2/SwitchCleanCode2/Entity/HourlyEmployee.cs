using System;

namespace SwitchCleanCode2.Entity
{
    public class HourlyEmployee : Employee
    {
        public override double CalculatePay()
        {
            return 200.52;
        }

        public override void DeliverPay()
        {
            Console.WriteLine("Hourly pay sucessfull");
        }

        public override string IsPayDay()
        {
            return "Quincena";
        }
    }
}

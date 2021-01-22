using System;

namespace SwitchCleanCode2.Entity
{
    public class SalariedEmployee : Employee
    {
        public override double CalculatePay()
        {
            return 300.53;
        }

        public override void DeliverPay()
        {
            Console.WriteLine("Salaried pay sucessfull");
        }

        public override string IsPayDay()
        {
            return "Fin de mes";
        }
    }
}

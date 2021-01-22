using System;

namespace SwitchCleanCode2.Entity
{
    public class CommissionedEmployee : Employee
    {
        public override double CalculatePay()
        {
            return 100.51;
        }

        public override void DeliverPay()
        {
            Console.WriteLine("Comissioned pay sucessfull");
        }

        public override string IsPayDay()
        {
            return "Primer fin de semana";
        }       
    }
}

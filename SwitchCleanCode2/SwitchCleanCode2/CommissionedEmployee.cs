using System;
using System.Collections.Generic;
using System.Text;

namespace SwitchCleanCode2
{
    class CommissionedEmployee : Employee
    {
        public CommissionedEmployee(string type) : base(type) { }

        public override double CalculatePay()
        {
            return 100.51;
        }

        public override void DeliverPay()
        {
            Console.WriteLine("Comissioned pay sucessfull");
        }

        public override String IsPayDay()
        {
            return "Primer fin de semana";
        }       
    }
}

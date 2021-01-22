using System;
using System.Collections.Generic;
using System.Text;

namespace SwitchCleanCode2
{
    class EmployeeFactoryImpl : IEmployeeFactory
    {
        public Employee MakeEmployee(String Type)
        {
            switch (Type)
            {
                case "COMMISSIONED":
                    return new CommissionedEmployee(Type);
                case "HOURLY":
                    return new HourlyEmployee(Type);
                case "SALARIED":
                    return new SalariedEmployee(Type);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}

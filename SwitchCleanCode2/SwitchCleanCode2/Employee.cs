using System;
using System.Collections.Generic;
using System.Text;

namespace SwitchCleanCode2
{
    abstract class Employee
    {
        private String employeeType;

        public Employee(String type)
        {
            employeeType = type;
        }

        public String GetEmployeeType()
        {
            return employeeType;
        }

        public abstract String IsPayDay();

        public abstract Double CalculatePay();

        public abstract void DeliverPay();
    }
}

using System;

namespace SwitchCleanCode2
{
    class Program
    {
        static void Main(string[] args)
        {
            EmployeeFactoryImpl employee = new EmployeeFactoryImpl();

            Employee basicEmployee = employee.MakeEmployee("COMMISIONED");

            basicEmployee.CalculatePay();
            basicEmployee = employee.MakeEmployee("HOURLY");
            basicEmployee.CalculatePay();

            var marvin = employee.MakeEmployee("COMMISSIONED");
            Console.Write(
                    $"Tipo: {marvin.GetEmployeeType()}, " +
                    $"Pago: {marvin.CalculatePay()}, " +
                    $"Dia: {marvin.IsPayDay()}, " +
                    $"Estado: "
                );
            marvin.DeliverPay();

            var goku = employee.MakeEmployee("HOURLY");
            Console.Write(
                    $"Tipo: {goku.GetEmployeeType()}, " +
                    $"Pago: {goku.CalculatePay()}, " +
                    $"Dia: {goku.IsPayDay()}, " +
                    $"Estado: "
                );
            goku.DeliverPay();

            var gohan = employee.MakeEmployee("SALARIED");
            Console.Write(
                    $"Tipo: {gohan.GetEmployeeType()}, " +
                    $"Pago: {gohan.CalculatePay()}, " +
                    $"Dia: {gohan.IsPayDay()}, " +
                    $"Estado: "
                );
            gohan.DeliverPay();
        }
    }
}

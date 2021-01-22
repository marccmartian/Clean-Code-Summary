using System.Collections.Generic;
using SwitchCleanCode2.Entity;

namespace SwitchCleanCode2
{
    internal class Program
    {
        public static void Main()
        {
            IEnumerable<Employee> employees = new[] {Employee.MakeCommissionedEmployee, Employee.MakeHourlyEmployee, Employee.MakeSalariedEmployee};
            employees.Foreach(apply_pay);
            
            void apply_pay(Employee employee)
            {
                employee.CalculatePay();
            }
        }


        // static void Main(string[] args)
        // {
        //     EmployeeFactoryImpl employee = new EmployeeFactoryImpl();
        //
        //     Employee basicEmployee = employee.MakeEmployee("COMMISIONED");
        //
        //     basicEmployee.CalculatePay();
        //     basicEmployee = employee.MakeEmployee("HOURLY");
        //     basicEmployee.CalculatePay();
        //
        //     var marvin = employee.MakeEmployee("COMMISSIONED");
        //     Console.Write(
        //             $"Tipo: {marvin.GetEmployeeType()}, " +
        //             $"Pago: {marvin.CalculatePay()}, " +
        //             $"Dia: {marvin.IsPayDay()}, " +
        //             $"Estado: "
        //         );
        //     marvin.DeliverPay();
        //
        //     var goku = employee.MakeEmployee("HOURLY");
        //     Console.Write(
        //             $"Tipo: {goku.GetEmployeeType()}, " +
        //             $"Pago: {goku.CalculatePay()}, " +
        //             $"Dia: {goku.IsPayDay()}, " +
        //             $"Estado: "
        //         );
        //     goku.DeliverPay();
        //
        //     var gohan = employee.MakeEmployee("SALARIED");
        //     Console.Write(
        //             $"Tipo: {gohan.GetEmployeeType()}, " +
        //             $"Pago: {gohan.CalculatePay()}, " +
        //             $"Dia: {gohan.IsPayDay()}, " +
        //             $"Estado: "
        //         );
        //     gohan.DeliverPay();
        // }
    }
}
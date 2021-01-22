namespace SwitchCleanCode2.Entity
{
    public abstract class Employee
    {
        public abstract string IsPayDay();

        public abstract double CalculatePay();

        public abstract void DeliverPay();

        public static Employee MakeCommissionedEmployee
            => new CommissionedEmployee();

        public static Employee MakeHourlyEmployee
            => new HourlyEmployee();

        public static Employee MakeSalariedEmployee
            => new SalariedEmployee();
    }
}
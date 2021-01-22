using System;
using System.Collections.Generic;
using System.Text;

namespace SwitchCleanCode2
{
    interface IEmployeeFactory
    {
        Employee MakeEmployee(String type);
    }
}

// See https://aka.ms/new-console-template for more information
using HealthCare;
using HealthCare.Secretary;



//Manager m = new Manager();
//m.Load();



Secretary manu = new Secretary();
bool showMenu = true;
    while (showMenu)
    {
       showMenu = manu.WriteManu();
}




Console.WriteLine("aasa");
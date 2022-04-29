// See https://aka.ms/new-console-template for more information
using HealthCare;
using HealthCare.Secretary;
using System.Text.Json;

SecretaryMenu manu = new SecretaryMenu();
bool showMenu = true;
    while (showMenu)
    {
       showMenu = manu.WriteManu();
}




Console.WriteLine("aasa");
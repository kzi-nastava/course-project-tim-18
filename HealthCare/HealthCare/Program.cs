// See https://aka.ms/new-console-template for more information
using HealthCare;
using System.Text.Json;

SecretaryManu manu = new SecretaryManu();
bool showMenu = true;
    while (showMenu)
    {
       showMenu = manu.WriteManu();
}





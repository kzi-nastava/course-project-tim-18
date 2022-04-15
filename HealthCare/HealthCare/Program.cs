// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using HealthCare;


Equipment e1 = new Equipment(EquipmentType.RoomFurniture, "Stolica", 7);
Equipment e2 = new Equipment(EquipmentType.RoomFurniture, "Sto", 1);
Equipment e3 = new Equipment(EquipmentType.RoomFurniture, "Krevet", 2);
Equipment e4 = new Equipment(EquipmentType.OperationTools, "Skalper", 13);
Equipment e5 = new Equipment(EquipmentType.OperationTools, "Makaze", 5);

Room r1 = new Room(RoomType.OperationRoom, "A1", new List<Equipment>() { e1,e2,e3,e4,e5});


 e1 = new Equipment(EquipmentType.RoomFurniture, "Stolica", 20);
 e2 = new Equipment(EquipmentType.RoomFurniture, "Sto", 4);
 e3 = new Equipment(EquipmentType.RoomFurniture, "Krevet", 10);
 e4 = new Equipment(EquipmentType.HallwayEquipment, "Metla", 2);
 e5 = new Equipment(EquipmentType.HallwayEquipment, "Sredstvo za ciscenje", 3);

Room r2 = new Room(RoomType.RestingRoom, "B1", new List<Equipment>() { e1,e2,e3,e4,e5});

e1 = new Equipment(EquipmentType.RoomFurniture, "Stolica", 3);
e2 = new Equipment(EquipmentType.RoomFurniture, "Sto", 1);
e3 = new Equipment(EquipmentType.RoomFurniture, "Krevet", 1);
e4 = new Equipment(EquipmentType.MedicalExaminationTools, "Kompjuter", 2);
e5 = new Equipment(EquipmentType.MedicalExaminationTools, "Merac za pritisak", 1);

Room r3 = new Room(RoomType.MedicalExaminationRoom, "C1", new List<Equipment>() { e1,e2,e3,e4,e5});



 e1 = new Equipment(EquipmentType.RoomFurniture, "Stolica", 3);
 e2 = new Equipment(EquipmentType.RoomFurniture, "Sto", 3);
 e3 = new Equipment(EquipmentType.RoomFurniture, "Krevet", 5);
 e4 = new Equipment(EquipmentType.OperationTools, "Sekac", 3);
 e5 = new Equipment(EquipmentType.OperationTools, "Skalper", 3);

Room r4 = new Room(RoomType.OperationRoom, "A2", new List<Equipment>() { e1,e2,e3,e4,e5});


e1 = new Equipment(EquipmentType.RoomFurniture, "Stolica", 20);
e2 = new Equipment(EquipmentType.RoomFurniture, "Sto", 4);
e3 = new Equipment(EquipmentType.RoomFurniture, "Krevet", 10);
e4 = new Equipment(EquipmentType.HallwayEquipment, "Metla", 2);
e5 = new Equipment(EquipmentType.HallwayEquipment, "Sredstvo za ciscenje", 3);

Room r5 = new Room(RoomType.RestingRoom, "B2", new List<Equipment>() { e1,e2,e3,e4,e5});


Hospital h = new Hospital("Medlab", new List<Room>(){r1,r2,r3,r4,r5});
Manager m = new Manager("admin", "admin", h);



string fileName = "..\\Data\\ManagerData.json"; 
string jsonString = JsonSerializer.Serialize(m);
Console.WriteLine(jsonString);
File.WriteAllText(fileName, jsonString);



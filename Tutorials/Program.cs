//// See https://aka.ms/new-console-template for more information

//using Tutorials.Models;
//using Tutorials.Validators;
////create a new instance of person class
//PersonDetails person = new PersonDetails();
////this is for making sure that the name to be entered is valid
//start:
//Console.Write("Enter a valid Name :");
////receive the string name
//var name = Console.ReadLine();
////hey
//if (!Validate.CheckIfNameIsValid(name!.Trim()))
//{
//    //start afresh
//    goto start;
//}
////assign the name provided to the person.Name
//person.Name = name;
////go to the next input after the first one is valid
//next:
//Console.Write("Enter a valid Age :");

//var enteredage = Console.ReadLine();

//if (!Validate.CheckIfAgeIsValid(enteredage!.ToString()))
//{
//    goto next;
//}
//person.Dob = Validate.getDateAfterSubtraction(int.Parse(enteredage));

//Console.WriteLine($"Hello you Name is {person.Name} and the Date you were born is {person.Dob.ToShortDateString()}");
using Tutorials.LuckyNumber;

LuckyNumber.GetLuckyNumber();
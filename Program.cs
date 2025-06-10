

using LINQ_Challeges.Models;
using static System.Console;

public class Program
{

    private static void Main(string[] args)
    {
        WriteLine("Hello, LINQ");
        Challnege_1();
        

    }


    static void Challnege_1()
    {
        var numbers = new List<int> { 1, 2, 3, 4, 5 };
        var query = from num in numbers select num;
        foreach (var num in numbers) 
        {
            WriteLine(num);
        }
    }
}
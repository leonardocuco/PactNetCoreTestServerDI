using System;
using System.Linq;

namespace ConsumerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("List of Vehiculs :");
            SimpleConsumerApiClient.GetAllVehiculs("http://localhost:53707").GetAwaiter().GetResult()
                .ToList()
                .ForEach(v => Console.WriteLine(v.Label));

            Console.WriteLine();
            Console.WriteLine("Best vehicul :");
            Console.WriteLine(SimpleConsumerApiClient.GetAllVehiculById(2, "http://localhost:53707").GetAwaiter().GetResult().Label);
                
            Console.ReadKey();
        }
    }
}

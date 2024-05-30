using BusinessLogicLayer;
using DataAccessLayer;
using System.Runtime.InteropServices;

namespace PresentationLayer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get input file from the user
            Console.Write("Enter the path of the input Excel file: ");
            string inputFilePath = Console.ReadLine();
            
            if (!File.Exists(inputFilePath))
            {
                Console.WriteLine("TestInput.xlsx is not located at the given path");
                return;
            }

            // Get output file from the user
            string outputDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string outputFilePath = Path.Combine(outputDirectory, "TestOutput.xlsx");

            var excelDataAccess = new ExcelDataAccess();
            var productParser = new ProductParser();

            // Read product names from the input file
            List<string> productNames  = excelDataAccess.ReadProductNamesFromExcel(inputFilePath);

            // Parse product names and create product objects
            var products = new List<(string Name, List<string> Attributes)>();

            var size = productNames.Count;
            var span = CollectionsMarshal.AsSpan(productNames);

            for (var i = 0; i < size; i++)
            {
                var result = span[i];
                var attribute = productParser.ParseProductName(result);
                products.Add((result, attribute));
            }

            // Write products to the output file
            excelDataAccess.WriteProductsToExcel(outputFilePath, products);

            Console.WriteLine("Output file created successfully.");
        }
    }
}
using OfficeOpenXml;
using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {
        // Get input file from the user
        Console.Write("Enter the path of the input Excel file: ");
        string inputFilePath = Console.ReadLine();

        // Get output file from the user
        Console.Write("Enter the path for the output Excel file: ");
        string outputFilePath = Console.ReadLine();

        // Create FileInfo objects
        var inputFile = new FileInfo(inputFilePath);
        var outputFile = new FileInfo(outputFilePath);

        ReadExcelFile(inputFile, outputFile);
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        ReadExcelFile(inputFile, outputFile);
    }

    static void ReadExcelFile(FileInfo inputFile, FileInfo outputFile)
    {
        var products = new List<Product>();

        using (var package = new ExcelPackage(inputFile))
        {
            var worksheet = package.Workbook.Worksheets.FirstOrDefault();
            if (worksheet == null)
            {
                Console.WriteLine("No worksheet found in the input file.");
                return;
            }

            var rowCount = worksheet.Dimension.Rows;
            for (int row = 2; row <= rowCount; row++)
            {
                var productName = worksheet.Cells[row, 1].Value?.ToString();
                if (!string.IsNullOrEmpty(productName))
                {
                    var product = ParseProductName(productName);
                    products.Add(product);
                }
            }
        }

        CreateOutputFile(outputFile, products);
    }

    static Product ParseProductName(string productName)
    {
        var product = new Product { Name = productName };
        var attributes = new List<string>();

        var regex = new Regex(@"(?<=\()(.*?)(?=\))");
        var matches = regex.Matches(productName);

        foreach (Match match in matches)
        {
            attributes.Add(match.Value);
        }

        product.Attributes = attributes;
        return product;
    }

    static void CreateOutputFile(FileInfo outputFile, List<Product> products)
    {
        using (var package = new ExcelPackage(outputFile))
        {
            var worksheet = package.Workbook.Worksheets.Add("Sheet1");

            // Write headers
            worksheet.Cells[1, 1].Value = "Product Name";
            for (int i = 1; i <= products.Max(p => p.Attributes.Count); i++)
            {
                worksheet.Cells[1, i + 1].Value = $"Attribute {i}";
            }

            // Write data
            int row = 2;
            foreach (var product in products)
            {
                worksheet.Cells[row, 1].Value = product.Name;
                for (int i = 0; i < product.Attributes.Count; i++)
                {
                    worksheet.Cells[row, i + 2].Value = product.Attributes[i];
                }
                row++;
            }

            package.Save();
        }
    }
}

class Product
{
    public string Name { get; set; }
    public List<string> Attributes { get; set; }
}
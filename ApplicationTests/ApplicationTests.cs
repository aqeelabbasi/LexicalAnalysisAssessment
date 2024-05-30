using BusinessLogicLayer;
using DataAccessLayer;

namespace ApplicationTests
{
    public class ProgramTests
    {
        [Fact]
        public void Main_ValidInputAndOutput_ProcessesDataCorrectly()
        {
            // Arrange
            var inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "TestData", "TestInput.xlsx");
            string outputDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string outputFilePath = Path.Combine(outputDirectory, "TestOutput.xlsx");
            var expectedProducts = new List<(string Name, List<string> Attributes)>
            {
                ("ALEVE LIQUID GELS 80CT 2DZ", new List<string> { "ALEVE", "LIQUID", "GELS", "80CT", "2DZ" }),
                ("ALEVE LIQUID GELS ARTHRITIS 80S 2DZ", new List<string> { "ALEVE", "LIQUID", "GELS", "ARTHRITIS", "80S", "2DZ" }),
                ("ALEVE ARTHRITIS CPLT 90S 2 DZ", new List<string> { "ALEVE", "ARTHRITIS", "CPLT", "90S", "2DZ" }),
                ("ALEVE CAPLET 6CT BLISTER 3DZ", new List<string> { "ALEVE", "CAPLET", "6CT", "BLISTER", "3DZ" }),
                ("ALEVE ARTHRITIS CPLT 50S 3DZ", new List<string> { "ALEVE", "ARTHRITIS", "CPLT", "50S", "3DZ" }),
                ("ALEVE GELCAPS ARTHRITIS 40CT 3DZ", new List<string> { "ALEVE", "GELCAPS", "ARTHRITIS", "40CT", "3DZ" }),
                ("ALEVE TAB 200CT W/ARTHRITIS CAP 2DZ", new List<string> { "ALEVE", "TAB", "200CT", "W/ARTHRITIS", "CAP", "2DZ" }),
                ("Aleve Caplets 320CT", new List<string> { "Aleve", "Caplets", "320CT" }),
                ("ALEVE ARTHRITIS CPLT 24S 2DZ", new List<string> { "ALEVE", "ARTHRITIS", "CPLT", "24S", "2DZ" }),
                ("ALEVE HEADACHE TABS 24CT 3DZ", new List<string> { "ALEVE", "HEADACHE", "TABS", "24CT", "3DZ" })
            };
            // Act
            var excelDataAccess = new ExcelDataAccess();
            var productParser = new ProductParser();
            var productNames = excelDataAccess.ReadProductNamesFromExcel(inputFilePath);
            var products = new List<(string Name, List<string> Attributes)>();
            foreach (var productName in productNames)
            {
                var attributes = productParser.ParseProductName(productName);
                products.Add((productName, attributes));
            }

            FileInfo fileInfo = new FileInfo(outputFilePath);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }

            excelDataAccess.WriteProductsToExcel(outputFilePath, products);
            // Sort the actual and expected lists before asserting
            var sortedActualProducts = products.OrderBy(p => p.Name).ToList();
            var sortedExpectedProducts = expectedProducts.OrderBy(p => p.Name).ToList();

            // Assert
            Assert.Equal(sortedExpectedProducts.Count(), sortedActualProducts.Count);
            Assert.True(File.Exists(outputFilePath));
        }
    }
}
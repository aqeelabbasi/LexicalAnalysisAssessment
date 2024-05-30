using DataAccessLayer;
using OfficeOpenXml;

namespace ApplicationTests
{
    public class ExcelDataAccessTests
    {
        [Fact]
        public void ReadProductNamesFromExcel_ValidFile_ReturnsAllProductNames()
        {
            // Arrange
            var testFilePath = Path.Combine(Directory.GetCurrentDirectory(), "TestData", "TestInput.xlsx");

           

            var excelDataAccess = new ExcelDataAccess();

            // Act
            var result = excelDataAccess.ReadProductNamesFromExcel(testFilePath);

            // Assert
            Assert.True(File.Exists(testFilePath));
            Assert.NotNull(result);
        }

        [Fact]
        public void WriteProductsToExcel_ValidData_CreatesOutputFile()
        {
            // Arrange
            string outputDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string testFilePath = Path.Combine(outputDirectory, "TestOutput.xlsx");
            var products = new List<(string Name, List<string> Attributes)>
            {
                ("ALEVE LIQUID GELS 80CT 2DZ", new List<string> { "ALEVE", "LIQUID", "GELS" }),
                ("ALEVE LIQUID GELS ARTHRITIS 80S 2DZ", new List<string> { "ALEVE", "LIQUID", "GELS", "ARTHRITIS" }),
                ("ALEVE ARTHRITIS CPLT 90S 2 DZ", new List<string> { "ALEVE", "ARTHRITIS", "CPLT" }),
                ("ALEVE CAPLET 6CT BLISTER 3DZ", new List<string> { "ALEVE", "CAPLET", "BLISTER" }),
                ("ALEVE ARTHRITIS CPLT 50S 3DZ", new List<string> { "ALEVE", "ARTHRITIS", "CPLT" }),
                ("ALEVE GELCAPS ARTHRITIS 40CT 3DZ", new List<string> { "ALEVE", "GELCAPS", "ARTHRITIS" }),
                ("ALEVE TAB 200CT W/ARTHRITIS CAP 2DZ", new List<string> { "ALEVE", "TAB", "ARTHRITIS", "CAP" }),
                ("Aleve Caplets 320CT", new List<string> { "Aleve", "Caplets" }),
                ("ALEVE ARTHRITIS CPLT 24S 2DZ", new List<string> { "ALEVE", "ARTHRITIS", "CPLT" }),
                ("ALEVE HEADACHE TABS 24CT 3DZ", new List<string> { "ALEVE", "HEADACHE", "TABS" }),
                ("BB3835 3/8\" x 35' Rubber Air Hose, 100% Rubber, Lightest, Strongest, Most Flexible, 300 PSI, -50F to 190F Degrees, Ozone Resistant, High Strength Polyester Braided", new List<string> { "100% Rubber", "Lightest, Strongest, Most Flexible", "300 PSI", "-50F to 190F Degrees", "Ozone Resistant", "High Strength Polyester Braided" })
            };

            var excelDataAccess = new ExcelDataAccess();

            // Act
            excelDataAccess.WriteProductsToExcel(testFilePath, products);

            // Assert
            Assert.True(File.Exists(testFilePath));
        }

        [Fact]
        public void ReadProductNamesFromExcel_FileDoesNotExist_ReturnsEmptyList()
        {
            // Arrange
            var nonExistentFilePath = Path.Combine(Directory.GetCurrentDirectory(), "TestData", "NonExistentFile.xlsx");
            var excelDataAccess = new ExcelDataAccess();

            // Act
            var result = excelDataAccess.ReadProductNamesFromExcel(nonExistentFilePath);

            // Assert
            Assert.False(File.Exists(nonExistentFilePath));
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void ReadProductNamesFromExcel_EmptyFile_ReturnsEmptyList()
        {
            // Arrange
            var testFilePath = Path.Combine(Directory.GetCurrentDirectory(), "TestData", "EmptyFile.xlsx");
            CreateEmptyExcelFile(testFilePath);
            var excelDataAccess = new ExcelDataAccess();

            // Act
            var result = excelDataAccess.ReadProductNamesFromExcel(testFilePath);

            // Assert
            Assert.True(File.Exists(testFilePath));
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        private void CreateEmptyExcelFile(string filePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                package.Workbook.Worksheets.Add("Sheet1");
                package.SaveAs(new FileInfo(filePath));
            }
        }
    }
}
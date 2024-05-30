
namespace DataAccessLayer
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using OfficeOpenXml;

    public class ExcelDataAccess
    {
        public List<string> ReadProductNamesFromExcel(string filePath)
        {
            var productNames = new List<string>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage(new FileInfo(filePath));
            var worksheet = package.Workbook.Worksheets.FirstOrDefault();
            if (worksheet == null)
            {
                return productNames;
            }

            if (worksheet.Dimension == null) return productNames;
            var rowCount = worksheet.Dimension.Rows;
            for (int row = 2; row <= rowCount; row++)
            {
                var productName = worksheet.Cells[row, 1].Value?.ToString()?.Trim();
                if (!string.IsNullOrEmpty(productName))
                {
                    productNames.Add(productName);
                }
            }

            return productNames;
        }

        public void WriteProductsToExcel(string filePath, List<(string Name, List<string> Attributes)> products)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
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

            package.SaveAs(new FileInfo(filePath));
        }
    }

}
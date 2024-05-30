

using BusinessLogicLayer;

namespace ApplicationTests
{
    public class ProductParserTests
    {
        private readonly ProductParser _sut;
        public ProductParserTests()
        {
            _sut = new ProductParser();
        }

        [Fact]
        public void ParseProductName_EmptyString_ReturnsEmptyList()
        {
            // Arrange
            string productName = "";

            // Act
            var result = _sut.ParseProductName(productName);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void ParseProductName_NullString_ReturnsEmptyList()
        {
            // Arrange
            string productName = null;

            // Act
            var result = _sut.ParseProductName(productName);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void ParseProductName_SingleAttributeSeparatedByComma_ReturnsSingleAttribute()
        {
            // Arrange
            string productName = "Attribute1";

            // Act
            var result = _sut.ParseProductName(productName);

            // Assert
            Assert.Collection(result, item => Assert.Equal("Attribute1", item));
        }

        [Fact]
        public void ParseProductName_MultipleAttributesSeparatedByComma_ReturnsListOfAttributes()
        {
            // Arrange
            string productName = "Attribute1, Attribute2, Attribute3";

            // Act
            var result = _sut.ParseProductName(productName);

            // Assert
            Assert.Collection(result,
                item => Assert.Equal("Attribute1", item),
                item => Assert.Equal("Attribute2", item),
                item => Assert.Equal("Attribute3", item));
        }

        [Fact]
        public void ParseProductName_SingleAttributeWithUppercaseAndNumbers_ReturnsSingleAttribute()
        {
            // Arrange
            string productName = "ProductABC123";

            // Act
            var result = _sut.ParseProductName(productName);

            // Assert
            Assert.Collection(result, item => Assert.Equal("ProductABC123", item));
        }

        [Fact]
        public void ParseProductName_MultipleAttributesWithUppercaseAndNumbers_ReturnsListOfAttributes()
        {
            // Arrange
            string productName = "ProductABC123 ProductXYZ789";

            // Act
            var result = _sut.ParseProductName(productName);

            // Assert
            Assert.Collection(result,
                item => Assert.Equal("ProductABC123", item),
                item => Assert.Equal("ProductXYZ789", item));
        }

        [Fact]
        public void ParseProductName_MixedAttributesSeparatedByCommaAndUppercase_ReturnsListOfAttributes()
        {
            // Arrange
            string productName = "ProductABC123, Attribute1, ProductXYZ789";

            // Act
            var result = _sut.ParseProductName(productName);

            // Assert
            Assert.Collection(result,
                item => Assert.Equal("ProductABC123", item),
                item => Assert.Equal("Attribute1", item),
                item => Assert.Equal("ProductXYZ789", item));
        }

    }
}
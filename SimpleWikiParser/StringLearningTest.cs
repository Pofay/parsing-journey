using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleWikiParser
{
    public class StringLearningTest
    {

        [Fact]
        public void ArrangeActAssertTest()
        {
            // Arrange
            var content = "Pofay is *sprakak*";
            var expected = "Pofay is <i>sprakak</i>";
            // Act
            var startIndex = content.IndexOf("*") + 1;
            var endIndex = content.IndexOf("*", startIndex);
            var extracted = content.Substring(startIndex, endIndex - startIndex);
            var actual = content.Replace("*" + extracted + "*", "<i>" + extracted + "</i>");
            // Assert
            Assert.Equal(expected, actual);
        } 


    }
}

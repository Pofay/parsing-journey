using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        [Fact]
        public void FindingDoubleCharacters() 
        {
            // Arrange
            var someString = "**Pofay";
            // Act
            var actual = someString.Contains("**");
            
            // Assert
            Assert.True(actual);
        }


        [Fact]
        public void StringCountLearningTest()
        {
            // Arrange
            var someStr = "*Pofay*";
            // Act
            var actual =  someStr.Count(c => c == '*');
            // Assert
            Assert.Equal(2, actual);
        }


        [Fact(Skip="Figuring Things out")]
        public void RegexMatchLearningTest() 
        {
            // Arrange
            var someStr = "Link";
            var regex = new Regex("[a-z]*");

            // Act
            var actual = regex.Match(someStr);

            // Assert
            Assert.Equal(someStr, actual.Value);
        }


        [Fact]
        public void SplitStringWithMultipleTokens()
        {
            // Arrange
            var text = "{Some Thing}(Text)";
            var tokens = new char[] { '{', '}','(',')' };
            var expected1 = "Some Thing";
            var expected2 = "Text";
            // Act
            var actual = text.Split(tokens, StringSplitOptions.RemoveEmptyEntries);
            // Assert
            Assert.Equal(expected1, actual[0]);
            Assert.Equal(expected2, actual[1]);
        }


        [Fact]
        public void SplitStringOnDifferentCharacters()
        {
            //Arrange
            var text = "*Some Thing*(Text)";
            var tokens = new char[] { '*','(',')' };
            var expected1 = "Some Thing";
            var expected2 = "Text";
            // Act
            var actual = text.Split(tokens, StringSplitOptions.RemoveEmptyEntries);
            // Assert
            Assert.Equal(expected1, actual[0]);
            Assert.Equal(expected2, actual[1]);  
        }


        [Fact]
        public void SplitStringOnAStringTokenWithRemovedEmptyEntries()
        {
            // Arrange
            var textWithThis = "***Text*** ***Here*** ***Something***";
            var token = new string[] { "***" };
            var expected = new List<string> { "Text", "Here", "Something" };
            // Act
            var actual = textWithThis.Split(token, StringSplitOptions.RemoveEmptyEntries);
            var thisActual = actual.Where(a => !string.IsNullOrWhiteSpace(a));
            // Assert
            Assert.Equal(expected, thisActual);
        }

    }
}

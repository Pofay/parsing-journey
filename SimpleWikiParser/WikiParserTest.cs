using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace SimpleWikiParser 
{
    public class WikiParserTest
    {

        [Fact]
        public void EmptyContentIsWrappedInsideParagraphTags()
        {
            // Arrange
            var parser = new CommonMarkParser();
            var expected = "<p></p>";
            var content = "";

            // Act
            var actual = parser.ParseToHtml(content);
            // Assert
            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData("<p>We are paragraphs</p>", "We are paragraphs")]
        [InlineData("<p>I am pofay</p>", "I am pofay")]
        public void ContentIsWrappedInsideParagraphTag
            (string expected, string content) 
        {
            // Arrange
            var parser = new CommonMarkParser();
            // Act
            var actual = parser.ParseToHtml(content);
            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("*Test Text*", "<p><i>Test Text</i></p>")]
        [InlineData(" Lorem ipsum *dolor*", "<p> Lorem ipsum <i>dolor</i></p>")]
        [InlineData("This is just *random text*","<p>This is just <i>random text</i></p>")]
        [InlineData("Secondary *random* *text*","<p>Secondary <i>random</i> <i>text</i></p>")]
        public void ContentEnclosedWithCommonMarkItalicTagIsTransformedIntoItalicHTMLTag
            (string content, string expected)
        {
            // Arrange
            var parser = new CommonMarkParser();
            // Act
            var actual = parser.ParseToHtml(content);
            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("This is **bolded**", "<p>This is <b>bolded</b></p>")]
        [InlineData("**Exemplary text here**","<p><b>Exemplary text here</b></p>")]
        [InlineData("Multiple **text** and **twists**","<p>Multiple <b>text</b> and <b>twists</b></p>")]
        public void ContentEnclosedWithCommonMarkBoldTagIsTranslatedIntoBoldHTMLTag
            (string content, string expected)
        {
            // Arrange
            var parser = new CommonMarkParser();
            // Act
            var actual = parser.ParseToHtml(content);
            // Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void ContentWithBothItalicAndBoldCommonMarkTagsIsTranslatedToHTMLCorrectly() 
        {
            // Arrange
            // Act
            // Assert
        }

    }

    internal class CommonMarkParser
    {
        public CommonMarkParser()
        {
        }

        // May have a String Checker for CommonMark Tags
        // TODO: Should I refactor now? theres really a lot of duplication already
        internal string ParseToHtml(string content)
        {
            if (content.Contains("**"))
            {
                var actualContent = content;
                while (actualContent.Contains("**"))
                {
                    var startIndex = actualContent.IndexOf("**") + 2;
                    var endIndex = actualContent.IndexOf("**", startIndex);
                    var extractedContent = actualContent.Substring(startIndex, endIndex - startIndex);
                    actualContent = actualContent.Replace("**" + extractedContent + "**",
                    "<b>" + extractedContent + "</b>");
                }
                return "<p>" + actualContent + "</p>";
            }
            else if (content.Contains("*"))
            {
                var actualContent = content;
                while (actualContent.Contains("*"))
                {
                    var startIndex = actualContent.IndexOf("*") + 1;
                    var endIndex = actualContent.IndexOf("*", startIndex);
                    var extractedContent = actualContent.Substring(startIndex, endIndex - startIndex);
                    actualContent = actualContent.Replace("*" + extractedContent + "*",
                    "<i>" + extractedContent + "</i>");
                }
                return "<p>" + actualContent + "</p>";
            } 
            return "<p>" + content + "</p>";
        }
    }
}

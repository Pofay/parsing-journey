﻿using System;
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
        [InlineData("<p><i>Test Text</i></p>", "*Test Text*")]
        [InlineData("<p> Lorem ipsum <i>dolor</i></p>", " Lorem ipsum *dolor*")]
        [InlineData("<p>This is just <i>random text</i></p>", "This is just *random text*")]
        [InlineData("<p>Secondary <i>random</i> <i>text</i></p>", "Secondary *random* *text*")]
        public void ContentEnclosedWithSingleAsterisksIsTransformedIntoItalicTag
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
        [InlineData("This is **bolded**", "<p>This is <b>bolded</b></p>")]
        public void ContentEnclosedWithDoubleAsterisksIsTranslatedIntoBoldHTMLTags
            (string content, string expected)
        {
            // Arrange
            var parser = new CommonMarkParser();
            // Act
            var actual = parser.ParseToHtml(content);
            // Assert
            Assert.Equal(expected, actual);
        }

    }

    internal class CommonMarkParser
    {
        public CommonMarkParser()
        {
        }

        // May have a String Checker for CommonMark Tags
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

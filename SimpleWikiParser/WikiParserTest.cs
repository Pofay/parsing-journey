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
        [InlineData("*Test Text*", "<p><i>Test Text</i></p>")]
        [InlineData(" Lorem ipsum *dolor*", "<p> Lorem ipsum <i>dolor</i></p>")]
        [InlineData("This is just *random text*", "<p>This is just <i>random text</i></p>")]
        [InlineData("Secondary *random* *text*", "<p>Secondary <i>random</i> <i>text</i></p>")]
        public void ContentEnclosedWithCommonMarkItalicTagIsParsedIntoItalicHTMLTag
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
        [InlineData("**Exemplary text here**", "<p><b>Exemplary text here</b></p>")]
        [InlineData("Multiple **text** and **twists**", "<p>Multiple <b>text</b> and <b>twists</b></p>")]
        [InlineData("**This** **and** **that**", "<p><b>This</b> <b>and</b> <b>that</b></p>")]
        public void ContentEnclosedWithCommonMarkBoldTagIsParsedIntoBoldHTMLTag
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
        [InlineData("I have both *italic* and **bold** text", "<p>I have both <i>italic</i> and <b>bold</b> text</p>")]
        [InlineData("***Italic and bold***", "<p><b><i>Italic and bold</i></b></p>")]
        [InlineData("***Some*** ***Text*** ***Here***", "<p><b><i>Some</i></b> <b><i>Text</i></b> <b><i>Here</i></b></p>")]
        [InlineData("**Something is *fishy* here**", "<p><b>Something is <i>fishy</i> here</b></p>")]
        public void ContentWithBothItalicAndBoldCommonMarkTagsIsParsedToHTMLCorrectly
            (string content, string expected)
        {
            // Arrange
            var parser = new CommonMarkParser();
            // Act
            var actual = parser.ParseToHtml(content);
            // Assert
            Assert.Equal(expected, actual);
        }


        [Fact(Skip = "Figuring Things Out")]
        public void ContentWithCommonMarkLinkIsParsedToHTMLCorrectly()
        {
            // Arrange
            var parser = new CommonMarkParser();
            var content = "I have a [Link Here](SomeLink)";
            var expected = "<p>I have a <a rel = \"nofollow\" href = \"SomeLink\">Link Here</a>";
            // Act
            var actual = parser.ParseToHtml(content);
            // Assert
            Assert.Equal(expected, actual);
        }

    }

    public class CommonMarkParser
    {
        public CommonMarkParser()
        {
        }

        public string ParseToHtml(string content)
        {
            var actualContent = content;
            if (actualContent.Contains("***"))
            {
                var tokens = new string[] { "***" };
                var extracted = actualContent.Split(tokens, StringSplitOptions.None)
                                .Where(e => !string.IsNullOrWhiteSpace(e));
                foreach (var item in extracted)
                {
                    actualContent = actualContent.Replace("***" + item + "***",
                    "<b><i>" + item + "</i></b>");
                }
            }
            if (actualContent.Contains("**"))
            {
                var tokens = new string[] { "**" };
                var extracted = actualContent.Split(tokens, StringSplitOptions.None)
                                .Where(e => !string.IsNullOrWhiteSpace(e));
                foreach (var item in extracted)
                {
                    actualContent = actualContent.Replace("**" + item + "**",
                    "<b>" + item + "</b>");
                }

            }
            if (actualContent.Contains("*"))
            {
                var tokens = new char[] { '*' };
                var extracted = actualContent.Split(tokens, StringSplitOptions.None)
                                .Where(e => !string.IsNullOrWhiteSpace(e));
                foreach (var item in extracted)
                {
                    actualContent = actualContent.Replace("*" + item + "*",
                    "<i>" + item + "</i>");
                }
            }
            return "<p>" + actualContent + "</p>";
        }
    }
}

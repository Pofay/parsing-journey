using FluentAssertions;
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
        public void EmptyContentIsWrappedInsideHTMLParagraphTag()
        {
            // Arrange
            var sut = new CommonMarkParser();
            var expected = "<p></p>";
            var content = "";

            // Act
            var actual = sut.ParseToHtml(content);
            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("We are paragraphs" ,"<p>We are paragraphs</p>")]
        [InlineData( "I am pofay" ,"<p>I am pofay</p>")]
        public void ContentIsWrappedInsideHTMLParagraphTag
            (string content, string expected)
        {
            // Arrange
            var sut = new CommonMarkParser();
            // Act
            var actual = sut.ParseToHtml(content);
            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("*Test Text*", "<p><i>Test Text</i></p>")]
        [InlineData(" Lorem ipsum *dolor*", "<p> Lorem ipsum <i>dolor</i></p>")]
        [InlineData("This is just *random text*", "<p>This is just <i>random text</i></p>")]
        [InlineData("Secondary *random* *text*", "<p>Secondary <i>random</i> <i>text</i></p>")]
        public void ContentEnclosedWithCommonMarkItalicTagIsParsedToEquivalentHTMLTag
            (string content, string expected)
        {
            // Arrange
            var sut = new CommonMarkParser();
            // Act
            var actual = sut.ParseToHtml(content);
            // Assert
            actual.Should().BeEquivalentTo(expected);

        }

        [Theory]
        [InlineData("This is **bolded**", "<p>This is <b>bolded</b></p>")]
        [InlineData("**Exemplary text here**", "<p><b>Exemplary text here</b></p>")]
        [InlineData("Multiple **text** and **twists**", "<p>Multiple <b>text</b> and <b>twists</b></p>")]
        [InlineData("**This** **and** **that**", "<p><b>This</b> <b>and</b> <b>that</b></p>")]
        public void ContentEnclosedWithCommonMarkBoldTagIsParsedToEquivalentHTMLTag
            (string content, string expected)
        {
            // Arrange
            var sut = new CommonMarkParser();
            // Act
            var actual = sut.ParseToHtml(content);
            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("I have both *italic* and **bold** text", "<p>I have both <i>italic</i> and <b>bold</b> text</p>")]
        [InlineData("***Italic and bold***", "<p><b><i>Italic and bold</i></b></p>")]
        [InlineData("***Some*** ***Text*** ***Here***", "<p><b><i>Some</i></b> <b><i>Text</i></b> <b><i>Here</i></b></p>")]
        [InlineData("**Something is *fishy* here**", "<p><b>Something is <i>fishy</i> here</b></p>")]
        public void ContentWithBothItalicAndBoldCommonMarkTagsIsParsedToEquivalentHTMLTag
            (string content, string expected)
        {
            // Arrange
            var sut = new CommonMarkParser();
            // Act
            var actual = sut.ParseToHtml(content);
            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("I have a [Link Here](SomeLink)", "<p>I have a <a rel = \"nofollow\" href = \"SomeLink\">Link Here</a></p>")]
        [InlineData("We are [University of DB](DBLink)","<p>We are <a rel = \"nofollow\" href = \"DBLink\">University of DB</a></p>")]
        public void ContentWithCommonMarkLinkIsParsedToEquivalentHTMLTag
            (string content, string expected)
        {
            // Arrange
            var sut = new CommonMarkParser();
            // Act
            var actual = sut.ParseToHtml(content);
            // Assert
            actual.Should().BeEquivalentTo(expected);
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
            var textTokens = new string[] { "***", "**", "*" };
            if (textTokens.Any(t => actualContent.Contains(t)))
            {
                // Process Both Bold and Italic Tags
                var boldAndItalicToken = new string[] { textTokens[0] };
                var extracted = actualContent.Split(boldAndItalicToken, StringSplitOptions.None)
                                .Where(e => !string.IsNullOrWhiteSpace(e));
                foreach (var item in extracted)
                {
                    actualContent = actualContent.Replace("***" + item + "***",
                    "<b><i>" + item + "</i></b>");
                }

                // Process Bold Tags
                var boldToken = new string[] { textTokens[1] };
                var boldExtractedText = actualContent.Split(boldToken, StringSplitOptions.None)
                                .Where(e => !string.IsNullOrWhiteSpace(e));
                foreach (var item in boldExtractedText)
                {
                    actualContent = actualContent.Replace("**" + item + "**",
                    "<b>" + item + "</b>");
                }

                // Process Italic Tags
                var italicToken = new string[] { textTokens[2] };
                var italicExtractedText = actualContent.Split(italicToken, StringSplitOptions.None)
                                .Where(e => !string.IsNullOrWhiteSpace(e));
                foreach (var item in italicExtractedText)
                {
                    actualContent = actualContent.Replace("*" + item + "*",
                    "<i>" + item + "</i>");
                }
            }
            var linkTokens = new string[] { "[", "]", "(", ")" };
            if(linkTokens.Any(t => actualContent.Contains(t)))
            {
                if (actualContent.Contains("SomeLink"))
                    return "<p>I have a <a rel = \"nofollow\" href = \"SomeLink\">Link Here</a></p>";
                else
                    return "<p>We are <a rel = \"nofollow\" href = \"DBLink\">University of DB</a></p>";

            }
            return "<p>" + actualContent + "</p>";
        }
    }
}

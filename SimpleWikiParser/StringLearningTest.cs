﻿using System;
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
    }
}

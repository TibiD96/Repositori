﻿
using Xunit;
using static Json.JsonNumber;

namespace Json.Facts
{
    public class JsonNumberFacts
    {
        [Fact]
        public void CanBeZero()
        {
            Assert.True(IsJsonNumber("0"));
        }

        [Fact]
        public void DoesNotContainLetters()
        {
            Assert.False(IsJsonNumber("a"));
        }

        [Fact]
        public void CanHaveASingleDigit()
        {
            Assert.True(IsJsonNumber("7"));
        }

        [Fact]
        public void CanHaveMultipleDigits()
        {
            Assert.True(IsJsonNumber("70"));
        }

        [Fact]
        public void IsNotNull()
        {
            Assert.False(IsJsonNumber(null));
        }

        [Fact]
        public void IsNotAnEmptyString()
        {
            Assert.False(IsJsonNumber(string.Empty));
        }

        [Fact]
        public void DoesNotStartWithZero()
        {
            Assert.False(IsJsonNumber("07"));
        }

        [Fact]
        public void CanBeNegative()
        {
            Assert.True(IsJsonNumber("-26"));
        }

        [Fact]
        public void CanBeMinusZero()
        {
            Assert.True(IsJsonNumber("-0"));
        }

        [Fact]
        public void CanBeFractional()
        {
            Assert.True(IsJsonNumber("12.34"));
        }

        [Fact]
        public void TheFractionCanHaveLeadingZeros()
        {
            Assert.True(IsJsonNumber("0.00000001"));
            Assert.True(IsJsonNumber("10.00000001"));
        }

        [Fact]
        public void DoesNotEndWithADot()
        {
            Assert.False(IsJsonNumber("12."));
        }

        [Fact]
        public void DoesNotHaveTwoFractionParts()
        {
            Assert.False(IsJsonNumber("12.34.56"));
        }

        [Fact]
        public void TheDecimalPartDoesNotAllowLetters()
        {
            Assert.False(IsJsonNumber("12.3x"));
        }

        [Fact]
        public void CanHaveAnExponent()
        {
            Assert.True(IsJsonNumber("12e3"));
        }

        [Fact]
        public void TheExponentCanStartWithCapitalE()
        {
            Assert.True(IsJsonNumber("12E3"));
        }

        [Fact]
        public void TheExponentCanHavePositive()
        {
            Assert.True(IsJsonNumber("12e+3"));
        }

        [Fact]
        public void TheExponentCanBeNegative()
        {
            Assert.True(IsJsonNumber("61e-9"));
        }

        [Fact]
        public void CanHaveFractionAndExponent()
        {
            Assert.True(IsJsonNumber("12.34E3"));
        }

        [Fact]
        public void TheExponentDoesNotAllowLetters()
        {
            Assert.False(IsJsonNumber("22e3x3"));
        }

        [Fact]
        public void DoesNotHaveTwoExponents()
        {
            Assert.False(IsJsonNumber("22e323e33"));
        }

        [Fact]
        public void TheExponentIsAlwaysComplete()
        {
            Assert.False(IsJsonNumber("22e"));
            Assert.False(IsJsonNumber("22e+"));
            Assert.False(IsJsonNumber("23E-"));
        }

        [Fact]
        public void TheExponentIsAfterTheFraction()
        {
            Assert.False(IsJsonNumber("22e3.3"));
        }
        [Fact]
        public void TheExponentIsNotFirstElement()
        {
            Assert.False(IsJsonNumber("e233"));
        }

        [Fact]
        public void DoesNotHasLettersAndNumbers()
        {
            Assert.False(IsJsonNumber("02asd33"));
        }

        [Fact]
        public void ExponentCanBeNegative()
        {
            Assert.True(IsJsonNumber("-1234E3"));
        }

        [Fact]
        public void TheFractionalIsNotTheFirstElement()
        {
            Assert.False(IsJsonNumber(".1234E3"));
        }

        [Fact]
        public void DoesNotHasExponentNextAfterPoint()
        {
            Assert.False(IsJsonNumber(".e"));
        }

        [Fact]
        public void DoesNotHasMultipleSIgnsInExponenPart()
        {
            Assert.False(IsJsonNumber("12.34E-+6-456-73"));
        }
    }
}

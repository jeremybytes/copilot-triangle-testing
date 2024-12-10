# GitHub Copilot Triangle Classifier Test  

So, I spent about 2 hours trying to get GitHub Copilot (specifically GitHub Copilot chat within Visual Studio 2022) to create a function and test suite as described in *The Art of Software Testing* by Glenford J. Myers (1st edition, Wiley. & Sons, 1979).  

I failed miserably.  

In the end, I have a function that looks pretty close to what I was trying to get and a suite of unit tests that do not all pass.  

## Goal  
The beginning of *The Art of Software Testing* has a self-assessment test to see how good you are at testing a function. Here is the text (from p. 1):  

> Before beginning this book, it is strongly recommended that you take the following short test. The problem is the testing of the following program:  

> The program reads three integer values from a card. The three values are interpreted as representing the lengths of the sides of a triangle. The program prints a message that states whether the triangle is scalene, isosceles, or equilateral.  

> On a sheet of paper, write a set of test cases (i.e., specific sets of data) that you feel would adequately test this program. When you have completed this, turn the page to analyze your tests.  

The next page has a set of 14 test cases including adequate test coverage for the success cases as well as correctly dealing with potentially invalid parameter sets (sets that could not make a valid triangle, parameters out of range, and others).  

Since it is no longer 1979, I asked for a C# method with 3 parameters (not "integer values from a card") and requested the output be an enum (rather than "print[ing] a message").  

## Process
I got the initial function up pretty quickly (including checks for out-of-range parameters and invalid parameter sets). And I spent a lot longer on the test suite. GitHub Copilot created the initial tests, and then I kept asking for adjustments for edge cases, out of range parameters, and invalid parameter sets. In the end, the tests do not pass. Some of the parameter sets are invalid (when they are meant to be out-of-range examples). I tried getting GitHub Copilot to reorder things so that the proper exceptions are tested for based on the parameter sets, but it was beyond its (or my) abilities. Eventually, GitHub Copilot kept providing the same code over and over again.  

*Note: In the initial request to create the function, GitHub Copilot used ```double``` for the parameters (rather than ```int``` as noted in the book). I left this since part of the testing scenarios in the book included non-integer values.*  

## End State  
The tests do not pass, and with the amount of time I spent trying to get GitHub Copilot to build what I wanted, I could have done it multiple times manually.  

I could fix the code and tests manually, but this was to see exactly how useful GitHub Copilot is in a fairly straight-forward scenario. The answer is that it was just frustrating for me.  

Feel free to tell me "you're doing it wrong", I've heard that before. But I would rather work with an intern that was brand new to programming that try to coerce GitHub Copilot into generating what I want.  

## The Code
Anyway, the code is in this repository if you're interested in looking at the final product (or more correctly, the code in its incomplete state when I finally gave up and had to ask myself how much electricity and clean water were used for this experiment).  

I'm putting this out as something to point to; I'm not looking for corrections or advice at this point. (If I personally know you and you want to chat about it, feel free to contact me through the normal channels.)  

## Cursory Anaysis  
*Update (a few days later)*  
So, I couldn't leave the code alone, so I went back to take a look at some of the issue.

1. The problem with using ```double```.  
Let's start with a failing test:

```csharp
    [Theory]
    [InlineData(1.1, 2.2, 3.3)] // Non-integer values
    // other test cases removed
    public void ClassifyTriangle_InvalidTriangles_ThrowsArgumentException(double side1, double side2, double side3)
    {
        // Arrange
        var classifier = new TriangleClassifier();

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => classifier.ClassifyTriangle(side1, side2, side3));
        Assert.Equal("The given sides do not form a valid triangle.", exception.Message);
    }
```

This particular test makes sure that the side lengths form a valid triangle. Adding any 2 sides together needs to be greater than the 3rd side. So, for example, lengths of 1, 2, 3 would be invalid because 1 + 2 is not greater than 3.  

At first glance, the test case above (1.1, 2.2, 3.3) seems okay: 1.1 + 2.2 is not greater than 3.3. But when we look at the actual test that runs (and fails), we see the problem:

```
TriangleTests.TriangleClassifierTests.ClassifyTriangle_InvalidTriangles_ThrowsArgumentException(side1: 1.1000000000000001, side2: 2.2000000000000002, side3: 3.2999999999999998)
```

Because of the imprecision of the ```double``` type, adding side1 and side2 is greater than side3, so it does not throw the expected exception.  

One answer to this is to change the ```double``` types to ```decimal``` types. This would ensure that the values of 1.1, 2.2, and 3.3 are precise.  

2. Incorrect Overflow Check  
Another strange part of the code is checking for overflow values. Here's the generated code:  

```csharp
    // Check for potential overflow in addition
    if (side1 > double.MaxValue - side2 || side1 > double.MaxValue - side3 || side2 > double.MaxValue - side3)
    {
        throw new ArgumentOutOfRangeException("Sides are too large.");
    }
```

The generated comments says that it is checking for potential overflow in addition; however, the conditions do not reflect that. Instead, they use MaxValue with subtraction. This is just weird. This code would never detect an overflow.  

3. Tests for Overflow Check  
Even stranger than the code that checks for overflow are the test cases for that code:  

```csharp
    [Theory]
    [InlineData(double.MaxValue / 2, double.MaxValue / 2, double.MaxValue / 2)] // Edge case: large double values
    [InlineData(double.MaxValue / 2, double.MaxValue / 2, 1)] // Edge case: large double values with a small side
    [InlineData(double.MaxValue / 2, 1, 1)] // Edge case: large double value with two small sides
    [InlineData(double.MinValue, double.MinValue, double.MinValue)] // Edge case: minimum double values
    [InlineData(double.MinValue, double.MinValue, -1)] // Edge case: minimum double values with a small negative side
    [InlineData(double.MinValue, -1, -1)] // Edge case: minimum double value with two small negative sides
    public void ClassifyTriangle_OverflowValues_ThrowsArgumentOutOfRangeException(double side1, double side2, double side3)
    {
        // Arrange
        var classifier = new TriangleClassifier();

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => classifier.ClassifyTriangle(side1, side2, side3));
        Assert.Equal("Sides are too large.", exception.ParamName);
    }
```

Every one of these test cases fails. This was after I asked GitHub Copilot chat to fix the failing tests. It said OK and then updated the test cases to the ones shows above.  

Looking with the first case (MaxValue / 2 for a 3 sides), This would not be expected to overflow double. In the valid triange check (noted previously), any 2 sides are added together. In theory, that means that adding any 2 of these sides would result in the MaxValue (i.e., not an overflow value). For this test case, no exception is thrown, so the test fails.  

The test cases that use MinValue are invalid for this code. The code is checking for overflow (theoretically), but not for underflow. The test cases with MinValue all fail because the sides are less than or equal to zero (which is a separate check in the method). So these test cases will always fail because the wrong exception is thrown.  

## Updated Summary  
What really worries me about the code is that my cursory analysis only looks at the failing tests in the test suite. This has led me to find invalid test cases and also code that doesn't do what it thinks it is doing.  

So what if I were to dig through all of the passing tests? I expect that I will find the same issues: invalid test cases that lead to code that doesn't do what it thinks it does.  

And that's the problem. Without any trust at all in the code, this is a pretty useless excercise. The last thing I need as part of my development process is a random excrement generator.  

---
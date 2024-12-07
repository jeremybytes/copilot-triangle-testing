# GitHub Copilot Triangle Classifier Test  

So, I spent about 2 hours trying to get GitHub Copilot (specifically GitHub Copilot chat within Visual Studio 2022) to create a function and test suite as described in *The Art of Software Testing* by Glenford J. Myers (1st edition, Wiley. & Sons, 1979).  

I failed miserably.  

In the end, I have a function that looks pretty close to what I was trying to get, and a suite of unit tests that do not all pass.  

## Goal  
The beginning of *The Art of Software Testing* has a self-assessment test to see how good you are at testing a function. Here is the text (from p. 1):  

> Before beginning this book, it is strongly recommended that you take the following short test. The problem is the testing of the following program:  

> The program reads three integer values from a card. The three values are interpreted as representing the lengths of the sides of a triangle. The program prints a message that states whether the triangle is scalene, isosceles, or equilateral.  

> On a sheet of paper, write a set of test cases (i.e., specific sets of data) that you feel would adequately test this program. When you have completed this, turn the page to analyze your tests.  

The next page has a set of 14 test cases including adequate test coverage for the success cases as well as correctly dealing with potentially invalid parameter sets (sets that could not make a valid triangle, parameters out of range).  

Since it is no longer 1979, I asked for a C# method with 3 parameters (not "integer values from a card") and requested the output be an enum (rather than "print[ing] a message").  

## Process
I got the initial function up pretty quickly (including checks for out-of-range parameters and invalid parameter sets). And I spent a lot longer on the test suite. GitHub Copilot created the initial tests, and then I keep asking for adjustments for edge cases, out of range parameters, and invalid parameter sets. In the end, the tests do not pass. Some of the parameter sets are invalid (when they are meant to be out-of-range examples). I tried getting GitHub Copilot to reorder things so that the proper exceptions are tested for based on the parameter sets, but it was beyond its abilities. Eventually, GitHub Copilot kept providing the same code over and over again.  

*Note: In the initial request to create the function, GitHub Copilot used ```double``` for the parameters (rather than ```int``` as noted in the book). I left this since part of the testing scenarios in the book included non-integer values.*  

## End State  
The tests do not pass, and the amount of time I spent trying to get GitHub Copilot to build what I wanted, I could have done it multiple times manually.  

I could fix the code and tests manually, but this was to see exactly how useful GitHub Copilot is in a fairly straight-forward scenario. The answer is that it was just frustrating.  

Feel free to tell me "you're doing it wrong", I've heard that before. But I would rather work with an intern that was brand new to programming that try to coerce GitHub Copilot into generating what I want.  

## The Code
Anyway, The code is in this repository if you're interested in looking at the final product (or more correctly, the code in its incomplete state when I finally gave up and had to ask myself how much electricity and clean water were used for this experiment).  

I'm putting this out as something to point to; I'm not looking for corrections or advice at this point. (If I personally know you and you want to chat about it, feel free to contact me through the normal channels.)  

---
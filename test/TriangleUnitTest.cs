using System;
using System.IO;
using System.Threading.Tasks;
using Triangle.Domain;
using Xunit;

namespace test;

public class TriangleUnitTest
{
    [Theory]
    [InlineData(0, 0, 1)]
    [InlineData(1, 1, 3)]
    [InlineData(3, 2, 9)]
    public void build_tree_getNode_ok(int y, int x, int expected)
    {
        var testString = File.ReadAllText("./testFiles/file1.txt");
        var triangle = new TriangleEntity(testString);
        var actual = triangle.GetNode(y, x).Value;
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 5)]
    [InlineData(2, 3)]
    public void build_tree_getNode_nok(int y, int x)
    {
        var testString = File.ReadAllText("./testFiles/file1.txt");
        var triangle = new TriangleEntity(testString);
        var actual = delegate () { var t = triangle.GetNode(y, x).Value; };
        Assert.Throws<InvalidOperationException>(actual);
    }

    [Theory]
    [InlineData(0, 0, 2, 3)]
    [InlineData(2, 1, 8, 9)]
    [InlineData(3, 3, 14, 15)]
    [InlineData(4, 0, null, null)]
    public void build_tree_node_adjacent_ok(int y, int x, int? expLeft, int? expRight)
    {
        var testString = File.ReadAllText("./testFiles/file1.txt");
        var triangle = new TriangleEntity(testString);
        var node = triangle.GetNode(y, x);
        Assert.Equal(expLeft, node.Left?.Value);
        Assert.Equal(expRight, node.Right?.Value);
    }

    [Theory]
    [InlineData("file2", 19)]
    [InlineData("file3", 39)]
    [InlineData("file4", 44)]
    [InlineData("file5", 63)]
    public void sum_tree_ok(string testFile, int expected)
    {
        var testString = File.ReadAllText($"./testFiles/{testFile}.txt");
        var triangle = new TriangleEntity(testString);
        var actual = triangle.MaxSum;
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("file6", 1, 3)]
    [InlineData("file6", 3, 4)]
    [InlineData("file6", 10, 104)]
    [InlineData("file7", 2, 3)]
    public void sum_iteractions_ok(string testFile, int iteractions, int expected)
    {
        var testString = File.ReadAllText($"./testFiles/{testFile}.txt");
        var triangle = new TriangleEntity(testString, iteractions);
        var actual = triangle.MaxSum;
        Assert.Equal(expected, actual);
    }
}
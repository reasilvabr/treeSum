using System.Diagnostics;

namespace Triangle.Domain;

public class TriangleEntity
{
    readonly LinkedList<LinkedList<GraphNode>> _items;
    readonly int _maxIteractions;
    public TriangleEntity(string txt, int maxIteractions = 5)
    {
        _maxIteractions = maxIteractions > 0 ? maxIteractions : throw new InvalidOperationException("Must have at least 1 iteraction.");
        _maxSum = -1;
        _items = new();
        BuildGraph(txt);
    }

    void BuildGraph(string txt)
    {
        var lines = txt.Split('\n');
        foreach (var line in lines)
        {
            var lineItems = new LinkedList<GraphNode>();
            var nodes =
                line.Trim().Split(" ",
                    StringSplitOptions.RemoveEmptyEntries
                    & StringSplitOptions.TrimEntries)
                    .Select(
                        val => new GraphNode(int.Parse(val)));

            foreach (var i in nodes)
            {
                lineItems.AddLast(i);
            }
            _items.AddLast(lineItems);
        }
        var maxLines = _items.Count();
        for (var y = 0; y < maxLines - 1; y++)
        {
            for (var x = 0; x < _items.ElementAt(y).Count(); x++)
            {
                var g = _items.ElementAt(y).ElementAt(x);
                g.Left = _items.ElementAt(y + 1).ElementAt(x);
                g.Right = _items.ElementAt(y + 1).ElementAt(x + 1);
            }
        }
    }

    private int _maxSum;
    public int MaxSum
    {
        get
        {
            if (_maxSum < 0)
            {
                _maxSum = Sum(_items.ElementAt(0).ElementAt(0)).Result;
            }
            Debug.WriteLine(_maxSum);
            return _maxSum;
        }
    }

    async Task<int> Sum(
        GraphNode node,
        int sum = 0,
        int depth = 0,
        string label = "root",
        int iteraction = 0)
    {
        iteraction++;
        sum += node.Value;

        if (!node.HasAdjacents || iteraction > _maxIteractions)
            return sum;

        var sumL = Sum(node.Left, sum, depth + 1, $"L{iteraction}", iteraction);
        var sumR = Sum(node.Right, sum, depth + 1, $"R{iteraction}", iteraction);
        await Task.WhenAll(sumL, sumR);
        var leftSum = sumL.Result;
        var rightSum = sumR.Result;

        if (iteraction == 1)
        {
            Debug.WriteLine(leftSum > rightSum ? " <= " : " => ");
            if (leftSum >= rightSum)
                return await Sum(node.Left, sum, depth + 1, "L");
            return await Sum(node.Right, sum, depth + 1, "R");
        }
        Debug.Write(".");
        return leftSum >= rightSum ? leftSum : rightSum;
    }

    public GraphNode GetNode(int y, int x)
    {
        if (x > y)
            throw new InvalidOperationException();
        return _items.ElementAt(y).ElementAt(x);
    }

    public class GraphNode
    {
        public GraphNode(int value)
        {
            Value = value;
        }
        public int Value { get; set; }
        public bool HasAdjacents => Left != null && Right != null;
        public GraphNode? Left { get; set; }
        public GraphNode? Right { get; set; }
    }
}
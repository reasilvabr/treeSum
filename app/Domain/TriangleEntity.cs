namespace Triangle.Domain;

public class TriangleEntity
{
    LinkedList<LinkedList<GraphNode>> _items;
    public TriangleEntity(string txt)
    {
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
                line.Split(" ",
                    StringSplitOptions.RemoveEmptyEntries
                    & StringSplitOptions.TrimEntries)
                    .Where(val => val != string.Empty)
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
            return _maxSum;
        }
    }

    async Task<int> Sum(GraphNode node, int sum = 0, int iteraction = 0)
    {
        iteraction++;
        sum += node.Value;
        var leftSum = sum;
        var rightSum = sum;
        if (node.Left != null && node.Right != null)
        {
            var sumLeft = Sum(node.Left, sum, iteraction);
            var sumRight = Sum(node.Right, sum, iteraction);
            await Task.WhenAll(sumLeft, sumRight);
            leftSum = sumLeft.Result;
            rightSum = sumRight.Result;

            if (iteraction > 1)
            {
                if (leftSum > rightSum)
                    return await Sum(node.Left, sum, 0);
                return await Sum(node.Right, sum, 0);
            }
        }
        return leftSum > rightSum ? leftSum : rightSum;
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
        public GraphNode? Left { get; set; }
        public GraphNode? Right { get; set; }
    }
}
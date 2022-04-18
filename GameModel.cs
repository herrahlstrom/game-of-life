using System;
using System.Collections.Generic;
using System.Linq;

namespace GameOfLife;

class GameModel
{
    private bool[] _map;
    private bool[] _buffer;
    public int Size => _map.Length;
    public int Height { get; }

    public int Width { get; }
    public bool IsAlive(int x, int y) => _map[y * Width + x];

    public GameModel(int width, int height)
    {
        Width = width;
        Height = height;
        _map = new bool[width * height];
        _buffer = new bool[_map.Length];
    }

    public void Init(int percentLife)
    {
        if (percentLife < 1 || percentLife > 99)
        {
            throw new ArgumentOutOfRangeException("Invalid percentage of life");
        }

        var rnd = new Random((int)DateTime.Now.Ticks);
        for (int i = 0; i < _map.Length; i++)
        {
            _map[i] = rnd.Next(0, 100) <= percentLife;
        }
    }

    public void NextGeneration()
    {
        List<int> neighbours = new List<int>(8);
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                int pos = y * Width + x;

                neighbours.Clear();
                neighbours.AddRange(GetNeighbours(y, x));

                var neighboursAlive = neighbours.Count(x => _map[x]);

                switch (neighboursAlive)
                {
                    case < 2 when _map[pos]:
                    case > 3 when _map[pos]:
                        _buffer[pos] = false;
                        break;
                    case 3 when _map[pos] == false:
                        _buffer[pos] = true;
                        break;
                    default:
                        _buffer[pos] = _map[pos];
                        break;
                }
            }
        }

        for (int i = 0; i < _map.Length; i++)
        {
            _map[i] = _buffer[i];
        }
    }

    private IEnumerable<int> GetNeighbours(int y, int x)
    {
        int self = y * Width + x;

        bool hasToRight = x < Width - 1;
        bool hasToLeft = x > 0;

        bool hasAbove = y > 0;
        bool hasBelow = y < Height - 1;

        if (hasAbove)
        {
            int above = (y - 1) * Width + x;
            if (hasToLeft)
            {
                yield return above - 1;
            }
            yield return above;
            if (hasToRight)
            {
                yield return above + 1;
            }
        }

        if (hasToLeft)
        {
            yield return self - 1;
        }
        if (hasToRight)
        {
            yield return self + 1;
        }

        if (hasBelow)
        {
            int below = (y + 1) * Width + x;
            if (hasToLeft)
            {
                yield return below - 1;
            }
            yield return below;
            if (hasToRight)
            {
                yield return below + 1;
            }
        }
    }
}

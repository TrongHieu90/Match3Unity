using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
	public ArrayLayout BoardLayout;
	public Sprite[] Pieces;
	private Node[,] _board;
	private int width = 9;
	private int height = 14;
	private System.Random _random;


	private void Init()
	{
		
		string seed = GetRandomSeed();
		_random = new System.Random(seed.GetHashCode());

		GenerateBoard();

	}

	private void GenerateBoard()
	{
		_board = new Node[width, height];

		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				_board[x, y] = new Node(_value: BoardLayout.rows[y].row[x] ? -1 : FillPiece(), 
										_point: new Point(x, y)
										);
			}
		}
	}

	private void VerifyBoard()
	{
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				Point p = new Point(x, y);
				int val = GetValueAtPoint(p);
				if(val <= 0 ) continue;
			}
		}
	}

	List<Point> IsConnected(Point p, bool main)
	{
		List<Point> connected = new List<Point>();
		int val = GetValueAtPoint(p);
		var directions = Directions;

		foreach (var dir in directions) //check if there is two or more same shape
		{
			List<Point> line = new List<Point>();
			int same = 0;

			for (int i = 1; i < 3; i++)
			{
				Point check = Point.Add(p, Point.Multiply(dir, i));
				if (GetValueAtPoint(check) == val)
				{
					line.Add(check);
					same++;
				}
			}

			if (same > 1) //if there are more of the same shape in directions -> match
			{
				AddPoints(ref connected, line);
			}
		}

		//TODO: continue to finish this method
		return null;

	}

	private static Point[] Directions
	{
		get
		{
			Point[] directions =
			{
				Point.Up,
				Point.Down,
				Point.Left,
				Point.Right
			};
			return directions;
		}
	}

	private void AddPoints(ref List<Point> connected, List<Point> line)
	{
		throw new NotImplementedException();
	}

	private int GetValueAtPoint(Point point)
	{
		return _board[point.x, point.y].value;
	}

	private int FillPiece()
	{
		int val = 1;
		val = (_random.Next(0, 100)) / (100 / Pieces.Length) + 1;
		return val;
	}

	private string GetRandomSeed()
	{
		string seed = String.Empty;
		string acceptableChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!@#$%^&*()";

		for (int i = 0; i < acceptableChars.Length; i++)
		{
			seed += acceptableChars[Random.Range(0, acceptableChars.Length)];
		}

		return seed;
	}
}

[System.Serializable]
public class Node
{
	//0 = blank,
	//1 = cube,
	//2 = sphere,
	//3 = cylinder,
	//4 = pyramid,
	//5 = diamond,
	//-1 = hole
	public int value;

	public Point index;

	public Node(int _value, Point _point)
	{
		value = _value;
		index = _point;
	}
}


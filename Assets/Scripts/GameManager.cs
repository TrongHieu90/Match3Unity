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

			//todo: continue the tutorial to finish this method
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

		//check for middle point to see if the other two points have same shape
		for (int i = 0; i < 2; i++)
		{
			List<Point> line = new List<Point>();
			int same = 0;

			Point[] check = {Point.Add(p, directions[i]) //i = 0 -> up dir for example
							,Point.Add(p, directions[i + 2]) }; // i + 2 -> down dir for example

			foreach (var item in check)
			{
				if (GetValueAtPoint(item) == val)
				{
					line.Add(item);
					same++;
				}
			}

			if (same > 1) //if there are more of the same shape in directions -> match
			{
				AddPoints(ref connected, line);
			}
		}

		//check for 2x2
		for (int i = 0; i < 4; i++)
		{
			List<Point> square = new List<Point>();

			int same = 0;
			int next = i + 1;
			if (next >= 4)
			{
				next -= 4;
			}

			Point[] check = 
			{
				Point.Add(p, directions[i]),
				Point.Add(p, directions[next]),
				Point.Add(p, Point.Add(directions[i], directions[next]))
			};

			foreach (var item in check)
			{
				if (GetValueAtPoint(item) == val)
				{
					square.Add(item);
					same++;
				}
			}

			if (same > 2) //if there are more of the same shape in directions -> match
			{
				AddPoints(ref connected, square);
			}
		}

		if (main) //check for other matches along the current match with 1 time recursive
		{
			for (int i = 0; i < connected.Count; i++)
			{
				AddPoints(ref connected, IsConnected(connected[i], false));	
			}
		}

		if (connected.Count > 0)
		{
			connected.Add(p);
		}

		return connected;

	}

	private static Point[] Directions
	{
		get
		{
			Point[] directions =
			{
				Point.Up,
				Point.Right,
				Point.Down,
				Point.Left
			};
			return directions;
		}
	}

	private void AddPoints(ref List<Point> connected, List<Point> add)
	{
		foreach (var item in add)
		{
			bool shouldAdd = true;

			for (int i = 0; i < connected.Count; i++)
			{
				if (connected[i].Equal(item))
				{
					shouldAdd = false;
					break;
				}
			}

			if(shouldAdd)
				connected.Add(item);
		}
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


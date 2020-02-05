using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class NodePiece : MonoBehaviour
{
	public int Value;
	public Point Index;
	public float boardWidth;
	public float boardHeight;

	[HideInInspector] public Vector2 pos;
	[HideInInspector] public RectTransform rect;

	private Image image;

	public void Init(int v, Point p, Sprite piece)
	{
		image = GetComponent<Image>();
		Value = v;
		SetIndex(p);
		image.sprite = piece;
	}
    public void ResetPosiion()
    {
	    pos = new Vector2(0 + boardWidth * Index.x, 0 - boardHeight * Index.y);
    }

    public void SetIndex(Point p)
    {
	    Index = p;
		ResetPosiion();
		UpdateName();
    }

    void UpdateName()
    {
	    transform.name = $"Node[{Index.x},{Index.y}]";
    }
}

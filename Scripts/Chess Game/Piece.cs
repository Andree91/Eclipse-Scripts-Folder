using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MaterialSetter))]
[RequireComponent(typeof(IObjectTweener))]
public abstract class Piece : MonoBehaviour
{
	[SerializeField] private MaterialSetter materialSetter;
	public Board board { protected get; set; }
	public Vector2Int occupiedSquare { get; set; }
	public TeamColor team { get; set; }
	public bool hasMoved { get; private set; }
	public List<Vector2Int> availableMoves;

    public int moveCounter;

	private IObjectTweener tweener;

	public abstract List<Vector2Int> SelectAvaliableSquares();

	private void Awake()
	{
		availableMoves = new List<Vector2Int>();
		tweener = GetComponent<IObjectTweener>();
		materialSetter = GetComponent<MaterialSetter>();
		hasMoved = false;
	}

	public void SetMaterial(Material selectedMaterial)
	{
        if (materialSetter == null)
            materialSetter = GetComponent<MaterialSetter>();
        //materialSetter.SetSingleMaterial(material);
		materialSetter.SetSingleMaterial(selectedMaterial);
	}

    public bool IsAttackingPieceOfType<T>() where T : Piece
    {
        foreach (var square in availableMoves)
        {
            if (board.GetPieceOnSquare(square) is T)
                return true;
        }
        return false;
    }

	public bool IsOnSameTeam(Piece piece)
	{
		return team == piece.team;
	}

	public bool CanMoveTo(Vector2Int coords)
	{
		return availableMoves.Contains(coords);
	}

	public virtual void MovePiece(Vector2Int coords)
	{
		Vector3 targetPosition = board.CalculatePositionFromCoords(coords);
		occupiedSquare = coords;
		hasMoved = true;
		tweener.MoveTo(transform, targetPosition);
		moveCounter++;
	}


	protected void TryToAddMove(Vector2Int coords)
	{
		availableMoves.Add(coords);
	}

	public void SetData(Vector2Int coords, TeamColor team, Board board)
	{
		this.team = team;
		occupiedSquare = coords;
		this.board = board;
		transform.position = board.CalculatePositionFromCoords(coords);
	}

    protected Piece GetPieceInDirection<T>(TeamColor team, Vector2Int direction) where T : Piece
    {
		for (int i = 1; i <= Board.BOARD_SIZE; i++)
		{
			Vector2Int nextCoordinates = occupiedSquare + direction * i;
			Piece piece = board.GetPieceOnSquare(nextCoordinates);
			if (!board.CheckIfCoordinatesAreOnBoard(nextCoordinates))
				return null;
			if (piece != null)
			{
				if (piece.team != team || !(piece is T))
					return null;
				else if (piece.team == team && piece is T)
					return piece;
			}
		}
		return null;
    }
}
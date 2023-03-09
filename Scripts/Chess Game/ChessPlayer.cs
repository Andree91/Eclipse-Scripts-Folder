using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class ChessPlayer
{
	public TeamColor team { get; set; }
	public Board board { get; set; }
	public List<Piece> activePieces { get; private set; }

	public ChessPlayer(TeamColor team, Board board)
	{
		activePieces = new List<Piece>();
		this.board = board;
		this.team = team;
	}

	public void AddPiece(Piece piece)
	{
		if (!activePieces.Contains(piece))
			activePieces.Add(piece);
	}

	public void RemovePiece(Piece piece)
	{
		if (activePieces.Contains(piece))
			activePieces.Remove(piece);
	}

	public void GenerateAllPossibleMoves()
	{
		foreach (var piece in activePieces)
		{
			if(board.HasPiece(piece))
				piece.SelectAvaliableSquares();
		}
	}

    public Piece[] GetPiecesAttackingOppositePieceOfType<T>() where T : Piece
    {
        return activePieces.Where(p => p.IsAttackingPieceOfType<T>()).ToArray();
    }

    public Piece[] GetPiecesOfType<T>() where T : Piece
    {
        return activePieces.Where(p => p is T).ToArray();
    }

    public void RemoveMovesEnablingAttackOnPiece<T>(ChessPlayer opponent, Piece selectedPiece) where T : Piece
    {
        List<Vector2Int> coordinatesToRemove = new List<Vector2Int>();
        foreach (var coordinates in selectedPiece.availableMoves)
        {
            Piece pieceOnSquare = board.GetPieceOnSquare(coordinates);
            board.UpdateBoardOnPieceMove(coordinates, selectedPiece.occupiedSquare, selectedPiece, null);
            opponent.GenerateAllPossibleMoves();
            if (opponent.ChecklIfIsAttackingPiece<T>())
                coordinatesToRemove.Add(coordinates);
            board.UpdateBoardOnPieceMove(selectedPiece.occupiedSquare, coordinates, selectedPiece, pieceOnSquare);
        }
        foreach (var coordinates in coordinatesToRemove)
        {
            selectedPiece.availableMoves.Remove(coordinates);
        }
    }

    public void OnGameRestarted()
    {
        activePieces.Clear();
    }

    private bool ChecklIfIsAttackingPiece<T>() where T : Piece
    {
        foreach (var piece in activePieces)
        {
            if (board.HasPiece(piece) && piece.IsAttackingPieceOfType<T>())
                return true;
        }
        return false;
    }

    public bool CanHidePieceFromAttack<T>(ChessPlayer opponent) where T : Piece
    {
        foreach (var piece in activePieces)
        {
            foreach (var coordinates in piece.availableMoves)
            {
                Piece pieceOnCoordinates = board.GetPieceOnSquare(coordinates);
                board.UpdateBoardOnPieceMove(coordinates, piece.occupiedSquare, piece, null);
                opponent.GenerateAllPossibleMoves();
                if (!opponent.ChecklIfIsAttackingPiece<T>())
                {
                    board.UpdateBoardOnPieceMove(piece.occupiedSquare, coordinates, piece, pieceOnCoordinates);
                    return true;
                }
                board.UpdateBoardOnPieceMove(piece.occupiedSquare, coordinates, piece, pieceOnCoordinates);
            }
        }
        return false;
    }
}
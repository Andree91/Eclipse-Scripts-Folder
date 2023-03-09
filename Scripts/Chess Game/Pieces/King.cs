using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    Vector2Int[] directions = new Vector2Int[]
    {
        new Vector2Int(-1, 1),
        new Vector2Int(0, 1),
        new Vector2Int(1, 1),
        new Vector2Int(-1, 0),
        new Vector2Int(1, 0),
        new Vector2Int(-1, -1),
        new Vector2Int(0, -1),
        new Vector2Int(1, -1),
    };

    private Vector2Int leftCastlingMove;
    private Vector2Int rightCastlingMove;

    private Piece leftRook;
    private Piece rightRook;

    public override List<Vector2Int> SelectAvaliableSquares()
    {
        availableMoves.Clear();
        AssingStandardMoves();
        AssingCastlingMoves();
        //Debug.Log("Has moved is: " + hasMoved);
        return availableMoves;
    }

    private void AssingStandardMoves()
    {
        float range = 1;
        foreach (var direction in directions)
        {
            for (int i = 1; i <= range; i++)
            {
                Vector2Int nextCoords = occupiedSquare + direction * i;
                Piece piece = board.GetPieceOnSquare(nextCoords);
                if (!board.CheckIfCoordinatesAreOnBoard(nextCoords))
                    break;
                if (piece == null)
                    TryToAddMove(nextCoords);
                else if (!piece.IsOnSameTeam(this))
                {
                    TryToAddMove(nextCoords);
                    break;
                }
                else if (piece.IsOnSameTeam(this))
                    break;
            }
        }
    }

    private void AssingCastlingMoves()
    {
        leftCastlingMove = new Vector2Int(-1, -1);
        rightCastlingMove = new Vector2Int(-1, -1);

        if (hasMoved)
            return;

        leftRook = GetPieceInDirection<Rook>(team, Vector2Int.left);
        if (leftRook && !leftRook.hasMoved)
        {
            leftCastlingMove = occupiedSquare + Vector2Int.left * 2;
            availableMoves.Add(leftCastlingMove);
        }

        rightRook = GetPieceInDirection<Rook>(team, Vector2Int.right);
        if (rightRook && !rightRook.hasMoved)
        {
            rightCastlingMove = occupiedSquare + Vector2Int.right * 2;
            availableMoves.Add(rightCastlingMove);
        }
    }

    public override void MovePiece(Vector2Int coordinates)
    {
        base.MovePiece(coordinates);
        if (coordinates == leftCastlingMove)
        {
            board.UpdateBoardOnPieceMove(coordinates + Vector2Int.right, leftRook.occupiedSquare, leftRook, null);
            leftRook.MovePiece(coordinates + Vector2Int.right);
        }
        else if (coordinates == rightCastlingMove)
        {
            board.UpdateBoardOnPieceMove(coordinates + Vector2Int.left, rightRook.occupiedSquare, rightRook, null);
            rightRook.MovePiece(coordinates + Vector2Int.left);
        }

    }

    // private Piece GetPieceInDirection<T>(TeamColor team, Vector2Int direction)
    // {
    //     for (int i = 1; i <= Board.BOARD_SIZE; i++)
    //     {
    //         Vector2Int nextCoords = occupiedSquare + direction * i;
    //         Piece piece = board.GetPieceOnSquare(nextCoords);
    //         if (!board.CheckIfCoordinatesAreOnBoard(nextCoords))
    //             return null;
    //         if (piece != null)
    //         {
    //             if (piece.team != team || !(piece is T))
    //                 return null;
    //             else if (piece.team == team && piece is T)
    //                 return piece;
    //         }
    //     }
    //     return null;
    // }
}

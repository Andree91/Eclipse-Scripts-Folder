using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    Vector2Int leftSquare;
    Vector2Int rightSquare;

    public override List<Vector2Int> SelectAvaliableSquares()
    {
        availableMoves.Clear();

        Vector2Int direction = team == TeamColor.White ? Vector2Int.up : Vector2Int.down;
        float range = hasMoved ? 1 : 2;
        for (int i = 1; i <= range; i++)
        {
            Vector2Int nextCoords = occupiedSquare + direction * i;
            Piece piece = board.GetPieceOnSquare(nextCoords);
            if (!board.CheckIfCoordinatesAreOnBoard(nextCoords))
                break;
            if (piece == null)
                TryToAddMove(nextCoords);
            else
                break;
        }

        Vector2Int[] takeDirections = new Vector2Int[] { new Vector2Int(1, direction.y), new Vector2Int(-1, direction.y) };
        for (int i = 0; i < takeDirections.Length; i++)
        {
            Vector2Int nextCoords = occupiedSquare + takeDirections[i];
            Piece piece = board.GetPieceOnSquare(nextCoords);
            if (!board.CheckIfCoordinatesAreOnBoard(nextCoords))
                continue;
            if (piece != null && !piece.IsOnSameTeam(this))
            {
                TryToAddMove(nextCoords);
            }
        }

        CheckForEnPassat();
        return availableMoves;
    }

    public override void MovePiece(Vector2Int coords)
    {
        base.MovePiece(coords);
        CheckForPromotion();
        CheckForEnPassatTake();
    }

    private void CheckForPromotion()
    {
        int endOfBoardYCoordinate = team == TeamColor.White ? Board.BOARD_SIZE - 1 : 0;
        if (occupiedSquare.y == endOfBoardYCoordinate)
            board.PromotePiece(this);
    }

    private void CheckForEnPassat()
    {
        Vector2Int leftSquareCoordinates = occupiedSquare + Vector2Int.left;
        Vector2Int rightSquareCoordinates = occupiedSquare + Vector2Int.right;
        Piece leftPiece = board.GetPieceOnSquare(leftSquareCoordinates);
        Piece rightPiece = board.GetPieceOnSquare(rightSquareCoordinates);
        if (board.CheckIfCoordinatesAreOnBoard(leftSquareCoordinates))
        {
            if (leftPiece != null && !IsOnSameTeam(leftPiece) && leftPiece.moveCounter == 1)
            {
                if (team == TeamColor.White)
                {
                    TryToAddMove(leftSquareCoordinates + Vector2Int.up);
                }
                else
                {
                    TryToAddMove(leftSquareCoordinates + Vector2Int.down);
                }
                AddOpponentLeftPawnCoordinates(leftSquareCoordinates);
            }
        }
        else if (board.CheckIfCoordinatesAreOnBoard(rightSquareCoordinates))
        {
            if (rightPiece != null && !IsOnSameTeam(rightPiece) && rightPiece.moveCounter == 1)
            {
                if (team == TeamColor.White)
                {
                    TryToAddMove(rightSquareCoordinates + Vector2Int.up);
                }
                else
                {
                    TryToAddMove(rightSquareCoordinates + Vector2Int.down);
                }
                AddOpponentRightPawnCoordinates(rightSquareCoordinates);
            }
        }
    }

    private void AddOpponentLeftPawnCoordinates(Vector2Int opponentPawnsCoords)
    {
        leftSquare = opponentPawnsCoords;
    }

    private void AddOpponentRightPawnCoordinates(Vector2Int opponentPawnsCoords)
    {
        rightSquare = opponentPawnsCoords;
    }

    private void CheckForEnPassatTake()
    {
        if (occupiedSquare.x == leftSquare.x)
        {
            board.TakePiece(board.GetPieceOnSquare(leftSquare));
        }
        else if (occupiedSquare.x == rightSquare.x)
        {
            board.TakePiece(board.GetPieceOnSquare(rightSquare));
        }
    }
}

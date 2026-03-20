using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Solves Lights Out puzzles using Gaussian elimination over GF(2).
/// Attach to any GameObject, or call LightsOutSolver.Solve() statically.
///
/// Usage:
///   bool[,] puzzle = new bool[5, 5];
///   // ... set puzzle[row, col] = true for lights that are ON ...
///   List<Vector2Int> moves = LightsOutSolver.Solve(puzzle);
///   // Press each move position to turn all lights off.
/// </summary>
public class LightsOutSolver
{
    /// <summary>
    /// Returns a list of grid positions to press in order to solve the puzzle.
    /// Each Vector2Int is (row, col). Returns null if no solution exists.
    /// </summary>
    public static List<Vector2Int> Solve(bool[,] puzzle)
    {
        int m = puzzle.GetLength(0);
        int n = puzzle.GetLength(1);
        int mn = m * n;

        bool[,] toggle = BuildToggleMatrix(m, n, mn);
        bool[] board = Flatten(puzzle, m, n, mn);

        GaussianEliminate(toggle, board, mn);

        bool[] solution = BackSubstitute(toggle, board, mn);
        if (solution == null)
        {
            Debug.LogWarning("LightsOutSolver: This puzzle has no solution.");
            return null;
        }

        var moves = new List<Vector2Int>();
        for (int i = 0; i < mn; i++)
            if (solution[i])
                moves.Add(new Vector2Int(i / n, i % n));

        return moves;
    }

    // -------------------------------------------------------------------------

    // Builds the MN×MN matrix where column `btn` has 1s at every cell
    // toggled by pressing button `btn`.
    static bool[,] BuildToggleMatrix(int m, int n, int mn)
    {
        bool[,] t = new bool[mn, mn];

        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                int btn = i * n + j;
                t[btn, btn] = true; // the button itself

                // orthogonal neighbors
                if (i > 0)     t[btn, (i - 1) * n + j] = true;
                if (i < m - 1) t[btn, (i + 1) * n + j] = true;
                if (j > 0)     t[btn, i * n + (j - 1)] = true;
                if (j < n - 1) t[btn, i * n + (j + 1)] = true;
            }
        }
        return t;
    }
    // Row-major flatten of a 2D bool grid into a 1D array.
    static bool[] Flatten(bool[,] puzzle, int m, int n, int mn)
    {
        bool[] v = new bool[mn];
        for (int i = 0; i < m; i++)
            for (int j = 0; j < n; j++)
                v[i * n + j] = puzzle[i, j];
        return v;
    }
    // Forward Gaussian elimination over GF(2). Modifies toggle and board in-place.
    static void GaussianEliminate(bool[,] toggle, bool[] board, int mn)
    {
        int nextFree = 0;
        for (int col = 0; col < mn; col++)
        {
            // Find a pivot row
            int pivot = -1;
            for (int row = nextFree; row < mn; row++)
            {
                if (toggle[col, row]) { pivot = row; break; }
            }
            if (pivot == -1) continue;

            // Swap pivot row into nextFree position
            if (pivot != nextFree)
            {
                for (int c = 0; c < mn; c++)
                {
                    bool tmp = toggle[c, pivot];
                    toggle[c, pivot] = toggle[c, nextFree];
                    toggle[c, nextFree] = tmp;
                }
                bool btmp = board[pivot];
                board[pivot] = board[nextFree];
                board[nextFree] = btmp;
            }
            // Eliminate this column from all rows below
            for (int row = pivot + 1; row < mn; row++)
            {
                if (!toggle[col, row]) continue;
                for (int c = 0; c < mn; c++)
                    toggle[c, row] ^= toggle[c, nextFree];
                board[row] ^= board[nextFree];
            }
            nextFree++;
        }
    }
    // Back-substitution. Returns null if the system is inconsistent.
    static bool[] BackSubstitute(bool[,] toggle, bool[] board, int mn)
    {
        bool[] result = new bool[mn];

        for (int row = mn - 1; row >= 0; row--)
        {
            int pivot = -1;
            for (int col = 0; col < mn; col++)
            {
                //if (toggle[col, row]) { pivot = col; break; }
            }

            if (pivot == -1)
            {
                if (board[row]) return null; // no solution
            }
            else
            {
                result[pivot] = board[row];
                for (int col = pivot + 1; col < mn; col++)
                    result[pivot] ^= (toggle[col, row] & result[col]);
            }
        }
        return result;
    }
}
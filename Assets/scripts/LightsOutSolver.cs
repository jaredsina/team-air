using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Solves Lights Out puzzles using Gaussian elimination over GF(2).
/// 
/// The puzzle is modeled as a system of linear equations over the finite field GF(2),
/// where each button press is represented as a toggle vector. The solver constructs
/// a toggle matrix V and solves V * a = G, where G is the puzzle state and a is the
/// solution vector indicating which buttons to press.
/// 
/// Complexity: O(M^3 * N^3) where M and N are the grid dimensions.
/// </summary>
public class LightsOutSolver
{
    /// <summary>
    /// Solves a Lights Out puzzle and returns the list of (row, col) positions to press.
    /// </summary>
    /// <param name="puzzle">2D bool array where true = light on, false = light off.</param>
    /// <returns>List of (row, col) tuples indicating which buttons to press.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the puzzle has no solution.</exception>
    public static bool[,] toggle;
    public static List<(int row, int col)> Solve(bool[,] puzzle)
    {
        int m = puzzle.GetLength(0);
        int n = puzzle.GetLength(1);
        int mn = m * n;

        bool[,] toggle = MakeToggleMatrix(m, n);
        bool[] puzzleVector = LinearizePuzzle(puzzle, m, n);
        bool[] solution = SolvePuzzle(toggle, puzzleVector, mn);

    return SolutionVectorToPairs(solution, m, n);
    }
    public static List<(int row, int col)> Solve(bool[][] puzzle)
    {
        int m = puzzle.Length;
        int n = puzzle[0].Length;
        var grid = new bool[m, n];

        for (int i = 0; i < m; i++)
            for (int j = 0; j < n; j++)
                grid[i, j] = puzzle[i][j];

        return Solve(grid);
    }

    /// <summary>
    /// Overload that accepts an int grid (0/1 values) for convenience.
    /// </summary>
    public static List<(int row, int col)> Solve(int[,] puzzle)
    {
        int m = puzzle.GetLength(0);
        int n = puzzle.GetLength(1);
        var grid = new bool[m, n];

        for (int i = 0; i < m; i++)
            for (int j = 0; j < n; j++)
                grid[i, j] = puzzle[i, j] != 0;

        return Solve(grid);
    }

    /// <summary>
    /// Checks whether a given Lights Out puzzle is solvable.
    /// </summary>
    public static bool IsSolvable(bool[,] puzzle)
    {
        bool allOff = true;
        foreach (bool element in puzzle)
        {
            if( element){
                    allOff = false;
            }
        }
        if (allOff) return false;
        try
        {
            Solve(puzzle);
            return true;
        }
        catch (InvalidOperationException)
        {
            return false;
        }
    }

    #region Internal Implementation

    private static int RowMajorIndex(int i, int j, int n)
    {
        return i * n + j;
    }

    /// <summary>
    /// Constructs the MN x MN toggle matrix for an M x N Lights Out grid.
    /// Each column represents the effect of pressing one button.
    /// </summary>
    private static bool[,] MakeToggleMatrix(int m, int n)
    {
        int mn = m * n;
        var result = new bool[mn, mn];

        // Neighbor offsets: up, down, left, right
        int[] dr = { -1, 1, 0, 0 };
        int[] dc = { 0, 0, -1, 1 };

        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                int col = RowMajorIndex(i, j, n);

                // Pressing a button toggles itself.
                result[RowMajorIndex(i, j, n), col] = true;

                // And its four neighbors (if in bounds).
                for (int d = 0; d < 4; d++)
                {
                    int ni = i + dr[d];
                    int nj = j + dc[d];

                    if (ni >= 0 && ni < m && nj >= 0 && nj < n)
                        result[RowMajorIndex(ni, nj, n), col] = true;
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Flattens a 2D puzzle grid into a 1D array in row-major order.
    /// </summary>
    private static bool[] LinearizePuzzle(bool[,] puzzle, int m, int n)
    {
        int mn = m * n;
        var vector = new bool[mn];

        for (int i = 0; i < m; i++)
            for (int j = 0; j < n; j++)
                vector[RowMajorIndex(i, j, n)] = puzzle[i, j];

        return vector;
    }

    /// <summary>
    /// Finds a row at or below startRow that has a true value in the given column.
    /// Returns -1 if no pivot is found.
    /// </summary>
    private static int FindPivot(bool[,] matrix, int size, int startRow, int pivotColumn)
    {
        for (int row = startRow; row < size; row++)
            if (matrix[row, pivotColumn])
                return row;

        return -1;
    }

    /// <summary>
    /// Reduces the toggle matrix to row echelon form via Gaussian elimination over GF(2),
    /// applying the same row operations to the puzzle vector.
    /// </summary>
    private static void PerformGaussianElimination(bool[,] toggle, bool[] puzzle, int mn)
    {
        int nextFreeRow = 0;

        for (int col = 0; col < mn; col++)
        {
            int pivotRow = FindPivot(toggle, mn, nextFreeRow, col);
            if (pivotRow == -1) continue;

            // Swap the pivot row with the next free row.
            if (pivotRow != nextFreeRow)
            {
                for (int c = 0; c < mn; c++)
                {
                    bool temp = toggle[pivotRow, c];
                    toggle[pivotRow, c] = toggle[nextFreeRow, c];
                    toggle[nextFreeRow, c] = temp;
                }

                bool pTemp = puzzle[pivotRow];
                puzzle[pivotRow] = puzzle[nextFreeRow];
                puzzle[nextFreeRow] = pTemp;
            }

            // XOR the pivot row into every lower row that has a 1 in this column.
            for (int row = pivotRow + 1; row < mn; row++)
            {
                if (!toggle[row, col]) continue;

                for (int c = 0; c < mn; c++)
                    toggle[row, c] ^= toggle[nextFreeRow, c];

                puzzle[row] ^= puzzle[nextFreeRow];
            }

            nextFreeRow++;
        }
    }

    /// <summary>
    /// Performs back-substitution on the row-echelon matrix to extract a solution.
    /// Free variables default to false (0).
    /// </summary>
    private static bool[] BackSubstitute(bool[,] toggle, bool[] puzzle, int mn)
    {
        var result = new bool[mn];

        for (int row = mn - 1; row >= 0; row--)
        {
            // Find the pivot column in this row.
            int pivot = -1;
            for (int col = 0; col < mn; col++)
            {
                if (toggle[row, col])
                {
                    pivot = col;
                    break;
                }
            }

            if (pivot == -1)
            {
                // All-zero row: if the puzzle value is true, no solution exists.
                if (puzzle[row])
                    throw new InvalidOperationException("Puzzle has no solution.");
            }
            else
            {
                // x_pivot = puzzle[row] XOR (sum of toggle[row,c] AND result[c] for c > pivot)
                result[pivot] = puzzle[row];
                for (int col = pivot + 1; col < mn; col++)
                    result[pivot] ^= toggle[row, col] & result[col];
            }
        }

        return result;
    }

    /// <summary>
    /// Solves the linear system by running Gaussian elimination then back-substitution.
    /// </summary>
    private static bool[] SolvePuzzle(bool[,] toggle, bool[] puzzle, int mn)
    {
        PerformGaussianElimination(toggle, puzzle, mn);
        return BackSubstitute(toggle, puzzle, mn);
    }

    /// <summary>
    /// Converts the solution vector back into a list of (row, col) button presses.
    /// </summary>
    private static List<(int row, int col)> SolutionVectorToPairs(bool[] solution, int m, int n)
    {
        var result = new List<(int, int)>();

        for (int i = 0; i < m * n; i++)
        {
            if (solution[i])
                result.Add((i / n, i % n));
        }

        return result;
    }

    #endregion
}
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AIAgent : MonoBehaviour
{
    public GameManager gameManager;
    public PlayerManager playerManager;
    public ScoreManager scoreManager;
    public int easyDepth = 2;
    public int mediumDepth = 3;
    public int hardDepth = 5;
    private int currentDepth;
    public TitleManager scenesManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        playerManager = FindObjectOfType<PlayerManager>();
        scoreManager = FindObjectOfType<ScoreManager>();

        scenesManager = FindObjectOfType<TitleManager>();
        switch (scenesManager.difficultyLevel)
        {
            case "Easy":
                currentDepth = easyDepth;
                break;
            case "Medium":
                currentDepth = mediumDepth;
                break;
            case "Hard":
                currentDepth = hardDepth;
                break;
            default:
                currentDepth = mediumDepth; // Default to medium if not specified
                break;
        }
    }

    public void MakeMove()
    {
        StartCoroutine(PerformMinimax());
        UnityEngine.Debug.Log("Player two is making a move...");
    }

    private IEnumerator PerformMinimax()
    {
        GameObject bestMove = null;
        float bestValue = float.MinValue;

        List<GameObject> availableWalls = GetAvailableWalls();
        availableWalls.Sort((a, b) => EvaluateMove(a).CompareTo(EvaluateMove(b)));

        foreach (var wall in availableWalls)
        {
            float moveValue = Minimax(gameManager.boxToWalls, wall, currentDepth, float.MinValue, float.MaxValue, true);
            if (moveValue > bestValue)
            {
                bestValue = moveValue;
                bestMove = wall;
            }

            yield return null; // Yield to allow Unity to process other operations
        }

        if (bestMove != null)
        {
            // Simulate the best move
            bestMove.GetComponent<ClickToChangeColor>().SimulateClick();
        }
    }

    private float Minimax(BoxWallAssociation[] state, GameObject wall, int depth, float alpha, float beta, bool isMaximizingPlayer)
    {
        if (depth == 0 || gameManager.IsGameOver())
        {
            return Utility(state);
        }

        // Simulate the move
        Color originalColor = wall.GetComponent<Renderer>().material.color;
        bool originalColliderState = wall.GetComponent<Collider>().enabled;

        wall.GetComponent<Renderer>().material.color = isMaximizingPlayer ? playerManager.player1Material.color : playerManager.player2Material.color;
        wall.GetComponent<Collider>().enabled = false;

        float bestValue = isMaximizingPlayer ? float.MinValue : float.MaxValue;

        List<GameObject> availableWalls = GetAvailableWalls();
        foreach (var nextWall in availableWalls)
        {
            if (nextWall == wall) continue; // Skip the current wall being evaluated
            float newValue = Minimax(state, nextWall, depth - 1, alpha, beta, !isMaximizingPlayer);

            if (isMaximizingPlayer)
            {
                bestValue = Mathf.Max(bestValue, newValue);
                alpha = Mathf.Max(alpha, bestValue);
            }
            else
            {
                bestValue = Mathf.Min(bestValue, newValue);
                beta = Mathf.Min(beta, bestValue);
            }

            if (beta <= alpha)
            {
                break; // Beta cutoff
            }
        }

        // Restore the move
        wall.GetComponent<Renderer>().material.color = originalColor;
        wall.GetComponent<Collider>().enabled = originalColliderState;

        return bestValue;
    }

    private float Utility(BoxWallAssociation[] state)
    {
        float playerScore = playerManager.currentPlayer == Player.Player1 ? scoreManager.player1Score : scoreManager.player2Score;
        float boxControl = 0f;

        foreach (var association in state)
        {
            bool allWallsPlayerColor = true;
            foreach (var wall in association.walls)
            {
                if (wall.GetComponent<Renderer>().material.color != playerManager.player1Material.color)
                {
                    allWallsPlayerColor = false;
                    break; // Exit the loop early if any wall is not of the player's color
                }
            }

            if (allWallsPlayerColor)
            {
                boxControl += playerManager.currentPlayer == Player.Player1 ? 1 : 0;
            }
        }

        return 0.6f * playerScore + 0.4f * boxControl;
    }

    private float EvaluateMove(GameObject wall)
    {
        // Evaluate move based on box closure
        float score = 0f;

        // Simulate the move
        Color originalColor = wall.GetComponent<Renderer>().material.color;
        wall.GetComponent<Renderer>().material.color = playerManager.currentPlayer == Player.Player1 ? playerManager.player1Material.color : playerManager.player2Material.color;
        wall.GetComponent<Collider>().enabled = false;

        // Check for closed boxes
        foreach (var association in gameManager.boxToWalls)
        {
            bool allWallsPlayerColor = true;
            foreach (var boxWall in association.walls)
            {
                if (boxWall.GetComponent<Renderer>().material.color != (playerManager.currentPlayer == Player.Player1 ? playerManager.player1Material.color : playerManager.player2Material.color))
                {
                    allWallsPlayerColor = false;
                    break;
                }
            }

            if (allWallsPlayerColor)
            {
                // Increase score for each closed box
                score += 1f;
            }
        }

        // Restore the move
        wall.GetComponent<Renderer>().material.color = originalColor;
        wall.GetComponent<Collider>().enabled = true;

        return score;
    }

    private List<GameObject> GetAvailableWalls()
    {
        List<GameObject> availableWalls = new List<GameObject>();

        foreach (var association in gameManager.boxToWalls)
        {
            foreach (var wall in association.walls)
            {
                if (wall.GetComponent<Collider>().enabled)
                {
                    availableWalls.Add(wall);
                }
            }
        }

        return availableWalls;
    }
}

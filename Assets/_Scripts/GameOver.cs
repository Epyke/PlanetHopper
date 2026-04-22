using Unity.VisualScripting;
using UnityEngine;
using LootLocker.Requests;
using System.Collections;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.Rendering;
using TMPro;
using UnityEngine.SceneManagement;
public class GameOver : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField inputField;
    [SerializeField]
    private TextMeshProUGUI leaderboardScoreText;
    [SerializeField]
    private TextMeshProUGUI leaderboardNameText;
    [SerializeField]
    private TextMeshProUGUI scoreText;
    private int leaderboardTopCount = 5;
    private int score = 0;
    private string leaderboardID = "33678";
    public void StopGame(int score)
    {
        this.score = score;
        scoreText.text = score.ToString();
        GetLeaderboard();
    }

    public void SubmitScore()
    {
        StartCoroutine(SubmitScoreToLeaderboard());
    }

    private IEnumerator SubmitScoreToLeaderboard()
    {
        bool? nameSet = null;
        LootLockerSDKManager.SetPlayerName(inputField.text, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfully set the player name.");
                nameSet = true;
            }
            else
            {
                Debug.Log("Was not able to add the player name.");
                nameSet = false;
            }
        });
        yield return new WaitUntil(() => nameSet.HasValue);
        //if (!nameSet.Value) yield break;
        bool? scoreSubmitted = null;
        LootLockerSDKManager.SubmitScore("", score, leaderboardID, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfully submitted the score to the leaderboard.");
                scoreSubmitted = true;
            }
            else
            {
                Debug.Log("Unsuccessfully submitted the score to the leaderboard.");
                scoreSubmitted = false;
            }
        });
        yield return new WaitUntil(() => scoreSubmitted.HasValue);
        if (!scoreSubmitted.Value) yield break;
        GetLeaderboard();
    }

    private void GetLeaderboard()
    {
        LootLockerSDKManager.GetScoreList(leaderboardID, leaderboardTopCount, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfully retireved the score from the leaderboard.");
                string leaderboardName = "";
                string leaderboardScore = "";
                LootLockerLeaderboardMember[] members = response.items;
                for (int i = 0; i < members.Length; ++i)
                {
                    LootLockerPlayer player = members[i].player;
                    if (player == null) continue;

                    if (player.name != "")
                    {
                        leaderboardName += player.name + "\n";
                    }
                    else
                    {
                        leaderboardName += player.id;
                    }
                    leaderboardScore += members[i].score + "\n";
                }
                leaderboardNameText.SetText(leaderboardName);
                leaderboardScoreText.SetText(leaderboardScore);
            }
            else
            {
                Debug.Log("Failed to get the score. Error: " + response.errorData?.message);
            }
        });
    }

    public void AddXP(int score)
    {

    }

    public void ReloadScene()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.gameMusic);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.menuMusic);
        }
        SceneManager.LoadScene("MainMenu");
    }
}

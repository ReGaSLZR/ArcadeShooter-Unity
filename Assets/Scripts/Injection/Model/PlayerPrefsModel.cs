using UnityEngine;

namespace Injection.Model {

    public class PlayerPrefsModel : MonoBehaviour, PlayerPrefsModel.IGetter, PlayerPrefsModel.ISetter
    {

        #region Interfaces
        public interface IGetter {
            int GetHighScore();
            string GetHighScorePlayerName();
        }

        public interface ISetter {
            void SetHighScore(int score, string playerName);
        }
        #endregion

        private const string KEY_PREFIX = "ArcadeShooter-";
        private const string KEY_HIGH_SCORE = KEY_PREFIX + "HS";
        private const string KEY_HIGH_SCORE_PLAYER_NAME = KEY_PREFIX + "HS-PlayerName";

        public int GetHighScore() {
            return PlayerPrefs.GetInt(KEY_HIGH_SCORE, 0);
        }

        public string GetHighScorePlayerName() {
            return PlayerPrefs.GetString(KEY_HIGH_SCORE_PLAYER_NAME, null);
        }

        public void SetHighScore(int score, string playerName) {
            PlayerPrefs.SetInt(KEY_HIGH_SCORE, score);
            PlayerPrefs.SetString(KEY_HIGH_SCORE_PLAYER_NAME, playerName);

            PlayerPrefs.Save();
        }


    }

}
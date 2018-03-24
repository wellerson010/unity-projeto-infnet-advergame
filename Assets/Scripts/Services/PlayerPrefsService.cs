using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public static class PlayerPrefsService
    {
        public static int GetStarsHighScoreFromLevel(int level)
        {
            return PlayerPrefs.GetInt("level" + level, 0);
        }

        public static void SetStarsHighScoreFromLevel(int stars, int level)
        {
            PlayerPrefs.SetInt("level" + level, stars);
        }
    }
}

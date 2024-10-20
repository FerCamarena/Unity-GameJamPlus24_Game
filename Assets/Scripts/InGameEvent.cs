//Libraries
using System;
using UnityEngine;

//Global base class to define multiple in-game-events
public static class InGameEvent {
    public static Action OnGameOver;
    public static Action OnGameStart;

    public static Action<Enemy> enemyKill;

    public static Action GameOver;
    public static Action GameWon;
    public static Action GameStarted;

    public static Action WaveStarted;
    public static Action WaveEnded;
}
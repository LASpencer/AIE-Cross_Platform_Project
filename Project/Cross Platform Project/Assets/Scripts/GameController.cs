using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController {

    private static GameController instance;

    public static GameController Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new GameController();
            }
            return instance;
        }
    }

    public int LastScore = 0;

    public int HighScore = 0;

    private GameController()
    {

    }
}

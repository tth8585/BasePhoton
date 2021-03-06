﻿using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance = null;
    public Text InfoText;

    public GameObject[] foodPrefabs;
    public GameObject[] SnakeBotPrefabs;

    #region UNITY

    public void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable
            {
                {GameData.PLAYER_LOADED_LEVEL, true}
            };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        StartGame();
    }
    #endregion

    #region COROUTINES

    private IEnumerator SpawnFood()
    {
        //use scene instantiate
        //object[] instantiationData = { force, torque, true };

        //PhotonNetwork.InstantiateSceneObject("BigAsteroid", position, Quaternion.Euler(Random.value * 360.0f, Random.value * 360.0f, Random.value * 360.0f), 0, instantiationData);
        yield return null;
    }
    private IEnumerator SpawnBot()
    {
        yield return null;
    }
    private IEnumerator EndOfGame(string winner, int score)
    {
        float timer = 5.0f;
        while (timer > 0.0f)
        {
            //show text winner
            InfoText.text = string.Format("Player {0} won with {1} points.\n\n\nReturning to login screen in {2} seconds.", winner, score, timer.ToString("n2"));

            yield return new WaitForEndOfFrame();

            timer -= Time.deltaTime;
        }
        PhotonNetwork.LeaveRoom();
    }


    #endregion

    #region PUN CALLBACKS


    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("disconect from master " + cause);
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            //when master client switch, start spawn again
            StartCoroutine(SpawnFood());
            StartCoroutine(SpawnBot());
            //StartCoroutine(SpawnAsteroid());
        }
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        CheckEndOfGame();
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey(GameData.PLAYER_LIVES))
        {
            CheckEndOfGame();
            return;
        }
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        if (changedProps.ContainsKey(GameData.PLAYER_LOADED_LEVEL))
        {
            if (CheckAllPlayerLoadedLevel())
            {
                ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable
                    {
                        {CountdownTimer.CountdownStartTime, (float) PhotonNetwork.Time}
                    };
                PhotonNetwork.CurrentRoom.SetCustomProperties(props);
            }
        }
    }


    #endregion

    #region Other

    void StartGame()
    {
        //spawn player prefabs
        //PhotonNetwork.Instantiate("Spaceship", position, rotation, 0);


        if (PhotonNetwork.IsMasterClient)
        {
            //spawn food
            StartCoroutine(SpawnFood());
            //spawn snake ai
            StartCoroutine(SpawnBot());
            //StartCoroutine(SpawnAsteroid());
        }
    }
    private void CheckEndOfGame()
    {
        bool allDestroyed = true;
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            object lives;
            if (p.CustomProperties.TryGetValue(GameData.PLAYER_LIVES, out lives))
            {
                if ((int)lives > 0)
                {
                    allDestroyed = false;
                    break;
                }
            }
        }

        if (allDestroyed)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                StopAllCoroutines();
            }

            string winner = "";
            int score = -1;

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                if (p.GetScore() > score)
                {
                    winner = p.NickName;
                    score = p.GetScore();
                }
            }

            StartCoroutine(EndOfGame(winner, score));
        }
    }
    private bool CheckAllPlayerLoadedLevel()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            object playerLoadedLevel;

            if (p.CustomProperties.TryGetValue(GameData.PLAYER_LOADED_LEVEL, out playerLoadedLevel))
            {
                if ((bool)playerLoadedLevel)
                {
                    continue;
                }
            }

            return false;
        }

        return true;
    }

    #endregion
}

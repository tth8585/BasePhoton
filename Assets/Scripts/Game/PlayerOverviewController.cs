using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using System.Linq;

public class PlayerOverviewController : MonoBehaviourPunCallbacks
{
    public GameObject PlayerOverviewPrefab;
    private Dictionary<int, GameObject> playerListEntries;

    #region UNITY
    private void Awake()
    {
        playerListEntries = new Dictionary<int, GameObject>();

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            GameObject entry = Instantiate(PlayerOverviewPrefab);
            entry.transform.SetParent(gameObject.transform);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<Text>().color = GameData.GetColor(p.GetPlayerNumber());
            entry.GetComponent<Text>().text = string.Format("{0} "+"Score: {1}", p.NickName, p.GetScore());

            playerListEntries.Add(p.ActorNumber, entry);
        }
    }
    #endregion

    #region PUN CALLBACKS

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Destroy(playerListEntries[otherPlayer.ActorNumber].gameObject);
        playerListEntries.Remove(otherPlayer.ActorNumber);
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        GameObject entry;
        if (playerListEntries.TryGetValue(targetPlayer.ActorNumber, out entry))
        {
            entry.GetComponent<Text>().text = string.Format("{0} "+"Score: {1}", targetPlayer.NickName, targetPlayer.GetScore());
        }
    }


    #endregion
}

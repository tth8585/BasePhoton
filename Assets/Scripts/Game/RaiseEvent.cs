using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class RaiseEvent : MonoBehaviourPunCallbacks
{
    SpriteRenderer spriteRenderer;

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }
    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }
    private void NetworkingClient_EventReceived(EventData obj)
    {
       if(obj.Code == GameRaiseEvent.COLOR_CHANGE_EVENT)
        {
            object[] datas = (object[])obj.CustomData;
            float r = (float)datas[0];
            float g = (float)datas[1];
            float b = (float)datas[2];

            spriteRenderer.color = new Color(r, g, b);
        }
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (photonView.IsMine && Input.GetKeyDown("space"))
        {
            ChangeColor();
        }
    }
    void ChangeColor()
    {
        float r = Random.Range(0f, 1f);
        float g = Random.Range(0f, 1f);
        float b = Random.Range(0f, 1f);

        spriteRenderer.color = new Color(r, g, b);

        object[] colorDatas = new object[] { r, g, b };
        PhotonNetwork.RaiseEvent(GameRaiseEvent.COLOR_CHANGE_EVENT, colorDatas, RaiseEventOptions.Default, SendOptions.SendUnreliable);
    }
}

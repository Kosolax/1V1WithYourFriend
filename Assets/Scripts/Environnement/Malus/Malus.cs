using System.Collections;
using System.Collections.Generic;

using Mirror;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class Malus : NetworkBehaviour
{
    public GameObject Canvas;

    public KeyCode KeyCode;

    public TextMeshProUGUI Text;

    public float Price;

    public string TextToInsert;

    public int BuyableCount;

    private int timeBought;

    private GameObject playerInTrigger;

    private bool isInTrigger;

    public GameObject PlayerThatPaidMalus;

    public WaveManager WaveManager;

    public Image Image;

    private void Start()
    {
        this.Text.text = $"Press [{KeyCode}] to {TextToInsert}. It will cost {Price}";
        this.timeBought = 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            this.Canvas.SetActive(true);
            this.isInTrigger = true;
            this.playerInTrigger = other.gameObject;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(this.KeyCode) && this.isInTrigger)
        {
            if (playerInTrigger.GetComponent<NetworkIdentity>().isLocalPlayer && this.timeBought <= this.BuyableCount)
            {
                ZombiePlayer zombiePlayer = playerInTrigger.GetComponent<ZombiePlayer>();
                if (zombiePlayer.Money >= this.Price)
                {
                    this.timeBought++;
                    zombiePlayer.ZombiePlayerMoneyManager.LoseMoney(this.Price);
                    this.AddMalus(playerInTrigger.gameObject);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            this.isInTrigger = false;
            this.Canvas.SetActive(false);
            this.playerInTrigger = null;
        }
    }

    public virtual void SendMalus()
    {

    }

    public virtual void RevertMalus()
    {

    }

    [Command(requiresAuthority = false)]
    public void AddMalus(GameObject player)
    {
        this.AddMalusRpc(player);
    }

    [ClientRpc]
    public void AddMalusRpc(GameObject player)
    {
        this.PlayerThatPaidMalus = player;
        if (this.PlayerThatPaidMalus == this.WaveManager.FirstPlayer)
        {
            Image image = Instantiate(Image);
            image.transform.SetParent(this.WaveManager.SecondPlayer.GetComponent<ZombiePlayer>().Malus.transform);
            this.WaveManager.ListMalusPlayer2.Add(this);
        }
        else if (this.PlayerThatPaidMalus == this.WaveManager.SecondPlayer)
        {
            Image image = Instantiate(Image);
            image.transform.SetParent(this.WaveManager.FirstPlayer.GetComponent<ZombiePlayer>().Malus.transform);
            this.WaveManager.ListMalusPlayer1.Add(this);
        }
    }
}

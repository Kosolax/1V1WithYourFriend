
using Mirror;

using TMPro;

using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Interact : MonoBehaviour
{
    public float Price;

    public KeyCode KeyCode;

    public TextMeshProUGUI Text;

    public string TextToInsert;

    public GameObject Canvas;

    private bool isInTrigger;

    public GameObject PlayerInTrigger;

    public int BuyableCount;

    private int timeBought;

    public GameObject PlayerThatPaid;

    private void Start()
    {
        this.Text.text = $"Press [{KeyCode}] to {TextToInsert}. It will cost {Price}";
        this.isInTrigger = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            this.Canvas.SetActive(true);
            this.isInTrigger = true;
            this.PlayerInTrigger = other.gameObject;
        }
    }

    public bool CanInteract()
    {
        if (Input.GetKeyDown(this.KeyCode) && this.isInTrigger)
        {
            if (this.PlayerInTrigger.GetComponent<NetworkIdentity>().isLocalPlayer && this.timeBought <= this.BuyableCount)
            {
                ZombiePlayer zombiePlayer = this.PlayerInTrigger.GetComponent<ZombiePlayer>();
                if (zombiePlayer.Money >= this.Price)
                {
                    this.timeBought++;
                    zombiePlayer.ZombiePlayerMoneyManager.LoseMoney(this.Price);
                    return true;
                }
            }
        }

        return false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            this.isInTrigger = false;
            this.Canvas.SetActive(false);
            this.PlayerInTrigger = null;
        }
    }
}

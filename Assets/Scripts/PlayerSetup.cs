using Mirror;
using UnityEngine;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;

    [SerializeField]
    GameObject[] gameObjectToDisable;

    Camera sceneCamera;

    private void Start()
    {
        if(!isLocalPlayer)
        {
            //disable components that should'nt be used by the player when joining (like other player controler)
            for (int i = 0; i < componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;
            }

            for (int i = 0; i < gameObjectToDisable.Length; i++)
            {
                gameObjectToDisable[i].SetActive(false);
            }
        }
        else
        {
            //disable main camera on player join
            sceneCamera = Camera.main;
            if(sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }
        }

        this.GetComponent<Player>().Setup();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        uint netId = this.GetComponent<NetworkIdentity>().netId;
        Player player = this.GetComponent<Player>();

        GameManager.AddPlayer(netId, player);
    }

    private void OnDisable()
    {
        if(sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }
    }
}

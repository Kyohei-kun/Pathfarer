using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.VFX;
using static UnityEditor.PlayerSettings;

public class CS_Door : MonoBehaviour
{
    [Required("Mettre le préfab, pas l'instance !")][SerializeField] GameObject myKey;
    string keyName;
    GameObject player;
    CS_FeatureUnlocker inventaire;
    [SerializeField][MinValue(0)] float lerpTime = 3;
    [SerializeField][MinValue(0)] float openingTime = 3;
    GameObject openedPosition;
    [SerializeField][MinValue(0)] float openingDistance = 2;
    bool opening;
    bool opened;

    private void Start()
    {
        RefreshVisuel();
        player = GameObject.FindGameObjectWithTag("Player");
        inventaire = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CS_FeatureUnlocker>();
        openedPosition = transform.Find("OpenedPosition").gameObject;
        keyName = myKey.name;
    }

    [Button]
    public void RefreshVisuel()
    {
        transform.Find("Visu/Lock1").GetComponent<Renderer>().material = myKey.transform.Find("Key_Silver").GetComponent<Renderer>().sharedMaterial;
        transform.Find("Visu/Lock2").GetComponent<Renderer>().material = myKey.transform.Find("Key_Silver").GetComponent<Renderer>().sharedMaterial;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player && inventaire.CheckIfInInventory(keyName))
        {
            myKey = Instantiate(myKey, player.transform.position, Quaternion.identity);
            myKey.GetComponent<SphereCollider>().enabled = false;
            myKey.GetComponent<VisualEffect>().Play();
            StartCoroutine(LerpKey());
        }
    }

    private void Update()
    {
        if (inventaire.CheckIfInInventory(keyName) && !opened && !opening && Vector3.Distance(player.transform.position, transform.position) < openingDistance)
        {
            StartCoroutine(OpenDoor());
        }
    }

    IEnumerator LerpKey()
    {
        Vector3 pos;
        Vector3 rot;

        for (float f = 0; f < lerpTime; f += Time.deltaTime)
        {
            pos = Vector3.Lerp(player.transform.position, transform.position + (Vector3.up * 2), f.Remap(0, lerpTime, 0, 1));
            rot = new Vector3 (f * 50, f * 50, f * 50);
            myKey.transform.SetPositionAndRotation(pos, Quaternion.Euler(rot));

            yield return new WaitForSeconds(0);
        }

        Destroy(myKey);
    }

    IEnumerator OpenDoor()
    {
        opening = true;
        Vector3 startpos = transform.position;
        Vector3 endpos = openedPosition.transform.position;
        Vector3 pos;

        for (float f = 0; f < openingTime; f += Time.deltaTime)
        {
            pos = Vector3.Lerp(startpos, endpos, f.Remap(0, openingTime, 0, 1));
            transform.position = pos;

            yield return new WaitForSeconds(0);
        }

        opened = true;
        opening = false;
    }
}

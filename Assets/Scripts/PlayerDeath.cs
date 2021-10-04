using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    private bool weAreDead;
    public bool IsDead { get { return weAreDead; } }
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Health>().SubscribeToHealthChange(DieOnZero);
        weAreDead = false;
    }

    public void DieOnZero(int currentHealth){
        if (currentHealth <= 0){
            StartCoroutine(Death());
        }
    }

    IEnumerator Death(){
        weAreDead = true;
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnDestroy(){
        GetComponent<Health>().UnsubscribeToHealthChange(DieOnZero);
    }

}
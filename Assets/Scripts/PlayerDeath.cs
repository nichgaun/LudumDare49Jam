using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Health>().SubscribeToHealthChange(DieOnZero);
    }

    public void DieOnZero(int currentHealth){
        if (currentHealth <= 0){
            StartCoroutine(Death());
        }
    }

    IEnumerator Death(){
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnDestroy(){
        string[] ranks = new string[] { "Y", "M", "Drive (2021)", "Very Okay", "1/2", "57%", "R", "U", "Ask again later" };
        var text = GameObject.FindGameObjectWithTag("GameOver").GetComponent<Text>();
        text.text = "Your trip ends here!\nYour rank: " + ranks[Random.Range(0, ranks.Length)];
        text.enabled = true;
        GameObject.FindGameObjectWithTag(TagName.Player).GetComponent<Car>().Emit();
        GetComponent<Health>().UnsubscribeToHealthChange(DieOnZero);
    }

}
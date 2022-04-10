using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{

    private DeathTagHandler deathTagHandler;

    // Start is called before the first frame update
    void Start()
    {
        deathTagHandler = GameObject.Find("DeathTagsManager").GetComponent<DeathTagHandler>();
        InternalClock.beatEvent.AddListener(CheckIfDead);
    }

    // Update is called once per frame
    void CheckIfDead()
    {
        if (transform.position.x < 1f)
        {
            deathTagHandler.AddTag();
            Destroy(gameObject);
            Music.StopMusic();
        }
    }
}

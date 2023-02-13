using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ACollectable : MonoBehaviour
{
    protected abstract void OnCollect();

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() == PlayerController.Instance)
        {
            OnCollect();
        }
    }



    protected virtual IEnumerator OnCollectCoroutine()
    {
        //scale down to 0
        float time = 0.5f;
        float elapsedTime = 0f;
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;
        GetComponent<Collider>().enabled = false;
        while (elapsedTime < time)
        {
            transform.localScale = Vector3.Lerp(startScale, endScale, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
        
    }
}

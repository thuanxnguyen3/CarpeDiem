using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
public class PlayerTakeDamageVFX : MonoBehaviour
{
    public float intensity;
    PostProcessVolume volume;
    Vignette vignette;
    public BasicAI[] basicAI;

    private void Start()
    {
        volume = GetComponent<PostProcessVolume>();

        volume.profile.TryGetSettings<Vignette>(out vignette);

        if (!vignette)
        {
            print("error, vignette empty");
        }
        else
        {
            vignette.enabled.Override(false);
        }
    }
    private void Update()
    {
        for (int i = 0; i < basicAI.Length; i++)
        {
            if (basicAI[i].finishAttack)
                StartCoroutine(TakeDamageEffect());
        }
    }

    private IEnumerator TakeDamageEffect()
    {
        intensity = 0.4f;
        vignette.enabled.Override(true);
        vignette.intensity.Override(0.4f);

        yield return new WaitForSeconds(0.4f);

        while (intensity > 0)
        {
            intensity -= 0.01f;

            if (intensity < 0) intensity = 0;

            vignette.intensity.Override(intensity);

            yield return new WaitForSeconds(0.1f);
        }

        vignette.enabled.Override(false);
        yield break;
    }
}

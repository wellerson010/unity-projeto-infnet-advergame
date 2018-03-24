using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class FadeService: MonoBehaviour
    {
        public void FadeOut(float time)
        {
            StartCoroutine(Fade(false, time));
        }

        public void FadeIn(float time)
        {
            StartCoroutine(Fade(true, time));
        }

        private IEnumerator Fade(bool fadeIn, float duration)
        {
            float counter = 0;
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            Color spriteColor = renderer.color;

            while (counter < duration)
            {
                counter += Time.deltaTime;

                float alpha = (fadeIn) ? Mathf.Lerp(0, 1, counter / duration):Mathf.Lerp(1, 0, counter / duration);
                renderer.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);

                yield return null;
            }
        }
    }
}

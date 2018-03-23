using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public static class TouchService
    {
        public static void DetectTouch(Action<RaycastHit2D> method, bool limitOneTouch = false)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                if (touch.phase == TouchPhase.Began)
                {
                    RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero);

                    if (hitInfo)
                    {
                        method(hitInfo);

                        if (limitOneTouch)
                        {
                            return;
                        }
                    }
                }
            }
        }
    }
}

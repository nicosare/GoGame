using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalControl : MonoBehaviour
{
    public CameraMove cameraMove;

    public void SetAnimatingTrue()
    {
        cameraMove.isEndAnimating = true;
    }
}

using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
#endif

namespace StarterAssets
{
    public class StarterAssetsInputs : MonoBehaviour
    {
        [Header("Character Input Values")]
        [SerializeField] Vector2 move;
        [SerializeField] bool jump;

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;

        public Vector2 Move { get => move; set => move = value; }
        public bool Jump { get => jump; set => jump = value; }

        #region EventFunctions
        public void OnMove(CallbackContext context)
        {
            Move = context.ReadValue<Vector2>();
        }
        public void OnJump(CallbackContext context)
        {
            jump = context.ReadValueAsButton();
        }
        #endregion

        private void OnApplicationFocus(bool hasFocus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
using UnityEngine.InputSystem;

public static class CS_VibrationControler
{
    private static int ID = 0;

    public static void SetVibration(float low, float hight, float duration)
    {
        Gamepad gamepad = Gamepad.current;
        if (gamepad != null)
        {
            ID++;
            gamepad.SetMotorSpeeds(low, hight);
            CancelVibration(gamepad, duration, ID);
        }
    }

    private static async void CancelVibration(Gamepad gamepad, float duration, int functionID)
    {
        await System.Threading.Tasks.Task.Delay((int)(duration * 1000));
        if (ID == functionID)
        {
            gamepad.SetMotorSpeeds(0f, 0f);
        }
    }
}
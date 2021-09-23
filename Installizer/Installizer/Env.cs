namespace Installizer
{

    public static void UserEnv()
    {
        System.Environment.SetEnvironmentVariable("text", "value", System.EnvironmentVariableTarget.Machine)
    }
}

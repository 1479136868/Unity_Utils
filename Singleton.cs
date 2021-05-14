/// <summary>
/// 泛型的单例类
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> where T : new()
{
    private static T instance;

    private static object LocObj = new object();

    public static T GetInstance()
    {
        if (instance == null)
        {
            lock (LocObj)
            {
                if (instance == null)
                {
                    instance = new T();
                }
            }
        }
        return instance;
    }

}

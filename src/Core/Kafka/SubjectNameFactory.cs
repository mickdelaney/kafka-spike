namespace Messages
{
    public class SubjectNameFactory
    {
        public static string KeySubjectNameFrom<T>(string topic)
        {
            return $"{topic}.{typeof(T).Name}-key";
        }
        
        public static string ValueSubjectNameFrom<T>(string topic)
        {
            return $"{topic}.{typeof(T).Name}-value";
        }
    }
}
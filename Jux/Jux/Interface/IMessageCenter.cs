namespace Jux.Interface
{
    public interface IMessageCenter
    {
        void LongMessage(string Message);
        void ShortMessage(string Message);
        void Notification(string Title, string Message);
    }
}

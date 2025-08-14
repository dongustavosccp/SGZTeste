namespace Utils.DomainNotification
{
    public interface INotifier
    {
        bool HasNotification();
        List<ErrorMessage> GetNotifications();
        void AddNotification(ErrorMessage notification);
        void AddNotification(string notification);
        void ClearNotifications();
    }
}

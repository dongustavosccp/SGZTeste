using Utils;
using Utils.DomainNotification;
namespace EnterpriseLibrary.Service.DomainNotification
{
    public class Notifier : INotifier
    {
        public List<ErrorMessage> _notifications;

        public Notifier()
        {
            _notifications = new List<ErrorMessage>();
        }

        public void AddNotification(ErrorMessage notificacao)
        {
            _notifications.Add(notificacao);
        }

        public void AddNotification(string notification)
        {
            _notifications.Add(new ErrorMessage(notification));
        }

        public void ClearNotifications()
        {
            _notifications = new();
        }

        public List<ErrorMessage> GetNotifications()
        {
            return _notifications;
        }

        public bool HasNotification()
        {
            return _notifications.Any();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAnalysis
{
    public class EventManager
    {
        private static EventManager? _instance;

        public static EventManager? Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new EventManager();
                return _instance;
            }
        }

        private Dictionary<Type, List<object>> _events = new Dictionary<Type, List<object>>();

        private EventManager()
        {
        }

        public void Subscribe<T, U>(Action<U> action)
        {
            if (_events.TryGetValue(typeof(T), out var actions))
            {
                actions.Add(action);
            }
            else
            {
                _events.Add(typeof(T), new List<object>() { action });
            }
        }

        public void Unsubscribe<T, U>(Action<U> action)
        {
            if (_events.TryGetValue(typeof(T), out var actions))
            {
                actions.Remove(action);
            }
        }

        public void Publish<T, U>(U payload)
        {
            if (_events.TryGetValue(typeof(T), out var actions)) // Direct dictionary lookup
            {
                foreach (var action in actions.ToList()) // Avoid modification issues
                {
                    if (action is Action<U> typedAction) // Ensure correct delegate type
                    {
                        typedAction.Invoke(payload);
                    }
                }
            }
        }

    }
}

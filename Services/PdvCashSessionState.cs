namespace PDV.Services
{
    public class PdvCashSessionState
    {
        private readonly object _sync = new();
        private string? _databasePath;
        private int? _movementId;

        public event Action? StateChanged;

        public bool HasOpenCash
        {
            get
            {
                lock (_sync)
                {
                    return _movementId.HasValue;
                }
            }
        }

        public bool IsOpen(string databasePath, int? movementId)
        {
            lock (_sync)
            {
                return movementId.HasValue &&
                       string.Equals(_databasePath, databasePath, StringComparison.OrdinalIgnoreCase) &&
                       _movementId == movementId;
            }
        }

        public void MarkOpen(string databasePath, int? movementId)
        {
            var changed = false;
            lock (_sync)
            {
                changed = !string.Equals(_databasePath, databasePath, StringComparison.OrdinalIgnoreCase) ||
                          _movementId != movementId;
                _databasePath = databasePath;
                _movementId = movementId;
            }

            if (changed)
                StateChanged?.Invoke();
        }

        public void MarkClosed(string databasePath, int? movementId)
        {
            var changed = false;
            lock (_sync)
            {
                if (string.Equals(_databasePath, databasePath, StringComparison.OrdinalIgnoreCase) &&
                    _movementId == movementId)
                {
                    _databasePath = null;
                    _movementId = null;
                    changed = true;
                }
            }

            if (changed)
                StateChanged?.Invoke();
        }
    }
}

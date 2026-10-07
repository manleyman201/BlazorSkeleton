namespace BlazorSkeleton.Data
{
    /// <summary>
    /// Scoped (per-circuit) state for this application. Add app-specific properties here and call
    /// <see cref="UpdateState"/> after changing them so subscribed components re-render.
    /// Cross-app state (current user, etc.) lives in <see cref="ApplicationState"/>.
    /// </summary>
    public class SessionState
    {
        #region Update UI
        public event Action? OnChange;

        public void UpdateState()
        {
            OnChange?.Invoke();
        }
        #endregion
    }
}

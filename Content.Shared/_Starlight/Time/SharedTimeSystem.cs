using Content.Shared.Examine;
using Content.Shared.GameTicking;
using Robust.Shared.Timing;

namespace Content.Shared._Starlight.Time
{
    public sealed class TimeSystem : EntitySystem
    {
        [Dependency] private readonly IGameTiming _timing = default!;
        
        private DateTime _date = DateTime.UtcNow.AddYears(1000);

        private TimeSpan _roundStart;

        public override void Initialize()
        {
            base.Initialize();
            SubscribeNetworkEvent<TickerLobbyStatusEvent>(LobbyStatus);
        }

        private void LobbyStatus(TickerLobbyStatusEvent ev)
        {
            _roundStart = ev.RoundStartTimeSpan;
        }

        public (TimeSpan Time, string Date) GetStationTime()
        {
            var stationTime = _timing.CurTime.Subtract(_roundStart).Add(TimeSpan.FromHours(12));

            var totalDays = (int) stationTime.TotalDays;
            stationTime = stationTime.Subtract(TimeSpan.FromDays(totalDays));

            _date = _date.AddDays(totalDays);

            return (stationTime, _date.ToString("dd.MM.yyyy"));
        }

        public string GetDate()
        {
            // please tell me you guys aren't gonna have a 4 week round yet...
            return _date.ToString("dd.MM.yyyy");
        }
    }
}
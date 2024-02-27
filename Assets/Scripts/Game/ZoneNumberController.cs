public class ZoneNumberController : IZoneNumberController
{
    private int _zoneNumber;
    public const int SuperZonePeriod = 30;
    public const int SafeZonePeriod = 5;
    
    public void Initialize()
    {
        _zoneNumber = 1;
    }
    
    public void IncrementZoneNumber()
    {
        _zoneNumber += 1;
    }

    public int GetZoneNumber()
    {
        return _zoneNumber;
    }

    public ZoneType GetCurrentZoneType()
    {
        if (_zoneNumber % SuperZonePeriod == 0)
        {
            return ZoneType.Super;
        }

        if (_zoneNumber % SafeZonePeriod == 0 || _zoneNumber == 1)
        {
            return ZoneType.Safe;
        }

        return ZoneType.Standard;
    }
}

public interface IZoneNumberController
{
    void Initialize();
    void IncrementZoneNumber();
    int GetZoneNumber();
    ZoneType GetCurrentZoneType();
}

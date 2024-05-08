namespace ConsoleApp
{
    public struct Flight
    {
        public string Fnum;
        public string Destination;
        public string TakeOff;
        public DateTime TakeOffDate;
        public DateTime ArrivalDate;
        public int? AircraftId;
        public int? CreatorId;
        public override readonly string ToString()
        {
            return $"{Fnum} {Destination} {TakeOff} {TakeOffDate} {ArrivalDate} {AircraftId} {CreatorId}";
        }
        public Flight(string fnum, string destination, string takeOff, DateTime takeOffDate, DateTime arrivalDate, int? aircraftId, int? creatorId)
        {
            Fnum = fnum;
            Destination = destination;
            TakeOff = takeOff;
            TakeOffDate = takeOffDate;
            ArrivalDate = arrivalDate;
            AircraftId = aircraftId;
            CreatorId = creatorId;
        }
    }
}


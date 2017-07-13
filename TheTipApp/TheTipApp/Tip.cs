using System;

namespace TheTipApp
{
    class Tip
    {
        public int WaiterRating { get; set; }
        public string WaiterName { get; set; }
        public string RestaurantName { get; set; }
        public float TipAmt { get; set; }
        public float TicketTotal { get; set; }
        public string Date { get; set; }

        public override string ToString()
        {
            return RestaurantName;
        }

        public static explicit operator Tip(Java.Lang.Object v)
        {
            throw new NotImplementedException();
        }
    }
}
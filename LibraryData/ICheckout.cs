using LibraryData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryData
{
    public interface ICheckout
    {
        IEnumerable<Checkout> GetAll();
        Checkout GetById(int CheckoutId);
        void Add(Checkout newCheckout);
        void CheckOutItem(int assetId, int LibraryCardId);
        void CheckInItem(int assetId, int LibraryCardId);
        IEnumerable<CheckoutHistory> GetCheckoutHistory(int id);
        void PlaceHold(int assetId, int LibraryCardId);
        void GetCurrentHoldPatronName(int Id);
        DateTime GetCurrentHoldPlaced(int Id);
        IEnumerable<Hold> GetCurrentHolds(int id);
        void MarkLost(int assetId);
        void MarkFound(int assetId);
        Checkout GetLatestCheckout(int assetId);
    }
}

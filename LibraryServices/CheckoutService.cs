using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibraryData;
using LibraryData.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryServices
{
    public class CheckoutService : ICheckout
    {
        private LibraryContext _context;
        public CheckoutService(LibraryContext context)
        {
            _context = context;
        }
        public void Add(Checkout newCheckout)
        {
            _context.Add(newCheckout);
            _context.SaveChanges();
        }

        public void CheckInItem(int assetId, int LibraryCardId)
        {
            var asset = _context.LibraryAssets.FirstOrDefault(asset => asset.Id == assetId);
            _context.Update(asset);
            asset.Status = _context.Statuses.FirstOrDefault(status => status.Name == "Available");
            var checkoutHistory = _context.CheckoutHistories.FirstOrDefault(h => h.LibraryAsset.Id == assetId && h.CheckIn == null);
            if(checkoutHistory != null )
            {
                _context.Update(checkoutHistory);
                checkoutHistory.CheckIn = DateTime.Now;
            }
            
        }

        public void CheckOutItem(int assetId, int LibraryCardId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Checkout> GetAll()
        {
            return _context.Checkouts;
        }

        public Checkout GetById(int CheckoutId)
        {
            return GetAll().FirstOrDefault(Checkout => Checkout.Id == CheckoutId);
        }

        public IEnumerable<CheckoutHistory> GetCheckoutHistory(int id)
        {
            return _context.CheckoutHistories
                .Include(h=>h.LibraryAsset)
                .Include(h=>h.LibraryCard)
                .Where(history => history.LibraryAsset.Id == id);
        }

        public string GetCurrentHoldPatronName(int Id)
        {
            var LibraryCardId = _context.CheckoutHistories.FirstOrDefault(_history => _history.LibraryAsset.Id == Id && _history.CheckIn == null).LibraryCard.Id;
            return _context.Patrons.FirstOrDefault(Patron => Patron.LibraryCard.Id == LibraryCardId).FirstName;
        }

        public DateTime GetCurrentHoldPlaced(int Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Hold> GetCurrentHolds(int id)
        {
            return _context.Holds
                .Include(h => h.LibraryAsset)              
                .Where(h => h.LibraryAsset.Id == id);
        }

        public Checkout GetLatestCheckout(int assetId)
        {
            return _context.Checkouts.Where(c => c.LibraryAsset.Id == assetId)
                .OrderByDescending ( c=>c.Since )
                .FirstOrDefault();
        }

        public void MarkFound(int assetId)
        {
            UpdateAssetStatus(assetId, "Available");
            RemoveExistingCheckout(assetId);
            CloseExistingCheckoutHistory(assetId);           
            _context.SaveChanges();
        }

        private void CloseExistingCheckoutHistory(int assetId)
        {
            var checkoutHistory = _context.CheckoutHistories
               .FirstOrDefault(h => h.LibraryAsset.Id == assetId && h.CheckIn == null);
            if (checkoutHistory != null)
            {
                _context.Update(checkoutHistory);
                checkoutHistory.CheckIn = DateTime.Now;
            }
        }

        private void RemoveExistingCheckout(int assetId)
        {
            var checkout = _context.Checkouts
              .FirstOrDefault(asset => asset.LibraryAsset.Id == assetId);
            if (checkout != null)
            {
                _context.Remove(checkout);
            }
        }

        public void MarkLost(int assetId)
        {            
            UpdateAssetStatus(assetId, "Lost");           
            _context.SaveChanges();
        }

        private void UpdateAssetStatus(int assetId, string v)
        {
            var asset = _context.LibraryAssets
                .FirstOrDefault(asset => asset.Id == assetId);
            _context.Update(asset);
            asset.Status = _context.Statuses
                .FirstOrDefault(status => status.Name == "Lost");
        }

        public void PlaceHold(int assetId, int LibraryCardId)
        {
            throw new NotImplementedException();
        }

        void ICheckout.GetCurrentHoldPatronName(int Id)
        {
            throw new NotImplementedException();
        }
    }
}

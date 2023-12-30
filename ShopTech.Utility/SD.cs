using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopTech.Utility
{
    public static class SD
    {
        public const string Role_Cust = "Customer";
        public const string Role_Comp = "Company";
        public const string Role_Admin = "Admin";
        public const string Role_Emp = "Employee";

        public const string StatusPending = "Pending";
        public const string StatusAproved = "Aproved";
        public const string StatusInProcess = "Processing";
        public const string StatusCanceled = "Canceled";
        public const string StatusShipped = "Shipped";
        public const string StatusRefunded = "Refunded";

        public const string PaymentPending = "Pending";
        public const string PaymentAproved = "Aproved";
        public const string PaymentForDelay = "AprovedForDelayPayment";
        public const string PaymentRejected = "Rejected";

        public const string SessionCart = "SessionShoppingCart";

    }
}

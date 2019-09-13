using System;

namespace Common.exception
{
    public class Constants
    {
        public static long Location = 1;
        public static string PaymentMethodEzcard = "EZCARD";//Thẻ thành viên EZCARD
        public static string PaymentMethodCash = "CASH";//Thanh toán bằng tiền mặt BANKCARD
        public static string PaymentMethodBankcard = "BANKCARD";//Thanh toán bằng thẻ ngân hàng
        public static string TimeRefund = "TIMEREFUND";// Thời gian tối thiểu cho phép trả vé.
        public static string TimeReprint = "TIMEREPRINT";// Thời gian tối thiểu cho int lại vé.
        public static string SettingKeyReprintBooking = "ReprintBooking";
        public static string SettingKeyReprintMinutes = "ReprintMinutes";
        public static string SettingKeyRefundMinutes = "RefundMinutes";
        public static string Tax = "Tax";
        public static string SettingKeyPayDelayTicket = "PayTicketDelay";
        public static string Login = "CheckLogingDb";
        public static string User = "CheckLog";
        public static string Checkrunningshift = "CheckRunningShift";
        public static string Getstatusshiftsale = "GetStatusShiftSale";
        public static string Selectshiftsale = "SelectShiftSale";
        public static string BookingonlineTicketCode = "BookingOnlineTicketType";
        public static string BookingonlineMnitue = "BookingOnlineBlockSeatMinute";
        public static string BookingofflineMnitue = "BookingOfflineBlockSeatMinute";
        public static string BookingonlineSetting = "BookingOnlineSettingTIme";

        public static class Error
        {
            public static String NoData = "No Data!";
            public static String SqlException = "Sql Exeption!";
            public static String SqlUnhandler = "Unhandler Exception!";
            public static String LoginFail = "Incorrect username or password!";
            public static String InvalidValue = "Invalid Value!";
            public static String SsNotRunning = "Shift sale not running";
            public static String MoneyInvalid = "Money Invalid!";
            public static String SeatHasBeenBought = "Seat has been bought!";
            public static String BuyTicketReserveOvertime = "Over Time To Reserve Booking!";
            public static String EzcardNotEnoughMoney = "Ezcard Not Enough Money!";
            public static string NotYetLogin = "You are not yet login!";
            public static string UsernameLoginAnotherComputer = "Your username has been login another computer!";
            public static String CanNotReadHeader = "Can not read header!";
            public static string YouNotEnoughPermission = "You don't have permission to access!";
            public static String Errokey = "Key Invalid!";
        }

        public static class Define
        {
            // Trạng thái bán ghế
            public static int SeatSoldStatusUnsold = 0;

            public static int SeatSoldStatusSolded = 1;
            public static int SeatSoldStatusReserved = 2;

            // Trạng thái ghế
            public static int SeatStatusNormal = 0;

            public static int SeatStatusDamaged = 1;

            // Money
            public static int MinMoney = 0;

            public static int PaySuccess = 0;

            // SPLIT CHARACTER TYPE CHAR
            public static char SplitPaymentmethod = '-';

            // Trạng thái ca bán hàng
            public static int ShiftOpenNotActive = 0;

            public static int ShiftOpenActive = 1;
            public static int ShiftCloseNotConfirm = 2;
            public static int ShiftClose = 3;

            // Trạng thái booking
            public static int BookingTypeOnline = 0;

            public static int BookingTypeOffline = 1;

            public static int BookingStateOk = 2;
            public static int BookingStateClose = 3;
            public static int BookingStateChange = 4;

            // Trạng thái BookingCustomer
            public static int BookingCustomerError = 0;

            public static int BookingCustomerOk = 1;
            public static int BookingCustomerRefundOk = 2;
            public static int BookingCustomerRefundError = 3;

            public static int True = 0;
            public static int False = 1;

            public static String BookingCustomerBo = "BO";
            public static String BookingCustomerCo = "CO";
        }
    }
}
using System.Runtime.Serialization;

namespace Common.exception
{ //
    [DataContract]
    //[ServiceBehavior(IncludeExceptionDetailInFaults=false)]
    public class CodedException // Exception
    {
        public static string NotConnect = "10000";
        public static string InvalidValUe = "10001"; // Bỏ ,chỉ đê lỗi câu truc dữ liệu.//18/11 k bo trương này
        public static string InvalidTypeExist = "10002";
        public static string InvalidPayment = "10003"; //
        public static string InvalidStructureData = "10004"; // lỗi cấu trúc field,vd : mail
        public static string Unhandler = "-1";
        public static string SqlExceptionUnhandler = "10005";
        public static string InvalidSeat = "10006";
        public static string InvalidDiscount = "10007";
        public static string InvalidVoucher = "10008";
        public static string DataNotExitance = "10009";
        public static string IncorrectLogin = "10011";
        public static string InvalidShiftsale = "10012";
        public static string InvalidRunningShiftsale = "10013";
        public static string InvalidCloseShiftsale = "10014";
        public static string InvalidActiveShiftsale = "10015";
        public static string UnknowHeader = "10016";
        public static string ShiftNotRunning = "10017";
        public static string CanNotOpenSession = "10018";
        public static string InvalidMoney = "10019";
        public static string SeatHasBeenBought = "10020";
        public static string BuyTicketReserveOvertime = "10021";
        public static string EzcardNotEnoughMoney = "10022";
        public static string NotYetLogin = "10023";
        public static string UsernameLoginAnotherComputer = "10024";
        public static string YouNotEnoughPermission = "10025";
        public static string ExceptionKey = "10005";
        public static string NullEntity = "10025";
        //lôi sql : inser ,update k thực hiện được
        // Lỗi Không tồn tai dl update.

        public CodedException(string exceptionCode, string message)
        // : base(message)
        {
            Message = message;
            ExceptionCode = exceptionCode;
        }

        [DataMember]
        public string ExceptionCode { get; set; }

        [DataMember]
        public string Message { get; set; }
    }
}